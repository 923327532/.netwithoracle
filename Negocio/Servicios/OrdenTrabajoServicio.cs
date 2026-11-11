using AutoMapper;
using Taller.Datos.Repositorios;
using Taller.Entidades;
using Taller.Entidades.DTOs;
using Taller.Negocio.Excepciones;
using Taller.Negocio.ReglasNegocio;

namespace Taller.Negocio.Servicios;

public class OrdenTrabajoServicio : IOrdenTrabajoServicio
{
    private readonly IOrdenTrabajoRepositorio _repositorio;
    private readonly IMapper _mapper;

    public OrdenTrabajoServicio(IOrdenTrabajoRepositorio repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrdenTrabajoDto>> ObtenerAsync(string? estado = null, int? vehiculoId = null)
    {
        var ordenes = await _repositorio.ObtenerAsync(estado, vehiculoId);
        return _mapper.Map<IEnumerable<OrdenTrabajoDto>>(ordenes);
    }

    public async Task<OrdenTrabajoDto> ObtenerAsync(int id)
    {
        var orden = await _repositorio.ObtenerConVehiculoAsync(id);
        if (orden is null)
            throw new NotFoundException($"No se encontró la orden de trabajo con id {id}.");

        return _mapper.Map<OrdenTrabajoDto>(orden);
    }

    public async Task<OrdenTrabajoDto> CrearAsync(CrearOrdenTrabajoDto dto)
    {
        if (!await _repositorio.ExistePorVehiculoAsync(dto.VehiculoId))
            throw new ReglaNegocioException($"No existe el vehículo con id {dto.VehiculoId}.");

        if (!EsEstadoValido(dto.Estado))
            throw new ReglaNegocioException(
                $"El estado '{dto.Estado}' no es válido. Estados válidos: Pendiente, EnProceso, Completada, Cancelada.");

        var entidad = _mapper.Map<OrdenTrabajo>(dto);
        await _repositorio.AgregarAsync(entidad);
        await _repositorio.GuardarAsync();

        return _mapper.Map<OrdenTrabajoDto>(await _repositorio.ObtenerConVehiculoAsync(entidad.Id));
    }

    public async Task<OrdenTrabajoDto> ActualizarAsync(int id, ActualizarOrdenTrabajoDto dto)
    {
        if (dto is null)
            throw new ReglaNegocioException("El DTO de actualización no puede ser nulo.");

        var orden = await _repositorio.ObtenerPorIdAsync(id);
        if (orden is null)
            throw new NotFoundException($"No se encontró la orden de trabajo con id {id}.");

        if (!await _repositorio.ExistePorVehiculoAsync(dto.VehiculoId))
            throw new ReglaNegocioException($"No existe el vehículo con id {dto.VehiculoId}.");

        if (!EsEstadoValido(dto.Estado))
            throw new ReglaNegocioException(
                $"El estado '{dto.Estado}' no es válido. Estados válidos: Pendiente, EnProceso, Completada, Cancelada.");

        _mapper.Map(dto, orden);
        await _repositorio.ActualizarAsync(orden);
        await _repositorio.GuardarAsync();

        return _mapper.Map<OrdenTrabajoDto>(await _repositorio.ObtenerConVehiculoAsync(orden.Id));
    }

    public async Task EliminarAsync(int id)
    {
        var orden = await _repositorio.ObtenerPorIdAsync(id);
        if (orden is null)
            throw new NotFoundException($"No se encontró la orden de trabajo con id {id}.");

        await _repositorio.EliminarAsync(id);
        await _repositorio.GuardarAsync();
    }

    private bool EsEstadoValido(string estado)
    {
        return estado is "Pendiente" or "EnProceso" or "Completada" or "Cancelada";
    }
}