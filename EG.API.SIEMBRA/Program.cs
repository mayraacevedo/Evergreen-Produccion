using EG.Core.API;
using EG.DAL;
using EG.DAL.Repositories;
using EG.Services.BaseSystem;
using EG.Services.MasterData;
using EG.Services.Sembrado;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<IApiVersion>(x => new ApiVersion("0.00", "1"));
builder.Services.AddScoped<IMDMService, MDMService>();
builder.Services.AddScoped<ISembradoService, SembradoService>();
builder.Services.AddScoped<ISystemService, SystemService>();

builder.Services.AddScoped<SembradoRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var defaultConectionString = builder.Configuration.GetConnectionString("EG");

builder.Services.AddDbContext<EGDbContext>(options =>
    options.UseLazyLoadingProxies(false).UseSqlServer(defaultConectionString, x => x.MigrationsHistoryTable("__MyMigrationsHistory", "EG")), ServiceLifetime.Transient
);



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
