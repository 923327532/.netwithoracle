using Taller.Entidades;

namespace Taller.Datos.Repositorios;

public interface IOrdenTrabajoRepositorio : IRepositorio<OrdenTrabajo>
{
    Task<IEnumerable<OrdenTrabajo>> ObtenerAsync(string? estado = null, int? vehiculoId = null);

    Task<OrdenTrabajo?> ObtenerConVehiculoAsync(int id);

    Task<bool> ExistePorVehiculoAsync(int vehiculoId);
}