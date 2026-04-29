using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ApiFinanceiro.Profiles;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// É necessário adiconar um contexto de banco de dados
var connectionString = builder.Configuration.GetConnectionString("mysql");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)))
    );

// Para encerrar ciclo de busca infinita em relacionamentos
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adição de service
builder.Services.AddScoped<DespesaService>();
builder.Services.AddAutoMapper(config => config.AddProfile<DespesaProfile>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
