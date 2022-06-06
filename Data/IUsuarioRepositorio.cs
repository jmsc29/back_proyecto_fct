﻿using control_de_accesos_back.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back.Data
{
    /// <summary>Interfaz IUsuarioRepositorio - para manejar los métodos de los usuarios.
    /// </summary>
    public interface IUsuarioRepositorio
    {
        /// <summary>Definición del método 'Create' para crear nuevos usuarios.
        /// </summary>
        /// <param name="usuario"></param>
        Usuario Create(Usuario usuario);
        /// <summary>Definición del método 'GetByNombreUsuario' para obtener un usuario por su nombre de usuario.
        /// </summary>
        /// <param name="nombre_usuario"></param>
        Usuario GetByNombreUsuario(string nombre_usuario);
        /// <summary>Definición del método 'GetAllByNombreUsuario' para obtener el número de usuarios que tienen el nombre de usuario pasado por parámetro.
        /// </summary>
        /// <param name="nombre_usuario"></param>
        int GetAllByNombreUsuario(string nombre_usuario);
        /// <summary>Definición del método 'GetById' para obtener el usuario cuyo id coincide con el pasado por parámetro.
        /// </summary>
        /// <param name="id"></param>
        Usuario GetById(int id);
    }
}
