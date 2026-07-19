using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Infra.Repositories;

public class BookingRepository(LtrAppDbContext dbContext) : IBookingRepository
{
    public Task<Result<bool>> AddAsync(Booking entity)
    {
        dbContext.Bookings.Add(entity);
        return Task.FromResult<Result<bool>>(true);
    }

    public async Task<Result<Booking>> FindByIdAsync(int id)
    {
        var booking = await dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking is null)
            return BookingErrors.BookingNotFound;

        return booking;
    }

    public IQueryable<Booking> GetAllAsQueryable() => dbContext.Bookings.AsNoTracking();

    public async Task<IEnumerable<Booking>> GetAllAsync() =>
        await dbContext.Bookings.AsNoTracking().ToListAsync();

    public async Task<Booking?> GetByAsync(Expression<Func<Booking, bool>> predicate) =>
        await dbContext.Bookings.FirstOrDefaultAsync(predicate);

    public async Task<Booking?> GetActiveTripHeadAsync(int vehicleId) =>
        await dbContext.Bookings
            .Where(b => b.VehicleId == vehicleId && b.TripId == null && b.Status == BookingStatus.Started)
            .OrderByDescending(b => b.LastActivityOn)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Booking>> GetTripMembersAsync(int headId) =>
        await dbContext.Bookings
            .Where(b => b.Id == headId || b.TripId == headId)
            .ToListAsync();
}
