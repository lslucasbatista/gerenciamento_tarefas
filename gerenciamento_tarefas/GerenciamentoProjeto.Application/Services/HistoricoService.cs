using System.Text.Json;
using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Interfaces;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Domain.Enums;
using GerenciamentoProjeto.Infrastructure.Interfaces;

namespace GerenciamentoProjeto.Application.Services
{
    public class HistoricoService(IHistoricoRepository repository, ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository) : IHistoricoService
    {
        private readonly IHistoricoRepository _repository = repository;
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<Historico> Insert(Historico historico)
        {
            await Validate(Operation.Insert, historico);

            historico = await _repository.InsertAsync(historico);

            return historico;
        }

        private async Task Validate(Operation op, Historico historico)
        {
            if (historico == null)
                throw new NotFoundException($"Histórico não encontrado.");

            var erros = new List<string>();

            switch (op)
            {
                case Operation.Insert:
                    if (historico.TarefaId <= 0)
                        erros.Add($"A tarefa é obrigatória.");
                    else if (await _tarefaRepository.ExistTarefaByIdAsync(historico.TarefaId) == false)
                        erros.Add($"Tarefa não encontrada.");
                    if (historico.UsuarioId <= 0)
                        erros.Add($"O usuário é obrigatório.");
                    else if (await _usuarioRepository.ExistUserByIdAsync(historico.UsuarioId) == false)
                        erros.Add($"Usuário não encontrado.");                   
                    if (string.IsNullOrEmpty(historico.Comentario))
                        erros.Add($"O comentário é obrigatório.");

                    break;
            }

            if (erros.Count != 0)
                throw new ValidationException(erros);
        }
    }
}
