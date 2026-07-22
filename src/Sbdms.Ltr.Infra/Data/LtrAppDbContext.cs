using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.Data;

public class LtrAppDbContext(DbContextOptions<LtrAppDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<CurrentStatus> CurrentStatuses => Set<CurrentStatus>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<VehicleDriverAssignmentLog> VehicleDriverAssignmentLogs => Set<VehicleDriverAssignmentLog>();
    public DbSet<VehicleLocation> VehicleLocations => Set<VehicleLocation>();
    public DbSet<VehicleLocationHistory> VehicleLocationHistories => Set<VehicleLocationHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LtrAppDbContext).Assembly);
    }
}
