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

        [HttpPost("registro")]
        public IActionResult Register(RegisterDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Departamento = dto.Departamento,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };


            return Created("success", _repositorio.Create(usuario));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {

            var usuario = _repositorio.GetByEmail(dto.Email);

            if (usuario == null) return BadRequest(new { message = "Credenciales inválidas" });

            if(!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password)) return BadRequest(new { message = "Contraseña incorrecta" });

            var jwt = _jwtService.Generate(usuario.Id_usuario);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new
            {
                message = "success"
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
