using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public static class VehicleDriverAssignmentLogMappingExtensions
{
    public static VehicleDriverAssignmentLogResponse ToResponse(this VehicleDriverAssignmentLog log) =>
        new(
            log.Id,
            log.VehicleId,
            log.VehicleNumber,
            log.VehicleCompany,
            log.Modal,
            log.VendorId,
            log.OldDriverId,
            log.OldDriverName,
            log.OldDriverNumber,
            log.OldLicenceNumber,
            log.NewDriverId,
            log.ChangedOn
        );
}
