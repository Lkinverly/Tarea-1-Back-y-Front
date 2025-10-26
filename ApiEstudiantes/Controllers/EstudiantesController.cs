using Microsoft.AspNetCore.Mvc;
using ApiEstudiantes.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEstudiantes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudiantesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            return await _context.Estudiantes
                .AsNoTracking()
                .Include(e => e.TipoSangre)
                .ToListAsync();
        }


        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes
                .AsNoTracking()
                .Include(e => e.TipoSangre)
                .FirstOrDefaultAsync(e => e.Id_Estudiante == id);

            if (estudiante == null)
                return NotFound();

            return estudiante;
        }


        // POST: api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            // Valida FK existente para evitar "NA" por FK inválida
            var existeTipo = await _context.Tipos_Sangre
                .AsNoTracking()
                .AnyAsync(t => t.Id_Tipo_Sangre == estudiante.Id_Tipo_Sangre);

            if (!existeTipo)
                return BadRequest($"El id_tipo_sangre={estudiante.Id_Tipo_Sangre} no existe.");

            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            // Vuelve a leer con Include para devolver la navegación
            var creado = await _context.Estudiantes
                .AsNoTracking()
                .Include(e => e.TipoSangre)
                .FirstAsync(e => e.Id_Estudiante == estudiante.Id_Estudiante);

            return CreatedAtAction(nameof(GetEstudiante), new { id = creado.Id_Estudiante }, creado);
        }


        // PUT: api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id_Estudiante)
                return BadRequest();

            // Valida que exista el estudiante a actualizar
            var current = await _context.Estudiantes.FindAsync(id);
            if (current == null)
                return NotFound();

            // Valida FK existente
            var existeTipo = await _context.Tipos_Sangre
                .AsNoTracking()
                .AnyAsync(t => t.Id_Tipo_Sangre == estudiante.Id_Tipo_Sangre);

            if (!existeTipo)
                return BadRequest($"El id_tipo_sangre={estudiante.Id_Tipo_Sangre} no existe.");

            // Actualiza campos permitidos
            current.Carne = estudiante.Carne;
            current.Nombres = estudiante.Nombres;
            current.Apellidos = estudiante.Apellidos;
            current.Direccion = estudiante.Direccion;
            current.Telefono = estudiante.Telefono;
            current.Correo_Electronico = estudiante.Correo_Electronico;
            current.Fecha_Nacimiento = estudiante.Fecha_Nacimiento;
            current.Id_Tipo_Sangre = estudiante.Id_Tipo_Sangre;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
                return NotFound();

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
