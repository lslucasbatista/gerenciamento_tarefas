using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Application.DTOs
{
    public class ProjetoCreateDTO
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int UsuarioId { get; set; }
    }
}
