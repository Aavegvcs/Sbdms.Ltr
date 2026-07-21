using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Feature.Drivers;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleByQrCodeHandler(IVehicleRepository vehicleRepository, IDriverRepository driverRepository)
{
    public async Task<Result<CoreResponse<VehicleDetailsResponse>>> HandleAsync(string qrCode)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.QrUniqueCode == qrCode);
        if (vehicle is null)
            return VehicleErrors.VehicleNotFound;

        var driver = vehicle.DriverId is not null
            ? await driverRepository.GetByAsync(d => d.Id == vehicle.DriverId)
            : null;

        var response = new VehicleDetailsResponse(vehicle.ToResponse(), driver?.ToResponse());

        return new CoreResponse<VehicleDetailsResponse>(response, true, "Vehicle retrieved successfully.");
    }
}
