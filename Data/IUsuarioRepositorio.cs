using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    public interface IUsuarioRepositorio
    {
        Usuario Create(Usuario usuario);
        Usuario GetByEmail(string email);
        Usuario GetById(int id);
    }
}
