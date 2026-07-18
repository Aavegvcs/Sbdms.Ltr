using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class AddVehicleHandler(
    IVehicleRepository vehicleRepository,
    IVehicleTypeRepository vehicleTypeRepository,
    ICurrentStatusRepository currentStatusRepository,
    IDriverRepository driverRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleResponse>>> HandleAsync(AddVehicleRequest request)
    {
        var vehicleType = await vehicleTypeRepository.GetByAsync(vt => vt.Id == request.VehicleTypeCode);
        if (vehicleType is null)
            return VehicleErrors.InvalidVehicleType;

        var currentStatus = await currentStatusRepository.GetByAsync(cs => cs.Id == request.CurrentStatusId);
        if (currentStatus is null)
            return VehicleErrors.InvalidCurrentStatus;

        if (request.DriverId is not null)
        {
            var driver = await driverRepository.GetByAsync(d => d.Id == request.DriverId);
            if (driver is null)
                return VehicleErrors.InvalidDriver;
        }

        var vehicle = Vehicle.Create(request.VehicleTypeCode, request.DriverId, request.CurrentStatusId, request.QrUniqueCode, DateTime.UtcNow);

        var result = await vehicleRepository.AddAsync(vehicle);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleResponse>(vehicle.ToResponse(), true, "Vehicle added successfully.");
    }
}
