using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((hostingConfig, loggintBuilder) =>
{
	loggintBuilder.AddConfiguration(hostingConfig.Configuration.GetSection("Logging"));
	loggintBuilder.AddConsole();
	loggintBuilder.AddDebug();
});

builder.Services.AddOcelot();

var app = builder.Build();

app.UseOcelot().Wait();

app.Run();
