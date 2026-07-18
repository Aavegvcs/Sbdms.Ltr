using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sbdms.Ltr.Infra.Data;

// Used only by `dotnet ef migrations`/`database update` at design time — the SBDMS_LTR_CONNECTION
// environment variable overrides this fallback if set.
public class LtrAppDbContextFactory : IDesignTimeDbContextFactory<LtrAppDbContext>
{
    public LtrAppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("SBDMS_LTR_CONNECTION")
            ?? "Server=103.208.202.180;Database=AdaniDb;User Id=tenant-admin;Password=tenantadmin@1;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<LtrAppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new LtrAppDbContext(optionsBuilder.Options);
    }
}
