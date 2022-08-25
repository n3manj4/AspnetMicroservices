using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((hostingConfig, loggintBuilder) =>
{
	loggintBuilder.AddConfiguration(hostingConfig.Configuration.GetSection("Logging"));
	loggintBuilder.AddConsole();
	loggintBuilder.AddDebug();
});

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
	config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);

});

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

app.UseOcelot().Wait();

app.Run();
