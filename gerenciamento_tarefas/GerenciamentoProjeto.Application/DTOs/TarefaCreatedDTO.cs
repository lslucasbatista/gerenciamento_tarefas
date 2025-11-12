namespace GerenciamentoProjeto.Application.DTOs
{
    public class TarefaCreatedDTO
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }

        public int StatusId { get; set; }

        public int PrioridadeId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataVencimento { get; set; }
    }
}
