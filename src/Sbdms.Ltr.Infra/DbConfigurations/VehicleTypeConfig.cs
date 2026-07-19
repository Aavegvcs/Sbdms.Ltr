using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class VehicleTypeConfig : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.ToTable("VehicleType");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("VehicleTypeCode").ValueGeneratedOnAdd();

        builder.Property(x => x.VehicleTypeId).IsRequired();
        builder.HasIndex(x => x.VehicleTypeId).IsUnique();

        builder.Property(x => x.VehicleTypeDesc)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ModBy)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedOn).IsRequired();
    }
}
