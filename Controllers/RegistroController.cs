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
    /// <summary>Clase RegistroController - para gestionar la gestión de registros.
    /// </summary>
    [Route("api/registros")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        /// <summary>Instance variable <c>_context</c>Representa el contexto de la BBDD para poder acceder a ella.</summary>
        private readonly MyContext _context;
        /// <summary>Instance variable <c>_repositorio</c>Representa el repositorio de métodos de los registros.</summary>
        private readonly IRegistroRepositorio _repositorio;

        /// <summary>
        /// Constructor de la clase RegistroController.
        /// </summary>
        public RegistroController(MyContext context, IRegistroRepositorio repositorio)
        {
            _context = context;
            _repositorio = repositorio;
        }

        /// <summary>
        /// Se encarga de Obtener una lista con todos los registros del día actual con el formato necesario.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Registro> GetRegistro()
        {
            //Trabajo con los formatos de las fechas para que se entienda el formato de react con el de c# y con el de la base de datos en mysql
            try
            {
            DateTime thisDay = DateTime.Today;
            var fechaHoy = thisDay.Date.ToString().Split(" ")[0].Split("/");
            var fechaOrdenada = fechaHoy[2] + "-" + fechaHoy[1] + "-" + fechaHoy[0];
            var registros = _context.Registro.ToList();
            var registrosHoy = new List<Registro>();

            //Recorro todos los registros y añado en una lista secundaria aquellos que coincidan con el día en el que se está efectuando la operación
            foreach (Registro registro in registros)
            {
                var usuario = _context.Usuario.ToList().Find(a => a.Id_usuario == registro.Id_usuario);
                registro.Usuario = usuario;
                var fechaFormateada = registro.Fecha.Split(" ")[0].Split("/");
                var fechaFormateadaOrdenada = fechaFormateada[2] + "-" + fechaFormateada[1] + "-" + fechaFormateada[0];
                if (fechaFormateadaOrdenada == fechaOrdenada)
                {
                    registrosHoy.Add(registro);
                }
            }

            //Los retorno ordenados para que visualmente aparezcan por orden desde el marcaje más reciente hasta el menos reciente
            return registrosHoy.OrderByDescending(b => b.Hora).ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene una lista con todos los registros que coincidan con el mes pasado por parámetro
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpGet("mes/{mes}")]
        public List<Registro> GetRegistroMes(int mes)
        {
            var registros = _context.Registro.ToList();
            var registrosMes = new List<Registro>();

            foreach (Registro registro in registros)
            {
                var usuario = _context.Usuario.ToList().Find(a => a.Id_usuario == registro.Id_usuario);
                long mes_registro = 0;
                registro.Usuario = usuario;
                try
                {
                mes_registro = Int64.Parse(registro.Fecha.Split(" ")[0].Split("/")[1]);
                }
                catch
                {
                    mes_registro = 0;
                }
                if (mes == mes_registro)
                {
                    registrosMes.Add(registro);
                }
            }

            return registrosMes.OrderByDescending(b => b.Hora).ToList();
        }

        /// <summary>
        /// Obtiene el registro que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Registro> GetRegistro(int id)
        {
            var registros = _context.Registro.ToList(); ;

            var registro = registros.FirstOrDefault(a => a.Id_registro == id);

            if (registro == null) return NotFound();
            var usuario = _context.Usuario.ToList().Find(a => a.Id_usuario == registro.Id_usuario);
            registro.Usuario = usuario;
            var fechaFormateada = registro.Fecha.Split(" ")[0].Split("/");
            var fechaFormateadaOrdenada = fechaFormateada[2] + "-" + fechaFormateada[1] + "-" + fechaFormateada[0];



            return registro;
        }

        /// <summary>
        /// Obtiene el último registro del usuario cuyo id coincide con el id recibido.
        /// Esto es para mandarle al front el último registro y así saber si el próximo marcaje
        /// a realizar tiene que ser entrada o salida, se tiene en cuenta que no tenga ningún marcaje porque sea el primero
        /// y en ese caso se establece como si fuera de salida para que así el primer registro sea de entrada obligatoriamente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ultimo/{id}")]
        public ActionResult<Registro> GetUltimoRegistro(int id)
        {
            try
            {

            var registro = _context.Registro.ToList().FindAll(a => a.Id_usuario == id).OrderByDescending(a => a.Id_registro).First();
            if (registro == null) return NotFound();
            var usuario = _context.Usuario.ToList().Find(a => a.Id_usuario == registro.Id_usuario);
            registro.Usuario = usuario;
            var fechaFormateada = registro.Fecha.Split(" ")[0].Split("/");
            var fechaFormateadaOrdenada = fechaFormateada[2] + "-" + fechaFormateada[1] + "-" + fechaFormateada[0];
            return registro;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Modifico el registro que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registro"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Inserto el registro que puede ser tanto un marcaje de entrada como uno de salida.
        /// Los marcajes de entrada tienen el valor de 'true' y en la base de datos valor '1'.
        /// Los marcajes de salida tienen el valor de 'false' y en la base de datos valor '0'.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MarcarRegistro(RegistroDto dto)
        {

            var registro = new Registro();

            DateTime fechaHoy = DateTime.Now;
            var hoy = fechaHoy.ToString().Split(" ")[0].Split("/");
            var hoyOrdenado = hoy[2] + "-" + hoy[1] + "-" + hoy[0];
            var hora = fechaHoy.ToString().Split(" ")[1];


            registro = new Registro
            {
                Fecha = hoyOrdenado,
                Hora = hora,
                Tipo = dto.Tipo,
                Id_usuario = dto.Id_usuario,
                Usuario = dto.Usuario
            };

            _repositorio.Create(registro);

            return Ok(new
            {
                Fecha = registro.Fecha,
                Hora = registro.Hora,
                Tipo = registro.Tipo,
                Id_usuario = registro.Id_usuario,
                Usuario = dto.Usuario
            }
            );
        }

        /// <summary>
        /// Elimino el registro que coincide con el id recibido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
