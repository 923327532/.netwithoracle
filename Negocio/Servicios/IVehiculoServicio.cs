using Taller.Entidades.DTOs;

namespace Taller.Negocio.Servicios;

public interface IVehiculoServicio
{
    Task<IEnumerable<VehiculoDto>> ObtenerAsync(
        string? marca = null,
        string? modelo = null,
        int? anioMin = null,
        int? anioMax = null,
        string? placa = null);

    Task<VehiculoDto> ObtenerAsync(int id);

    Task<VehiculoDto> CrearAsync(CrearVehiculoDto dto);

    Task<VehiculoDto> ActualizarAsync(int id, ActualizarVehiculoDto dto);

    Task EliminarAsync(int id);

    Task<IEnumerable<OrdenTrabajoDto>> ObtenerOrdenesAsync(int vehiculoId);
}