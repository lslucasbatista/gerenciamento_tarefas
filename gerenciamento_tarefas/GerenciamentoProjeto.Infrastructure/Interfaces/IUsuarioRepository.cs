using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Infrastructure.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<bool> ExistUserByIdAsync(int id);

        Task<Usuario> GetByIdAsync(int id);
    }
}
