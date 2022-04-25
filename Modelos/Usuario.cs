using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace control_de_accesos_back.Modelos
{
    public class Usuario
    {
        public int Id_usuario { get; set; } //Clave primaria de usuario
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string  Departamento { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        //Navegacional de registros
        //Usuario:Registro => 1:N
        public List<Registro> Registros { get; set; }
    }
}
