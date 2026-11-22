using aplicaciones_libreria.implementaciones;
using aplicaciones_libreria.interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cadena = builder.Configuration.GetConnectionString("Biblioteca")
    ?? "server=localhost;database=biblioteca_db;Integrated Security=True;TrustServerCertificate=true;";

builder.Services.AddDbContext<Conexion>(opciones => opciones.UseSqlServer(cadena));
builder.Services.AddScoped<IConexion>(proveedor => proveedor.GetRequiredService<Conexion>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
