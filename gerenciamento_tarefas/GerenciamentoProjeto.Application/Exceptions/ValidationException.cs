namespace GerenciamentoProjeto.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }

        public ValidationException(IEnumerable<string> erros) : base("Ocorreram erro(s) de validação.")
        {
            Erros = erros.ToList();
        }

        public List<string> Erros { get; } = [];
    }
}
