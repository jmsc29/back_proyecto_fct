using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    /// <summary>Clase RegistroDto - es el dto de los registros
    /// </summary>
    public class RegistroDto
    {
        /// <value>Property <c>Id_registro</c>Representa el id del registro.</value>
        public int Id_registro { get; set; }
        /// <value>Property <c>Fecha</c>Representa la fecha a la que se ha realizado el registro.</value>
        public string Fecha { get; set; }
        /// <value>Property <c>Hora</c>Representa la hora a la que se ha realizado el registro.</value>
        public string Hora { get; set; }
        /// <value>Property <c>Tipo</c>Representa el tipo de registro, es de tipo bool, el valor 1 corresponde
        /// a registro de entrada y el valor 0 a registro de salida.</value>
        public bool Tipo { get; set; }
        /// <value>Property <c>Id_usuario</c>Representa el id del usuario al que pertenece dicho registro.</value>
        public int Id_usuario { get; set; }
        /// <value>Property <c>Usuario</c>Representa el usuario al que pertenece dicho registro.</value>

        public Usuario Usuario { get; set; }
    }
}
