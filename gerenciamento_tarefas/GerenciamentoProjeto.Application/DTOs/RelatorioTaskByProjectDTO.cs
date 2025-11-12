namespace GerenciamentoProjeto.Application.DTOs
{
    public class RelatorioTaskByProjectDTO
    {
        public string Projeto { get; set; }

        public int Quantidade { get; set; }

        public int QuantidadePendente { get; set; }

        public int QuantidadeAndamento { get; set; }

        public int QuantidadeConcluida { get; set; }

        public int QuantidadeBaixa {  get; set; }

        public int QuantidadeMedia {  get; set; }

        public int QuantidadeAlta {  get; set; }

    }
}
