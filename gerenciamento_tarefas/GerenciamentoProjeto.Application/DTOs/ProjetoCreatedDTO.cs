namespace GerenciamentoProjeto.Application.DTOs
{
    public class ProjetoCreatedDTO
    {
        public int Id {  get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int UsuarioId { get; set; }
    }
}
