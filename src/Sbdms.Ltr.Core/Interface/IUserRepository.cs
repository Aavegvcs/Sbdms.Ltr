using System.Linq.Expressions;
using Sbdms.Ltr.Core.Domain;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Interface;

// User uses a plain int identity key (no Guid surrogate), matching Vehicle/Driver's convention —
// hence this bespoke interface rather than the shared Guid-based IGenericInterface<T>.
public interface IUserRepository
{
    Task<Result<bool>> AddAsync(User entity);
    Task<Result<bool>> UpdateAsync(User entity);
    Task<Result<User>> FindByIdAsync(int id);
    IQueryable<User> GetAllAsQueryable();
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByAsync(Expression<Func<User, bool>> predicate);
}
