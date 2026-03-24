using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Models;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    /// <summary>
    /// Obtiene todos los clientes registrados
    /// </summary>
    /// <returns>Lista de clientes</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _db.Clientes.ToListAsync();
        return Ok(clientes);
    }

    /// <summary>
    /// Obtiene un cliente por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }

    // POST api/clientes - crear nuevo cliente
    [HttpPost]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = cliente.ClienteId }, cliente);
    }

    // PUT api/clientes/1 — editar cliente existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Cliente cliente)
    {
        if (id != cliente.ClienteId) return BadRequest();

        _db.Entry(cliente).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/clientes/1 — borrar cliente
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();

        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}