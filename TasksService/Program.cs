using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;
using Scalar.AspNetCore;
using TasksService.Data;
using TasksService.Services;

var builder = WebApplication.CreateBuilder(args);

// Repositorios
//builder.Services.AddScoped<IEjemplarRepository, EjemplarRepository>();

// Servicios
//builder.Services.AddScoped<IEjemplarService, EjemplarService>();

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient<SupabaseAuthService>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Esta es mi api",
            Version = "v1",
            Description = "Is Swagger dead in .NET? With the release of .NET 9 and .NET 10, Microsoft has officially removed Swashbuckle (Swagger) from the default project templates — although you can still use it by manually adding the NuGet package. Now, it's time to explore the new alternative: Scalar — a modern, high-performance solution for API documentation. In this video, we’ll transform a raw OpenAPI JSON specification into a stunning, interactive, Stripe-level documentation UI using Scalar — complete with built-in dark mode and a professional API playground."
        };
        return Task.CompletedTask;
    });
});
/* builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EstaEsMi API",
        Version = "v1",
        Description = "API para gestión de clientes y tareas",
        Contact = new OpenApiContact
        {
            Name = "Emi Alvizo",
            Email = "emi@taskflow.com"
        }
    });

    // Agrega el botón Authorize en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        In           = ParameterLocation.Header,
        Description  = "Escribe tu token aquí"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

    // Activa los comentarios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
}); */

//esto es para el primer endpoint que hice
/* builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); */
    
// (PostgreSQL / Supabase)
builder.Services.AddDbContext<GranjaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Base de datos (SQL Server)
/* builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 */
// JWT
var secretKey = builder.Configuration["Jwt:SecretKey"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(secretKey))
        };
    });
    
builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    //scalar
    app.MapOpenApi();
    app.MapScalarApiReference();

    // swagger
    /* app.UseSwagger();
    app.UseSwaggerUI(); */
}

app.UseHttpsRedirection();
app.UseAuthentication(); // primero autenticación
app.UseAuthorization();  // luego autorización — el orden importa
app.MapControllers();
app.Run();