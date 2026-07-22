using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class VehicleLocationHistoryConfig : IEntityTypeConfiguration<VehicleLocationHistory>
{
    public void Configure(EntityTypeBuilder<VehicleLocationHistory> builder)
    {
        builder.ToTable("VehicleLocationHistories");

        builder.HasKey(x => x.Id);

        // Not unique — every report gets its own row here, unlike VehicleLocations.
        builder.HasIndex(x => new { x.VehicleId, x.RecordedOn });

        builder.Property(x => x.Latitude).HasColumnType("decimal(9,6)").IsRequired();
        builder.Property(x => x.Longitude).HasColumnType("decimal(9,6)").IsRequired();
        builder.Property(x => x.RecordedOn).IsRequired();

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
