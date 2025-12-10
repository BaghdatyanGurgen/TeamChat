//using TeamChat.Infrastructure.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamChat.Application.Abstraction.Infrastructure.Email;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.Abstraction.Infrastructure.Security;
using TeamChat.Contracts.Grpc;
using TeamChat.Infrastructure.Email;
using TeamChat.Infrastructure.File;
using TeamChat.Infrastructure.Messaging;
using TeamChat.Infrastructure.Persistance;
using TeamChat.Infrastructure.Persistance.Repositories;
using TeamChat.Infrastructure.Security.Jwt;
using TeamChat.Infrastructure.Security.RefreshToken;

namespace TeamChat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistanceInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                       .AddScoped<IUserRepository, UserRepository>()
                       .AddScoped<ICompanyRepository, CompanyRepository>()
                       .AddScoped<ICompanyUserRepository, CompanyUserRepository>()
                       .AddScoped<IPositionRepository, PositionRepository>()
                       .AddScoped<IChatRepository, ChatRepository>()
                       .AddScoped<IChatMemberRepository, ChatMemberRepository>()
                       .AddScoped<ITeamRepository, TeamRepository>()
                       .AddScoped<IMessageRepository, MessageRepository>()
                       .AddScoped<IDepartmentRepository, DepartmentRepository>();

        public static IServiceCollection AddMessagingInfrastructure(this IServiceCollection services)
            => services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        public static IServiceCollection AddEmailInfrastructure(this IServiceCollection services)
            => services.AddScoped<IEmailSender, SmtpEmailSender>();


        public static IServiceCollection AddSecurityInfrastructure(this IServiceCollection services)
            => services.AddScoped<IJwtTokenService, JwtTokenService>()
                       .AddScoped<IRefreshTokenService, RefreshTokenService>();

        public static IServiceCollection AddFileInfrestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<FileService.FileServiceClient>(o =>
            {
                o.Address = new Uri(configuration["FileServiceUrl"]!);
            });

            services.AddScoped<IFileService, GrpcFileServiceAdapter>();
            return services;

        }

        public static IServiceCollection AddAllInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services.AddPersistanceInfrastructure(configuration)
                       .AddMessagingInfrastructure()
                       .AddEmailInfrastructure()
                       .AddSecurityInfrastructure()
                       .AddFileInfrestructure(configuration);
    }
}
