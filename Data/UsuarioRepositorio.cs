using control_de_accesos_back.Modelos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly MyContext _context;

        public UsuarioRepositorio(MyContext context)
        {
            _context = context;
        }

        public Usuario Create(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public Usuario GetByEmail(string email)
        {
            
            try
            {
                var ret = _context.Usuario.FirstOrDefault(a => a.Email == email);
                return ret;
            }
            catch(System.AggregateException)
            {
                return null;
            }
        }

        public Usuario GetById(int id)
        {
            return _context.Usuario.FirstOrDefault(a => a.Id_usuario == id);
        }
    }
}
