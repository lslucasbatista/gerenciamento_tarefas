namespace GerenciamentoProjeto.Application.DTOs
{
    public class ComentarioCreateDTO
    {
        public int TarefaId { get; set; }

        public string Comentario { get; set; }

        public int UsuarioId { get; set; }
    }
}
