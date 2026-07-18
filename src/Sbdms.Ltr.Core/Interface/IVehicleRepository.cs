using System.Linq.Expressions;
using Sbdms.Ltr.Core.Domain;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Interface;

// Vehicle uses a plain int identity key (no Guid surrogate), so it can't implement the
// shared IGenericInterface<T> (its FindByIdAsync is Guid-only) — hence this bespoke interface.
public interface IVehicleRepository
{
    Task<Result<bool>> AddAsync(Vehicle entity);
    Task<Result<bool>> UpdateAsync(Vehicle entity);
    Task<Result<Vehicle>> FindByIdAsync(int id);
    IQueryable<Vehicle> GetAllAsQueryable();
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByAsync(Expression<Func<Vehicle, bool>> predicate);
}
