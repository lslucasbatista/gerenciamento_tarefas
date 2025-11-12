using AutoMapper;
using GerenciamentoProjeto.Application.DTOs;
using GerenciamentoProjeto.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoProjeto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController(IRelatorioService service, IMapper mapper) : ControllerBase
    {
        private readonly IRelatorioService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet("performancereport")]
        public async Task<ActionResult<IEnumerable<RelatorioPerformanceDTO>>> GetUsersPerformanceAsync(int idUsuarioEmissao, DateTime dataInicio, DateTime dataFim)
        {
            IEnumerable<RelatorioPerformanceDTO> relatorio = await _service.GetUsersPerformanceAsync(idUsuarioEmissao, dataInicio, dataFim);

            return Ok(relatorio);
        }

        [HttpGet("projecttasksreport")]
        public async Task<ActionResult<IEnumerable<RelatorioTaskByProjectDTO>>> GetAllTaskByProject(int idUsuarioEmissao)
        {
            IEnumerable<RelatorioTaskByProjectDTO> relatorio = await _service.GetAllTaskByProjectAsync(idUsuarioEmissao);

            return Ok(relatorio);
        }
    }
}
