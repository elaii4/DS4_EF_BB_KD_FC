using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CrudWindowsFormsLinq
{
    // PeopleApiClient
    // Cliente HTTP que centraliza la comunicación con la API REST remota.
    // Serializa solicitudes JSON y gestiona respuestas para todas las operaciones CRUD.
    public class PeopleApiClient
    {
        private static readonly HttpClient httpClient;
        private static readonly string baseApiUrl;

        static PeopleApiClient() // Inicializa HttpClient único y lee ApiBaseUrl desde App.config
        {
            baseApiUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            if (string.IsNullOrWhiteSpace(baseApiUrl))
            {
                baseApiUrl = "http://localhost:5000";
            }

            if (!baseApiUrl.EndsWith("/"))
            {
                baseApiUrl += "/";
            }

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseApiUrl),
                Timeout = TimeSpan.FromSeconds(10)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Person>> GetAsync() // Solicita el listado completo de personas vía HTTP
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("api/people");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Person>>(json) ?? new List<Person>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado al conectar con el servidor.", ex);
            }
        }

        public List<Person> Get()
        {
            return Task.Run(() => GetAsync()).GetAwaiter().GetResult();
        }

        public async Task<Person> GetByIdAsync(int id) // Obtiene una persona específica por su identificador único
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"api/people/{id}");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Person>(json);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado al conectar con el servidor.", ex);
            }
        }

        public Person GetById(int id)
        {
            return Task.Run(() => GetByIdAsync(id)).GetAwaiter().GetResult();
        }

        public async Task<int> AddAsync(string name, int age) // Envía datos para registrar una persona nueva
        {
            try
            {
                var payload = new { name = name, age = age };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/people", content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception("Error al crear el registro: " + error);
                }

                string json = await response.Content.ReadAsStringAsync();
                Person created = JsonConvert.DeserializeObject<Person>(json);
                return created != null ? created.Id : 0;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado al conectar con el servidor.", ex);
            }
        }

        public int Add(string name, int age)
        {
            return Task.Run(() => AddAsync(name, age)).GetAwaiter().GetResult();
        }

        public async Task<bool> UpdateAsync(int id, string name, int age) // Envía cambios para actualizar un registro existente
        {
            try
            {
                var payload = new { name = name, age = age };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"api/people/{id}", content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception("Error al actualizar el registro: " + error);
                }

                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado al conectar con el servidor.", ex);
            }
        }

        public bool Update(int id, string name, int age)
        {
            return Task.Run(() => UpdateAsync(id, name, age)).GetAwaiter().GetResult();
        }

        public async Task<bool> DeleteAsync(int id) // Solicita eliminar un registro por su identificador
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"api/people/{id}");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado al conectar con el servidor.", ex);
            }
        }

        public bool Delete(int id)
        {
            return Task.Run(() => DeleteAsync(id)).GetAwaiter().GetResult();
        }

        public async Task<List<Person>> SearchAsync(string text) // Consulta personas cuyo nombre coincida con el criterio
        {
            try
            {
                string encoded = Uri.EscapeDataString(text);
                HttpResponseMessage response = await httpClient.GetAsync($"api/people?search={encoded}");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Person>>(json) ?? new List<Person>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado al conectar con el servidor.", ex);
            }
        }

        public List<Person> Search(string text)
        {
            return Task.Run(() => SearchAsync(text)).GetAwaiter().GetResult();
        }
    }
}
