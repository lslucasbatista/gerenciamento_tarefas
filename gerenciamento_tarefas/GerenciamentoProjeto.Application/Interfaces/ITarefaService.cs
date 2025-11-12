using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<Tarefa> InsertAsync(Tarefa tarefa);

        Task<Tarefa> UpdateAsync(Tarefa tarefaUpdate);

        Task DeleteAsync(int id);

        Task<IEnumerable<Tarefa>> GetAllTaskProjectAsync(int id);
    }
}
