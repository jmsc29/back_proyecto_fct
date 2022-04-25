using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Modelos
{
    public class Registro
    {
        public int Id_registro { get; set; } //Clave primaria de registro
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public bool Tipo { get; set; }
        public int Id_usuario { get; set; } //Clave foránea de Usuario
        //Registro:Usuario => N:1
        //Navegacional de usuario
        public Usuario Usuario { get; set; }


    }
}
