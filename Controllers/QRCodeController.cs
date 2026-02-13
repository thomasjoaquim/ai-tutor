using Microsoft.AspNetCore.Mvc;
using MyProject.Services;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        private readonly QRCodeService _qrCodeService;
        private readonly IMemorialService _memorialService;

        public QRCodeController(QRCodeService qrCodeService, IMemorialService memorialService)
        {
            _qrCodeService = qrCodeService;
            _memorialService = memorialService;
        }

        [HttpGet("generate/{memorialId}")]
        public async Task<IActionResult> GenerateQRCode(int memorialId)
        {
            var memorial = await _memorialService.GetMemorialByIdAsync(memorialId);
            
            if (memorial == null)
            {
                return NotFound();
            }

            var url = $"{Request.Scheme}://{Request.Host}/memorial/{memorial.UniqueId}";
            var qrCodeBytes = _qrCodeService.GenerateQRCode(url);

            return File(qrCodeBytes, "image/png", $"qrcode-{memorial.UniqueId}.png");
        }
    }
}
