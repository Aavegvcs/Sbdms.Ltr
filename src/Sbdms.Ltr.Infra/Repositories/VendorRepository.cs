using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class VendorRepository(LtrAppDbContext dbContext) : IVendorRepository
{
    public Task<Result<bool>> AddAsync(Vendor entity)
    {
        dbContext.Vendors.Add(entity);
        return Task.FromResult<Result<bool>>(true);
    }

    public Task<Result<bool>> UpdateAsync(Vendor entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            dbContext.Vendors.Update(entity);

        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<Vendor>> FindByIdAsync(int id)
    {
        var vendor = await dbContext.Vendors.FirstOrDefaultAsync(v => v.Id == id);
        if (vendor is null)
            return VendorErrors.VendorNotFound;

        return vendor;
    }

    public IQueryable<Vendor> GetAllAsQueryable() => dbContext.Vendors.AsNoTracking();

    public async Task<IEnumerable<Vendor>> GetAllAsync() =>
        await dbContext.Vendors.AsNoTracking().ToListAsync();

    public async Task<Vendor?> GetByAsync(Expression<Func<Vendor, bool>> predicate) =>
        await dbContext.Vendors.FirstOrDefaultAsync(predicate);
}
