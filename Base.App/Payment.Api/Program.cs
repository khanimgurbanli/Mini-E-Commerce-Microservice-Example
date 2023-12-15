using MassTransit;
using Payment.Api.Consumers;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(confiurator =>
{
    confiurator.AddConsumer<StockReservedEventConsumer>();
    confiurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["Secret-Key"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.StockReservedEventQueue_Payment, e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();