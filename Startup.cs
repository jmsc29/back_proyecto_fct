using control_de_accesos_back.Data;
using control_de_accesos_back.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace control_de_accesos_back
{
    /// <summary>Clase Startup - encargada de la configuración inicial al arrancar el servidor.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor de la clase Startup.
        /// <param name="configuration"></param>
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>Instance variable <c>Configuration</c>Representa la configuración del programa.</summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Método ConfigureServices para añadir los servicios al contenedor.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Establezco la conexión con la base de datos, el string está guardado en el archivo de configuración
            string connection = Configuration.GetConnectionString("defaultConnection");

            services.AddDbContext<MyContext>(options =>
            {
                options.UseMySQL(connection);
            });

            services.AddControllers();

            //Añado swagger para la documentación y lo configuro
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "APIRestControlAccesos",
                    Version = "v1",
                    Description = "Información de la API del control de accesos",

                    //Datos del autor
                    Contact = new OpenApiContact
                    {
                        Name = "José María Sáez Castro",
                        Email = "jsaecas1710@g.educaand.es",
                    }
                });
                //Obtengo el nombre del archivo xml que va a ser de la forma
                //nombre_proyecto.xml
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //Obtengo el nombre completo del archivo, incluyendo su ruta
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //Incluyo los comentarios XML que estén en el archivo
                c.IncludeXmlComments(xmlPath);
            });
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IRegistroRepositorio, RegistroRepositorio>();
            services.AddScoped<JwtService>();

            //Añado los Cors
            services.AddCors();

        }

        /// <summary>
        /// Método Configure configurar las canalización de solicitudes HTTP
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Variable de configuración con la url del front
            var frontend_url = Configuration.GetValue<string>("frontend_url");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIRestControlAccesos v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Configuración de los Cors
            app.UseCors(
                options => options.WithOrigins(frontend_url)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
