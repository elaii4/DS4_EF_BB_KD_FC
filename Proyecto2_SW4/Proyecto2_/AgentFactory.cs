using System;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Google.GenAI;

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
                _ => throw new ArgumentException("Proveedor no válido o no implementado.")
            };
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
