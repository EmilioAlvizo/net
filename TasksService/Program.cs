using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using System.Text;
using Scalar.AspNetCore;

using TasksService.Data;
using TasksService.Repositories;
using TasksService.Repositories.Interfaces;
using TasksService.Services;

Env.Load(); // carga .env si existe
Console.WriteLine($"JWT Secret cargado: {(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SUPABASE_JWT_SECRET")) ? "VACÍO ❌" : "OK ✅")}");

var builder = WebApplication.CreateBuilder(args);

// Lee desde variables de entorno (que .env inyectó)
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")!;
var supabaseUrl      = Environment.GetEnvironmentVariable("SUPABASE_URL")!;
var jwtSecret        = Environment.GetEnvironmentVariable("SUPABASE_JWT_SECRET")!;
var anonKey          = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY")!;

// ── Repositorios ──────────────────────────────────────────────
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IGranjaRepository, GranjaRepository>();

// ── Servicios ─────────────────────────────────────────────────
builder.Services.AddSingleton(new SupabaseConfig(supabaseUrl, anonKey));
builder.Services.AddHttpClient<SupabaseAuthService>();

// ── Controllers ───────────────────────────────────────────────
builder.Services.AddControllers();

// ── OpenAPI (para Scalar) ──────────────────────────────────────
builder.Services.AddOpenApi();

// ── Base de datos (EF Core + Npgsql → Supabase) ───────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ── Autenticación JWT ─────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer   = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// ─────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Pipeline ──────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // disponible en /scalar/v1
}

app.UseHttpsRedirection();
app.UseAuthentication(); // primero autenticación
app.UseAuthorization();  // luego autorización — el orden importa
app.MapControllers();

app.Run();