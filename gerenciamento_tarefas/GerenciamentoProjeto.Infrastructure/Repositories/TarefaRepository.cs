using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Context;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoProjeto.Infrastructure.Repositories
{
    public class TarefaRepository(DataContext context) : ITarefaRepository
    {
        private readonly DataContext _context = context;

        public async Task<Tarefa> InsertAsync(Tarefa tarefa)
        {
            _context.Tarefa.Add(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task<Tarefa> UpdateAsync(Tarefa tarefa)
        {
            _context.Tarefa.Update(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task DeleteAsync(Tarefa tarefa)
        {
            _context.Tarefa.Remove(tarefa);

            await _context.SaveChangesAsync();
        }

        public async Task<Tarefa> GetByIdAsync(int id)
        {
            Tarefa tarefa = await _context.Tarefa.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return tarefa;
        }

        public async Task<IEnumerable<Tarefa>> GetAllTaskProjectAsync(int id)
        {
            IEnumerable<Tarefa> tarefas = await _context.Tarefa
                                                .AsNoTracking()
                                                .Include(x => x.Prioridade)
                                                .Include(x => x.Status)
                                                .Include(x => x.Usuario)
                                                .Include(x => x.Historico)
                                                    .ThenInclude(h => h.Usuario)
                                                .Where(x => x.ProjetoId == id)
                                                .ToListAsync();

            return tarefas;
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            IEnumerable<Tarefa> tarefas = await (from tarefa in _context.Tarefa.AsNoTracking()
                                                 join prioridade in _context.Prioridade.AsNoTracking() on tarefa.PrioridadeId equals prioridade.Id
                                                 join status in _context.Status.AsNoTracking() on tarefa.StatusId equals status.Id
                                                 join projeto in _context.Projeto.AsNoTracking() on tarefa.ProjetoId equals projeto.Id
                                                 select new Tarefa
                                                 {
                                                     Id = tarefa.Id,
                                                     Prioridade = prioridade,
                                                     Status = status,
                                                     Projeto = projeto,
                                                     StatusId = status.Id,
                                                     PrioridadeId = prioridade.Id
                                                 }).ToListAsync();

            return tarefas;
        }

        public async Task<bool> ExistPendingTaskByProjectAsync(int id)
        {
            bool existe = await _context.Tarefa.AsNoTracking().AnyAsync(x => x.ProjetoId == id && x.StatusId != 3);

            return existe;
        }

        public async Task<int> GetCountTaskbyProjectAsync(int id)
        {
            int count = await _context.Tarefa.AsNoTracking().Where(x => x.ProjetoId == id).CountAsync();

            return count;
        }

        public async Task<bool> ExistTarefaByIdAsync(int id)
        {
            bool existe = await _context.Tarefa.AsNoTracking().AnyAsync(x => x.Id == id);

            return existe;
        }

        public async Task<bool> ExistStatusByIdAsync(int id)
        {
            bool existe = await _context.Status.AsNoTracking().AnyAsync(x => x.Id == id);

            return existe;
        }

        public async Task<bool> ExistPrioridadeByIdAsync(int id)
        {
            bool existe = await _context.Prioridade.AsNoTracking().AnyAsync(x => x.Id == id);

            return existe;
        }
    }
}
