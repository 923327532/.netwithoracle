using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TallerApi.Models;

namespace TallerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private static readonly string[] EstadosValidos = { "Pendiente", "EnProceso", "Completada", "Cancelada" };

    private readonly TallerDbContext _context;

    public OrdenesController(TallerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdenTrabajo>>> GetOrdenes(
        [FromQuery] string? estado,
        [FromQuery] int? vehiculoId)
    {
        var query = _context.OrdenTrabajos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(o => o.Estado == estado);
        if (vehiculoId.HasValue)
            query = query.Where(o => o.VehiculoId == vehiculoId.Value);

        return Ok(await query.OrderBy(o => o.Id).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdenTrabajo>> GetOrden(int id)
    {
        var orden = await _context.OrdenTrabajos
            .AsNoTracking()
            .Include(o => o.Vehiculo)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden is null)
            return NotFound();

        return Ok(orden);
    }

    [HttpPost]
    public async Task<ActionResult<OrdenTrabajo>> PostOrden(OrdenTrabajo orden)
    {
        if (!await _context.Vehiculos.AnyAsync(v => v.Id == orden.VehiculoId))
            return BadRequest(new { mensaje = $"No existe el vehiculo con id {orden.VehiculoId}." });

        if (!EstadosValidos.Contains(orden.Estado))
            return Conflict(new { mensaje = $"Estado '{orden.Estado}' no valido. Use: {string.Join(", ", EstadosValidos)}." });

        _context.OrdenTrabajos.Add(orden);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23514" })
        {
            return Conflict(new { mensaje = "La orden de trabajo no cumple las reglas del negocio." });
        }

        return CreatedAtAction(nameof(GetOrden), new { id = orden.Id }, orden);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutOrden(int id, OrdenTrabajo orden)
    {
        if (id != orden.Id)
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });

        var existe = await _context.OrdenTrabajos.AnyAsync(o => o.Id == id);
        if (!existe)
            return NotFound();

        if (!await _context.Vehiculos.AnyAsync(v => v.Id == orden.VehiculoId))
            return BadRequest(new { mensaje = $"No existe el vehiculo con id {orden.VehiculoId}." });

        if (!EstadosValidos.Contains(orden.Estado))
            return Conflict(new { mensaje = $"Estado '{orden.Estado}' no valido. Use: {string.Join(", ", EstadosValidos)}." });

        _context.Entry(orden).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23514" })
        {
            return Conflict(new { mensaje = "La orden de trabajo no cumple las reglas del negocio." });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrden(int id)
    {
        var orden = await _context.OrdenTrabajos.FindAsync(id);
        if (orden is null)
            return NotFound();

        _context.OrdenTrabajos.Remove(orden);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
