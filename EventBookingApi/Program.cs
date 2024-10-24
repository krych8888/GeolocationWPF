using DataService.Data;
using DataService.Repository;
using DataService.Repository.Interfaces;
using Entities.Dtos.Requests;
using EventBookingApi.Validation;
using FluentValidation;
using IpStack.Extensions;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("LocalDb"))
);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGeolocationService, GeolocationService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IValidator<GetGeolocationRequest>, CreateEventRequestValidator>();

builder.Services.AddIpStack("894209c3edbcb65f2be464faa0cea0d1");
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
