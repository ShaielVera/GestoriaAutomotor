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
    public class VehiculosController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public VehiculosController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/Vehiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.Vehiculos
                .Include(v => v.Cliente)
                .Where(v => v.Patente.ToUpper().Contains(filtro) ||
                            v.Marca.ToUpper().Contains(filtro) ||
                            v.Modelo.ToUpper().Contains(filtro) ||
                            v.Tipo.ToUpper().Contains(filtro))
                .OrderBy(v => v.Patente)
                .ToListAsync();
        }

        // devolvemos el total de vehiculos que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalVehiculos()
        {
            return await _context.Vehiculos.CountAsync(v => !v.IsDeleted);
        }

        // GET: api/Vehiculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.Include(v => v.Cliente).FirstOrDefaultAsync(v => v.Id == id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return vehiculo;
        }

        // PUT: api/Vehiculos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiculo(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculoExists(id))
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

        // POST: api/Vehiculos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehiculo>> PostVehiculo(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehiculo", new { id = vehiculo.Id }, vehiculo);
        }

        // DELETE: api/Vehiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            vehiculo.IsDeleted = true;
            _context.Entry(vehiculo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetDeleteds()
        {
            return await _context.Vehiculos.IgnoreQueryFilters().Include(v => v.Cliente).Where(v => v.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            vehiculo.IsDeleted = false;
            _context.Entry(vehiculo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.Id == id);
        }
    }
}
