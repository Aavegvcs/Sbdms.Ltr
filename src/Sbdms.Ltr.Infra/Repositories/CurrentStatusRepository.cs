using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class CurrentStatusRepository(LtrAppDbContext dbContext) : ICurrentStatusRepository
{
    public async Task<Result<bool>> AddAsync(CurrentStatus entity)
    {
        var duplicate = await GetByAsync(cs => cs.StatusName == entity.StatusName);
        if (duplicate is not null)
            return CurrentStatusErrors.DuplicateStatusName;

        dbContext.CurrentStatuses.Add(entity);
        return true;
    }

    public Task<Result<bool>> UpdateAsync(CurrentStatus entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            dbContext.CurrentStatuses.Update(entity);

        return Task.FromResult<Result<bool>>(true);
    }

    public Task<Result<bool>> DeleteAsync(CurrentStatus entity)
    {
        dbContext.CurrentStatuses.Remove(entity);
        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<CurrentStatus>> FindByIdAsync(Guid id)
    {
        var currentStatus = await dbContext.CurrentStatuses.FirstOrDefaultAsync(cs => cs.CurrentStatusId == id);
        if (currentStatus is null)
            return CurrentStatusErrors.CurrentStatusNotFound;

        return currentStatus;
    }

    public IQueryable<CurrentStatus> GetAllAsQueryable() => dbContext.CurrentStatuses.AsNoTracking();

    public async Task<IEnumerable<CurrentStatus>> GetAllAsync() =>
        await dbContext.CurrentStatuses.AsNoTracking().ToListAsync();

    public async Task<CurrentStatus> GetByAsync(Expression<Func<CurrentStatus, bool>> predicate) =>
        await dbContext.CurrentStatuses.FirstOrDefaultAsync(predicate) ?? null!;
}
