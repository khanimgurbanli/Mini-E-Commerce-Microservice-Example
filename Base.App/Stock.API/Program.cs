using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(confiurator =>
{
    confiurator.AddConsumer<OrderCreatedEventConsumer>();
    confiurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["Secret-Key"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.OrderCreatedEventQueue_Stock, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDbService>();

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDbService mongoDbService = scope.ServiceProvider.GetService<MongoDbService>();
var collection = mongoDbService.GetCollection<Stock.API.Entities.Stock>();

//for first time
if (!collection.Find(x => true).Any())
{
    await collection.InsertOneAsync(new() { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Couunt = 200 });
    await collection.InsertOneAsync(new() { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Couunt = 300 });
    await collection.InsertOneAsync(new() { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Couunt = 400 });
    await collection.InsertOneAsync(new() { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Couunt = 500 });
    await collection.InsertOneAsync(new() { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Couunt = 600 });
}

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
