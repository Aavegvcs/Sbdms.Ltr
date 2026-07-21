using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class BookingConfig : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Purpose).HasMaxLength(200);

        builder.Property(x => x.VehicleNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Modal)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DriverNumber).HasMaxLength(20);
        builder.Property(x => x.DriverName).HasMaxLength(100);

        builder.Property(x => x.PickLatitude).HasColumnType("decimal(9,6)");
        builder.Property(x => x.PickLongitude).HasColumnType("decimal(9,6)");
        builder.Property(x => x.DropLatitude).HasColumnType("decimal(9,6)");
        builder.Property(x => x.DropLongitude).HasColumnType("decimal(9,6)");

        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired();
        builder.Property(x => x.BookedOn).IsRequired();
        builder.Property(x => x.LastActivityOn).IsRequired();

        builder.HasIndex(x => new { x.VehicleId, x.TripId, x.Status });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Self-referencing: for the head/first rider, TripId equals its own Id; for a pooled
        // co-rider, it points at the head's Id instead.
        builder.HasOne<Booking>()
            .WithMany()
            .HasForeignKey(x => x.TripId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
