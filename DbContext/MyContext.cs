using control_de_accesos_back.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace control_de_accesos_back
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Establezco las CLAVES PRIMARIAS
            //Establezco la clave de la clase Usuario
            modelBuilder.Entity<Usuario>().HasKey(a => new { a.Id_usuario });
            //Establezco la clave de la clase Registro
            modelBuilder.Entity<Registro>().HasKey(a => new { a.Id_registro });

            //Usuario:Registro => 1:N
            modelBuilder.Entity<Usuario>().HasMany(w => w.Registros) //propiedad navegacional en Usuario
                 .WithOne(w => w.Usuario)
                 //Para la relación anterior establezco la clave foránea
                 .HasForeignKey(w => w.Id_usuario)
                 .OnDelete(DeleteBehavior.Cascade);
        
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Registro> Registro { get; set; }

    }
}
