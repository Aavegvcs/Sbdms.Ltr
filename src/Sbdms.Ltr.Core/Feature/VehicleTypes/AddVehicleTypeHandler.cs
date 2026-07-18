using Sbdms.Ltr.Contracts.VehicleType;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.VehicleTypes;

public class AddVehicleTypeHandler(IVehicleTypeRepository vehicleTypeRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleTypeResponse>>> HandleAsync(AddVehicleTypeRequest request)
    {
        var duplicate = await vehicleTypeRepository.GetByAsync(vt => vt.VehicleTypeDesc == request.VehicleTypeDesc);
        if (duplicate is not null)
            return VehicleTypeErrors.DuplicateVehicleTypeDesc;

        var vehicleType = VehicleType.Create(
            request.LocCode,
            request.VehicleTypeDesc,
            request.Capacity,
            request.BillingCode,
            request.IsActive,
            request.Occupancy,
            request.CreatedBy,
            DateTime.UtcNow);

        var result = await vehicleTypeRepository.AddAsync(vehicleType);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleTypeResponse>(vehicleType.ToResponse(), true, "Vehicle type added successfully.");
    }
}
