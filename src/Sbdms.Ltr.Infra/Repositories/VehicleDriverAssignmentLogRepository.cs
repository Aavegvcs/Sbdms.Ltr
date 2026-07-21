using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;

namespace Sbdms.Ltr.Infra.Repositories;

public class VehicleDriverAssignmentLogRepository(LtrAppDbContext dbContext) : IVehicleDriverAssignmentLogRepository
{
    public void Add(VehicleDriverAssignmentLog entity) =>
        dbContext.VehicleDriverAssignmentLogs.Add(entity);

    public async Task<IEnumerable<VehicleDriverAssignmentLog>> GetByVehicleIdAsync(int vehicleId) =>
        await dbContext.VehicleDriverAssignmentLogs
            .AsNoTracking()
            .Where(l => l.VehicleId == vehicleId)
            .OrderByDescending(l => l.ChangedOn)
            .ToListAsync();
}
