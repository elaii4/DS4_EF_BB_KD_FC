using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Google.GenAI;
using OpenAI;
using OpenAI.Chat;

namespace Proyecto2_
{
    // Patrón Factory: Desacopla la creación de agentes de la interfaz. Facilita inyectar distintos modelos de IA sin alterar la UI.
    public static class AgentFactory
    {
        // Evalúa el string del proveedor e inicializa el modelo correspondiente. Lanza excepción si el negocio no soporta el proveedor.
        public static AIAgent Crear(
            string proveedor, string nombre, string instrucciones)
        {
            return proveedor switch
            {
                "Gemini" => CrearGemini(nombre, instrucciones),
                "OpenZen" => CrearOpenZen(nombre, instrucciones),
                _ => throw new ArgumentException("Proveedor no válido o no implementado.")
            };
        }

        private static AIAgent CrearOpenZen(string nombre, string instrucciones)
        {
            string key = Required("OPENZEN_API_KEY");
            
            // Configurar el cliente usando el SDK de OpenAI pero apuntando a la API de OpenZen.
            // Asegúrate de que la URL base de OpenZen sea correcta (puede ser diferente según su documentación).
            var options = new OpenAI.OpenAIClientOptions { Endpoint = new Uri("https://opencode.ai/zen/v1/") };
            var openAiClient = new OpenAI.OpenAIClient(new ApiKeyCredential(key), options);
            
            // OpenAI v2 requiere obtener el ChatClient especificando el modelo, luego adaptarlo
            var chatClient = openAiClient.GetChatClient("space-bunny-free");
            
            return new ChatClientAgent(
                chatClient.AsIChatClient(),
                name: nombre,
                instructions: instrucciones);
        }

        // Adapta el SDK nativo de Google Gemini a la abstracción estándar IChatClient de Microsoft y le inyecta el System Prompt.
        private static AIAgent CrearGemini(string nombre, string instrucciones)
        {
            string key = Required("GOOGLE_GENAI_API_KEY");
            return new ChatClientAgent(
                new Client(vertexAI: false, apiKey: key).AsIChatClient("gemini-3.8-flash"),
                name: nombre,
                instructions: instrucciones);
        }

        // Regla de seguridad (Fail-fast): Verifica la existencia de credenciales en el entorno antes de arrancar.
        private static string Required(string variable) =>
            Environment.GetEnvironmentVariable(variable)
            ?? throw new InvalidOperationException($"Defina la variable de entorno {variable}.");
    }
}
