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
    public class TurnosController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public TurnosController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/Turnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turno>>> GetTurnos([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.Turnos
                .Include(t => t.Tramite)
                .ThenInclude(tr => tr.Vehiculo)
                .Include(t => t.Empleado)
                .Where(t => t.Estado.ToUpper().Contains(filtro))
                .OrderBy(t => t.Fecha)
                .ThenBy(t => t.Hora)
                .ToListAsync();
        }

        // devolvemos el total de turnos que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalTurnos()
        {
            return await _context.Turnos.CountAsync(t => !t.IsDeleted);
        }

        // GET: api/Turnos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> GetTurno(int id)
        {
            var turno = await _context.Turnos.Include(t => t.Tramite).ThenInclude(tr => tr.Vehiculo).Include(t => t.Empleado).FirstOrDefaultAsync(t => t.Id == id);

            if (turno == null)
            {
                return NotFound();
            }

            return turno;
        }

        // PUT: api/Turnos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurno(int id, Turno turno)
        {
            if (id != turno.Id)
            {
                return BadRequest();
            }

            _context.Entry(turno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurnoExists(id))
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

        // POST: api/Turnos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Turno>> PostTurno(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTurno", new { id = turno.Id }, turno);
        }

        // DELETE: api/Turnos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }

            turno.IsDeleted = true;
            _context.Entry(turno).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Turno>>> GetDeleteds()
        {
            return await _context.Turnos.IgnoreQueryFilters().Include(t => t.Tramite).ThenInclude(tr => tr.Vehiculo).Include(t => t.Empleado).Where(t => t.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreTurno(int id)
        {
            var turno = await _context.Turnos.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            turno.IsDeleted = false;
            _context.Entry(turno).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}
