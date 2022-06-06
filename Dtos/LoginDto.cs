using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    /// <summary>Clase LoginDto - es el dto del login y logout
    /// </summary>
    public class LoginDto
    {
        /// <value>Property <c>Nombre_usuario</c>Representa el nombre de usuario del usuario.</value>
        public string Nombre_usuario { get; set; }
        /// <value>Property <c>Password</c>Representa la contraseña del usuario.</value>
        public string Password { get; set; }
    }
}
