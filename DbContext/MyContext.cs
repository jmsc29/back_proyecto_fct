using control_de_accesos_back.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back
{
    /// <summary>Clase MyContext - para gestionar las relaciones de la base de datos.
    /// </summary>
    public class MyContext : DbContext
    {

        /// <summary>
        /// Constructor de la clase MyContext.
        /// </summary>
        public MyContext(DbContextOptions options) : base(options)
        {

        }

        /// <summary>
        /// Método OnModelCreating para establecer toda la configuración de las tablas de la BBDD
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Establezco las CLAVES PRIMARIAS
            //Establezco la clave de la clase Usuario
            modelBuilder.Entity<Usuario>().HasKey(a => new { a.Id_usuario });
            //Establezco la clave de la clase Registro
            modelBuilder.Entity<Registro>().HasKey(a => new { a.Id_registro });
            //Establezo la clabe de la clase Departamento
            modelBuilder.Entity<Departamento>().HasKey(a => new { a.Id_departamento });

            //Propiedades
            modelBuilder.Entity<Usuario>()
                .Property(a => a.Nombre)
                .HasMaxLength(25)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(a => a.Apellidos)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(a => a.Telefono)
                .HasMaxLength(9)
                .IsRequired();

            //Registro:Usuario => 1:N
            modelBuilder.Entity<Registro>().HasOne(a => a.Usuario) //propiedad navegacional en Usuario
                 .WithMany()
                 //Para la relación anterior establezco la clave foránea
                 .HasForeignKey(w => w.Id_usuario)
                 .OnDelete(DeleteBehavior.Cascade);

            //Usuario:Departamento => 1:N
            modelBuilder.Entity<Usuario>().HasOne(a => a.Departamento) //propiedad navegacional en Usuario
                 .WithMany()
                 //Para la relación anterior establezco la clave foránea
                 .HasForeignKey(w => w.Id_departamento)
                 .OnDelete(DeleteBehavior.Cascade);

        }

        /// <value>Property <c>Usuario</c>Representa la tabla Usuario en la BBDD.</value>
        public DbSet<Usuario> Usuario { get; set; }
        /// <value>Property <c>Registro</c>Representa la tabla Registro en la BBDD.</value>
        public DbSet<Registro> Registro { get; set; }
        /// <value>Property <c>Departamento</c>Representa la tabla Departamento en la BBDD.</value>
        public DbSet<Departamento> Departamento { get; set; }

    }
}
