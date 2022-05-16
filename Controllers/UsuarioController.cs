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
using System.Text.RegularExpressions;

namespace control_de_accesos_back.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly IUsuarioRepositorio _repositorio;

        public UsuarioController(MyContext context, IUsuarioRepositorio repositorio)
        {
            _context = context;
            _repositorio = repositorio;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // GET: api/Usuario/5
        [HttpGet("{nombre_usuario}/{password}")]
        public ActionResult<Usuario> GetUsuarioLogin(string nombre_usuario, string password)
        {
            var usuario = _context.Usuario.Where(usuario => usuario.Nombre_usuario.Equals(nombre_usuario) && usuario.Password.Equals(password)).ToList().FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id_usuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpPost("registro")]
        public IActionResult Register(UsuarioDto dto)
        {
            var usuario = new Usuario();

            var compUsuario = _repositorio.GetAllByNombreUsuario(Utils.GenerarNombreUsuario(dto.Nombre, dto.Apellidos));

            if (compUsuario > 0)
            {
                string nombre_usuario_actualizado = Utils.GenerarNombreUsuario(dto.Nombre, dto.Apellidos) + (compUsuario);
                usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Apellidos = dto.Apellidos,
                    Nombre_usuario = Regex.Replace(nombre_usuario_actualizado, @"[^0-9A-Za-z]", "", RegexOptions.None),
                    Telefono = dto.Telefono,
                    Departamento = dto.Departamento,
                    Tipo_usuario = dto.Tipo_usuario,
                    Password = BCrypt.Net.BCrypt.HashPassword(nombre_usuario_actualizado)
                };
            }
            else
            {
                usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Apellidos = dto.Apellidos,
                    Nombre_usuario = Utils.GenerarNombreUsuario(dto.Nombre, dto.Apellidos),
                    Telefono = dto.Telefono,
                    Departamento = dto.Departamento,
                    Tipo_usuario = dto.Tipo_usuario,
                    Password = BCrypt.Net.BCrypt.HashPassword(Utils.GenerarNombreUsuario(dto.Nombre, dto.Apellidos))
                };

            }

            _repositorio.Create(usuario);

            return Ok(new
            {
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Nombre_usuario = usuario.Nombre_usuario,
                Telefono = usuario.Telefono,
                Departamento = usuario.Departamento,
                Tipo_usuario = usuario.Tipo_usuario
                //Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password)
            }
            );

        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id_usuario }, usuario);
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{nombre_usuario}/{password}")]
        public ActionResult<Usuario> PostMiUsuario(string nombre_usuario, string password)
        {
            var usuario = _context.Usuario.Where(usuario => usuario.Nombre_usuario.Equals(nombre_usuario) && usuario.Password.Equals(password)).ToList().FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id_usuario == id);
        }
    }
}
