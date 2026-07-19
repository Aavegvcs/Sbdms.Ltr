using System.Linq.Expressions;
using Sbdms.Ltr.Core.Domain;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Interface;

// Driver uses a plain int identity key (no Guid surrogate), matching Vehicle's convention —
// hence this bespoke interface rather than the shared Guid-based IGenericInterface<T>.
public interface IDriverRepository
{
    Task<Result<bool>> AddAsync(Driver entity);
    Task<Result<bool>> UpdateAsync(Driver entity);
    Task<Result<Driver>> FindByIdAsync(int id);
    IQueryable<Driver> GetAllAsQueryable();
    Task<IEnumerable<Driver>> GetAllAsync();
    Task<Driver?> GetByAsync(Expression<Func<Driver, bool>> predicate);
}
