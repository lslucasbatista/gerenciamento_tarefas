using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoProjeto.Domain.Entities
{
    [Table("historico")]
    public class Historico
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tarefa_id")]
        public int TarefaId { get; set; }

        public Tarefa Tarefa { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
        [Column("data_modificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("comentario")]
        public string? Comentario { get; set; }

        [Column("original")]
        public string? Original { get; set; }

        [Column("alteracao")]
        public string? Alteracao { get; set; }
    }
}
