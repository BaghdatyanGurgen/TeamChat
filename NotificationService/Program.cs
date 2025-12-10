using NotificationService;
using NotificationService.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, builder) =>
    {
        builder.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<RabbitMQSettings>(ctx.Configuration.GetSection("RabbitMQ"));
        services.AddSingleton<RabbitMqListener>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();