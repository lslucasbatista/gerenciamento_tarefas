using GerenciamentoProjeto.Application.DTOs;

namespace GerenciamentoProjeto.Application.Interfaces
{
    public interface IRelatorioService
    {
        Task<IEnumerable<RelatorioPerformanceDTO>> GetUsersPerformanceAsync(int idUsuarioEmissaoRel, DateTime dataInicio, DateTime dataFim);

        Task<IEnumerable<RelatorioTaskByProjectDTO>> GetAllTaskByProjectAsync(int idUsuarioEmissaoRel);
    }
}
