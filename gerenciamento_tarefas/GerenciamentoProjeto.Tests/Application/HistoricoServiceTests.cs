using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Services;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace GerenciamentoProjeto.Tests.Application
{
    public class HistoricoServiceTests
    {
        private readonly Mock<IHistoricoRepository> _repositoryMock;
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly HistoricoService _service;

        public HistoricoServiceTests()
        {
            _repositoryMock = new Mock<IHistoricoRepository>();
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _service = new HistoricoService(_repositoryMock.Object, _tarefaRepositoryMock.Object, _usuarioRepositoryMock.Object);
        }

        [Fact(DisplayName = "Deve inserir com sucesso")]
        public async Task InsertAsyncTest()
        {
            Historico historico = new() { TarefaId = 1, UsuarioId = 2, Comentario = "Comentário válido" };

            Historico historicoInserido = new() { Id = 10, TarefaId = 1, UsuarioId = 2, Comentario = "Comentário válido" };

            _tarefaRepositoryMock.Setup(r => r.ExistTarefaByIdAsync(historico.TarefaId)).ReturnsAsync(true);
            _usuarioRepositoryMock.Setup(r => r.ExistUserByIdAsync(historico.UsuarioId)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Historico>())).ReturnsAsync(historicoInserido);

            Historico resultado = await _service.Insert(historico);

            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Historico>()), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.ExistTarefaByIdAsync(historico.TarefaId), Times.Once);
            _usuarioRepositoryMock.Verify(r => r.ExistUserByIdAsync(historico.UsuarioId), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal(10, resultado.Id);
            Assert.Equal(historico.Comentario, resultado.Comentario);
        }

        [Fact(DisplayName = "Deve validar campos obrigatórios")]
        public async Task InsertAsyncValidationTest()
        {
            Historico historico = new() { };

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.Insert(historico));

            _tarefaRepositoryMock.Verify(r => r.ExistTarefaByIdAsync(It.IsAny<int>()), Times.Never);
            _usuarioRepositoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Never);

            Assert.Contains("A tarefa é obrigatória.", ex.Erros);
            Assert.Contains("O usuário é obrigatório.", ex.Erros);
            Assert.Contains("O comentário é obrigatório.", ex.Erros);
        }

        [Fact(DisplayName = "Deve validar usuario e projeto")]
        public async Task InsertAsyncValidationUserTest()
        {
            Historico historico = new() { TarefaId = 99, UsuarioId = 99, Comentario = "Teste" };

            _usuarioRepositoryMock.Setup(r => r.ExistUserByIdAsync(historico.UsuarioId)).ReturnsAsync(false);
            _tarefaRepositoryMock.Setup(r => r.ExistTarefaByIdAsync(historico.TarefaId)).ReturnsAsync(false);

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.Insert(historico));

            _tarefaRepositoryMock.Verify(r => r.ExistTarefaByIdAsync(It.IsAny<int>()), Times.Once);
            _usuarioRepositoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Once);

            Assert.Contains("Tarefa não encontrada.", ex.Erros);
            Assert.Contains("Usuário não encontrado.", ex.Erros);
        }
    }
}
