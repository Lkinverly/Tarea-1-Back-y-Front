using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEstudiantes.Models
{
    [Table("tipos_sangre")]
    public class TipoSangre
    {
        [Key]
        [Column("id_tipo_sangre")]
        public int Id_Tipo_Sangre { get; set; }

        [Required]
        [Column("sangre")]
        public string Sangre { get; set; } = null!;

        public ICollection<Estudiante>? Estudiantes { get; set; }
    }
}
