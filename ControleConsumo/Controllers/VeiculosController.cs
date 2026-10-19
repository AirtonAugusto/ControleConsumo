using ControleConsumo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleConsumo.Controllers;

[Authorize]   // só usuários logados (aula 4)
public class VeiculosController : Controller
{
    private readonly AppDbContext _context;

    public VeiculosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Veiculos
    public async Task<IActionResult> Index()
    {
        return View(await _context.Veiculos.ToListAsync());
    }

    // GET: /Veiculos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var veiculo = await _context.Veiculos.FirstOrDefaultAsync(m => m.Id == id);
        if (veiculo == null) return NotFound();

        return View(veiculo);
    }

    // GET: /Veiculos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Veiculos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Placa,AnoFabricacao,AnoModelo")] Veiculo veiculo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(veiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(veiculo);
    }

    // GET: /Veiculos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null) return NotFound();

        return View(veiculo);
    }

    // POST: /Veiculos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Placa,AnoFabricacao,AnoModelo")] Veiculo veiculo)
    {
        if (id != veiculo.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(veiculo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(veiculo.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(veiculo);
    }

    // GET: /Veiculos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var veiculo = await _context.Veiculos.FirstOrDefaultAsync(m => m.Id == id);
        if (veiculo == null) return NotFound();

        return View(veiculo);
    }

    // POST: /Veiculos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo != null)
        {
            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: /Veiculos/Relatorio/5  (aula 16)
    public async Task<IActionResult> Relatorio(int? id)
    {
        if (id == null) return NotFound();

        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null) return NotFound();

        var consumos = await _context.Consumos
            .Where(c => c.VeiculoId == id)
            .OrderByDescending(c => c.Data)
            .ToListAsync();

        decimal total = consumos.Sum(c => c.Valor);

        ViewBag.Veiculo = veiculo;
        ViewBag.Total = total;

        return View(consumos);
    }

    private bool VeiculoExists(int id)
    {
        return _context.Veiculos.Any(e => e.Id == id);
    }
}
