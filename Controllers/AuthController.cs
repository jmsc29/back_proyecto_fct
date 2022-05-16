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
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IUsuarioRepositorio _repositorio;
        private readonly JwtService _jwtService;
        public AuthController(IUsuarioRepositorio repositorio, JwtService jwtService)
        {
            _repositorio = repositorio;
            _jwtService = jwtService;
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {

            var usuario = _repositorio.GetByNombreUsuario(dto.Nombre_usuario);


            if (usuario == null) return  BadRequest(new { message = "Credenciales inválidas" });

            if(!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password)) return  BadRequest(new { message = "Contraseña incorrecta" });

            var jwt = _jwtService.Generate(usuario.Id_usuario);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

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
