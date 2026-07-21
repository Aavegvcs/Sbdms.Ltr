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
    IVendorRepository vendorRepository,
    IVehicleTypeRepository vehicleTypeRepository,
    ICurrentStatusRepository currentStatusRepository,
    IDriverRepository driverRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleResponse>>> HandleAsync(AddVehicleRequest request)
    {
        var vendor = await vendorRepository.GetByAsync(v => v.Id == request.VendorId);
        if (vendor is null)
            return VehicleErrors.InvalidVendor;

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

        var vehicle = Vehicle.Create(
            request.VendorId,
            request.VehicleTypeCode,
            request.DriverId,
            request.CurrentStatusId,
            request.VehicleNumber,
            request.VehicleCompany,
            request.Modal,
            request.QrUniqueCode,
            DateTime.UtcNow);

        var result = await vehicleRepository.AddAsync(vehicle);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleResponse>(vehicle.ToResponse(), true, "Vehicle added successfully.");
    }
}
