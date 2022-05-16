using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace control_de_accesos_back

    
{
    public static class Utils
    {

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
