using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Context;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoProjeto.Infrastructure.Repositories
{
    public class ProjetoRepository(DataContext context) : IProjetoRepository
    {
        private readonly DataContext _context = context;

        public async Task<Projeto> InsertAsync(Projeto projeto)
        {
            _context.Projeto.Add(projeto);
            await _context.SaveChangesAsync();

            return projeto;
        }

        public async Task DeleteAsync(Projeto projeto)
        {
            _context.Projeto.Remove(projeto);

            await _context.SaveChangesAsync();
        }

        public async Task<Projeto> GetByIdAsync(int id)
        {
            Projeto projeto = await _context.Projeto.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return projeto;
        }

        public async Task<IEnumerable<Projeto>> GetAllProjectUserAsync(int id)
        {
            IEnumerable<Projeto> projetos = await _context.Projeto.AsNoTracking().Where(x => x.UsuarioId == id).ToListAsync();

            return projetos;
        }

        public async Task<bool> ExistProjectByIdAsync(int id)
        {
            bool existe = await _context.Projeto.AsNoTracking().AnyAsync(x => x.Id == id);

            return existe;
        }
    }
}
