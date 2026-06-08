using Fulbacho.Application.Hubs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Application.Modules.B2C.Patterns.Observer;
using Fulbacho.Application.Modules.B2C.Patterns.Strategy;
using Fulbacho.Application.Modules.B2C.Services;
using Fulbacho.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Base de Datos
builder.Services.AddDbContext<FulbachoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de Servicios B2C
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IPredioService, PredioService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();

// Patrón Strategy — Matchmaking
builder.Services.AddScoped<IMatchmakingStrategy, MatchmakingPorNivel>();
builder.Services.AddScoped<MotorMatchmaking>();

// Patrón Observer — Desafíos
builder.Services.AddScoped<IDesafioService, DesafioService>();
builder.Services.AddScoped<SignalRObserver>();
builder.Services.AddScoped<ReservaObserver>();

// Configuración de Autenticación JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontendNext", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();

// Swagger - Para probar endpoints protegidos, usar Postman con header:
// Authorization: Bearer {token_obtenido_de_/api/b2c/autenticacion/login}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PermitirFrontendNext");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<FulbachoHub>("/hubs/fulbacho");
app.Run();
