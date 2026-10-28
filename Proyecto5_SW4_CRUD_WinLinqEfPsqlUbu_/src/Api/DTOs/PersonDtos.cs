using System.ComponentModel.DataAnnotations;

// PersonDtos.cs
// Define los objetos de transferencia de datos para la API REST.
// Aplica anotaciones de validación para entradas y salidas del cliente.
namespace Api.DTOs;

public class PersonResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class PersonCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La edad es obligatoria.")]
    [Range(0, 120, ErrorMessage = "La edad debe ser un número entero entre 0 y 120.")]
    public int Age { get; set; }
}

public class PersonUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La edad es obligatoria.")]
    [Range(0, 120, ErrorMessage = "La edad debe ser un número entero entre 0 y 120.")]
    public int Age { get; set; }
}
