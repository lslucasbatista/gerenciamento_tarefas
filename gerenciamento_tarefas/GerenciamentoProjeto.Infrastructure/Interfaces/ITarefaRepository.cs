using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Infrastructure.Interfaces
{
    public interface ITarefaRepository
    {
        Task<Tarefa> InsertAsync(Tarefa tarefa);

        Task<Tarefa> UpdateAsync(Tarefa tarefa);

        Task DeleteAsync(Tarefa tarefa);

        Task<Tarefa> GetByIdAsync(int id);

        Task<IEnumerable<Tarefa>> GetAllTaskProjectAsync(int id);

        Task<IEnumerable<Tarefa>> GetAllAsync();

        Task<bool> ExistPendingTaskByProjectAsync(int id);

        Task<int> GetCountTaskbyProjectAsync(int id);

        Task<bool> ExistTarefaByIdAsync(int id);

        Task<bool> ExistStatusByIdAsync(int id);

        Task<bool> ExistPrioridadeByIdAsync(int id);
    }
}
