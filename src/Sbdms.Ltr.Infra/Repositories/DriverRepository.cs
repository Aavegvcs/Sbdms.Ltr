using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class DriverRepository(LtrAppDbContext dbContext) : IDriverRepository
{
    public async Task<Result<bool>> AddAsync(Driver entity)
    {
        var duplicate = await GetByAsync(d => d.LicenceNumber == entity.LicenceNumber);
        if (duplicate is not null)
            return DriverErrors.DuplicateLicenceNumber;

        dbContext.Drivers.Add(entity);
        return true;
    }

    public Task<Result<bool>> UpdateAsync(Driver entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            dbContext.Drivers.Update(entity);

        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<Driver>> FindByIdAsync(int id)
    {
        var driver = await dbContext.Drivers.FirstOrDefaultAsync(d => d.Id == id);
        if (driver is null)
            return DriverErrors.DriverNotFound;

        return driver;
    }

    public IQueryable<Driver> GetAllAsQueryable() => dbContext.Drivers.AsNoTracking();

    public async Task<IEnumerable<Driver>> GetAllAsync() =>
        await dbContext.Drivers.AsNoTracking().ToListAsync();

    public async Task<Driver?> GetByAsync(Expression<Func<Driver, bool>> predicate) =>
        await dbContext.Drivers.FirstOrDefaultAsync(predicate);
}
