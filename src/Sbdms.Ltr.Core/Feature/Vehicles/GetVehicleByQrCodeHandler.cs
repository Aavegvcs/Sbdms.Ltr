using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleByQrCodeHandler(IVehicleRepository vehicleRepository)
{
    public async Task<Result<CoreResponse<VehicleResponse>>> HandleAsync(string qrCode)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.QrUniqueCode == qrCode);
        if (vehicle is null)
            return VehicleErrors.VehicleNotFound;

        return new CoreResponse<VehicleResponse>(vehicle.ToResponse(), true, "Vehicle retrieved successfully.");
    }
}
