using Microsoft.Extensions.DependencyInjection;
using Sbdms.Ltr.Core.Feature.Bookings;
using Sbdms.Ltr.Core.Feature.CurrentStatuses;
using Sbdms.Ltr.Core.Feature.Drivers;
using Sbdms.Ltr.Core.Feature.Users;
using Sbdms.Ltr.Core.Feature.VehicleTypes;
using Sbdms.Ltr.Core.Feature.Vehicles;

namespace Sbdms.Ltr.Core;

public static class Extensions
{
    public static IServiceCollection AddCoreHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<AddVehicleHandler>()
            .AddScoped<UpdateVehicleHandler>()
            .AddScoped<GetAllVehiclesHandler>()
            .AddScoped<GetVehicleByIdHandler>()
            .AddScoped<GetVehicleByQrCodeHandler>()
            .AddScoped<GetVehicleQrImageHandler>()
            .AddScoped<AddVehicleTypeHandler>()
            .AddScoped<UpdateVehicleTypeHandler>()
            .AddScoped<GetAllVehicleTypesHandler>()
            .AddScoped<GetVehicleTypeByIdHandler>()
            .AddScoped<AddCurrentStatusHandler>()
            .AddScoped<UpdateCurrentStatusHandler>()
            .AddScoped<GetAllCurrentStatusesHandler>()
            .AddScoped<GetCurrentStatusByIdHandler>()
            .AddScoped<AddDriverHandler>()
            .AddScoped<UpdateDriverHandler>()
            .AddScoped<GetAllDriversHandler>()
            .AddScoped<GetDriverByIdHandler>()
            .AddScoped<AddUserHandler>()
            .AddScoped<RequestOtpHandler>()
            .AddScoped<VerifyOtpHandler>()
            .AddScoped<RefreshTokenHandler>()
            .AddScoped<GetAllUsersHandler>()
            .AddScoped<GetUserByIdHandler>()
            .AddScoped<GuestStartBookingHandler>()
            .AddScoped<CreateBookingHandler>()
            .AddScoped<GetAllBookingsHandler>()
            .AddScoped<GetBookingByIdHandler>()
            .AddScoped<GetLatestBookingByUserHandler>()
            .AddScoped<GetBookingHistoryByUserHandler>();

        return services;
    }
}

//dotnet ef migrations add DriverModal -p Sbdms.Ltr.Infra -s Sbdms.Ltr.Api -o Data/Migrations
//dotnet ef database update -p Sbdms.Ltr.Infra -s Sbdms.Ltr.Api