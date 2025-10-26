using Microsoft.EntityFrameworkCore;

namespace ApiEstudiantes.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Los nombres de los DbSet no afectan el nombre de la tabla (ya lo definimos con [Table])
        public DbSet<Estudiante> Estudiantes { get; set; } = null!;
        public DbSet<TipoSangre> Tipos_Sangre { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Estudiante (muchos) -> TipoSangre (uno)
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.TipoSangre)
                .WithMany(t => t.Estudiantes)
                .HasForeignKey(e => e.Id_Tipo_Sangre)
                .OnDelete(DeleteBehavior.Restrict); // tu FK en MySQL es RESTRICT
        }
    }
}
