using System.ComponentModel.DataAnnotations;

namespace Client.Blazor.Models;

// Modelo principal de Persona que representa la entidad recibida de la API REST
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

// DTO para la creación de una nueva persona con validaciones de entrada
public class PersonCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 120, ErrorMessage = "La edad debe estar comprendida entre 0 y 120 años.")]
    public int Age { get; set; } = 18;
}

// DTO para la edición de una persona existente
public class PersonUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 120, ErrorMessage = "La edad debe estar comprendida entre 0 y 120 años.")]
    public int Age { get; set; }
}
