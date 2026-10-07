using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Proyecto1_SW4.Models;

namespace Proyecto1_SW4.Services
{
    /// <summary>
    /// Clase de servicio para manejar las solicitudes a APIs externas.
    /// Su responsabilidad central es realizar la conexión HTTP de forma asíncrona,
    /// descargar los datos JSON, deserializarlos y mapearlos al modelo interno de la aplicación.
    /// Aisla la lógica de red de la capa de interfaz de usuario.
    /// </summary>
    public class ApiService
    {
        // HttpClient debe ser reutilizado en toda la aplicación para evitar agotar los sockets (exhaustion).
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Realiza la conexión a la API proporcionada, descarga la respuesta JSON y la convierte en una lista de Registro.
        /// Entrada: URL de la API (ej: DummyJSON).
        /// Transformación: JSON crudo -> DTO (DummyJsonUsersResponse) -> Mapeo a modelo interno -> List<Registro>.
        /// Salida: Una tupla con un valor booleano de éxito, la lista de registros mapeados, el JSON crudo y un mensaje de error.
        /// Este proceso existe para encapsular toda la lógica de obtención, parseo y transformación en una sola invocación.
        /// </summary>
        public async Task<(bool Exito, List<Registro>? Registros, string? JsonRaw, string? MensajeError)> ObtenerDatosAsync(string url)
        {
            try
            {
                // Validar el formato básico de URL para evitar intentos inútiles que terminarían en excepción.
                if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    return (false, null, null, "La URL proporcionada no es válida o está vacía.");
                }

                // Efectuar la petición HTTP GET asíncrona. No se bloquea la UI gracias a async/await.
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                
                // Si la respuesta no tiene éxito (ej. 404, 500, 401), se lanza una excepción de HTTP y es capturada en catch.
                response.EnsureSuccessStatusCode();

                // Extraer el contenido crudo (Raw JSON) para proveerlo a la UI (Vista previa) si lo solicita.
                string jsonRaw = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(jsonRaw))
                {
                    return (false, null, jsonRaw, "La API retornó una respuesta vacía.");
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                DummyJsonUsersResponse? apiResponse = null;
                
                try 
                {
                    apiResponse = JsonSerializer.Deserialize<DummyJsonUsersResponse>(jsonRaw, options);
                } 
                catch (JsonException)
                {
                    // Fallback
                }

                // Si la estructura no era de tipo DummyJSON raíz, intentamos si la API devolvió una lista directa.
                if (apiResponse == null || apiResponse.users == null)
                {
                    try 
                    {
                        var listResponse = JsonSerializer.Deserialize<List<DummyJsonUser>>(jsonRaw, options);
                        if (listResponse != null)
                        {
                            apiResponse = new DummyJsonUsersResponse { users = listResponse };
                        }
                        else
                        {
                            return (false, null, jsonRaw, "La estructura del JSON no es compatible con el formato esperado.");
                        }
                    }
                    catch
                    {
                         return (false, null, jsonRaw, "La estructura del JSON no es compatible con el formato esperado.");
                    }
                }

                // Mapeo manual de la respuesta DTO (DummyJsonUser) al modelo de negocio interno (Registro).
                // Se realiza esta transformación porque los nombres y estructura de la API pueden variar y no deben acoplarse con la UI.
                List<Registro> registros = new List<Registro>();
                foreach (var user in apiResponse.users)
                {
                    registros.Add(new Registro
                    {
                        Id = user.id,
                        Nombre = user.firstName,
                        Apellido = user.lastName,
                        Edad = user.age,
                        Genero = user.gender,
                        Email = user.email,
                        Telefono = user.phone,
                        Usuario = user.username
                    });
                }

                return (true, registros, jsonRaw, null);
            }
            catch (HttpRequestException ex)
            {
                // Maneja errores de red, DNS, timeouts o códigos HTTP de error devueltos.
                return (false, null, null, $"Error de red al conectar con la API: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Maneja fallos de sintaxis en el JSON.
                return (false, null, null, $"Error al procesar los datos JSON recibidos: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Maneja excepciones no previstas sin provocar el cierre de la aplicación.
                return (false, null, null, $"Ha ocurrido un error inesperado: {ex.Message}");
            }
        }
    }
}
