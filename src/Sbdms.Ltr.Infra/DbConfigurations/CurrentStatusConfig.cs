using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class CurrentStatusConfig : IEntityTypeConfiguration<CurrentStatus>
{
    public void Configure(EntityTypeBuilder<CurrentStatus> builder)
    {
        builder.ToTable("CurrentStatus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CurrentStatusId).IsRequired();
        builder.HasIndex(x => x.CurrentStatusId).IsUnique();

        builder.Property(x => x.StatusName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.StatusName).IsUnique();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ModBy)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedOn).IsRequired();
    }
}
