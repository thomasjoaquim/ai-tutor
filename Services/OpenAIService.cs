using OpenAI.Chat;

namespace MyProject.Services
{
    public interface IOpenAIService
    {
        Task<string> GetBiographyAssistanceAsync(string userMessage, List<string> conversationHistory);
    }

    public class OpenAIService : IOpenAIService
    {
        private readonly ChatClient _chatClient;
        private readonly string _systemPrompt = @"You are a compassionate AI assistant helping people write beautiful biographies for their deceased loved ones. 

Your role is to:
1. Ask thoughtful, sensitive questions that help gather meaningful information
2. Guide users through different aspects of their loved one's life
3. Be empathetic and respectful throughout the conversation
4. Ask one question at a time to avoid overwhelming the user
5. Focus on: childhood, personality, achievements, relationships, hobbies, values, memorable moments, and legacy

Keep responses warm, brief, and focused. Always be respectful when discussing someone who has passed away.";

        public OpenAIService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "your-openai-api-key-here")
            {
                throw new InvalidOperationException("OpenAI API key is not configured. Please set your API key in appsettings.json");
            }
            
            _chatClient = new ChatClient("gpt-3.5-turbo", apiKey);
        }

        public async Task<string> GetBiographyAssistanceAsync(string userMessage, List<string> conversationHistory)
        {
            try
            {
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
                return response.Value.Content[0].Text;
            }
            catch (Exception ex)
            {
                return $"I'm sorry, I'm having trouble connecting right now. Please try again later. Error: {ex.Message}";
            }
        }
    }
}