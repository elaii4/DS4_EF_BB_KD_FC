namespace Api.Models;

// Person
// Modelo de dominio que representa a una persona en el sistema.
// Define las propiedades fundamentales id, nombre y edad de cada registro.
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
