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
    public class ClientesTramitesController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public ClientesTramitesController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/ClientesTramites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteTramite>>> GetClientesTramites([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.ClientesTramites
                .Include(ct => ct.Cliente)
                .Include(ct => ct.Tramite)
                .ThenInclude(t => t.Vehiculo)
                .Where(ct => ct.RolCliente.ToUpper().Contains(filtro))
                .OrderBy(ct => ct.ClienteId)
                .ToListAsync();
        }

        // devolvemos el total de clientes-tramites que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalClientesTramites()
        {
            return await _context.ClientesTramites.CountAsync(ct => !ct.IsDeleted);
        }

        // GET: api/ClientesTramites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteTramite>> GetClienteTramite(int id)
        {
            var clienteTramite = await _context.ClientesTramites.Include(ct => ct.Cliente).Include(ct => ct.Tramite).ThenInclude(t => t.Vehiculo).FirstOrDefaultAsync(ct => ct.Id == id);

            if (clienteTramite == null)
            {
                return NotFound();
            }

            return clienteTramite;
        }

        // PUT: api/ClientesTramites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClienteTramite(int id, ClienteTramite clienteTramite)
        {
            if (id != clienteTramite.Id)
            {
                return BadRequest();
            }

            _context.Entry(clienteTramite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteTramiteExists(id))
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

        // POST: api/ClientesTramites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClienteTramite>> PostClienteTramite(ClienteTramite clienteTramite)
        {
            _context.ClientesTramites.Add(clienteTramite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClienteTramite", new { id = clienteTramite.Id }, clienteTramite);
        }

        // DELETE: api/ClientesTramites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClienteTramite(int id)
        {
            var clienteTramite = await _context.ClientesTramites.FindAsync(id);
            if (clienteTramite == null)
            {
                return NotFound();
            }

            clienteTramite.IsDeleted = true;
            _context.Entry(clienteTramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<ClienteTramite>>> GetDeleteds()
        {
            return await _context.ClientesTramites.IgnoreQueryFilters().Include(ct => ct.Cliente).Include(ct => ct.Tramite).ThenInclude(t => t.Vehiculo).Where(ct => ct.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreClienteTramite(int id)
        {
            var clienteTramite = await _context.ClientesTramites.IgnoreQueryFilters().FirstOrDefaultAsync(ct => ct.Id == id);
            if (clienteTramite == null)
            {
                return NotFound();
            }

            clienteTramite.IsDeleted = false;
            _context.Entry(clienteTramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteTramiteExists(int id)
        {
            return _context.ClientesTramites.Any(e => e.Id == id);
        }
    }
}
