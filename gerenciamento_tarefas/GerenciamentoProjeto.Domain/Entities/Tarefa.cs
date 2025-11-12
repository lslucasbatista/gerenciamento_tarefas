using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Domain.Entities
{
    [Table("tarefa")]
    public class Tarefa
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("projeto_id")]
        public int ProjetoId { get; set; }

        public Projeto Projeto { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }

        public Status Status { get; set; }

        [Required]
        [Column("prioridade_id")]
        public int PrioridadeId { get; set; }

        public Prioridade Prioridade { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Column("titulo")]
        [Required(AllowEmptyStrings = false)]
        public string Titulo { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Required]
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [Required]
        [Column("data_vencimento")]
        public DateTime DataVencimento { get; set; }

        [Required]
        [Column("data_atualizacao")]
        public DateTime DataUltimaAtualizacao { get; set; }

        public ICollection<Historico> Historico { get; set; }
    }
}
