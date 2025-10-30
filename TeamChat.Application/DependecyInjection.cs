using TeamChat.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using TeamChat.Application.Abstraction.Services;

namespace TeamChat.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanyService, CompanyService>();
        return services;
    }
}