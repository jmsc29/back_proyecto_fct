using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace control_de_accesos_back

    
{
    /// <summary>Clase Utils - donde establezco varios métodos que me son útiles y los puedo reutilizar
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// Método GenerarNombreUsuario para generar el nombre de usuario, para ello se basa en el nombre y apellidos.
        /// Usa la primera letra del nombre y si es compuesto la primera letra del primer y segundo nombre, como segunda
        /// parte usa el primer apellido para completar el nombre del usuario.
        /// En el caso de que el nombre de usuario se repita, automáticamente se modifica el nombre de usuario añadiéndole un número,
        /// este número se va incrementando de uno en uno dependiendo de cuantas veces coincida el nombre de usuario con otro igual.
        /// </summary>
        /// /// <param name="nombre"></param>
        /// /// <param name="apellidos"></param>
        /// <returns></returns>
        public static string GenerarNombreUsuario(string nombre, string apellidos)
        {

            string[] palabrasNombre = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] palabrasApellidos = apellidos.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (palabrasNombre.Length >= 2) //En el caso de que la persona tenga dos palabras o más en su apellido
            {
                if (palabrasApellidos.Length >= 2)
                {
                    return Regex.Replace(palabrasNombre[0].Substring(0, 1).ToLower() + palabrasNombre[1].Substring(0, 1).ToLower() + palabrasApellidos[0].ToLower().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                }
                else
                {
                    return Regex.Replace(palabrasNombre[0].Substring(0, 1).ToLower() + palabrasNombre[1].Substring(0, 1).ToLower() + apellidos.Trim().ToLower().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                }

            }
            else
            {
                if (palabrasApellidos.Length >= 2)
                {
                    return Regex.Replace(nombre.Trim().Substring(0, 1).ToLower() + palabrasApellidos[0].ToLower().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                }
                else
                {
                    return Regex.Replace(nombre.Trim().Substring(0, 1).ToLower() + apellidos.Trim().ToLower().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                }
            }
        }
    }
}
