using System;
using System.Text.Json;
using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Interfaces;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Domain.Enums;
using GerenciamentoProjeto.Infrastructure.Interfaces;

namespace GerenciamentoProjeto.Application.Services
{
    public class TarefaService(ITarefaRepository repository, IProjetoRepository projetoRepository, IHistoricoRepository historicoRepository, IUsuarioRepository usuarioRepository) : ITarefaService
    {
        private readonly ITarefaRepository _repository = repository;
        private readonly IProjetoRepository _projetoRepository = projetoRepository;
        private readonly IHistoricoRepository _historicoRepository = historicoRepository;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<Tarefa> InsertAsync(Tarefa tarefa)
        {
            tarefa.DataCriacao = DateTime.Now;
            tarefa.DataUltimaAtualizacao = DateTime.Now;

            await Validate(Operation.Insert, tarefa);

            tarefa = await _repository.InsertAsync(tarefa);

            return tarefa;
        }

        public async Task<Tarefa> UpdateAsync(Tarefa tarefaUpdate)
        {

            Tarefa tarefa = await _repository.GetByIdAsync(tarefaUpdate.Id);

            await Validate(Operation.Update, tarefa);

            string jsonAntes = JsonSerializer.Serialize(tarefa);

            tarefa.StatusId = tarefaUpdate.StatusId;
            tarefa.DataVencimento = tarefaUpdate.DataVencimento;
            tarefa.Descricao = tarefaUpdate.Descricao;
            tarefa.Titulo = tarefaUpdate.Titulo;
            tarefa.DataUltimaAtualizacao = DateTime.Now;

            string jsonDepois = JsonSerializer.Serialize(tarefa);

            tarefa = await _repository.UpdateAsync(tarefa);

            Historico historico = new()
            {
                TarefaId = tarefa.Id,
                Original = jsonAntes,
                Alteracao = jsonDepois,
                DataModificacao = DateTime.UtcNow,
                UsuarioId = tarefa.UsuarioId
            };

            await _historicoRepository.InsertAsync(historico);

            return tarefa;
        }

        public async Task DeleteAsync(int id)
        {
            Tarefa tarefa = await _repository.GetByIdAsync(id);

            await Validate(Operation.Delete, tarefa);

            await _repository.DeleteAsync(tarefa);
        }

        public async Task<IEnumerable<Tarefa>> GetAllTaskProjectAsync(int id)
        {
            bool existeProjeto = await _projetoRepository.ExistProjectByIdAsync(id);

            if (!existeProjeto)
                throw new NotFoundException($"Projeto não encontrado.");

            IEnumerable<Tarefa> tarefas = await _repository.GetAllTaskProjectAsync(id);
            return tarefas;
        }

        private async Task Validate(Operation op, Tarefa tarefa)
        {
            if (tarefa == null)
                throw new NotFoundException($"Tarefa não encontrada.");

            var erros = new List<string>();

            switch (op)
            {
                case Operation.Insert:
                    if (await _repository.GetCountTaskbyProjectAsync(tarefa.ProjetoId) == 20)
                        erros.Add($"O projeto atingiu o limite máximo de 20 tarefas.");
                    erros.AddRange(await ValidateInsertUpdate(tarefa));
                    break;
                case Operation.Update:
                    erros.AddRange(await ValidateInsertUpdate(tarefa));
                    break;
            }

            if (erros.Count != 0)
                throw new ValidationException(erros);
        }

        private async Task<List<string>> ValidateInsertUpdate(Tarefa tarefa)
        {
            var erros = new List<string>();

            if (tarefa.ProjetoId <= 0)
                erros.Add($"O projeto é obrigatório.");
            else if (await projetoRepository.ExistProjectByIdAsync(tarefa.ProjetoId) == false)
                erros.Add($"Projeto não encontrado.");
            if (tarefa.StatusId <= 0)
                erros.Add($"O status é obrigatório.");
            else if (await _repository.ExistStatusByIdAsync(tarefa.StatusId) == false)
                erros.Add($"Status não encontrado.");
            if (tarefa.PrioridadeId <= 0)
                erros.Add($"A prioridade é obrigatório.");
            else if (await _repository.ExistPrioridadeByIdAsync(tarefa.PrioridadeId) == false)
                erros.Add($"Prioridade não encontrada.");
            if (tarefa.UsuarioId <= 0)
                erros.Add($"O usuário é obrigatório.");
            else if (await _usuarioRepository.ExistUserByIdAsync(tarefa.UsuarioId) == false)
                erros.Add($"Usuário não encontrado.");
            if (string.IsNullOrEmpty(tarefa.Titulo))
                erros.Add($"O título é obrigatório.");
            if (string.IsNullOrEmpty(tarefa.Descricao))
                erros.Add($"A descrição é obrigatória.");
            if (tarefa.DataVencimento == DateTime.MinValue)
                erros.Add($"A data de vencimento é obrigatória.");

            return erros;
        }
    }
}
