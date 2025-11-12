using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Interfaces;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Domain.Enums;
using GerenciamentoProjeto.Infrastructure.Interfaces;

namespace GerenciamentoProjeto.Application.Services
{
    public class ProjetoService(IProjetoRepository repository, IUsuarioRepository usuarioRepository, ITarefaRepository tarefaRepository) : IProjetoService
    {
        private readonly IProjetoRepository _repository = repository;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;

        public async Task<Projeto> InsertAsync(Projeto projeto)
        {
            await Validate(Operation.Insert, projeto);

            projeto.DataCriacao = DateTime.Now;

            projeto = await _repository.InsertAsync(projeto);

            return projeto;
        }

        public async Task DeleteAsync(int id)
        {
            Projeto projeto = await _repository.GetByIdAsync(id);

            await Validate(Operation.Delete, projeto);

            await _repository.DeleteAsync(projeto);
        }

        public async Task<IEnumerable<Projeto>> GetAllProjectUserAsync(int id)
        {
            bool existeUsuario = await _usuarioRepository.ExistUserByIdAsync(id);

            if (!existeUsuario)
                throw new NotFoundException($"Usuário não encontrado.");

            IEnumerable<Projeto> projetos = await _repository.GetAllProjectUserAsync(id);

            return projetos;
        }

        private async Task Validate(Operation op, Projeto projeto)
        {
            if (projeto == null)
                throw new NotFoundException($"Projeto não encontrado.");

            var erros = new List<string>();

            switch (op)
            {
                case Operation.Insert:
                    if (string.IsNullOrEmpty(projeto.Nome))
                        erros.Add($"O nome do projeto é obrigatório.");
                    if (string.IsNullOrEmpty(projeto.Descricao))
                        erros.Add($"A descrição do projeto é obrigatória.");
                    if (projeto.UsuarioId <= 0)
                        erros.Add($"O usuário criador do projeto é obrigatório.");
                    else if (await _usuarioRepository.ExistUserByIdAsync(projeto.UsuarioId) == false)
                        erros.Add($"Usuário não encontrado.");
                    break;
                case Operation.Delete:
                    if (await _tarefaRepository.ExistPendingTaskByProjectAsync(projeto.Id))
                        erros.Add($"É necessário finalizar ou remover todas as tarefas do projeto antes de excui-lo.");
                    break;
            }

            if (erros.Count != 0)
                throw new ValidationException(erros);
        }
    }
}
