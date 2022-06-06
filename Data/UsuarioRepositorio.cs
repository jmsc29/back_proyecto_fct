using control_de_accesos_back.Modelos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    /// <summary>Clase UsuarioRepositorio - para implementar los métodos de los registros definidos en la interfaz IUsuarioRepositorio.
    /// </summary>
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        /// <summary>Instance variable <c>_context</c>Representa el contexto de la BBDD para poder acceder a ella.</summary>
        private readonly MyContext _context;

        /// <summary>
        /// Constructor de la clase UsuarioRepositorio.
        /// </summary>
        public UsuarioRepositorio(MyContext context)
        {
            _context = context;
        }

        /// <summary>Implementación del método 'Create' para crear nuevos usuarios.
        /// </summary>
        /// <param name="usuario"></param>
        public Usuario Create(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }

        /// <summary>Implementación del método 'GetByNombreUsuario' para obtener un usuario por su nombre de usuario.
        /// </summary>
        /// <param name="nombre_usuario"></param>
        public Usuario GetByNombreUsuario(string nombre_usuario)
        {
            
            try
            {
                var ret = _context.Usuario.FirstOrDefault(a => a.Nombre_usuario == nombre_usuario);
                return ret;
            }
            catch(System.AggregateException)
            {
                return null;
            }
        }

        /// <summary>Implementación del método 'GetAllByNombreUsuario' para obtener el número de usuarios que tienen el nombre de usuario pasado por parámetro.
        /// </summary>
        /// <param name="nombre_usuario"></param>
        public int GetAllByNombreUsuario(string nombre_usuario)
        {

            try
            {
                var ret = _context.Usuario.Count(a => a.Nombre_usuario.Contains(nombre_usuario));
                return ret;
            }
            catch (System.AggregateException)
            {
                return 0;
            }
        }

        /// <summary>Implementación del método 'GetById' para obtener el usuario cuyo id coincide con el pasado por parámetro.
        /// </summary>
        /// <param name="id"></param>
        public Usuario GetById(int id)
        {
            return _context.Usuario.FirstOrDefault(a => a.Id_usuario == id);
        }
    }
}
