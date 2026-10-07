namespace Proyecto2_
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Cargar variables de entorno desde el archivo .env si existe (busca en directorios padres también)
            DotNetEnv.Env.TraversePath().Load();
            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}