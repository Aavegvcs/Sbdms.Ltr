using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class VehicleTypeRepository(LtrAppDbContext dbContext) : IVehicleTypeRepository
{
    public async Task<Result<bool>> AddAsync(VehicleType entity)
    {
        var duplicate = await GetByAsync(vt => vt.VehicleTypeDesc == entity.VehicleTypeDesc);
        if (duplicate is not null)
            return VehicleTypeErrors.DuplicateVehicleTypeDesc;

        dbContext.VehicleTypes.Add(entity);
        return true;
    }

    public Task<Result<bool>> UpdateAsync(VehicleType entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            dbContext.VehicleTypes.Update(entity);

        return Task.FromResult<Result<bool>>(true);
    }

    public Task<Result<bool>> DeleteAsync(VehicleType entity)
    {
        dbContext.VehicleTypes.Remove(entity);
        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<VehicleType>> FindByIdAsync(Guid id)
    {
        var vehicleType = await dbContext.VehicleTypes.FirstOrDefaultAsync(vt => vt.VehicleTypeId == id);
        if (vehicleType is null)
            return VehicleTypeErrors.VehicleTypeNotFound;

        return vehicleType;
    }

    public IQueryable<VehicleType> GetAllAsQueryable() => dbContext.VehicleTypes.AsNoTracking();

    public async Task<IEnumerable<VehicleType>> GetAllAsync() =>
        await dbContext.VehicleTypes.AsNoTracking().ToListAsync();

    public async Task<VehicleType> GetByAsync(Expression<Func<VehicleType, bool>> predicate) =>
        await dbContext.VehicleTypes.FirstOrDefaultAsync(predicate) ?? null!;
}
