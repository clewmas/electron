

using elec.Logging;
using elec.Services;

namespace elec.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddSingleton(typeof(elec.Logging.ILogger<>), typeof(Log4Net<>));
        
        services.AddScoped<IMenuService, MenuService>();
        
        services.AddSingleton<IControllerExceptionHandler, ControllerExceptionHandler>();

        return services;

    }
}
