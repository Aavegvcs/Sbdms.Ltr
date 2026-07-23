using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.Vehicles;

// Re-pairs a vehicle with a (possibly different, possibly no) driver. A driver can only be
// assigned to one vehicle at a time: if the requested driver is currently assigned elsewhere,
// that other vehicle is automatically unassigned first. Every mapping replaced by this call —
// this vehicle's and, if applicable, the other vehicle's — is snapshotted into
// VehicleDriverAssignmentLog before it's overwritten.
public class ChangeVehicleDriverHandler(
    IVehicleRepository vehicleRepository,
    IDriverRepository driverRepository,
    IVehicleDriverAssignmentLogRepository logRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleResponse>>> HandleAsync(int vehicleId, ChangeVehicleDriverRequest request)
    {
        var existing = await vehicleRepository.FindByIdAsync(vehicleId);
        if (existing.IsError)
            return existing.Errors;

        var vehicle = existing.Value;
        var now = IndianStandardTime.Now;

        Driver? newDriver = null;
        if (request.DriverId is not null)
        {
            newDriver = await driverRepository.GetByAsync(d => d.Id == request.DriverId);
            if (newDriver is null)
                return VehicleErrors.InvalidDriver;

            var previousVehicle = await vehicleRepository.GetByAsync(v => v.DriverId == request.DriverId && v.Id != vehicleId);
            if (previousVehicle is not null)
            {
                logRepository.Add(VehicleDriverAssignmentLog.Create(
                    previousVehicle.Id,
                    previousVehicle.VehicleNumber,
                    previousVehicle.VehicleCompany,
                    previousVehicle.Modal,
                    previousVehicle.VendorId,
                    previousVehicle.DriverId,
                    newDriver.DriverName,
                    newDriver.DriverNumber,
                    newDriver.LicenceNumber,
                    null,
                    now));

                previousVehicle.ReassignDriver(null, now);

                var vacateResult = await vehicleRepository.UpdateAsync(previousVehicle);
                if (vacateResult.IsError)
                    return vacateResult.Errors;
            }
        }

        var oldDriver = vehicle.DriverId is not null
            ? await driverRepository.GetByAsync(d => d.Id == vehicle.DriverId)
            : null;

        logRepository.Add(VehicleDriverAssignmentLog.Create(
            vehicle.Id,
            vehicle.VehicleNumber,
            vehicle.VehicleCompany,
            vehicle.Modal,
            vehicle.VendorId,
            vehicle.DriverId,
            oldDriver?.DriverName,
            oldDriver?.DriverNumber,
            oldDriver?.LicenceNumber,
            request.DriverId,
            now));

        vehicle.ReassignDriver(request.DriverId, now);

        var result = await vehicleRepository.UpdateAsync(vehicle);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleResponse>(vehicle.ToResponse(), true, "Vehicle driver reassigned successfully.");
    }
}
