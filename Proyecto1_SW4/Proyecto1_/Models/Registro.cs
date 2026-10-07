using System;
using System.Collections.Generic;

namespace Proyecto1_SW4.Models
{
    /// <summary>
    /// Modelo central de datos que representa un registro de usuario.
    /// Su propósito es unificar la estructura de datos proveniente tanto de la API (JSON)
    /// como de archivos locales (CSV/JSON) para mantener una única fuente de verdad en la aplicación.
    /// Permite a las capas de presentación (ListBox, ListView) interactuar con una estructura uniforme.
    /// </summary>
    public class Registro
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int Edad { get; set; }
        public string? Genero { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Usuario { get; set; }

        // Muestra una representación comprensible en el ListBox
        public override string ToString()
        {
            return $"{Id} - {Nombre} {Apellido}";
        }
    }

    /// <summary>
    /// Estructura para deserializar la respuesta paginada de la API DummyJSON.
    /// Sirve como objeto de transferencia de datos (DTO) intermedio para recibir
    /// correctamente el arreglo "users" desde la raíz del JSON.
    /// </summary>
    public class DummyJsonUsersResponse
    {
        public List<DummyJsonUser> users { get; set; }
        public int total { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }
    }

    /// <summary>
    /// Representa un usuario devuelto por la API DummyJSON. 
    /// Sus propiedades coinciden exactamente (o mediante configuración JSON) con los campos de la API,
    /// facilitando la deserialización antes de mapearlo al modelo de negocio 'Registro'.
    /// </summary>
    public class DummyJsonUser
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int age { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? username { get; set; }
    }
}
