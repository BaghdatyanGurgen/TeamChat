using TeamChat.Domain;
using TeamChat.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using TeamChat.Infrastructure.Security.Jwt;
using Microsoft.Extensions.DependencyInjection;
using TeamChat.Infrastructure.Security.RefreshToken;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Application.Abstraction.Infrastructure.Security;
using TeamChat.Infrastructure.Persistance.Repositories;
using TeamChat.Application.Abstraction.Infrastructure.Email;
using TeamChat.Infrastructure.Persistance;

namespace TeamChat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistanceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyUserRepository, CompanyUserRepository>();

            return services;
        }

        public static IServiceCollection AddMessagingInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
            return services;
        }
        public static IServiceCollection AddEmailInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            return services;
        }
        public static IServiceCollection AddSecurityInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            return services;
        }
        public static IServiceCollection AddAllInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistanceInfrastructure(configuration);
            services.AddMessagingInfrastructure();
            services.AddEmailInfrastructure();
            services.AddSecurityInfrastructure();
            return services;
        }
    }
}
