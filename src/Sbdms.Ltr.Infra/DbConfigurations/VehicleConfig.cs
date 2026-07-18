using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QrUniqueCode)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.QrUniqueCode)
            .IsUnique();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.HasOne<VehicleType>()
            .WithMany()
            .HasForeignKey(x => x.VehicleTypeCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CurrentStatus>()
            .WithMany()
            .HasForeignKey(x => x.CurrentStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Driver>()
            .WithMany()
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
