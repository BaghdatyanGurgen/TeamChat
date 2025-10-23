using Microsoft.Extensions.DependencyInjection;
using TeamChat.Application.Abstraction;
using TeamChat.Application.Services;

namespace TeamChat.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
