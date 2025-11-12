using System.Text.Json;
using GerenciamentoProjeto.Application.DTOs;
using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Services;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace GerenciamentoProjeto.Tests.Application
{
    public class RelatorioServiceTests
    {
        private readonly Mock<IRelatorioRepository> _repositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly RelatorioService _service;

        public RelatorioServiceTests()
        {
            _repositoryMock = new Mock<IRelatorioRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _service = new RelatorioService(_repositoryMock.Object, _usuarioRepositoryMock.Object, _tarefaRepositoryMock.Object);
        }

        [Fact(DisplayName = "Deve retornar os dados com sucesso")]
        public async Task GetUsersPerformanceAsyncTest()
        {
            DateTime dataInicio = DateTime.Today.AddDays(-30);
            DateTime dataFim = DateTime.Today;
            Usuario usuario = new() { Id = 1, Nome = "Lucas", CargoId = 1 };

            List<Tarefa> tarefas =
            [
                new Tarefa { UsuarioId = 1, Usuario = usuario },
                new Tarefa { UsuarioId = 1, Usuario = usuario },
                new Tarefa { UsuarioId = 2, Usuario = new() { Nome = "Lorena" } }
            ];

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
            _repositoryMock.Setup(r => r.GetUsersPerformanceAsync(dataInicio, dataFim)).ReturnsAsync(tarefas);

            IEnumerable<RelatorioPerformanceDTO> resultado = await _service.GetUsersPerformanceAsync(usuario.Id, dataInicio, dataFim);

            _usuarioRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.GetUsersPerformanceAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            RelatorioPerformanceDTO primeiraLinha = resultado.First(r => r.Usuario == "Lucas");
            Assert.Equal(2, primeiraLinha.Quantidade);
            Assert.True(primeiraLinha.MediaDiaria > 0);
        }

        [Fact(DisplayName = "Deve validar usuário inexistente")]
        public async Task GetUsersPerformanceAsyncValidationTest()
        {
            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Usuario)null);

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.GetUsersPerformanceAsync(99, DateTime.Now.AddDays(-10), DateTime.Now));

            _usuarioRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.GetUsersPerformanceAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);

            Assert.Contains("Usuário não encontrado.", ex.Erros);
        }

        [Fact(DisplayName = "Deve validar o usuário não gerente")]
        public async Task GetUsersPerformanceAsyncValidationGerenteTest()
        {
            Usuario usuario = new() { Id = 1, Nome = "Lucas", CargoId = 99 };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(usuario);

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.GetUsersPerformanceAsync(usuario.Id, DateTime.Now.AddDays(-10), DateTime.Now));

            _usuarioRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.GetUsersPerformanceAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);

            Assert.Contains("Apenas gerentes podem emitir relatórios.", ex.Erros);
        }

        [Fact(DisplayName = "Deve retornar os dados com sucesso")]
        public async Task GetAllTaskByProjectAsyncTest()
        {
            Usuario usuario = new() { Id = 1, Nome = "Lucas", CargoId = 1 };

            List<Tarefa> tarefas =
            [
                new Tarefa { Id = 1, ProjetoId = 1, Projeto = new Projeto { Nome = "Projeto A" }, StatusId = 1, PrioridadeId = 1 },
                new Tarefa { Id = 2, ProjetoId = 1, Projeto = new Projeto { Nome = "Projeto A" }, StatusId = 2, PrioridadeId = 2 },
                new Tarefa { Id = 3, ProjetoId = 1, Projeto = new Projeto { Nome = "Projeto A" }, StatusId = 3, PrioridadeId = 3 },
                new Tarefa { Id = 4, ProjetoId = 2, Projeto = new Projeto { Nome = "Projeto B" }, StatusId = 1, PrioridadeId = 2 }
            ];

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
            _tarefaRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tarefas);

            IEnumerable<RelatorioTaskByProjectDTO> resultado = await _service.GetAllTaskByProjectAsync(1);

            _usuarioRepositoryMock.Verify(r => r.GetByIdAsync(usuario.Id), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            RelatorioTaskByProjectDTO projetoA = resultado.First(r => r.Projeto == "Projeto A");
            Assert.Equal(3, projetoA.Quantidade);
            Assert.Equal(1, projetoA.QuantidadePendente);
            Assert.Equal(1, projetoA.QuantidadeAndamento);
            Assert.Equal(1, projetoA.QuantidadeConcluida);
            Assert.Equal(1, projetoA.QuantidadeBaixa);
            Assert.Equal(1, projetoA.QuantidadeMedia);
            Assert.Equal(1, projetoA.QuantidadeAlta);

            RelatorioTaskByProjectDTO projetoB = resultado.First(r => r.Projeto == "Projeto B");
            Assert.Equal(1, projetoB.Quantidade);
            Assert.Equal(1, projetoB.QuantidadePendente);
            Assert.Equal(0, projetoB.QuantidadeAndamento);
            Assert.Equal(0, projetoB.QuantidadeConcluida);
            Assert.Equal(0, projetoB.QuantidadeBaixa);
            Assert.Equal(1, projetoB.QuantidadeMedia);
            Assert.Equal(0, projetoB.QuantidadeAlta);
        }

        [Fact(DisplayName = "Deve validar usuário inexistente")]
        public async Task GetAllTaskByProjectAsyncValidationTest()
        {
            int idUsuario = 99;

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Usuario)null);

            ValidationException ex =  await Assert.ThrowsAsync<ValidationException>(() => _service.GetAllTaskByProjectAsync(idUsuario));

            _usuarioRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.GetAllAsync(), Times.Never);

            Assert.Contains("Usuário não encontrado.", ex.Erros);
        }

        [Fact(DisplayName = "Deve validar o usuário não gerente")]
        public async Task GetAllTaskByProjectAsyncValidationGerenteTest()
        {
            Usuario usuario = new() { Id = 1, Nome = "Lucas", CargoId = 99 };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(usuario);

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.GetAllTaskByProjectAsync(usuario.Id));

            _usuarioRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.GetAllAsync(), Times.Never);

            Assert.Contains("Apenas gerentes podem emitir relatórios.", ex.Erros);
        }
    }
}
