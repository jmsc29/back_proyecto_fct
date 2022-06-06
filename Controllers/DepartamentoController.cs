using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using control_de_accesos_back;
using control_de_accesos_back.Modelos;
using control_de_accesos_back.Data;

namespace control_de_accesos_back.Controllers
{
    /// <summary>Clase DepartamentoController - para gestionar la gestión de departamentos.
    /// </summary>
    [Route("api/departamentos")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        /// <summary>Instance variable <c>_context</c>Representa el contexto de la BBDD para poder acceder a ella.</summary>
        private readonly MyContext _context;

        /// <summary>
        /// Constructor de la clase DepartamentoController.
        /// </summary>
        public DepartamentoController(MyContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Se encarga de Obtener una lista con todos los departamentos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Departamento> GetDepartamento()
        {
            return _context.Departamento.ToList();
        }

        /// <summary>
        /// Obtiene el departamento que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamento(int id)
        {
            var departamento = await _context.Departamento.FindAsync(id);

            if (departamento == null)
            {
                return NotFound();
            }

            return departamento;
        }


        /// <summary>
        /// Modifica el departamento que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departamento"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamento departamento)
        {
            if (id != departamento.Id_departamento)
            {
                return BadRequest();
            }

            _context.Entry(departamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(id))
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

        /// <summary>
        /// Inserta el departamento recibido.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Departamento>> PostDepartamento(Departamento departamento)
        {
            var departamentos = _context.Departamento.ToList();
            //En el caso de intentar agregar un departamento con el mismo nombre que otro existente da error y avisa al usuario.
            if (departamentos.Count(a => a.Nombre.ToLower().Equals(departamento.Nombre.ToLower())) > 0)
            {
                return BadRequest(new { message = "Departamento duplicado" });
            }

            _context.Departamento.Add(departamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartamento", new { id = departamento.Id_departamento }, departamento);
        }

        /// <summary>
        /// Elimina el departamento que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var departamento = await _context.Departamento.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }

            _context.Departamento.Remove(departamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Comprueba que el departamento con el id recibido existe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool DepartamentoExists(int id)
        {
            return _context.Departamento.Any(e => e.Id_departamento == id);
        }
    }
}
