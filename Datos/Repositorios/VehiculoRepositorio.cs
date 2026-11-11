using Microsoft.EntityFrameworkCore;
using Taller.Entidades;

namespace Taller.Datos.Repositorios;

public class VehiculoRepositorio : IVehiculoRepositorio
{
    private readonly TallerDbContext _context;

    public VehiculoRepositorio(TallerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehiculo>> ObtenerAsync(string? marca = null, string? modelo = null, int? anioMin = null, int? anioMax = null, string? placa = null)
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

        return await query.OrderBy(v => v.Id).ToListAsync();
    }

    public async Task<Vehiculo?> ObtenerConOrdenesAsync(int id)
    {
        return await _context.Vehiculos
            .Include(v => v.OrdenTrabajos)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<bool> ExistePorPlacaAsync(string placa, int? excluirId = null)
    {
        if (string.IsNullOrWhiteSpace(placa))
            return false;

        var query = _context.Vehiculos.Where(v => v.Placa == placa);
        if (excluirId.HasValue)
            query = query.Where(v => v.Id != excluirId.Value);

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<OrdenTrabajo>> ObtenerOrdenesPorVehiculoAsync(int vehiculoId)
    {
        return await _context.OrdenTrabajos.AsNoTracking().Include(o => o.Vehiculo)
            .Where(o => o.VehiculoId == vehiculoId)
            .OrderBy(o => o.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehiculo>> ObtenerTodosAsync()
    {
        return await _context.Vehiculos.OrderBy(v => v.Id).ToListAsync();
    }

    public async Task<Vehiculo?> ObtenerPorIdAsync(int id)
    {
        return await _context.Vehiculos.FindAsync(id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Vehiculos.AnyAsync(e => e.Id == id);
    }

    public async Task AgregarAsync(Vehiculo entidad)
    {
        await _context.Vehiculos.AddAsync(entidad);
    }

    public async Task ActualizarAsync(Vehiculo entidad)
    {
        _context.Vehiculos.Update(entidad);
    }

    public async Task EliminarAsync(int id)
    {
        var entidad = await _context.Vehiculos.FindAsync(id);
        if (entidad is not null)
            _context.Vehiculos.Remove(entidad);
    }

    public async Task<int> GuardarAsync()
    {
        return await _context.SaveChangesAsync();
    }
}