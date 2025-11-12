using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.Application.Interfaces
{
    public interface IHistoricoService
    {
        Task<Historico> Insert(Historico historico);
    }
}
