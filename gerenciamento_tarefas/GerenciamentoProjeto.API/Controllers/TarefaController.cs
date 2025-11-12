using AutoMapper;
using GerenciamentoProjeto.Application.DTOs;
using GerenciamentoProjeto.Application.Interfaces;
using GerenciamentoProjeto.Application.Services;
using GerenciamentoProjeto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoProjeto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController(ITarefaService service, IHistoricoService historicoService, IMapper mapper) : ControllerBase
    {
        private readonly ITarefaService _service = service;
        private readonly IHistoricoService _historicoService = historicoService;
        private readonly IMapper _mapper = mapper;

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] TarefaCreateDTO dto)
        {
            Tarefa tarefa = _mapper.Map<Tarefa>(dto);

            tarefa = await _service.InsertAsync(tarefa);

            TarefaCreatedDTO dtoInserido = _mapper.Map<TarefaCreatedDTO>(tarefa);

            return Ok(dtoInserido);
        }

        [HttpPost("insertcomment")]
        public async Task<IActionResult> InsertComment(ComentarioCreateDTO dto)
        {
            Historico historico = _mapper.Map<Historico>(dto);

            historico = await _historicoService.Insert(historico);

            ComentarioCreatedDTO dtoInserido = _mapper.Map<ComentarioCreatedDTO>(historico);

            return Ok(dtoInserido);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TarefaUpdateDTO dto)
        {
            Tarefa tarefa = _mapper.Map<Tarefa>(dto);

            tarefa = await _service.UpdateAsync(tarefa);

            dto = _mapper.Map<TarefaUpdateDTO>(dto);

            return Ok(dto);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("getalltaskproject")]
        public async Task<IActionResult> GetAllTaskProject(int idProjeto)
        {
            IEnumerable<Tarefa> tarefas = await service.GetAllTaskProjectAsync(idProjeto);

            IEnumerable<TarefaByProjetoDTO> tarefasDTO = _mapper.Map<IEnumerable<TarefaByProjetoDTO>>(tarefas);

            return Ok(tarefasDTO);
        }
    }
}
