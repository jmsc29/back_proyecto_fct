using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    /// <summary>Clase RegistroRepositorio - para implementar los métodos de los registros definidos en la interfaz IRegistroRepositorio.
    /// </summary>
    public class RegistroRepositorio : IRegistroRepositorio
    {
        /// <summary>Instance variable <c>_context</c>Representa el contexto de la BBDD para poder acceder a ella.</summary>
        private readonly MyContext _context;

        /// <summary>
        /// Constructor de la clase RegistroRepositorio.
        /// </summary>
        public RegistroRepositorio(MyContext context)
        {
            _context = context;
        }

        /// <summary>Implementación del método 'Create' para crear nuevos registros.
        /// </summary>
        /// <param name="registro"></param>
        /// <return></return>
        public Registro Create(Registro registro)
        {
            _context.Registro.Add(registro);
            _context.SaveChanges();

            return registro;
        }

    }
}
