using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Context;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoProjeto.Infrastructure.Repositories
{
    public class RelatorioRepository(DataContext context) : IRelatorioRepository
    {
        private readonly DataContext _context = context;

        public async Task<IEnumerable<Tarefa>> GetUsersPerformanceAsync(DateTime dataInicio, DateTime dataFim)
        {
            var resultado = await (from tarefa in _context.Tarefa.AsNoTracking()
                                   join usuario in _context.Usuario.AsNoTracking() on tarefa.UsuarioId equals usuario.Id
                                   where tarefa.StatusId == 3
                                   && tarefa.DataCriacao >= dataInicio
                                   && tarefa.DataCriacao <= dataFim
                                   select new Tarefa
                                   {
                                       Id = tarefa.Id,
                                       Usuario = usuario,
                                   }).ToListAsync();

            return resultado;
        }
    }
}
