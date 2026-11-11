using Microsoft.AspNetCore.Mvc;
using Taller.Entidades.DTOs;
using Taller.Negocio.Excepciones;
using Taller.Negocio.ReglasNegocio;
using Taller.Negocio.Servicios;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController : ControllerBase
{
    private readonly IVehiculoServicio _vehiculoServicio;

    public VehiculosController(IVehiculoServicio vehiculoServicio)
    {
        _vehiculoServicio = vehiculoServicio;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetVehiculos(
        [FromQuery] string? marca,
        [FromQuery] string? modelo,
        [FromQuery] int? anioMin,
        [FromQuery] int? anioMax,
        [FromQuery] string? placa)
    {
        var vehiculos = await _vehiculoServicio.ObtenerAsync(marca, modelo, anioMin, anioMax, placa);
        return Ok(vehiculos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehiculoDto>> GetVehiculo(int id)
    {
        try
        {
            var vehiculo = await _vehiculoServicio.ObtenerAsync(id);
            return Ok(vehiculo);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{id:int}/ordenes")]
    public async Task<ActionResult<IEnumerable<OrdenTrabajoDto>>> GetOrdenesPorVehiculo(int id)
    {
        try
        {
            var ordenes = await _vehiculoServicio.ObtenerOrdenesAsync(id);
            return Ok(ordenes);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<VehiculoDto>> PostVehiculo(CrearVehiculoDto vehiculo)
    {
        try
        {
            var creado = await _vehiculoServicio.CrearAsync(vehiculo);
            return CreatedAtAction(nameof(GetVehiculo), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutVehiculo(int id, ActualizarVehiculoDto vehiculo)
    {
        if (vehiculo is null)
            return BadRequest(new { mensaje = "El cuerpo de la solicitud no puede ser nulo." });

        if (id != vehiculo.Id)
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });

        try
        {
            await _vehiculoServicio.ActualizarAsync(id, vehiculo);
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
    public async Task<IActionResult> DeleteVehiculo(int id)
    {
        try
        {
            await _vehiculoServicio.EliminarAsync(id);
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