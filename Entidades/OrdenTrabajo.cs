using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taller.Entidades;

public partial class OrdenTrabajo
{
    public int Id { get; set; }

    [Required]
    public int VehiculoId { get; set; }

    [Required]
    [StringLength(200)]
    public string Descripcion { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [Range(0, 999999.99)]
    public decimal CostoEstimado { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    [StringLength(100)]
    public string? Tecnico { get; set; }

    public virtual Vehiculo? Vehiculo { get; set; }
}