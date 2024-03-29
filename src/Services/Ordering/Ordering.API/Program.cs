using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//General Configuration
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());
builder.Services.AddScoped<BasketCheckoutConsumer>();

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config => {

	config.AddConsumer<BasketCheckoutConsumer>();

	config.UsingRabbitMq((ctx, cfg) => {
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

		cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
		{
			c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
		});
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MigrateDatabase<OrderContext>((context, service) =>
{
	var logger = app.Services.GetService<ILogger<OrderContextSeed>>();
	OrderContextSeed.SeedAsync(context, logger!).Wait();
}); 

app.UseAuthorization();

app.MapControllers();

app.Run();
