using System.Text.Json;
using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Services;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace GerenciamentoProjeto.Tests.Application
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _repositoryMock;
        private readonly Mock<IProjetoRepository> _projetoRepositoryMock;
        private readonly Mock<IHistoricoRepository> _historicoRepoitoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepoitoryMock;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _repositoryMock = new Mock<ITarefaRepository>();
            _projetoRepositoryMock = new Mock<IProjetoRepository>();
            _historicoRepoitoryMock = new Mock<IHistoricoRepository>();
            _usuarioRepoitoryMock = new Mock<IUsuarioRepository>();
            _service = new TarefaService(_repositoryMock.Object, _projetoRepositoryMock.Object, _historicoRepoitoryMock.Object, _usuarioRepoitoryMock.Object);
        }

        [Fact(DisplayName = "Deve inserir com sucesso")]
        public async Task InsertAsyncTest()
        {
            DateTime dataCriacao = DateTime.Now;

            Tarefa tarefa = new Tarefa
            {
                Titulo = "Tarefa Teste",
                ProjetoId = 1,
                StatusId = 1,
                PrioridadeId = 1,
                UsuarioId = 1,
                Descricao = "Descrição da tarefa",
                DataVencimento = new DateTime(2026, 12, 31),
            };

            Tarefa tarefaInserida = new Tarefa
            {
                Titulo = "Tarefa Teste",
                ProjetoId = 1,
                StatusId = 1,
                PrioridadeId = 1,
                UsuarioId = 1,
                Descricao = "Descrição da tarefa",
                DataVencimento = new DateTime(2026, 12, 31),
                DataUltimaAtualizacao = dataCriacao,
                DataCriacao = dataCriacao,
            };

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Tarefa>())).ReturnsAsync(tarefaInserida);
            _repositoryMock.Setup(r => r.ExistStatusByIdAsync(tarefa.StatusId)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.ExistPrioridadeByIdAsync(tarefa.PrioridadeId)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.GetCountTaskbyProjectAsync(tarefa.ProjetoId)).ReturnsAsync(2);
            _projetoRepositoryMock.Setup(r => r.ExistProjectByIdAsync(tarefa.ProjetoId)).ReturnsAsync(true);
            _usuarioRepoitoryMock.Setup(r => r.ExistUserByIdAsync(tarefa.UsuarioId)).ReturnsAsync(true);

            Tarefa resultado = await _service.InsertAsync(tarefa);

            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Tarefa>()), Times.Once);
            _repositoryMock.Verify(r => r.ExistStatusByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.ExistPrioridadeByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.GetCountTaskbyProjectAsync(It.IsAny<int>()), Times.Once);
            _projetoRepositoryMock.Verify(r => r.ExistProjectByIdAsync(It.IsAny<int>()), Times.Once);
            _usuarioRepoitoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(tarefa.Titulo, resultado.Titulo);
            Assert.NotEqual(resultado.DataCriacao, DateTime.MinValue);
            Assert.NotEqual(resultado.DataUltimaAtualizacao, DateTime.MinValue);
            Assert.Equal(DateTime.Today, resultado.DataCriacao.Date);
        }

        [Fact(DisplayName = "Deve validar campos obrigatórios")]
        public async Task InsertAsyncValidationTest()
        {
            DateTime dataCriacao = DateTime.Now;

            Tarefa tarefa = new() { };

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.InsertAsync(tarefa));

            _repositoryMock.Verify(r => r.InsertAsync(tarefa), Times.Never);

            Assert.Contains("O projeto é obrigatório.", ex.Erros);
            Assert.Contains("O status é obrigatório.", ex.Erros);
            Assert.Contains("A prioridade é obrigatório.", ex.Erros);
            Assert.Contains("O usuário é obrigatório.", ex.Erros);
            Assert.Contains("O título é obrigatório.", ex.Erros);
            Assert.Contains("A data de vencimento é obrigatória.", ex.Erros);
        }

        [Fact(DisplayName = "Deve validar projeto, status, prioridade e usuario")]
        public async Task InsertAsyncValidationFKTest()
        {
            DateTime dataCriacao = DateTime.Now;

            Tarefa tarefa = new()
            {
                ProjetoId = 99,
                StatusId = 99,
                PrioridadeId = 99,
                Titulo = "Tarefa Teste",
                Descricao = "Descricao Teste",
                DataVencimento = DateTime.Now,
                UsuarioId = 99,
            };

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.InsertAsync(tarefa));

            _repositoryMock.Verify(r => r.InsertAsync(tarefa), Times.Never);

            Assert.Contains("Projeto não encontrado.", ex.Erros);
            Assert.Contains("Status não encontrado.", ex.Erros);
            Assert.Contains("Prioridade não encontrada.", ex.Erros);
            Assert.Contains("Usuário não encontrado.", ex.Erros);
        }

        [Fact(DisplayName = "Deve deletar tarefa com sucesso")]
        public async Task DeleteAsync()
        {
            Tarefa tarefa = new() { Id = 1, Titulo = "Tarefa Teste" };

            _repositoryMock.Setup(r => r.GetByIdAsync(tarefa.Id)).ReturnsAsync(tarefa);
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);

            await _service.DeleteAsync(tarefa.Id);

            _repositoryMock.Verify(r => r.GetByIdAsync(tarefa.Id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.Is<Tarefa>(p => p.Id == tarefa.Id)), Times.Once);
        }

        [Fact(DisplayName = "Deve validar tarefa inexistente")]
        public async Task DeleteAsyncValidationNotFound()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Tarefa)null);

            NotFoundException ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99));

            _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Tarefa>()), Times.Never);

            Assert.Contains("Tarefa não encontrada.", ex.Message);
        }

        [Fact(DisplayName = "Deve atualizar com sucesso")]
        public async Task UpdateAsync()
        {
            Tarefa tarefaExistente = new()
            {
                Id = 1,
                Titulo = "Título antigo",
                Descricao = "Descrição antiga",
                StatusId = 1,
                DataCriacao = new DateTime(2025, 11, 10),
                DataUltimaAtualizacao = new DateTime(2025, 11, 11),
                PrioridadeId = 1,
                ProjetoId = 1,
                UsuarioId = 1,
                DataVencimento = new DateTime(2025, 12, 31)
            };

            Tarefa tarefaUpdate = new()
            {
                Id = 1,
                Titulo = "Título novo",
                Descricao = "Descrição nova",
                StatusId = 2,
                ProjetoId = 2,
                UsuarioId = 2,
                DataVencimento = new DateTime(2025, 11, 20),
                DataUltimaAtualizacao = DateTime.Now,
                DataCriacao = new DateTime(2025, 11, 10),
                PrioridadeId = 1,
            };

            Historico historico = new Historico
            {
                TarefaId = tarefaUpdate.Id,
                Original = JsonSerializer.Serialize(tarefaExistente),
                Alteracao = JsonSerializer.Serialize(tarefaUpdate),
                DataModificacao = DateTime.UtcNow,
                UsuarioId = tarefaUpdate.UsuarioId
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(tarefaUpdate.Id)).ReturnsAsync(tarefaExistente);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Tarefa>())).ReturnsAsync(tarefaUpdate);
            _historicoRepoitoryMock.Setup(r => r.InsertAsync(It.IsAny<Historico>())).ReturnsAsync(historico);
            _repositoryMock.Setup(r => r.ExistStatusByIdAsync(tarefaExistente.StatusId)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.ExistPrioridadeByIdAsync(tarefaExistente.PrioridadeId)).ReturnsAsync(true);
            _projetoRepositoryMock.Setup(r => r.ExistProjectByIdAsync(tarefaExistente.ProjetoId)).ReturnsAsync(true);
            _usuarioRepoitoryMock.Setup(r => r.ExistUserByIdAsync(tarefaExistente.UsuarioId)).ReturnsAsync(true);

            Tarefa resultado = await _service.UpdateAsync(tarefaUpdate);

            _repositoryMock.Verify(r => r.GetByIdAsync(tarefaUpdate.Id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tarefa>()), Times.Once);
            _historicoRepoitoryMock.Verify(r => r.InsertAsync(It.IsAny<Historico>()), Times.Once);
            _repositoryMock.Verify(r => r.ExistStatusByIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.ExistPrioridadeByIdAsync(It.IsAny<int>()), Times.Once);
            _projetoRepositoryMock.Verify(r => r.ExistProjectByIdAsync(It.IsAny<int>()), Times.Once);
            _usuarioRepoitoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(tarefaUpdate.Titulo, resultado.Titulo);
            Assert.Equal(tarefaUpdate.Descricao, resultado.Descricao);
            Assert.Equal(tarefaUpdate.StatusId, resultado.StatusId);
            Assert.Equal(tarefaExistente.Id, resultado.Id);
            Assert.True(resultado.DataUltimaAtualizacao.Date == DateTime.Now.Date);            
        }

        [Fact(DisplayName = "Deve retornar lista com sucesso")]
        public async Task GetAllTaskProjectAsync()
        {
            int projetoId = 1;
            List<Tarefa> tarefasEsperadas =
            [
                new Tarefa { Id = 1, Titulo = "Tarefa 1", ProjetoId = projetoId },
                new Tarefa { Id = 2, Titulo = "Tarefa 2", ProjetoId = projetoId }
            ];

            _projetoRepositoryMock.Setup(r => r.ExistProjectByIdAsync(projetoId)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.GetAllTaskProjectAsync(projetoId)).ReturnsAsync(tarefasEsperadas);

            IEnumerable<Tarefa> resultado = await _service.GetAllTaskProjectAsync(projetoId);

            _projetoRepositoryMock.Verify(r => r.ExistProjectByIdAsync(projetoId), Times.Once);
            _repositoryMock.Verify(r => r.GetAllTaskProjectAsync(projetoId), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Tarefa 1", resultado.First().Titulo);
        }

        [Fact(DisplayName = "Deve validar projeto não encontrado.")]
        public async Task GetAllTaskProjectAsyncValidation()
        {
            int projetoId = 99;

            _projetoRepositoryMock.Setup(r => r.ExistProjectByIdAsync(projetoId)).ReturnsAsync(false);

            NotFoundException ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetAllTaskProjectAsync(projetoId));

            _repositoryMock.Verify(r => r.GetAllTaskProjectAsync(It.IsAny<int>()), Times.Never);

            Assert.Equal("Projeto não encontrado.", ex.Message);
        }
    }
}
