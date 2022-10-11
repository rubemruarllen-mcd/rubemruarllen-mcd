using Microsoft.Extensions.Configuration;
using PollingPoc.Domain.Interfaces;
using PollingPoc.Services;
using PollingPoc.Services.Api;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var wayStationBase = builder.Configuration.GetSection("WayStationBaseAddress").Value;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefitClient<IXmlRPCApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(wayStationBase));
builder.Services.AddSingleton<IXmlRPC, XmlRPC>();

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
