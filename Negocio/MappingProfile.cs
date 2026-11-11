using AutoMapper;
using Taller.Entidades;
using Taller.Entidades.DTOs;

namespace Taller.Negocio;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1. Entidad -> DTO de lectura (lo que la API devuelve; la entidad nunca sale).
        CreateMap<Vehiculo, VehiculoDto>();

        // 2. Entidad -> DTO de lectura. ForMember/MapFrom: devuelve la placa del vehículo en lugar del objeto.
        CreateMap<OrdenTrabajo, OrdenTrabajoDto>()
            .ForMember(dest => dest.Vehiculo, opt => opt.MapFrom(src => src.Vehiculo != null ? src.Vehiculo.Placa : string.Empty));

        // 3. DTO de entrada -> Entidad (crear). La fecha solo se copia si el cliente la envía.
        CreateMap<CrearVehiculoDto, Vehiculo>()
            .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro ?? DateOnly.FromDateTime(DateTime.Today)));

        // 4. DTO de entrada -> Entidad (actualizar, Map(origen, destino)). Id y fecha nula no se sobrescriben.
        CreateMap<ActualizarVehiculoDto, Vehiculo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FechaRegistro, opt => opt.Condition(src => src.FechaRegistro.HasValue));

        // 5. DTO de entrada -> Entidad (crear).
        CreateMap<CrearOrdenTrabajoDto, OrdenTrabajo>()
            .ForMember(dest => dest.FechaIngreso, opt => opt.MapFrom(src => src.FechaIngreso ?? DateOnly.FromDateTime(DateTime.Today)));

        // 6. DTO de entrada -> Entidad (actualizar).
        CreateMap<ActualizarOrdenTrabajoDto, OrdenTrabajo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FechaIngreso, opt => opt.Condition(src => src.FechaIngreso.HasValue));
    }
}
