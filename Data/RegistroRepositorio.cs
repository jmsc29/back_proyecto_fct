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

        public Registro Create(Registro registro)
        {
            _context.Registro.Add(registro);
            _context.SaveChanges();

            return registro;
        }

        public List<Registro> GetAllRegistros()
        {
            throw new NotImplementedException();
        }

    }
}
