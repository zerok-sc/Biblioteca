using Microsoft.AspNetCore.Authentication.Cookies;
using presentacion_libreria.implementaciones;
using presentacion_libreria.interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<IComunicaciones, Comunicaciones>();
builder.Services.AddScoped(typeof(IPresentacion<>), typeof(Presentacion<>));
builder.Services.AddScoped<IAutenticacion, Autenticacion>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opciones =>
    {
        opciones.LoginPath = "/Login";
        opciones.AccessDeniedPath = "/AccesoDenegado";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
