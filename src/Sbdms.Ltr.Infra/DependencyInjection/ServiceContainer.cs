using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sbdms.Ltr.Core.Interface;
using Sbdms.Ltr.Infra.Common;
using Sbdms.Ltr.Infra.Data;
using Sbdms.Ltr.Infra.Persistence;
using Sbdms.Ltr.Infra.Repositories;
using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Infra.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<LtrAppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
        services.AddScoped<ICurrentStatusRepository, CurrentStatusRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
