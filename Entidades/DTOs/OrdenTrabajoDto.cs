namespace Taller.Entidades.DTOs;

/// <summary>DTO de lectura de una orden de trabajo. Devuelve la placa del vehículo en lugar del objeto.</summary>
public class OrdenTrabajoDto
{
    public int Id { get; set; }
    public int VehiculoId { get; set; }
    public string Vehiculo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public decimal CostoEstimado { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public DateOnly? FechaEntrega { get; set; }
    public string? Tecnico { get; set; }
}
