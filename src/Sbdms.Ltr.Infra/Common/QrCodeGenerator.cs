using QRCoder;
using Sbdms.Ltr.Core.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Sbdms.Ltr.Infra.Common;

public class QrCodeGenerator : IQrCodeGenerator
{
    private const string LogoResourceName = "Sbdms.Ltr.Infra.Assets.CommutrLogo.png";
    private const int Padding = 24;

    private static readonly byte[] LogoPngBytes = LoadEmbeddedLogo();

    public byte[] GeneratePng(string value)
    {
        using var qrCodeGenerator = new QRCodeGenerator();
        using var qrCodeData = qrCodeGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrCodeData);
        var qrBytes = pngQrCode.GetGraphic(20);

        using var qrImage = Image.Load<Rgba32>(qrBytes);
        using var logoImage = Image.Load<Rgba32>(LogoPngBytes);

        // Scale the logo banner to the QR code's width, preserving its own aspect ratio.
        var logoHeight = (int)Math.Round(logoImage.Height * (qrImage.Width / (double)logoImage.Width));
        logoImage.Mutate(ctx => ctx.Resize(qrImage.Width, logoHeight));

        var canvasWidth = qrImage.Width + Padding * 2;
        var canvasHeight = logoImage.Height + qrImage.Height + Padding * 3;

        using var canvas = new Image<Rgba32>(canvasWidth, canvasHeight, Color.White);
        canvas.Mutate(ctx => ctx
            .DrawImage(logoImage, new Point(Padding, Padding), 1f)
            .DrawImage(qrImage, new Point(Padding, Padding * 2 + logoImage.Height), 1f));

        using var outputStream = new MemoryStream();
        canvas.SaveAsPng(outputStream);
        return outputStream.ToArray();
    }

    private static byte[] LoadEmbeddedLogo()
    {
        var assembly = typeof(QrCodeGenerator).Assembly;
        using var stream = assembly.GetManifestResourceStream(LogoResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{LogoResourceName}' was not found.");

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
