var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GrpcFileService>();
app.MapGet("/", () => "TeamChat File Service");

app.Run();
