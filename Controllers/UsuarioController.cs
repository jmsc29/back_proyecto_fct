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
    /// <summary>Clase UsuarioController - para gestionar la gestión de usuarios.
    /// </summary>
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        /// <summary>Instance variable <c>_context</c>Representa el contexto de la BBDD para poder acceder a ella.</summary>
        private readonly MyContext _context;
        /// <summary>Instance variable <c>_repositorio</c>Representa el repositorio de métodos de los usuarios.</summary>
        private readonly IUsuarioRepositorio _repositorio;

        /// <summary>
        /// Constructor de la clase UsuarioController.
        /// </summary>
        public UsuarioController(MyContext context, IUsuarioRepositorio repositorio)
        {
            _context = context;
            _repositorio = repositorio;
        }

        /// <summary>
        /// Obtiene una lista con todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Usuario> GetUsuario()
        {

            var usuarios = _context.Usuario.ToList();

            foreach (Usuario u in usuarios){
                u.Departamento = _context.Departamento.ToList().Find(a => a.Id_departamento == u.Id_departamento);
            }

            return usuarios;
        }

        /// <summary>
        /// Obtiene el usuario que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Edita el usuario que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Modifica la contraseña del usuario con el id recibido poniéndole la nueva contraseña recibida en el parámetro 'newPassword'
        /// siempre que coincida la contraseña inicial con la recibida en el parámetro 'password'.
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("changePassword/{id_usuario}/{password}/{newPassword}")]
        public async Task<IActionResult> PutUsuarioNewPassword(int id_usuario, string password, string newPassword, Usuario usuario)
        {
            //Se comprueba que el id del usuario es correcto
            if (id_usuario != usuario.Id_usuario)
            {
                return BadRequest();
            }

            //Si la contraseña inicial recibida es incorrecta se envía un mensaje de error para avisar y no se realiza la modificación de la contraseña.
            if (!BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                return BadRequest(new { message = "Contraseña actual incorrecta" });
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id_usuario))
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

        /// <summary>
        /// Resetea la contraseña del usuario que tenga como id el recibido por parámetro.
        /// </summary>
        /// <param name="id_usuario"></param>
        /// /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("resetPassword/{id_usuario}")]
        public async Task<IActionResult> PutUsuarioResetPassword(int id_usuario, Usuario usuario)
        {
            if (id_usuario != usuario.Id_usuario)
            {
                return BadRequest();
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Nombre_usuario);

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id_usuario))
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

        /// <summary>
        /// Inserta un nuevo empleado.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
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
                    Id_departamento = dto.Id_departamento,
                    Departamento = _context.Departamento.ToList().Find(a => a.Id_departamento == dto.Id_departamento),
                    Tipo_usuario = dto.Tipo_usuario,
                    Activo = dto.Activo,
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
                    Id_departamento = dto.Id_departamento,
                    Departamento = _context.Departamento.ToList().Find(a => a.Id_departamento == dto.Id_departamento),
                    Tipo_usuario = dto.Tipo_usuario,
                    Activo = dto.Activo,
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
                Id_departamento = usuario.Id_departamento,
                Departamento = usuario.Departamento,
                Tipo_usuario = usuario.Tipo_usuario,
                Activo = usuario.Activo
                //Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password)
            }
            );

        }

        /// <summary>
        /// Elimina el empleado cuyo id coincide con el id recibido por parámetro.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Comprueba que el id recibido por parámetro coincide con el de un usuario para saber si existe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id_usuario == id);
        }
    }
}
