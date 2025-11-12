using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Infrastructure.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<IEnumerable<Tarefa>> GetUsersPerformanceAsync(DateTime dataInicio, DateTime dataFim);
    }
}
