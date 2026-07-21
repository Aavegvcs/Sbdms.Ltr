using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Infra.DbConfigurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MobileNumber)
            .HasMaxLength(15)
            .IsRequired();

        builder.HasIndex(x => x.MobileNumber).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.EmployeeCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.EmployeeCode);

        builder.Property(x => x.Otp).HasMaxLength(6);
        builder.Property(x => x.AccessToken).HasMaxLength(2000);
        builder.Property(x => x.RefreshToken).HasMaxLength(200);

        builder.HasIndex(x => x.RefreshToken);

        builder.Property(x => x.CreatedOn).IsRequired();
    }
}
