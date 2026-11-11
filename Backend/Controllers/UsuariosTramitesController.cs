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
    public class UsuariosTramitesController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public UsuariosTramitesController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/UsuariosTramites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioTramite>>> GetUsuariosTramites([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.UsuariosTramites
                .Include(ct => ct.Usuario)
                .Include(ct => ct.Tramite)
                .ThenInclude(t => t.Vehiculo)
                .Where(ct => ct.RolUsuario.ToUpper().Contains(filtro))
                .OrderBy(ct => ct.UsuarioId)
                .ToListAsync();
        }

        // devolvemos el total de usuarios-tramites que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalUsuariosTramites()
        {
            return await _context.UsuariosTramites.CountAsync(ct => !ct.IsDeleted);
        }

        // GET: api/UsuariosTramites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioTramite>> GetUsuarioTramite(int id)
        {
            var usuarioTramite = await _context.UsuariosTramites.Include(ct => ct.Usuario).Include(ct => ct.Tramite).ThenInclude(t => t.Vehiculo).FirstOrDefaultAsync(ct => ct.Id == id);

            if (usuarioTramite == null)
            {
                return NotFound();
            }

            return usuarioTramite;
        }

        // PUT: api/UsuariosTramites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioTramite(int id, UsuarioTramite usuarioTramite)
        {
            if (id != usuarioTramite.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuarioTramite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioTramiteExists(id))
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

        // POST: api/UsuariosTramites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioTramite>> PostUsuarioTramite(UsuarioTramite usuarioTramite)
        {
            _context.UsuariosTramites.Add(usuarioTramite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarioTramite", new { id = usuarioTramite.Id }, usuarioTramite);
        }

        // DELETE: api/UsuariosTramites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioTramite(int id)
        {
            var usuarioTramite = await _context.UsuariosTramites.FindAsync(id);
            if (usuarioTramite == null)
            {
                return NotFound();
            }

            usuarioTramite.IsDeleted = true;
            _context.Entry(usuarioTramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<UsuarioTramite>>> GetDeleteds()
        {
            return await _context.UsuariosTramites.IgnoreQueryFilters().Include(ct => ct.Usuario).Include(ct => ct.Tramite).ThenInclude(t => t.Vehiculo).Where(ct => ct.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreUsuarioTramite(int id)
        {
            var usuarioTramite = await _context.UsuariosTramites.IgnoreQueryFilters().FirstOrDefaultAsync(ct => ct.Id == id);
            if (usuarioTramite == null)
            {
                return NotFound();
            }

            usuarioTramite.IsDeleted = false;
            _context.Entry(usuarioTramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioTramiteExists(int id)
        {
            return _context.UsuariosTramites.Any(e => e.Id == id);
        }
    }
}
