using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Context;
using GerenciamentoProjeto.Infrastructure.Interfaces;

namespace GerenciamentoProjeto.Infrastructure.Repositories
{
    public class HistoricoRepository(DataContext context) : IHistoricoRepository
    {
        private readonly DataContext _context = context;

        public async Task<Historico> InsertAsync(Historico historico)
        {
            _context.Historico.Add(historico);
            await _context.SaveChangesAsync();

            return historico;
        }
    }
}
