using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoProjeto.Domain.Entities
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cargo_id")]
        [Required]
        public int CargoId { get; set; }

        public Cargo Cargo { get; set; }

        [Column("nome")]
        [Required(AllowEmptyStrings = false)]
        public string Nome { get; set; }

        [Column("email")]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        public ICollection<Historico> Historico { get; set; }

        public ICollection<Projeto> Projeto { get; set; }
    }
}
