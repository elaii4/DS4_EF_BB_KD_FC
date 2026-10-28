using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

// AppDbContext
// Gestiona la conexión y entidades para Entity Framework Core en PostgreSQL.
// Mapea la tabla people y aplica restricciones de integridad relacional.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Configura mapeo relacional, restricciones y datos semilla
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("people", t => t.HasCheckConstraint("CK_people_age", "age >= 0 AND age <= 120"));

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .UseIdentityAlwaysColumn();

            entity.Property(e => e.Name)
                  .HasColumnName("name")
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.Age)
                  .HasColumnName("age")
                  .IsRequired();

            entity.HasData(
                new Person { Id = 1, Name = "Ana", Age = 20 },
                new Person { Id = 2, Name = "Luis", Age = 25 },
                new Person { Id = 3, Name = "Maria", Age = 22 }
            );
        });
    }
}
