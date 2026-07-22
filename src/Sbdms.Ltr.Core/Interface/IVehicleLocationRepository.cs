using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Interface;

public interface IVehicleLocationRepository
{
    Task<VehicleLocation?> GetByVehicleIdAsync(int vehicleId);
    void Add(VehicleLocation entity);
}
