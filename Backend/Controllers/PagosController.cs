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
    public class PagosController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public PagosController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/Pagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.Pagos
                .Include(p => p.Turno)
                .ThenInclude(t => t.Tramite)
                .ThenInclude(tr => tr.Vehiculo)
                .Where(p => p.MetodoPago.ToUpper().Contains(filtro) ||
                            p.EstadoPago.ToUpper().Contains(filtro))
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        // devolvemos el total de pagos que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalPagos()
        {
            return await _context.Pagos.CountAsync(p => !p.IsDeleted);
        }

        // GET: api/Pagos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
            var pago = await _context.Pagos.Include(p => p.Turno).ThenInclude(t => t.Tramite).ThenInclude(tr => tr.Vehiculo).FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null)
            {
                return NotFound();
            }

            return pago;
        }

        // PUT: api/Pagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, Pago pago)
        {
            if (id != pago.Id)
            {
                return BadRequest();
            }

            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
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

        // POST: api/Pagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPago", new { id = pago.Id }, pago);
        }

        // DELETE: api/Pagos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            pago.IsDeleted = true;
            _context.Entry(pago).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetDeleteds()
        {
            return await _context.Pagos.IgnoreQueryFilters().Include(p => p.Turno).ThenInclude(t => t.Tramite).ThenInclude(tr => tr.Vehiculo).Where(p => p.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestorePago(int id)
        {
            var pago = await _context.Pagos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            pago.IsDeleted = false;
            _context.Entry(pago).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}
