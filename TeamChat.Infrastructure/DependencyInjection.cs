using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamChat.Domain.Interfaces;
using TeamChat.Infrastructure.Email;
using TeamChat.Infrastructure.Persistence;
using TeamChat.Infrastructure.Persistence.Repositories;

namespace TeamChat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEmailSender, SmtpEmailSender>();

            // Repositories 
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
