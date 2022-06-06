using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    /// <summary>Interfaz IRegistroRepositorio - para manejar los métodos de los registros.
    /// </summary>
    public interface IRegistroRepositorio
    {
        /// <summary>Definición del método 'Create' para crear nuevos registros.
        /// </summary>
        /// <param name="registro"></param>
        Registro Create(Registro registro);
    }
}
