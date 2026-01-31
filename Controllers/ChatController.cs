using Microsoft.AspNetCore.Mvc;
using MyProject.Services;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                var response = await _openAIService.GetBiographyAssistanceAsync(
                    request.Message, 
                    request.ConversationHistory ?? new List<string>()
                );

                return Ok(new { response });
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