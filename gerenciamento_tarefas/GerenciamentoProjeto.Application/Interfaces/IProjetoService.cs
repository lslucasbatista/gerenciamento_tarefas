using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Application.Interfaces
{
    public interface IProjetoService
    {
        Task<Projeto> InsertAsync(Projeto projeto);

        Task DeleteAsync(int id);

        Task<IEnumerable<Projeto>> GetAllProjectUserAsync(int id);
    }
}
