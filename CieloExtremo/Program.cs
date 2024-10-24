using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using CieloExtremo.Models;
using CieloExtremo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CieloExtremo.Data.ApplicationDbContext;
using CieloExtremo.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddEnvironmentVariables();
// Imprimir todas las variables de entorno para depuración

// Configura el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cs") ?? throw new InvalidOperationException("Connection string 'cs' not found.")));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString(css) ?? throw new InvalidOperationException("Connection string 'cs' not found.")));

// Agrega servicios al contenedor 
builder.Services.AddControllersWithViews();

// Configura autenticación y autorización
builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Cookies se enviarán solo a través de HTTPS
        options.Cookie.SameSite = SameSiteMode.None; // Ajuste según necesidades
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });
// Configuración SMTP
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

// Configura el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Asegúrate de que esto solo se aplique en producción
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Usa autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
