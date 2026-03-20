using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Models;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClientesController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/clientes — obtener todos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _db.Clientes.ToListAsync();
        return Ok(clientes);
    }

    // GET api/clientes/1 — obtener uno por id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }
}