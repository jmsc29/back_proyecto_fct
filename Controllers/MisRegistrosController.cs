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
    public class MisRegistrosController : Controller
    {

        private readonly IRegistroRepositorio _repositorioRegistros;
        private readonly IUsuarioRepositorio _repositorioUsuarios;
        public MisRegistrosController(IRegistroRepositorio repositorioRegistros, IUsuarioRepositorio repositorioUsuarios)
        {
            _repositorioRegistros = repositorioRegistros;
            _repositorioUsuarios = repositorioUsuarios;
        }

        

        [HttpGet("misRegistros")]
        public List<Registro> GetRegistros()
        {
            var registros = _repositorioRegistros.GetAllRegistros();

                foreach (Registro registro in registros)
                {
                    registro.Usuario = _repositorioUsuarios.GetById(registro.Id_usuario);
                }

            return registros;
            
        }


    }
}
