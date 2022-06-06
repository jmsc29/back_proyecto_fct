using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace control_de_accesos_back.Modelos
{
    /// <summary>Clase Registro - es el modelo de los registros
    /// </summary>
    public class Registro
    {
        /// <value>Property <c>Id_registro</c>Representa el id del registro.</value>
        public int Id_registro { get; set; } //Clave primaria de registro
        /// <value>Property <c>Fecha</c>Representa la fecha a la que se ha realizado el registro.</value>
        public string Fecha { get; set; }
        /// <value>Property <c>Hora</c>Representa la hora a la que se ha realizado el registro.</value>
        public string Hora { get; set; }
        /// <value>Property <c>Tipo</c>Representa el tipo de registro, es de tipo bool, el valor 1 corresponde
        /// a registro de entrada y el valor 0 a registro de salida.</value>
        public bool Tipo { get; set; }
        /// <value>Property <c>Id_usuario</c>Representa el id del usuario al que pertenece dicho registro.</value>
        public int Id_usuario { get; set; } //Clave foránea de Usuario
        /// <value>Property <c>Usuario</c>Representa el usuario al que pertenece dicho registro.</value>
        public Usuario Usuario { get; set; }


    }
}
