using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Infrastructure.Interfaces
{
    public interface IProjetoRepository
    {
        Task<Projeto> InsertAsync(Projeto projeto);

        Task DeleteAsync(Projeto projeto);

        Task<Projeto> GetByIdAsync(int id);

        Task<IEnumerable<Projeto>> GetAllProjectUserAsync(int id);

        Task<bool> ExistProjectByIdAsync(int id);
    }
}
