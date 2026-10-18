
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleConsumo.Models;
namespace ControleConsumo.Controllers;

public class VeiculosController : Controller
{
    private readonly AppDbContext _context;

    public VeiculosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: VEICULOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Veiculos.ToListAsync());
    }

    // GET: VEICULOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var veiculo = await _context.Veiculos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (veiculo == null)
        {
            return NotFound();
        }

        return View(veiculo);
    }

    // GET: VEICULOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: VEICULOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

    // GET: VEICULOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null)
        {
            return NotFound();
        }
        return View(veiculo);
    }

    // POST: VEICULOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nome,Placa,AnoFabricacao,AnoModelo")] Veiculo veiculo)
    {
        if (id != veiculo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(veiculo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(veiculo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(veiculo);
    }

    // GET: VEICULOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var veiculo = await _context.Veiculos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (veiculo == null)
        {
            return NotFound();
        }

        return View(veiculo);
    }

    // POST: VEICULOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo != null)
        {
            _context.Veiculos.Remove(veiculo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VeiculoExists(int? id)
    {
        return _context.Veiculos.Any(e => e.Id == id);
    }
}
