using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Proyecto1_SW4.Models;

namespace Proyecto1_SW4.Services
{
    /// <summary>
    /// Servicio encargado de la manipulación de archivos JSON.
    /// Su responsabilidad es abstraer la lectura, escritura y serialización de objetos 
    /// a formato JSON hacia o desde el sistema de archivos local.
    /// </summary>
    public class JsonService
    {
        /// <summary>
        /// Guarda una lista de registros en formato JSON en la ruta especificada.
        /// Transforma la lista en memoria (List<Registro>) a una representación de texto JSON
        /// y la persiste en disco de manera asíncrona.
        /// </summary>
        public async Task<(bool Exito, string MensajeError)> GuardarJsonAsync(List<Registro> registros, string rutaArchivo)
        {
            try
            {
                // Opciones de serialización para generar un JSON con formato amigable y legible (indentado)
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(registros, options);
                
                await File.WriteAllTextAsync(rutaArchivo, jsonString);
                
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                // Capturamos problemas de permisos, rutas inválidas o falta de espacio.
                return (false, $"Error al guardar el archivo JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Lee un archivo JSON desde el disco y lo transforma en una lista de registros en memoria.
        /// Entrada: Ruta del archivo.
        /// Salida: Tupla con éxito, lista de registros leídos y posible mensaje de error.
        /// </summary>
        public async Task<(bool Exito, List<Registro>? Registros, string MensajeError)> LeerJsonAsync(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    return (false, null, "El archivo no existe.");
                }

                string jsonString = await File.ReadAllTextAsync(rutaArchivo);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var registros = JsonSerializer.Deserialize<List<Registro>>(jsonString, options);
                
                return (true, registros, string.Empty);
            }
            catch (JsonException ex)
            {
                return (false, null, $"Estructura JSON inválida: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al leer el archivo JSON: {ex.Message}");
            }
        }
    }
}
