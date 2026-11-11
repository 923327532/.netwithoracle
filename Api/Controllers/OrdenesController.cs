using Microsoft.AspNetCore.Mvc;
using Taller.Negocio.ReglasNegocio;
using Taller.Entidades.DTOs;
using Taller.Negocio.Excepciones;
using Taller.Negocio.Servicios;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly IOrdenTrabajoServicio _ordenTrabajoServicio;

    public OrdenesController(IOrdenTrabajoServicio ordenTrabajoServicio)
    {
        _ordenTrabajoServicio = ordenTrabajoServicio;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdenTrabajoDto>>> GetOrdenes(
        [FromQuery] string? estado,
        [FromQuery] int? vehiculoId)
    {
        var ordenes = await _ordenTrabajoServicio.ObtenerAsync(estado, vehiculoId);
        return Ok(ordenes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdenTrabajoDto>> GetOrden(int id)
    {
        try
        {
            var orden = await _ordenTrabajoServicio.ObtenerAsync(id);
            return Ok(orden);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrdenTrabajoDto>> PostOrden(CrearOrdenTrabajoDto orden)
    {
        try
        {
            var creado = await _ordenTrabajoServicio.CrearAsync(orden);
            return CreatedAtAction(nameof(GetOrden), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutOrden(int id, ActualizarOrdenTrabajoDto orden)
    {
        if (orden is null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        if (id != orden.Id)
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });

        try
        {
            await _ordenTrabajoServicio.ActualizarAsync(id, orden);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrden(int id)
    {
        try
        {
            await _ordenTrabajoServicio.EliminarAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}