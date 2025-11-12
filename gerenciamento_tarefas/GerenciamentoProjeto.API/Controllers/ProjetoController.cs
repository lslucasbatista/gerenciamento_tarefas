using AutoMapper;
using GerenciamentoProjeto.Application.DTOs;
using GerenciamentoProjeto.Application.Interfaces;
using GerenciamentoProjeto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoProjeto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController(IProjetoService service, IMapper mapper) : ControllerBase
    {
        private readonly IProjetoService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ProjetoCreateDTO dto)
        {
            Projeto projeto = _mapper.Map<Projeto>(dto);

            projeto = await _service.InsertAsync(projeto);

            ProjetoCreatedDTO dtoInserido = _mapper.Map<ProjetoCreatedDTO>(projeto);

            return Ok(dtoInserido);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("getallprojectuser")]
        public async Task<IEnumerable<ProjetoByUsuarioDTO>> GetAllProjectUser(int id)
        {
            IEnumerable<Projeto> projetos = await service.GetAllProjectUserAsync(id);

            IEnumerable<ProjetoByUsuarioDTO> projetosDTO = _mapper.Map<IEnumerable<ProjetoByUsuarioDTO>>(projetos);

            return projetosDTO;
        }
    }
}
