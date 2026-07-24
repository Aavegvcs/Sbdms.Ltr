using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class VehicleRepository(LtrAppDbContext dbContext) : IVehicleRepository
{
    public async Task<Result<bool>> AddAsync(Vehicle entity)
    {
        var duplicate = await GetByAsync(v => v.QrUniqueCode == entity.QrUniqueCode);
        if (duplicate is not null)
            return VehicleErrors.DuplicateQrCode;

        dbContext.Vehicles.Add(entity);
        return true;
    }



    public Task<Result<bool>> UpdateAsync(Vehicle entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            dbContext.Vehicles.Update(entity);

        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<Vehicle>> FindByIdAsync(int id)
    {
        var vehicle = await dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null)
            return VehicleErrors.VehicleNotFound;

        return vehicle;
    }



    public async Task<IEnumerable<Vehicle>> GetAllByAsync(Expression<Func<Vehicle, bool>> predicate)
    {
        return await dbContext.Vehicles
            .Where(predicate)
            .ToListAsync();
    }
    public IQueryable<Vehicle> GetAllAsQueryable() => dbContext.Vehicles.AsNoTracking();

    public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
        await dbContext.Vehicles.AsNoTracking().ToListAsync();

    public async Task<Vehicle?> GetByAsync(Expression<Func<Vehicle, bool>> predicate) =>
        await dbContext.Vehicles.FirstOrDefaultAsync(predicate);
}
