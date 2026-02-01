using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Services;
using System.Security.Claims;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public ChatController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("biography-assistance")]
        public async Task<IActionResult> GetBiographyAssistance([FromBody] ChatRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { error = "User not authenticated" });
                }

                var response = await _openAIService.GetBiographyAssistanceAsync(
                    request.Message, 
                    request.ConversationHistory ?? new List<string>(),
                    userId
                );

                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpGet("token-usage")]
        public async Task<IActionResult> GetTokenUsage()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { error = "User not authenticated" });
                }

                var tokenLimitService = HttpContext.RequestServices.GetRequiredService<TokenLimitService>();
                var remaining = await tokenLimitService.GetRemainingTokensAsync(userId);
                
                return Ok(new { remainingTokens = remaining, monthlyLimit = 10000 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = "";
        public List<string>? ConversationHistory { get; set; }
    }
}