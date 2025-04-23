using BookApp.Data.EF.Access.Services;
using BookApp.Domain.Services;

namespace BookApp.Startup_Configuration;

public static class ServicesExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBooksService, BooksService>();
        return services;
    }
}