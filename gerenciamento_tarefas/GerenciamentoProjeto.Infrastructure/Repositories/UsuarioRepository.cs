using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Context;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoProjeto.Infrastructure.Repositories
{
    public class UsuarioRepository(DataContext context) : IUsuarioRepository
    {
        private readonly DataContext _context = context;

        public async Task<Usuario> GetByIdAsync(int id)
        {
            Usuario usuario = await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return usuario;
        }

        public async Task<bool> ExistUserByIdAsync(int id)
        {
            bool existe = await _context.Usuario.AsNoTracking().AnyAsync(x => x.Id == id);

            return existe;
        }
    }
}
