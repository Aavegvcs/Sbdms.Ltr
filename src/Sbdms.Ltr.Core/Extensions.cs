using Microsoft.Extensions.DependencyInjection;
using Sbdms.Ltr.Core.Feature.Bookings;
using Sbdms.Ltr.Core.Feature.CurrentStatuses;
using Sbdms.Ltr.Core.Feature.Drivers;
using Sbdms.Ltr.Core.Feature.Users;
using Sbdms.Ltr.Core.Feature.VehicleTypes;
using Sbdms.Ltr.Core.Feature.Vehicles;
using Sbdms.Ltr.Core.Feature.Vendors;

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
            .AddScoped<RegenerateVehicleQrCodeHandler>()
            .AddScoped<ChangeVehicleDriverHandler>()
            .AddScoped<GetVehicleDriverHistoryHandler>()
            .AddScoped<UpdateVehicleLocationHandler>()
            .AddScoped<GetVehicleLocationHandler>()
            .AddScoped<GetVehicleLocationHistoryHandler>()
            .AddScoped<AddVendorHandler>()
            .AddScoped<UpdateVendorHandler>()
            .AddScoped<GetAllVendorsHandler>()
            .AddScoped<GetVendorByIdHandler>()
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
            .AddScoped<GetUserByMobileNumberHandler>()
            .AddScoped<LoginOrRegisterHandler>()
            .AddScoped<GuestStartBookingHandler>()
            .AddScoped<CreateBookingHandler>()
            .AddScoped<GetAllBookingsHandler>()
            .AddScoped<GetBookingByIdHandler>()
            .AddScoped<GetLatestBookingByUserHandler>()
            .AddScoped<GetBookingHistoryByUserHandler>()
            .AddScoped<UpdateVehicleLocationHandler>()
            .AddScoped<BulkUpdateVehicleLocationHandler>()
            .AddScoped<CompleteBookingHandler>();

        return services;
    }
}

//dotnet ef migrations add DriverModal -p Sbdms.Ltr.Infra -s Sbdms.Ltr.Api -o Data/Migrations
//dotnet ef database update -p Sbdms.Ltr.Infra -s Sbdms.Ltr.Api