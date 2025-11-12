using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoProjeto.Domain.Entities
{
    [Table("cargo")]
    public class Cargo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Column("descricao")]
        public string Descricao { get; set; }

        public ICollection<Usuario> Usuario { get; set; }
    }
}
