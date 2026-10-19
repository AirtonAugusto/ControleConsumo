using System.Security.Claims;
using ControleConsumo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleConsumo.Controllers;

[Authorize(Roles = "Admin")]   // só administradores gerenciam usuários (aula 4)
public class UsuariosController : Controller
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    // ================= LOGIN / LOGOUT (aula 14) =================

    // GET: /Usuarios/Login
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Usuarios/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string nome, string senha)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == nome);

        // Usuário não existe OU a senha digitada não confere com o hash salvo
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
        {
            ViewBag.Message = "Usuário e/ou senha inválidos!";
            return View();
        }

        // "Crachá" do usuário: quem ele é e qual o perfil
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
        };

        var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identidade);

        var propriedades = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, propriedades);

        return RedirectToAction("Index", "Veiculos");
    }

    // GET: /Usuarios/Logout
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // GET: /Usuarios/AccessDenied
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // ================= CRUD DE USUÁRIOS (aulas 11 e 1) =================

    // GET: /Usuarios
    public async Task<IActionResult> Index()
    {
        return View(await _context.Usuarios.ToListAsync());
    }

    // GET: /Usuarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    // GET: /Usuarios/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Usuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Senha,Perfil")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            // Nunca salvar a senha "aberta": guarda só o hash (aula 1)
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // GET: /Usuarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    // POST: /Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Senha,Perfil")] Usuario usuario)
    {
        if (id != usuario.Id) return NotFound();

        if (ModelState.IsValid)
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // GET: /Usuarios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    // POST: /Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
