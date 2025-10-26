using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEstudiantes.Models
{
    [Table("estudiantes")]
    public class Estudiante
    {
        [Key]
        [Column("id_estudiante")]
        public int Id_Estudiante { get; set; }

        [Required]
        [Column("carne")]
        public string Carne { get; set; } = null!;

        [Required]
        [Column("nombres")]
        public string Nombres { get; set; } = null!;

        [Required]
        [Column("apellidos")]
        public string Apellidos { get; set; } = null!;

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Required]
        [Column("correo_electronico")]
        public string Correo_Electronico { get; set; } = null!;

        [Required]
        [Column("fecha_nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }

        // FK
        [Required]
        [Column("id_tipo_sangre")]
        public int Id_Tipo_Sangre { get; set; }

        // Navegación
        public TipoSangre? TipoSangre { get; set; }
    }
}
