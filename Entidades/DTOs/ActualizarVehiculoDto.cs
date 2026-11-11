using System.ComponentModel.DataAnnotations;

namespace Taller.Entidades.DTOs;

public class ActualizarVehiculoDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "La placa es obligatoria.")]
    [StringLength(10, MinimumLength = 6, ErrorMessage = "La placa debe tener entre 6 y 10 caracteres.")]
    public string Placa { get; set; } = null!;

    [Required(ErrorMessage = "La marca es obligatoria.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "La marca es obligatoria.")]
    public string Marca { get; set; } = null!;

    [Required(ErrorMessage = "El modelo es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El modelo es obligatorio.")]
    public string Modelo { get; set; } = null!;

    [Range(1990, 2030, ErrorMessage = "El año debe estar entre 1990 y 2030.")]
    public int Anio { get; set; }

    public string? Color { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del cliente es obligatorio.")]
    public string ClienteNombre { get; set; } = null!;

    public DateOnly? FechaRegistro { get; set; }
}