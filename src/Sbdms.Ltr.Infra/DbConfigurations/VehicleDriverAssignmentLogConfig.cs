using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class VehicleDriverAssignmentLogConfig : IEntityTypeConfiguration<VehicleDriverAssignmentLog>
{
    public void Configure(EntityTypeBuilder<VehicleDriverAssignmentLog> builder)
    {
        builder.ToTable("VehicleDriverAssignmentLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VehicleNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.VehicleCompany).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Modal).HasMaxLength(100).IsRequired();

        builder.Property(x => x.OldDriverName).HasMaxLength(100);
        builder.Property(x => x.OldDriverNumber).HasMaxLength(20);
        builder.Property(x => x.OldLicenceNumber).HasMaxLength(50);

        builder.Property(x => x.ChangedOn).IsRequired();

        builder.HasIndex(x => x.VehicleId);

        // Deliberately no FK constraints to Vehicle/Driver/Vendor — this is an immutable audit
        // trail and must still read back correctly even if those rows are later changed or removed.
    }
}
