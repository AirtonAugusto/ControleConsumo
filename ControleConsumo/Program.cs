using System.Globalization;
using ControleConsumo.Models;
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

var app = builder.Build();

// 2) CAMINHO DO PEDIDO: por onde cada requisição passa
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();