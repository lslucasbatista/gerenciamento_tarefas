using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoProjeto.Domain.Entities
{
    [Table("projeto")]
    public class Projeto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Column("nome")]
        public string Nome { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        public ICollection<Tarefa> Tarefa { get; set; } 
    }
}
