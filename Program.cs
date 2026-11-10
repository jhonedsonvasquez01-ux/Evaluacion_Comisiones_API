using Microsoft.EntityFrameworkCore;
using evaluacion_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conectar con SQL Server
builder.Services.AddDbContext<Base_de_datos>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionDb")
    )
);

var app = builder.Build();

// Habilitar Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();