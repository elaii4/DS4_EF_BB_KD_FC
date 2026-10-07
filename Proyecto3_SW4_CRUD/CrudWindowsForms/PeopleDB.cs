using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CrudWindowsForms
{
    public class PeopleDB
    {
      
        private readonly string connectionString =
            "Server=LAPTOP-5K9FMN9N;Database=CrudWindowsForms;Integrated Security=True;TrustServerCertificate=True;";

        // Abre y cierra una conexión. Si algo falla, lanza la excepción
        // para que el formulario la capture y muestre el mensaje.
        public bool TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
        }

        // Devuelve todas las personas ordenadas por Id.
        public List<People> Get()
        {
            List<People> list = new List<People>();
            const string sql = "SELECT Id, Name, Age FROM dbo.People ORDER BY Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        People person = new People();
                        person.Id = reader.GetInt32(0);
                        person.Name = reader.GetString(1);
                        person.Age = reader.GetInt32(2);
                        list.Add(person);
                    }
                }
            }
            return list;
        }

        // Inserta una persona. El Id lo genera SQL Server (IDENTITY).
        public void Add(People person)
        {
            const string sql = "INSERT INTO dbo.People (Name, Age) VALUES (@Name, @Age);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = person.Name;
                command.Parameters.Add("@Age", SqlDbType.Int).Value = person.Age;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Devuelve la persona con ese Id, o null si no existe.
        public People GetById(int id)
        {
            const string sql = "SELECT Id, Name, Age FROM dbo.People WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        People person = new People();
                        person.Id = reader.GetInt32(0);
                        person.Name = reader.GetString(1);
                        person.Age = reader.GetInt32(2);
                        return person;
                    }
                }
            }
            return null;
        }

        // Actualiza Name y Age de la persona con ese Id.
        // Devuelve true si se modificó una fila.
        public bool Update(People person)
        {
            const string sql = "UPDATE dbo.People SET Name = @Name, Age = @Age WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = person.Name;
                command.Parameters.Add("@Age", SqlDbType.Int).Value = person.Age;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = person.Id;

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Elimina la persona con ese Id.
        // Devuelve true si se eliminó una fila.
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM dbo.People WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
