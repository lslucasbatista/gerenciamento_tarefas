using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoProjeto.Domain.Entities
{
    [Table("prioridade")]
    public class Prioridade
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Column("descricao")]
        public string Descricao { get; set; }

        public ICollection<Tarefa> Tarefa { get; set; }
    }
}
