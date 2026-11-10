using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Services.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TramitesController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public TramitesController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/Tramites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tramite>>> GetTramites([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.Tramites
                .Include(t => t.Vehiculo)
                .ThenInclude(v => v.Cliente)
                .Where(t => t.TipoTramite.ToUpper().Contains(filtro) ||
                            t.Estado.ToUpper().Contains(filtro))
                .OrderBy(t => t.TipoTramite)
                .ToListAsync();
        }

        // devolvemos el total de tramites que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalTramites()
        {
            return await _context.Tramites.CountAsync(t => !t.IsDeleted);
        }

        // GET: api/Tramites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tramite>> GetTramite(int id)
        {
            var tramite = await _context.Tramites.Include(t => t.Vehiculo).ThenInclude(v => v.Cliente).FirstOrDefaultAsync(t => t.Id == id);

            if (tramite == null)
            {
                return NotFound();
            }

            return tramite;
        }

        // PUT: api/Tramites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTramite(int id, Tramite tramite)
        {
            if (id != tramite.Id)
            {
                return BadRequest();
            }

            _context.Entry(tramite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TramiteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tramites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tramite>> PostTramite(Tramite tramite)
        {
            _context.Tramites.Add(tramite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTramite", new { id = tramite.Id }, tramite);
        }

        // DELETE: api/Tramites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTramite(int id)
        {
            var tramite = await _context.Tramites.FindAsync(id);
            if (tramite == null)
            {
                return NotFound();
            }

            tramite.IsDeleted = true;
            _context.Entry(tramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Tramite>>> GetDeleteds()
        {
            return await _context.Tramites.IgnoreQueryFilters().Include(t => t.Vehiculo).ThenInclude(v => v.Cliente).Where(t => t.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreTramite(int id)
        {
            var tramite = await _context.Tramites.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id);
            if (tramite == null)
            {
                return NotFound();
            }

            tramite.IsDeleted = false;
            _context.Entry(tramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TramiteExists(int id)
        {
            return _context.Tramites.Any(e => e.Id == id);
        }
    }
}
