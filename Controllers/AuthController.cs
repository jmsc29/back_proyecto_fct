using control_de_accesos_back.Data;
using control_de_accesos_back.Dtos;
using control_de_accesos_back.Helpers;
using control_de_accesos_back.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace control_de_accesos_back.Controllers
{
    /// <summary>Clase AuthController - para gestionar la autenticación de usuarios en el login y logout.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {

        /// <summary>Instance variable <c>_repositorio</c>Representa el repositorio de métodos de los usuarios.</summary>
        private readonly IUsuarioRepositorio _repositorio;
        /// <summary>Instance variable <c>_jwtService</c>Representa la herramienta para trabajar con cookies en el navegador.</summary>
        private readonly JwtService _jwtService;

        /// <summary>
        /// Constructor de la clase AuthController.
        /// </summary>
        public AuthController(IUsuarioRepositorio repositorio, JwtService jwtService)
        {
            _repositorio = repositorio;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Se encarga de revisar el proceso de login y en el caso de que todo esté correcto crea la cookie de login y se envía junto al usuario.
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {

            var usuario = _repositorio.GetByNombreUsuario(dto.Nombre_usuario);

            //Si no encuentra una coincidencia con el nombre de usuario retorna con un mensaje de error
            if (usuario == null) return  BadRequest(new { message = "Credenciales inválidas" });

            //Si no coincide la contraseña retorna con otro mensaje
            if(!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password)) return  BadRequest(new { message = "Contraseña incorrecta" });

            //En el caso de que el usuario no esté activo tampoco le deja iniciar sesión y es avisado
            if(!usuario.Activo) return  BadRequest(new { message = "Usuario no activo" });

            //Genero las cookes
            var jwt = _jwtService.Generate(usuario.Id_usuario);

            //Concateno las cookies en el navegador
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            //Si todo está correcto devuelto Ok con el usuario correspondiente
            return Ok(new
            {
                Id_usuario = usuario.Id_usuario,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Nombre_usuario = usuario.Nombre_usuario,
                Telefono = usuario.Telefono,
                Id_departamento = usuario.Id_departamento,
                Departamento = usuario.Departamento,
                Tipo_usuario = usuario.Tipo_usuario,
                Activo = usuario.Activo,
                Password = usuario.Password
            }
            );

        }

        /// <summary>
        /// Obtiene el usuario que ha iniciado sesión basándonos en las cookies del navegador.
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        public IActionResult Usuario()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];   
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var usuario = _repositorio.GetById(userId);

                return Ok(usuario);

            }catch(Exception)
            {
                return Unauthorized();
            }
            
        }

        /// <summary>
        /// Obtiene el usuario que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        public IActionResult UsuarioEditar(int id)
        {
            try
            {
                var usuario = _repositorio.GetById(id);

                return Ok(usuario);

            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }

        /// <summary>
        /// Se encarga de cerrar sesión del usuario que previamente inició sesión eliminando las cookies del navegador.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new
            {
                message = "success"
            });
        }

    }
}
