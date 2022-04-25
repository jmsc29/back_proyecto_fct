using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    public class RegisterDto
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Departamento { get; set; }
        public string Password { get; set; }

    }
}
