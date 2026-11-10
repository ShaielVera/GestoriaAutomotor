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
    public class EmpleadosController : ControllerBase
    {
        private readonly GestoriaContext _context;

        public EmpleadosController(GestoriaContext context)
        {
            _context = context;
        }

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados([FromQuery] string filtro="")
        {
            filtro = filtro.ToUpper();
            return await _context.Empleados
                .Where(e => e.Nombre.ToUpper().Contains(filtro) ||
                            e.Apellido.ToUpper().Contains(filtro) ||
                            e.Cargo.ToUpper().Contains(filtro))
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        // devolvemos el total de empleados que no estan eliminados
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalEmpleados()
        {
            return await _context.Empleados.CountAsync(e => !e.IsDeleted);
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return empleado;
        }

        // PUT: api/Empleados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return BadRequest();
            }

            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
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

        // POST: api/Empleados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Empleado>> PostEmpleado(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpleado", new { id = empleado.Id }, empleado);
        }

        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            empleado.IsDeleted = true;
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("deleteds")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetDeleteds()
        {
            return await _context.Empleados.IgnoreQueryFilters().Where(e => e.IsDeleted).ToListAsync();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreEmpleado(int id)
        {
            var empleado = await _context.Empleados.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            empleado.IsDeleted = false;
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
