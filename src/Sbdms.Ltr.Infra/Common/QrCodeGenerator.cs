using QRCoder;
using Sbdms.Ltr.Core.Interface;

namespace Sbdms.Ltr.Infra.Common;

public class QrCodeGenerator : IQrCodeGenerator
{
    public byte[] GeneratePng(string value)
    {
        using var qrCodeGenerator = new QRCodeGenerator();
        using var qrCodeData = qrCodeGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrCodeData);
        return pngQrCode.GetGraphic(20);
    }
}
