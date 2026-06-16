using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Profiles;
using ApiFinanceiro.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// É necessário adiconar um contexto de banco de dados
var connectionString = builder.Configuration.GetConnectionString("mysql");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)))
    .UseSnakeCaseNamingConvention()
    );

// Para encerrar ciclo de busca infinita em relacionamentos
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

/**
 * Configuração de Versionamento de API
 */
builder.Services.AddApiVersioning(options =>
{
    // Versão padrão quando nenhuma é especificada (Fallback)
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);

    // Usa automaticamente a DefaultApiVersion se o cliente não fornecer uma
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Retorna as versões disponíveis/obsoletas nos cabeçalhos de resposta (api-supported-versions)
    options.ReportApiVersions = true;

    // Configura como a versão é lida da requisição (a partir do segmento da URL)
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    // Formata a string de versão como 'v1', 'v2', etc.
    options.GroupNameFormat = "'v'VVV";

    // Substitui o parâmetro de rota '{version:apiVersion}' automaticamente
    options.SubstituteApiVersionInUrl = true;
});

/**
 * Config. de Verificação de Autenticação - JWT
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Financeira",
        Description = "API de consumo para aplicação Financeira"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "API Financeira - V2",
        Description = "API de consumo para aplicação Financeira"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Cabeçalho de autorização JWT. Usando esquema de Bearer \r\n\r\n Digite 'Bearer + token'. Ex.: 'Bearer 123...'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
         new OpenApiSecurityScheme
         {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
         },
         Array.Empty<string>()
        }
    });
});

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
