using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Interface;

public interface IVehicleDriverAssignmentLogRepository
{
    void Add(VehicleDriverAssignmentLog entity);
    Task<IEnumerable<VehicleDriverAssignmentLog>> GetByVehicleIdAsync(int vehicleId);
}
