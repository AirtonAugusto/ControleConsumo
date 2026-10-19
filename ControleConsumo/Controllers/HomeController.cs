using ControleConsumo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleConsumo.Controllers
{
    public class HomeController : Controller
    {
        // A página inicial leva direto para a lista de veículos (aula 16)
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Veiculos");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
