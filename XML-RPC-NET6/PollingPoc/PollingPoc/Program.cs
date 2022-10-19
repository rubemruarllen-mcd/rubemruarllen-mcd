using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using PollingPoc.Domain.Interfaces;
using PollingPoc.Services;
using PollingPoc.Services.Api;
using PollingPoc.Swagger;
using Refit;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using NATS.Client;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var wayStationBase = builder.Configuration.GetSection("WayStationBaseAddress").Value;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamples();
builder.Services.AddSwaggerGen(ConfigureSwaggerGen);
//builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddRefitClient<IXmlRPCApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(wayStationBase));
builder.Services.AddSingleton<IXmlRPC, XmlRPC>();
builder.Services.AddSingleton(new ConnectionFactory());
builder.Services.AddSingleton<INatsConnectionFactory, NatsConnectionFactory>();
builder.Services.AddSingleton<IMessageBrokerFactory, MessageBrokerFactory>();

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("v1/swagger.json", "Identity Server API");
    });
}

app.UseHttpsRedirection();

app.UseCors(x =>
{
    x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});


app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureSwaggerGen(SwaggerGenOptions options)
{
    var projectAssemblyName = builder.Environment.ApplicationName;
    var documentationFilePath = Path.Combine(AppContext.BaseDirectory, $"{projectAssemblyName}.xml");

    options.OperationFilter<AddHeaderParameters>();

    options.CustomSchemaIds(type => type.ToString());

    options.DocumentFilter<SwaggerDocumentFilter>();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stld Integration Api",
        Version = "v1.0",
        Description = "Stld Api responsible for generating the Stld Xml and being consumed by clients and XML RPC"
    });


    options.ExampleFilters();
}
