using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerApi.Models;

namespace TallerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController : ControllerBase
{
    private readonly TallerDbContext _context;

    public VehiculosController(TallerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos(
        [FromQuery] string? marca,
        [FromQuery] string? modelo,
        [FromQuery] int? anioMin,
        [FromQuery] int? anioMax,
        [FromQuery] string? placa)
    {
        var query = _context.Vehiculos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(marca))
            query = query.Where(v => v.Marca.Contains(marca));
        if (!string.IsNullOrWhiteSpace(modelo))
            query = query.Where(v => v.Modelo.Contains(modelo));
        if (anioMin.HasValue)
            query = query.Where(v => v.Anio >= anioMin.Value);
        if (anioMax.HasValue)
            query = query.Where(v => v.Anio <= anioMax.Value);
        if (!string.IsNullOrWhiteSpace(placa))
            query = query.Where(v => v.Placa.Contains(placa));

        return Ok(await query.OrderBy(v => v.Id).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Vehiculo>> GetVehiculo(int id)
    {
        var vehiculo = await _context.Vehiculos
            .AsNoTracking()
            .Include(v => v.OrdenTrabajos)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehiculo is null)
            return NotFound();

        return Ok(vehiculo);
    }

    [HttpGet("{id:int}/ordenes")]
    public async Task<ActionResult<IEnumerable<OrdenTrabajo>>> GetOrdenesPorVehiculo(int id)
    {
        if (!await _context.Vehiculos.AnyAsync(v => v.Id == id))
            return NotFound();

        var ordenes = await _context.OrdenTrabajos
            .AsNoTracking()
            .Where(o => o.VehiculoId == id)
            .OrderBy(o => o.Id)
            .ToListAsync();

        return Ok(ordenes);
    }

    [HttpPost]
    public async Task<ActionResult<Vehiculo>> PostVehiculo(Vehiculo vehiculo)
    {
        if (await _context.Vehiculos.AnyAsync(v => v.Placa == vehiculo.Placa))
            return Conflict(new { mensaje = $"Ya existe un vehiculo con la placa {vehiculo.Placa}." });

        _context.Vehiculos.Add(vehiculo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVehiculo), new { id = vehiculo.Id }, vehiculo);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutVehiculo(int id, Vehiculo vehiculo)
    {
        if (id != vehiculo.Id)
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });

        var existe = await _context.Vehiculos.AnyAsync(v => v.Id == id);
        if (!existe)
            return NotFound();

        if (await _context.Vehiculos.AnyAsync(v => v.Placa == vehiculo.Placa && v.Id != id))
            return Conflict(new { mensaje = $"Ya existe otro vehiculo con la placa {vehiculo.Placa}." });

        _context.Entry(vehiculo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteVehiculo(int id)
    {
        var vehiculo = await _context.Vehiculos
            .Include(v => v.OrdenTrabajos)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehiculo is null)
            return NotFound();

        if (vehiculo.OrdenTrabajos.Count > 0)
            return Conflict(new { mensaje = "No se puede eliminar el vehiculo porque tiene ordenes de trabajo asociadas." });

        _context.Vehiculos.Remove(vehiculo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
