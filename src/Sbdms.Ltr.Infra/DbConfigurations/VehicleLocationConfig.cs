using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class VehicleLocationConfig : IEntityTypeConfiguration<VehicleLocation>
{
    public void Configure(EntityTypeBuilder<VehicleLocation> builder)
    {
        builder.ToTable("VehicleLocations");

        builder.HasKey(x => x.Id);

        // One current-location row per vehicle — every report upserts this row rather than
        // inserting a new one, so lookups stay O(1) regardless of report frequency.
        builder.HasIndex(x => x.VehicleId).IsUnique();

        builder.Property(x => x.Latitude).HasColumnType("decimal(9,6)").IsRequired();
        builder.Property(x => x.Longitude).HasColumnType("decimal(9,6)").IsRequired();
        builder.Property(x => x.RecordedOn).IsRequired();
        builder.Property(x => x.CreatedOn).IsRequired();

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
