using Microsoft.EntityFrameworkCore;
using Taller.Entidades;

namespace Taller.Datos.Repositorios;

public class OrdenTrabajoRepositorio : IOrdenTrabajoRepositorio
{
    private readonly TallerDbContext _context;

    public OrdenTrabajoRepositorio(TallerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrdenTrabajo>> ObtenerAsync(string? estado = null, int? vehiculoId = null)
    {
        var query = _context.OrdenTrabajos.AsNoTracking().Include(o => o.Vehiculo).AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(o => o.Estado == estado);
        if (vehiculoId.HasValue)
            query = query.Where(o => o.VehiculoId == vehiculoId.Value);

        return await query.OrderBy(o => o.Id).ToListAsync();
    }

    public async Task<OrdenTrabajo?> ObtenerConVehiculoAsync(int id)
    {
        return await _context.OrdenTrabajos
            .Include(o => o.Vehiculo)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<bool> ExistePorVehiculoAsync(int vehiculoId)
    {
        return await _context.Vehiculos.AnyAsync(v => v.Id == vehiculoId);
    }

    public async Task<IEnumerable<OrdenTrabajo>> ObtenerTodosAsync()
    {
        return await _context.OrdenTrabajos.OrderBy(o => o.Id).ToListAsync();
    }

    public async Task<OrdenTrabajo?> ObtenerPorIdAsync(int id)
    {
        return await _context.OrdenTrabajos.FindAsync(id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.OrdenTrabajos.AnyAsync(e => e.Id == id);
    }

    public async Task AgregarAsync(OrdenTrabajo entidad)
    {
        await _context.OrdenTrabajos.AddAsync(entidad);
    }

    public async Task ActualizarAsync(OrdenTrabajo entidad)
    {
        _context.OrdenTrabajos.Update(entidad);
    }

    public async Task EliminarAsync(int id)
    {
        var entidad = await _context.OrdenTrabajos.FindAsync(id);
        if (entidad is not null)
            _context.OrdenTrabajos.Remove(entidad);
    }

    public async Task<int> GuardarAsync()
    {
        return await _context.SaveChangesAsync();
    }
}