using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    /// <summary>Clase DepartamentoDto - es el dto de los departamentos
    /// </summary>
    public class DepartamentoDto
    {
        /// <value>Property <c>Id_departamento</c>Representa el id del departamento.</value>
        public int Id_departamento { get; set; }
        /// <value>Property <c>Nombre</c>Representa el nombre del departamento.</value>
        public string Nombre { get; set; }
    }
}
