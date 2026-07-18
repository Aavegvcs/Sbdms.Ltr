using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class DriverConfig : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DriverName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DriverNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.LicenceNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.LicenceNumber)
            .IsUnique();

        builder.Property(x => x.Dob).IsRequired();
        builder.Property(x => x.CreatedOn).IsRequired();

        builder.HasOne<CurrentStatus>()
            .WithMany()
            .HasForeignKey(x => x.CurrentStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
