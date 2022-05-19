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
using control_de_accesos_back.Dtos;

namespace control_de_accesos_back.Controllers
{
    [Route("api/registros")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly IRegistroRepositorio _repositorio;

        public RegistroController(MyContext context, IRegistroRepositorio repositorio)
        {
            _context = context;
            _repositorio = repositorio;
        }

        // GET: api/Registroes
        [HttpGet]
        public List<Registro> GetRegistro()
        {
            DateTime thisDay = DateTime.Today;
            var fechaHoy = thisDay.Date.ToString().Split(" ")[0].Split("/");
            var fechaOrdenada = fechaHoy[2] + "-" + fechaHoy[1] + "-" + fechaHoy[0];
            var registros = _context.Registro.ToList();
            var registrosHoy = new List<Registro>();

            foreach (Registro registro in registros)
            {
                var usuario = _context.Usuario.ToList().Find(a => a.Id_usuario == registro.Id_usuario);
                registro.Nombre = usuario.Nombre + " " + usuario.Apellidos;
                if (registro.Fecha == fechaOrdenada)
                {
                    registrosHoy.Add(registro);
                }
            }

            return registrosHoy.OrderByDescending(b => b.Hora).ToList();
        }

        // GET: api/Registroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Registro>> GetRegistro(int id)
        {
            var registro = await _context.Registro.FindAsync(id);

            if (registro == null)
            {
                return NotFound();
            }

            return registro;
        }

        // PUT: api/Registroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistro(int id, Registro registro)
        {
            if (id != registro.Id_registro)
            {
                return BadRequest();
            }

            _context.Entry(registro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroExists(id))
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

        // POST: api/Registroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult MarcarRegistro(RegistroDto dto)
        {
            var registro = new Registro();
            var hola = 0;


            registro = new Registro
            {
                Fecha = dto.Fecha,
                Hora = dto.Hora,
                Tipo = dto.Tipo,
                Id_usuario = dto.Id_usuario,
                Nombre = dto.Nombre
            };

            _repositorio.Create(registro);

            return Ok(new
            {
                Fecha = registro.Fecha,
                Hora = registro.Hora,
                Tipo = registro.Tipo,
                Id_usuario = registro.Id_usuario,
                Nombre = dto.Nombre
            }
            );
        }

        // DELETE: api/Registroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            var registro = await _context.Registro.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }

            _context.Registro.Remove(registro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroExists(int id)
        {
            return _context.Registro.Any(e => e.Id_registro == id);
        }
    }
}
