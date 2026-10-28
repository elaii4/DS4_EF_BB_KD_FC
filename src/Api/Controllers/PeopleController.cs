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

    /// <summary>
    /// Obtiene la lista de personas ordenada por id, o filtrada por nombre si se pasa el parametro search.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonResponseDto>>> GetAll([FromQuery] string? search) // Consulta y filtra personas con ILike insensible a mayúsculas
    {
        IQueryable<Person> query = _context.People.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmedSearch = search.Trim();
            // EF.Functions.ILike realiza la busqueda case-insensitive directamente en PostgreSQL (SQL ILIKE)
            query = query
                .Where(p => EF.Functions.ILike(p.Name, $"%{trimmedSearch}%"))
                .OrderBy(p => p.Name);
        }
        else
        {
            query = query.OrderBy(p => p.Id);
        }

        var list = await query
            .Select(p => new PersonResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age
            })
            .ToListAsync();

        return Ok(list);
    }

    /// <summary>
    /// Obtiene una persona por su identificador unico.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponseDto>> GetById(int id) // Busca una persona por su identificador único
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

        if (person == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No encontrado",
                Detail = $"No se encontró la persona con id {id}.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(person);
    }

    /// <summary>
    /// Crea una nueva persona.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponseDto>> Create([FromBody] PersonCreateDto dto) // Valida y registra una nueva persona en base
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var person = new Person
        {
            Name = dto.Name.Trim(),
            Age = dto.Age
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        var response = new PersonResponseDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };

        return CreatedAtAction(nameof(GetById), new { id = person.Id }, response);
    }

    /// <summary>
    /// Actualiza los datos de una persona existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] PersonUpdateDto dto) // Modifica los datos de una persona existente
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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

        person.Name = dto.Name.Trim();
        person.Age = dto.Age;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Elimina una persona por su identificador unico.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id) // Remueve una persona por su identificador único
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
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
