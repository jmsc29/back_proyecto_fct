using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    public class RegistroRepositorio : IRegistroRepositorio
    {

        private readonly MyContext _context;

        public RegistroRepositorio(MyContext context)
        {
            _context = context;
        }

        public Usuario Create(Registro registro)
        {
            throw new NotImplementedException();
        }

        public Usuario GetAllRegistros(string nombre_usuario)
        {
            throw new NotImplementedException();
        }
    }
}
