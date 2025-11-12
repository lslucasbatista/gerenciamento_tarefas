using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Infrastructure.Interfaces
{
    public interface IHistoricoRepository
    {
        Task<Historico> InsertAsync(Historico historico);
    }
}
