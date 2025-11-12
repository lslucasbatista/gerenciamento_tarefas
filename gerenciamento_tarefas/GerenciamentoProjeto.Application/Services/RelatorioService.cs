using GerenciamentoProjeto.Application.DTOs;
using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Interfaces;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Interfaces;

namespace GerenciamentoProjeto.Application.Services
{
    public class RelatorioService(IRelatorioRepository repository, IUsuarioRepository usuarioRepository, ITarefaRepository tarefaRepository) : IRelatorioService
    {
        private readonly IRelatorioRepository _repository = repository;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;

        public async Task<IEnumerable<RelatorioPerformanceDTO>> GetUsersPerformanceAsync(int idUsuarioEmissaoRel, DateTime dataInicio, DateTime dataFim)
        {
            Usuario usuario = await _usuarioRepository.GetByIdAsync(idUsuarioEmissaoRel);

            Validate(usuario);

            DateTime dataInicioDia = dataInicio.Date;
            DateTime dataFimDia = dataFim.Date.AddDays(1).AddTicks(-1);
            int totalDia = (int)Math.Round((dataFimDia - dataInicioDia).TotalDays);

            IEnumerable<Tarefa> tarefas = await _repository.GetUsersPerformanceAsync(dataInicioDia, dataFimDia);

            IEnumerable<RelatorioPerformanceDTO> resultado = (from tarefa in tarefas
                                                              group tarefa by new { tarefa.Usuario.Id, tarefa.Usuario.Nome } into grupo
                                                              select new RelatorioPerformanceDTO
                                                              {
                                                                  Quantidade = grupo.Count(),
                                                                  Usuario = grupo.Key.Nome,
                                                                  MediaDiaria = grupo.Count() == 0 ? 0 : grupo.Count() / totalDia
                                                              });

            return resultado;
        }

        public async Task<IEnumerable<RelatorioTaskByProjectDTO>> GetAllTaskByProjectAsync(int idUsuarioEmissaoRel)
        {
            Usuario usuario = await _usuarioRepository.GetByIdAsync(idUsuarioEmissaoRel);

            Validate(usuario);

            IEnumerable<Tarefa> tarefas = await _tarefaRepository.GetAllAsync();

            IEnumerable<RelatorioTaskByProjectDTO> resultado = (from tarefa in tarefas
                                                                group tarefa by new { tarefa.ProjetoId, tarefa.Projeto.Nome } into grupo
                                                                select new RelatorioTaskByProjectDTO
                                                                {
                                                                    Projeto = grupo.Key.Nome,
                                                                    Quantidade = grupo.Count(),
                                                                    QuantidadePendente = grupo.Count(x => x.StatusId == 1),
                                                                    QuantidadeAndamento = grupo.Count(x => x.StatusId == 2),
                                                                    QuantidadeConcluida = grupo.Count(x => x.StatusId == 3),
                                                                    QuantidadeBaixa = grupo.Count(x => x.PrioridadeId == 1),
                                                                    QuantidadeMedia = grupo.Count(x => x.PrioridadeId == 2),
                                                                    QuantidadeAlta = grupo.Count(x => x.PrioridadeId == 3)
                                                                });

            return resultado;
        }

        public static void Validate(Usuario usuario)
        {
            var erros = new List<string>();

            if(usuario == null)
                erros.Add($"Usuário não encontrado.");
            else if (usuario.CargoId != 1)
                erros.Add($"Apenas gerentes podem emitir relatórios.");

            if (erros.Count != 0)
                throw new ValidationException(erros);
        }
    }
}
