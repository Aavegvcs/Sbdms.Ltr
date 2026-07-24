using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;

namespace Sbdms.Ltr.Infra.Repositories;

public class VehicleLocationRepository(LtrAppDbContext dbContext) : IVehicleLocationRepository
{
    public async Task<VehicleLocation?> GetByVehicleIdAsync(int vehicleId) =>
        await dbContext.VehicleLocations.FirstOrDefaultAsync(l => l.VehicleId == vehicleId);

    public void Add(VehicleLocation entity) =>
        dbContext.VehicleLocations.Add(entity);

    public void Update(VehicleLocation entity)
    {
        dbContext.VehicleLocations.Update(entity);
    }
}
