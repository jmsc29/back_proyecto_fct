using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    public interface IRegistroRepositorio
    {
        Usuario Create(Registro registro);
        Usuario GetAllRegistros(string nombre_usuario);
    }
}
