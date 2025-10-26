using Microsoft.AspNetCore.Mvc;
using ApiEstudiantes.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEstudiantes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoSangresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoSangresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoSangre>>> GetTiposSangre()
        {
            return await _context.Tipos_Sangre.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoSangre>> GetTipoSangre(int id)
        {
            var tipo = await _context.Tipos_Sangre.FindAsync(id);
            if (tipo == null)
                return NotFound();
            return tipo;
        }

        [HttpPost]
        public async Task<ActionResult<TipoSangre>> PostTipoSangre(TipoSangre tipo)
        {
            _context.Tipos_Sangre.Add(tipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTipoSangre), new { id = tipo.Id_Tipo_Sangre }, tipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoSangre(int id, TipoSangre tipo)
        {
            if (id != tipo.Id_Tipo_Sangre)
                return BadRequest();

            _context.Entry(tipo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoSangre(int id)
        {
            var tipo = await _context.Tipos_Sangre.FindAsync(id);
            if (tipo == null)
                return NotFound();

            _context.Tipos_Sangre.Remove(tipo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
