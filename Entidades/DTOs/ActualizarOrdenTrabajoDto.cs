using System.ComponentModel.DataAnnotations;

namespace Taller.Entidades.DTOs;

public class ActualizarOrdenTrabajoDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "El vehículo es obligatorio.")]
    public int VehiculoId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 200 caracteres.")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El estado es obligatorio.")]
    public string Estado { get; set; } = null!;

    [Range(0, 999999.99, ErrorMessage = "El costo estimado debe ser mayor o igual a cero.")]
    public decimal CostoEstimado { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "El técnico es obligatorio.")]
    public string? Tecnico { get; set; }
}