namespace GerenciamentoProjeto.Application.DTOs
{
    public class TarefaUpdateDTO
    {
        public int Id { get; set; }

        public int StatusId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataVencimento { get; set; }
    }
}
