namespace Sbdms.Ltr.Core.Interface;

public interface IQrCodeGenerator
{
    byte[] GeneratePng(string value);
}
