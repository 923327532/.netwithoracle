using Taller.Entidades;

namespace Taller.Datos.Repositorios;

public interface IVehiculoRepositorio : IRepositorio<Vehiculo>
{
    Task<IEnumerable<Vehiculo>> ObtenerAsync(
        string? marca = null,
        string? modelo = null,
        int? anioMin = null,
        int? anioMax = null,
        string? placa = null);

    Task<Vehiculo?> ObtenerConOrdenesAsync(int id);

    Task<bool> ExistePorPlacaAsync(string placa, int? excluirId = null);

    Task<IEnumerable<OrdenTrabajo>> ObtenerOrdenesPorVehiculoAsync(int vehiculoId);
}