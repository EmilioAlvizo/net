using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Models;
using Microsoft.AspNetCore.Authorization;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // protege TODOS los endpoints de este controller
public class ProductosController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    /// <summary>
    /// Obtiene todos los productos registrados
    /// </summary>
    /// <returns>Lista de productos</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _db.Productos.ToListAsync();
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _db.Productos.FindAsync(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    // POST api/productos - crear nuevo producto
    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = producto.ProductoId }, producto);
    }

    // PUT api/clientes/1 — editar producto existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Producto producto)
    {
        if (id != producto.ProductoId) return BadRequest();

        _db.Entry(producto).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/clientes/1 — borrar producto
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _db.Productos.FindAsync(id);
        if (producto == null) return NotFound();

        _db.Productos.Remove(producto);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}