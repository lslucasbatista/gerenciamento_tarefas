using AutoMapper;
using GerenciamentoProjeto.Application.DTOs;
using GerenciamentoProjeto.Domain.Entities;

namespace GerenciamentoProjeto.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Projeto, ProjetoCreateDTO>().ReverseMap();
            CreateMap<Projeto, ProjetoCreatedDTO>().ReverseMap();
            CreateMap<Projeto, ProjetoByUsuarioDTO>().ReverseMap();

            CreateMap<Tarefa, TarefaCreateDTO>().ReverseMap();
            CreateMap<Tarefa, TarefaCreatedDTO>().ReverseMap();
            CreateMap<Tarefa, TarefaUpdateDTO>().ReverseMap();
            CreateMap<Tarefa, TarefaByProjetoDTO>()
           .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => src.Status.Descricao))
           .ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario.Nome))
           .ForMember(dest => dest.PrioridadeDescricao, opt => opt.MapFrom(src => src.Prioridade.Descricao))
           .ForMember(dest => dest.Historico, opt => opt.MapFrom(src => src.Historico));

            CreateMap<Usuario, UsuarioCreateDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioCreatedDTO>().ReverseMap();

            CreateMap<Historico, ComentarioCreatedDTO>().ReverseMap();
            CreateMap<Historico, ComentarioCreateDTO>().ReverseMap();
            CreateMap<Historico, HistoricoteDTO>().ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario.Nome));
        }
    }
}
