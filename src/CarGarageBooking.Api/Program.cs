// In Program.cs
using MongoDB.Driver;
using CarGarageBooking.Infrastructure.EventStore;
using CarGarageBooking.Domain.Interfaces;
using CarGarageBooking.Infrastructure.Configuration;

MongoDbConfiguration.ConfigureMongoDbSerializers();

var builder = WebApplication.CreateBuilder(args);

// MongoDB Configuration
// Existing configuration
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("CarGarageBookingDb");
});


// Register Event Store
builder.Services.AddScoped<IEventStore, MongoDbEventStore>();

// Standard API setup
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();