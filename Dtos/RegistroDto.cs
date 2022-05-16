using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Dtos
{
    public class RegistroDto
    {
        public int Id_registro { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public bool Tipo { get; set; }
        public int Id_usuario { get; set; }
    }
}
