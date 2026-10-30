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
    public enum ServerTarget
    {
        Windows,
        Ubuntu
    }

    // PeopleApiClient
    // Cliente HTTP que centraliza la comunicación con la API REST remota.
    // Serializa solicitudes JSON y gestiona respuestas para todas las operaciones CRUD.
    // Permite conmutar dinámicamente entre el Servidor Windows y el Servidor Ubuntu.
    public class PeopleApiClient
    {
        private static HttpClient httpClient;
        private static string currentBaseUrl;
        private static ServerTarget currentServer = ServerTarget.Windows;

        public static string WindowsUrl { get; private set; }
        public static string UbuntuUrl { get; private set; }

        public static ServerTarget CurrentServer
        {
            get => currentServer;
            set
            {
                currentServer = value;
                ConfigureClient(currentServer == ServerTarget.Windows ? WindowsUrl : UbuntuUrl);
            }
        }

        public static string CurrentBaseUrl => currentBaseUrl;

        static PeopleApiClient()
        {
            WindowsUrl = ConfigurationManager.AppSettings["ApiUrl_Windows"] ?? "http://localhost:5144";
            UbuntuUrl = ConfigurationManager.AppSettings["ApiUrl_Ubuntu"] ?? "http://localhost:5000";

            string defaultTarget = ConfigurationManager.AppSettings["DefaultServer"] ?? "Windows";
            currentServer = defaultTarget.Equals("Ubuntu", StringComparison.OrdinalIgnoreCase) 
                ? ServerTarget.Ubuntu 
                : ServerTarget.Windows;

            ConfigureClient(currentServer == ServerTarget.Windows ? WindowsUrl : UbuntuUrl);
        }

        private static void ConfigureClient(string url)
        {
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            currentBaseUrl = url;

            httpClient?.Dispose();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(currentBaseUrl),
                Timeout = TimeSpan.FromSeconds(10)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static ServerTarget ToggleServer()
        {
            CurrentServer = (currentServer == ServerTarget.Windows) 
                ? ServerTarget.Ubuntu 
                : ServerTarget.Windows;
            return CurrentServer;
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
                throw new Exception($"No se pudo conectar con el servidor ({currentServer}: {currentBaseUrl}).", ex);
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
                throw new Exception($"No se pudo conectar con el servidor ({currentServer}: {currentBaseUrl}).", ex);
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
                throw new Exception($"No se pudo conectar con el servidor ({currentServer}: {currentBaseUrl}).", ex);
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
                throw new Exception($"No se pudo conectar con el servidor ({currentServer}: {currentBaseUrl}).", ex);
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
                throw new Exception($"No se pudo conectar con el servidor ({currentServer}: {currentBaseUrl}).", ex);
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
                throw new Exception($"No se pudo conectar con el servidor ({currentServer}: {currentBaseUrl}).", ex);
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
