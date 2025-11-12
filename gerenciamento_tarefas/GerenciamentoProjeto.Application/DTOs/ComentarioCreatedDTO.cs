namespace GerenciamentoProjeto.Application.DTOs
{
    public class ComentarioCreatedDTO
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int TarefaId { get; set; }

        public string Comentario { get; set; }

    }
}
