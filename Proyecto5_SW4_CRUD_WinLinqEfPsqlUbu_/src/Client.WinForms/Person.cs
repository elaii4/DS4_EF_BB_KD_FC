namespace CrudWindowsFormsLinq
{
    // Person
    // Modelo de datos del cliente para representar personas en la interfaz.
    // Almacena las propiedades id, nombre y edad enlazadas a controles visuales.
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
