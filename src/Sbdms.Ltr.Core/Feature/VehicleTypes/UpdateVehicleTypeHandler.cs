using Sbdms.Ltr.Contracts.VehicleType;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.VehicleTypes;

public class UpdateVehicleTypeHandler(IVehicleTypeRepository vehicleTypeRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleTypeResponse>>> HandleAsync(UpdateVehicleTypeRequest request)
    {
        var existing = await vehicleTypeRepository.FindByIdAsync(request.VehicleTypeId);
        if (existing.IsError)
            return existing.Errors;

        var duplicate = await vehicleTypeRepository.GetByAsync(vt =>
            vt.VehicleTypeDesc == request.VehicleTypeDesc && vt.VehicleTypeId != request.VehicleTypeId);
        if (duplicate is not null)
            return VehicleTypeErrors.DuplicateVehicleTypeDesc;

        var vehicleType = existing.Value;
        vehicleType.Update(
            request.LocCode,
            request.VehicleTypeDesc,
            request.Capacity,
            request.BillingCode,
            request.IsActive,
            request.Occupancy,
            request.ModBy,
            DateTime.UtcNow);

        var result = await vehicleTypeRepository.UpdateAsync(vehicleType);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleTypeResponse>(vehicleType.ToResponse(), true, "Vehicle type updated successfully.");
    }
}
