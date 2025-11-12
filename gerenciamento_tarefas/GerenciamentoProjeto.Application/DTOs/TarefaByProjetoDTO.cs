namespace GerenciamentoProjeto.Application.DTOs
{
    public class TarefaByProjetoDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string StatusDescricao { get; set; }

        public string UsuarioNome { get; set; }

        public string PrioridadeDescricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataVencimento { get; set; }

        public ICollection<HistoricoteDTO> Historico { get; set; }
    }
}
