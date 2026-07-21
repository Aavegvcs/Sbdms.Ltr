using System.Linq.Expressions;
using Sbdms.Ltr.Core.Domain;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Interface;

// Vendor uses a plain int identity key (no Guid surrogate), matching Vehicle/Driver's
// convention — hence this bespoke interface rather than the shared Guid-based IGenericInterface<T>.
public interface IVendorRepository
{
    Task<Result<bool>> AddAsync(Vendor entity);
    Task<Result<bool>> UpdateAsync(Vendor entity);
    Task<Result<Vendor>> FindByIdAsync(int id);
    IQueryable<Vendor> GetAllAsQueryable();
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task<Vendor?> GetByAsync(Expression<Func<Vendor, bool>> predicate);
}
