using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    public class RegistroDto
    {
        public int Id_registro { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public bool Tipo { get; set; }
        public int Id_usuario { get; set; }
        public string Nombre { get; set; }
    }
}
