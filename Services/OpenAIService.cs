using OpenAI.Chat;

namespace MyProject.Services
{
    public interface IOpenAIService
    {
        Task<string> GetBiographyAssistanceAsync(string userMessage, List<string> conversationHistory, int userId);
    }

    public class OpenAIService : IOpenAIService
    {
        private readonly ChatClient _chatClient;
        private readonly TokenLimitService _tokenLimitService;
        private readonly string _systemPrompt = @"You are a compassionate AI assistant helping people write beautiful biographies for their deceased loved ones. 

Your role is to:
1. Ask thoughtful, sensitive questions that help gather meaningful information
2. Guide users through different aspects of their loved one's life
3. Be empathetic and respectful throughout the conversation
4. Ask one question at a time to avoid overwhelming the user
5. Focus on: childhood, personality, achievements, relationships, hobbies, values, memorable moments, and legacy

Keep responses warm, brief, and focused. Always be respectful when discussing someone who has passed away.";

        public OpenAIService(IConfiguration configuration, TokenLimitService tokenLimitService)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not configured. Please set OPENAI_API_KEY in your .env file");
            }
            
            _chatClient = new ChatClient("gpt-3.5-turbo", apiKey);
            _tokenLimitService = tokenLimitService;
        }

        public async Task<string> GetBiographyAssistanceAsync(string userMessage, List<string> conversationHistory, int userId)
        {
            try
            {
                // Estimate tokens needed (rough calculation)
                var estimatedTokens = EstimateTokens(userMessage, conversationHistory);
                
                // Check if user has enough tokens
                if (!await _tokenLimitService.CanUseTokensAsync(userId, estimatedTokens))
                {
                    var remaining = await _tokenLimitService.GetRemainingTokensAsync(userId);
                    return $"You have reached your monthly token limit. Remaining tokens: {remaining}. Your limit resets at the beginning of each month.";
                }

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(_systemPrompt)
                };

                // Add conversation history
                for (int i = 0; i < conversationHistory.Count; i++)
                {
                    if (i % 2 == 0)
                        messages.Add(new UserChatMessage(conversationHistory[i]));
                    else
                        messages.Add(new AssistantChatMessage(conversationHistory[i]));
                }

                // Add current user message
                messages.Add(new UserChatMessage(userMessage));

                var response = await _chatClient.CompleteChatAsync(messages);
                
                // Get actual token usage from response
                var actualTokens = response.Value.Usage?.TotalTokenCount ?? estimatedTokens;
                
                // Record actual token usage
                await _tokenLimitService.AddTokenUsageAsync(userId, actualTokens);
                
                return response.Value.Content[0].Text;
            }
            catch (Exception ex)
            {
                return $"I'm sorry, I'm having trouble connecting right now. Please try again later. Error: {ex.Message}";
            }
        }
        
        private int EstimateTokens(string userMessage, List<string> conversationHistory)
        {
            // Rough estimation: 1 token ≈ 4 characters
            var totalChars = userMessage.Length + conversationHistory.Sum(h => h.Length) + _systemPrompt.Length;
            return (int)(totalChars / 4.0 * 1.2); // Add 20% buffer
        }
    }
}