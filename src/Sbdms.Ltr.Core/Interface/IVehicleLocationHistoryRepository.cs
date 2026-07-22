using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Interface;

public interface IVehicleLocationHistoryRepository
{
    void Add(VehicleLocationHistory entity);
    Task<IEnumerable<VehicleLocationHistory>> GetByVehicleIdAsync(int vehicleId);
}
