using Microsoft.OpenApi.Models;
using ApiEstudiantes.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiEstudiantes", Version = "v1" });
});

// --- Configuración de la conexión a MySQL ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ConexionMySQL"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ConexionMySQL"))
    )
);

// --- Agregar controladores ---
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
    );

// --- CORS para permitir llamadas desde el Frontend (http://localhost:3000) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
        policy.WithOrigins("http://localhost:3000")   // agrega otro origin si lo necesitas
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

var app = builder.Build();

// --- Activar Swagger SOLO en entorno de desarrollo ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// *** IMPORTANTE: CORS ANTES DE MapControllers ***
app.UseCors("frontend");

app.MapControllers();

app.Run();
