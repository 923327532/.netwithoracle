using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taller.Datos.Repositorios;

public interface IRepositorio<T> where T : class
{
    Task<IEnumerable<T>> ObtenerTodosAsync();
    Task<T?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task AgregarAsync(T entidad);
    Task ActualizarAsync(T entidad);
    Task EliminarAsync(int id);
    Task<int> GuardarAsync();
}