using DataLayer;
using DescartesAssignment.Interfaces;
using DescartesAssignment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<Database>();
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddScoped<IBusinessLogic, BusinessLogic>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
