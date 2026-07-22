using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;

namespace Sbdms.Ltr.Infra.Repositories;

public class VehicleLocationHistoryRepository(LtrAppDbContext dbContext) : IVehicleLocationHistoryRepository
{
    public void Add(VehicleLocationHistory entity) =>
        dbContext.VehicleLocationHistories.Add(entity);

    public async Task<IEnumerable<VehicleLocationHistory>> GetByVehicleIdAsync(int vehicleId) =>
        await dbContext.VehicleLocationHistories
            .AsNoTracking()
            .Where(l => l.VehicleId == vehicleId)
            .OrderByDescending(l => l.RecordedOn)
            .ToListAsync();
}
