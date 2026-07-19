using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleQrImageHandler(IVehicleRepository vehicleRepository, IQrCodeGenerator qrCodeGenerator)
{
    public async Task<Result<byte[]>> HandleAsync(int id)
    {
        var result = await vehicleRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return qrCodeGenerator.GeneratePng(result.Value.QrUniqueCode);
    }
}
