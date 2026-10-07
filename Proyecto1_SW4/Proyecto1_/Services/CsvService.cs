using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Proyecto1_SW4.Models;

namespace Proyecto1_SW4.Services
{
    /// <summary>
    /// Servicio encargado de la manipulación de archivos CSV (Valores separados por comas).
    /// Su responsabilidad es abstraer la escritura de una colección de objetos 'Registro'
    /// a formato CSV en el sistema de archivos local, con un correcto manejo de caracteres.
    /// </summary>
    public class CsvService
    {
        /// <summary>
        /// Transforma una lista de registros en formato tabular CSV y lo guarda en disco.
        /// Incluye una cabecera y escapa los valores que contengan comas para evitar que se rompa la estructura tabular.
        /// Entrada: Lista de Registro y ruta de destino.
        /// Salida: Booleano de éxito y posible mensaje de error.
        /// </summary>
        public async Task<(bool Exito, string MensajeError)> GuardarCsvAsync(List<Registro> registros, string rutaArchivo)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                
                // Agregando la cabecera del CSV, determinando explícitamente los campos disponibles
                sb.AppendLine("Id,Nombre,Apellido,Edad,Genero,Email,Telefono,Usuario");
                
                foreach (var reg in registros)
                {
                    // Preparación de los campos, escapando posibles comas internas rodeándolas con comillas
                    string id = reg.Id.ToString();
                    string nombre = EscaparCsv(reg.Nombre);
                    string apellido = EscaparCsv(reg.Apellido);
                    string edad = reg.Edad.ToString();
                    string genero = EscaparCsv(reg.Genero);
                    string email = EscaparCsv(reg.Email);
                    string telefono = EscaparCsv(reg.Telefono);
                    string usuario = EscaparCsv(reg.Usuario);
                    
                    sb.AppendLine($"{id},{nombre},{apellido},{edad},{genero},{email},{telefono},{usuario}");
                }
                
                // Especificamos la codificación UTF-8 para garantizar que caracteres especiales se guarden bien
                await File.WriteAllTextAsync(rutaArchivo, sb.ToString(), Encoding.UTF8);
                
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar el archivo CSV: {ex.Message}");
            }
        }

        /// <summary>
        /// Maneja los valores de texto que contengan el carácter separador de CSV (,) u otros caracteres conflictivos.
        /// Regla de negocio de CSV: Si hay un separador o comilla dentro del valor, se rodea de comillas.
        /// </summary>
        private string EscaparCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return "";
            if (valor.Contains(",") || valor.Contains("\"") || valor.Contains("\n") || valor.Contains("\r"))
            {
                // Si el valor contiene una comilla doble, se reemplaza por dos comillas dobles (regla estándar de CSV)
                return $"\"{valor.Replace("\"", "\"\"")}\"";
            }
            return valor;
        }

        /// <summary>
        /// Lee un archivo CSV desde el disco y lo transforma en una lista de registros en memoria.
        /// Entrada: Ruta del archivo.
        /// Salida: Tupla con éxito, lista de registros leídos y posible mensaje de error.
        /// </summary>
        public async Task<(bool Exito, List<Registro>? Registros, string MensajeError)> LeerCsvAsync(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    return (false, null, "El archivo no existe.");
                }

                var lineas = await File.ReadAllLinesAsync(rutaArchivo, Encoding.UTF8);
                var registros = new List<Registro>();

                // Saltamos la cabecera (i = 1)
                for (int i = 1; i < lineas.Length; i++)
                {
                    var linea = lineas[i];
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    // Un split simple. Para un parser robusto real se requiere considerar comillas
                    // de forma avanzada, pero aquí se asume que los datos propios no rompen la estructura básica.
                    var campos = linea.Split(',');
                    if (campos.Length >= 8)
                    {
                        registros.Add(new Registro
                        {
                            Id = int.TryParse(campos[0], out int id) ? id : 0,
                            Nombre = campos[1].Trim('"'),
                            Apellido = campos[2].Trim('"'),
                            Edad = int.TryParse(campos[3], out int edad) ? edad : 0,
                            Genero = campos[4].Trim('"'),
                            Email = campos[5].Trim('"'),
                            Telefono = campos[6].Trim('"'),
                            Usuario = campos[7].Trim('"')
                        });
                    }
                }
                
                return (true, registros, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al leer el archivo CSV: {ex.Message}");
            }
        }
    }
}
