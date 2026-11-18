using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

// AppDbContext: Es el núcleo del ORM (Entity Framework Core).
// Actúa como el puente directo entre las clases C# en memoria y las tablas relacionales de PostgreSQL.
public class AppDbContext : DbContext
{
    // Constructor que recibe las opciones de configuración (como la cadena de conexión) mediante inyección de dependencias
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet<Person>: Representa la tabla 'people' en PostgreSQL.
    // Permite ejecutar consultas LINQ (Where, OrderBy, ToListAsync) sobre los registros de personas.
    public DbSet<Person> People => Set<Person>();

    // OnModelCreating: Configuración avanzada del modelo mediante Fluent API de EF Core
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            // Mapea la entidad C# a la tabla 'people' y define una restricción CHECK en PostgreSQL para la edad (0 a 120)
            entity.ToTable("people", t => t.HasCheckConstraint("CK_people_age", "age >= 0 AND age <= 120"));

            // Define 'Id' como la clave primaria (Primary Key)
            entity.HasKey(e => e.Id);

            // Mapea 'Id' a la columna 'id' autonumérica en PostgreSQL (IDENTITY)
            entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .UseIdentityAlwaysColumn();

            // Mapea 'Name' a la columna 'name' con longitud máxima de 100 caracteres y obligatorio
            entity.Property(e => e.Name)
                  .HasColumnName("name")
                  .HasMaxLength(100)
                  .IsRequired();

            // Mapea 'Age' a la columna 'age' obligatoria
            entity.Property(e => e.Age)
                  .HasColumnName("age")
                  .IsRequired();

            // Datos semilla iniciales para pruebas
            entity.HasData(
                new Person { Id = 1, Name = "Ana", Age = 20 },
                new Person { Id = 2, Name = "Luis", Age = 25 },
                new Person { Id = 3, Name = "Maria", Age = 22 }
            );
        });
    }
}
