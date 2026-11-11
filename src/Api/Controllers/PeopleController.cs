using Api.Data;
using Api.DTOs;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

// PeopleController
// Controlador HTTP REST que expone operaciones CRUD para la entidad Person.
// Gestiona validaciones, respuestas JSON y consultas LINQ asíncronas hacia EF.
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PeopleController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(AppDbContext context, ILogger<PeopleController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/people
    // GET: api/people?search=texto
    // Consulta el listado completo de personas o filtra por nombre si se recibe el parametro search.
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonResponseDto>>> GetAll([FromQuery] string? search)
    {
        // AsNoTracking(): optimizacion que desactiva el rastreo de cambios en memoria RAM, haciendo la lectura mucho mas rapida.
        IQueryable<Person> query = _context.People.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmedSearch = search.Trim();
            // EF.Functions.ILike traduce la busqueda directamente al operador ILIKE de PostgreSQL (insensible a mayusculas/minusculas)
            query = query
                .Where(p => EF.Functions.ILike(p.Name, $"%{trimmedSearch}%"))
                .OrderBy(p => p.Name);
        }
        else
        {
            // Orden predeterminado por identificador ascendente
            query = query.OrderBy(p => p.Id);
        }

        // Proyeccion a PersonResponseDto: protege la entidad de dominio y solo envia al cliente los campos necesarios
        var list = await query
            .Select(p => new PersonResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age
            })
            .ToListAsync();

        return Ok(list); // Retorna HTTP 200 OK con la lista en formato JSON
    }

    // GET: api/people/{id}
    // Busca una persona especifica por su clave primaria (ID).
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponseDto>> GetById(int id)
    {
        var person = await _context.People
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PersonResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age
            })
            .FirstOrDefaultAsync();

        // Si no existe en la base de datos, retorna HTTP 404 con un mensaje explicativo
        if (person == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No encontrado",
                Detail = $"No se encontró la persona con id {id}.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(person); // Retorna HTTP 200 OK con el objeto encontrado
    }

    // POST: api/people
    // Registra una nueva persona en la base de datos activa (recibe los datos en formato JSON en el cuerpo).
    [HttpPost]
    [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponseDto>> Create([FromBody] PersonCreateDto dto)
    {
        // Validacion automatica de las anotaciones del DTO (nombre obligatorio, edad positiva)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Mapeo del DTO recibido a la entidad de base de datos Person
        var person = new Person
        {
            Name = dto.Name.Trim(),
            Age = dto.Age
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync(); // EF Core ejecuta el SQL: INSERT INTO people ... RETURNING id

        var response = new PersonResponseDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };

        // Retorna HTTP 201 Created con la cabecera Location apuntando a GetById para consultar el nuevo registro
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, response);
    }

    // PUT: api/people/{id}
    // Modifica los datos de una persona existente en la base de datos.
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] PersonUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Busca el registro en la base de datos para confirmar su existencia antes de editar
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No encontrado",
                Detail = $"No se encontró la persona con id {id}.",
                Status = StatusCodes.Status404NotFound
            });
        }

        // Actualiza los valores en memoria
        person.Name = dto.Name.Trim();
        person.Age = dto.Age;

        await _context.SaveChangesAsync(); // EF Core genera y ejecuta el SQL UPDATE correspondiente

        return NoContent(); // Retorna HTTP 204 NoContent para indicar exito sin necesidad de devolver cuerpo
    }

    // DELETE: api/people/{id}
    // Remueve una persona por su identificador unico.
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No encontrado",
                Detail = $"No se encontró la persona con id {id}.",
                Status = StatusCodes.Status404NotFound
            });
        }

        _context.People.Remove(person);
        await _context.SaveChangesAsync(); // EF Core ejecuta el SQL DELETE correspondiente en PostgreSQL

        return NoContent(); // Retorna HTTP 204 indicando eliminacion correcta
    }
}
