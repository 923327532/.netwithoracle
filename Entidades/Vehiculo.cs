using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taller.Entidades;

public partial class Vehiculo
{
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Placa { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Marca { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Modelo { get; set; } = null!;

    [Range(1990, 2030)]
    public int Anio { get; set; }

    [StringLength(30)]
    public string? Color { get; set; }

    [Required]
    [StringLength(100)]
    public string ClienteNombre { get; set; } = null!;

    public DateOnly FechaRegistro { get; set; }

    public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; } = new List<OrdenTrabajo>();
}