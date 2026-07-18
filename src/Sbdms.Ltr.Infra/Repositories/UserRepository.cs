using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class UserRepository(LtrAppDbContext dbContext) : IUserRepository
{
    public async Task<Result<bool>> AddAsync(User entity)
    {
        var duplicate = await GetByAsync(u => u.MobileNumber == entity.MobileNumber);
        if (duplicate is not null)
            return UserErrors.DuplicateMobileNumber;

        dbContext.Users.Add(entity);
        return true;
    }

    public Task<Result<bool>> UpdateAsync(User entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            dbContext.Users.Update(entity);

        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<User>> FindByIdAsync(int id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            return UserErrors.UserNotFound;

        return user;
    }

    public IQueryable<User> GetAllAsQueryable() => dbContext.Users.AsNoTracking();

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await dbContext.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByAsync(Expression<Func<User, bool>> predicate) =>
        await dbContext.Users.FirstOrDefaultAsync(predicate);
}
