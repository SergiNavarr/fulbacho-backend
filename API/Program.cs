using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Application.Modules.B2C.Services;
using Fulbacho.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Base de Datos y Servicios
builder.Services.AddDbContext<FulbachoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IPredioService, IPredioService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontendNext", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader()                     
              .AllowAnyMethod();                    
    });
});

builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirFrontendNext");

app.UseAuthorization();

app.MapControllers();

app.Run();