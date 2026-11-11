using AutoMapper;
using Taller.Datos.Repositorios;
using Taller.Entidades;
using Taller.Entidades.DTOs;
using Taller.Negocio.Excepciones;
using Taller.Negocio.ReglasNegocio;

namespace Taller.Negocio.Servicios;

public class VehiculoServicio : IVehiculoServicio
{
    private readonly IVehiculoRepositorio _vehiculoRepositorio;
    private readonly IMapper _mapper;

    public VehiculoServicio(IVehiculoRepositorio vehiculoRepositorio, IMapper mapper)
    {
        _vehiculoRepositorio = vehiculoRepositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehiculoDto>> ObtenerAsync(
        string? marca = null,
        string? modelo = null,
        int? anioMin = null,
        int? anioMax = null,
        string? placa = null)
    {
        var vehiculos = await _vehiculoRepositorio.ObtenerAsync(marca, modelo, anioMin, anioMax, placa);
        return _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
    }

    public async Task<VehiculoDto> ObtenerAsync(int id)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerConOrdenesAsync(id);
        if (vehiculo is null)
            throw new NotFoundException($"No se encontró el vehículo con id {id}.");

        return _mapper.Map<VehiculoDto>(vehiculo);
    }

    public async Task<VehiculoDto> CrearAsync(CrearVehiculoDto dto)
    {
        if (await _vehiculoRepositorio.ExistePorPlacaAsync(dto.Placa))
            throw new ReglaNegocioException($"Ya existe un vehículo con la placa '{dto.Placa}'.");

        var entidad = _mapper.Map<Vehiculo>(dto);
        await _vehiculoRepositorio.AgregarAsync(entidad);
        await _vehiculoRepositorio.GuardarAsync();

        return _mapper.Map<VehiculoDto>(entidad);
    }

    public async Task<VehiculoDto> ActualizarAsync(int id, ActualizarVehiculoDto dto)
    {
        if (dto is null)
            throw new ReglaNegocioException("El DTO de actualización no puede ser nulo.");

        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            throw new NotFoundException($"No se encontró el vehículo con id {id}.");

        if (await _vehiculoRepositorio.ExistePorPlacaAsync(dto.Placa, id))
            throw new ReglaNegocioException($"Ya existe otro vehículo con la placa '{dto.Placa}'.");

        _mapper.Map(dto, vehiculo);
        await _vehiculoRepositorio.ActualizarAsync(vehiculo);
        await _vehiculoRepositorio.GuardarAsync();

        return _mapper.Map<VehiculoDto>(vehiculo);
    }

    public async Task EliminarAsync(int id)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            throw new NotFoundException($"No se encontró el vehículo con id {id}.");

        var ordenes = await _vehiculoRepositorio.ObtenerOrdenesPorVehiculoAsync(id);
        if (ordenes.Any())
            throw new ReglaNegocioException("No se puede eliminar el vehículo porque tiene órdenes de trabajo asociadas.");

        await _vehiculoRepositorio.EliminarAsync(id);
        await _vehiculoRepositorio.GuardarAsync();
    }

    public async Task<IEnumerable<OrdenTrabajoDto>> ObtenerOrdenesAsync(int vehiculoId)
    {
        if (!await _vehiculoRepositorio.ExisteAsync(vehiculoId))
            throw new NotFoundException($"No se encontró el vehículo con id {vehiculoId}.");

        var ordenes = await _vehiculoRepositorio.ObtenerOrdenesPorVehiculoAsync(vehiculoId);
        return _mapper.Map<IEnumerable<OrdenTrabajoDto>>(ordenes);
    }
}