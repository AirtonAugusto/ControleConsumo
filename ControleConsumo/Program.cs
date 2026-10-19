using System.Globalization;
using ControleConsumo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Padrão brasileiro (vírgula decimal, R$, datas dd/MM/yyyy)
var culturaBr = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culturaBr;
CultureInfo.DefaultThreadCurrentUICulture = culturaBr;

// 1) SERVIÇOS: o que o sistema tem disponível
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticação por cookie (aula 14)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/AccessDenied";
    });

var app = builder.Build();

// Cria o usuário administrador inicial (admin / 123456) se ainda não existir nenhum usuário
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (!db.Usuarios.Any())
        {
            db.Usuarios.Add(new Usuario
            {
                Nome = "admin",
                Senha = BCrypt.Net.BCrypt.HashPassword("123456"),
                Perfil = Perfil.Admin
            });
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Não foi possível criar o usuário admin. Rode o Update-Database. " + ex.Message);
    }
}

// 2) CAMINHO DO PEDIDO: por onde cada requisição passa
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   // quem é você? (login)
app.UseAuthorization();    // o que você pode fazer? (perfil)

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
