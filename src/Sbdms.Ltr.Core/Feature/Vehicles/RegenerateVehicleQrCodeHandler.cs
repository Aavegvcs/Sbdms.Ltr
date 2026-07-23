using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.Vehicles;

// Rotates an existing vehicle's QrUniqueCode to a brand-new value — the old QR sticker stops
// resolving to this vehicle the moment this runs.
public class RegenerateVehicleQrCodeHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleResponse>>> HandleAsync(int id)
    {
        var existing = await vehicleRepository.FindByIdAsync(id);
        if (existing.IsError)
            return existing.Errors;

        var vehicle = existing.Value;
        var now = IndianStandardTime.Now;

        string newQrCode;
        do
        {
            newQrCode = $"QR-{Guid.NewGuid():N}"[..20];
        } while (await vehicleRepository.GetByAsync(v => v.QrUniqueCode == newQrCode) is not null);

        vehicle.RegenerateQrCode(newQrCode, now);

        var result = await vehicleRepository.UpdateAsync(vehicle);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleResponse>(vehicle.ToResponse(), true, "QR code regenerated successfully.");
    }
}
