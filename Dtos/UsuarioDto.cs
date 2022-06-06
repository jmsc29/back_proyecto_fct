using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    /// <summary>Clase UsuarioDto - es el dto de los usuarios
    /// </summary>
    public class UsuarioDto
    {
        /// <value>Property <c>Nombre</c>Representa el nombre del usuario.</value>
        public string Nombre { get; set; }
        /// <value>Property <c>Apellidos</c>Representa los apellidos del usuario.</value>
        public string Apellidos { get; set; }
        /// <value>Property <c>Nombre_usuario</c>Representa el nombre de usuario del usuario.</value>
        public string Nombre_usuario { get; set; }
        /// <value>Property <c>Telefono</c>Representa el teléfono del usuario.</value>
        public string Telefono { get; set; }
        /// <value>Property <c>Id_departamento</c>Representa el id del departamento al que pertenece el usuario.</value>
        public int Id_departamento { get; set; }
        /// <value>Property <c>Departamento</c>Representa departamento al que pertenece el usuario.</value>
        public Departamento Departamento{ get; set; }
        /// <value>Property <c>Tipo_usuario</c>Representa el tipo de usuario al que pertenece el empleado, puede ser
        /// o Admin o Empleado.</value>
        public string Tipo_usuario { get; set; }
        /// <value>Property <c>Password</c>Representa la contraseña del usuario.</value>
        public string Password { get; set; }
        /// <value>Property <c>Activo</c>Representa si el usuario está activo o no, true en el caso de que
        /// esté activo y false en el caso de que esté inactivo.</value>
        public bool Activo { get; set; }

    }
}
