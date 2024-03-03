using DataLayer;
using DescartesAssignment.Interfaces;
using DescartesAssignment.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<Database>();
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddScoped<IBusinessLogic, BusinessLogic>();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSingleton<ILogger<BusinessLogic>, Logger<BusinessLogic>>();
builder.Services.AddSingleton<ILogger<IDataAccess>, Logger<DataAccess>>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Descartes Assignment", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Descartes Assignment V1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
