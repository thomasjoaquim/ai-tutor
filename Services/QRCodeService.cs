using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace MyProject.Services
{
    public class QRCodeService
    {
        public byte[] GenerateQRCode(string url)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}
