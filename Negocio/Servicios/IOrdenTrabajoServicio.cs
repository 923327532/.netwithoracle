using Taller.Entidades.DTOs;

namespace Taller.Negocio.Servicios;

public interface IOrdenTrabajoServicio
{
    Task<IEnumerable<OrdenTrabajoDto>> ObtenerAsync(string? estado = null, int? vehiculoId = null);

    Task<OrdenTrabajoDto> ObtenerAsync(int id);

    Task<OrdenTrabajoDto> CrearAsync(CrearOrdenTrabajoDto dto);

    Task<OrdenTrabajoDto> ActualizarAsync(int id, ActualizarOrdenTrabajoDto dto);

    Task EliminarAsync(int id);
}