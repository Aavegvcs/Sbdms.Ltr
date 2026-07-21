using System.Linq.Expressions;
using Sbdms.Ltr.Core.Domain;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Interface;

// Booking uses a plain int identity key, matching Vehicle/Driver/User's convention.
public interface IBookingRepository
{
    Task<Result<bool>> AddAsync(Booking entity);
    Task<Result<Booking>> FindByIdAsync(int id);
    IQueryable<Booking> GetAllAsQueryable();
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<Booking?> GetByAsync(Expression<Func<Booking, bool>> predicate);

    // The trip "head" (TripId == null) currently in progress for this vehicle, if any.
    Task<Booking?> GetActiveTripHeadAsync(int vehicleId);

    // Every booking belonging to the trip headed by headId — the head itself plus all co-riders.
    Task<IEnumerable<Booking>> GetTripMembersAsync(int headId);

    // The most recent booking made by this user, tracked (so stale-trip reconciliation can be
    // saved through the same instance without needing a re-fetch).
    Task<Booking?> GetLatestByUserAsync(int userId);
}
