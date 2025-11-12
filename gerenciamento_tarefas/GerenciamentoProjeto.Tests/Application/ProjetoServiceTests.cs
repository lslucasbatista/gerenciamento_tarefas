using GerenciamentoProjeto.Application.Exceptions;
using GerenciamentoProjeto.Application.Services;
using GerenciamentoProjeto.Domain.Entities;
using GerenciamentoProjeto.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace GerenciamentoProjeto.Tests.Application
{
    public class ProjetoServiceTests
    {
        private readonly Mock<IProjetoRepository> _repositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepoitoryMock;
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly ProjetoService _service;

        public ProjetoServiceTests()
        {
            _repositoryMock = new Mock<IProjetoRepository>();
            _usuarioRepoitoryMock = new Mock<IUsuarioRepository>();
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _service = new ProjetoService(_repositoryMock.Object, _usuarioRepoitoryMock.Object, _tarefaRepositoryMock.Object);
        }

        [Fact(DisplayName = "Deve inserir com sucesso")]
        public async Task InsertAsyncTest()
        {
            DateTime dataCriacao = DateTime.Now;

            Projeto projeto = new Projeto
            {
                Nome = "Projeto Teste",
                Descricao = "Descrição do projeto",
                UsuarioId = 1,
            };

            Projeto projetoInserido = new Projeto
            {
                Id = 1,
                Nome = "Projeto Teste",
                Descricao = "Descrição do projeto",
                UsuarioId = 1,
                DataCriacao = dataCriacao
            };

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Projeto>())).ReturnsAsync(projetoInserido);
            _usuarioRepoitoryMock.Setup(r => r.ExistUserByIdAsync(projeto.UsuarioId)).ReturnsAsync(true);

            Projeto resultado = await _service.InsertAsync(projeto);

            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Projeto>()), Times.Once);
            _usuarioRepoitoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Equal("Projeto Teste", resultado.Nome);
            Assert.NotEqual(resultado.DataCriacao, DateTime.MinValue);
            Assert.Equal(DateTime.Today, resultado.DataCriacao.Date);
        }

        [Fact(DisplayName = "Deve validar campos obrigatórios")]
        public async Task InsertAsyncValidationTest()
        {
            DateTime dataCriacao = DateTime.Now;

            Projeto projeto = new() { };

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.InsertAsync(projeto));

            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Projeto>()), Times.Never);
            _usuarioRepoitoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Never);

            Assert.Contains("O nome do projeto é obrigatório.", ex.Erros);
            Assert.Contains("A descrição do projeto é obrigatória.", ex.Erros);
            Assert.Contains("O usuário criador do projeto é obrigatório.", ex.Erros);
        }

        [Fact(DisplayName = "Deve validar usuario")]
        public async Task InsertAsyncValidationUserTest()
        {
            DateTime dataCriacao = DateTime.Now;

            Projeto projeto = new Projeto
            {
                Nome = "Projeto Teste",
                Descricao = "Descrição Teste",
                UsuarioId = 99,
            };

            _usuarioRepoitoryMock.Setup(r => r.ExistUserByIdAsync(projeto.UsuarioId)).ReturnsAsync(false);

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.InsertAsync(projeto));

            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Projeto>()), Times.Never);
            _usuarioRepoitoryMock.Verify(r => r.ExistUserByIdAsync(It.IsAny<int>()), Times.Once);

            Assert.Contains("Usuário não encontrado.", ex.Erros);
        }

        [Fact(DisplayName = "Deve deletar projeto com sucesso")]
        public async Task DeleteAsync()
        {
            Projeto projeto = new Projeto { Id = 1, Nome = "Projeto Teste" };

            _repositoryMock.Setup(r => r.GetByIdAsync(projeto.Id)).ReturnsAsync(projeto);
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Projeto>())).Returns(Task.CompletedTask);
            _tarefaRepositoryMock.Setup(r => r.ExistPendingTaskByProjectAsync(It.IsAny<int>())).ReturnsAsync(false);

            await _service.DeleteAsync(projeto.Id);

            _repositoryMock.Verify(r => r.GetByIdAsync(projeto.Id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.Is<Projeto>(p => p.Id == projeto.Id)), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.ExistPendingTaskByProjectAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Deve validar projeto inexistente")]
        public async Task DeleteValidationNotFoundAsync()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Projeto)null);

            NotFoundException ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99));

            Assert.Contains("Projeto não encontrado.", ex.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Projeto>()), Times.Never);
            _tarefaRepositoryMock.Verify(r => r.ExistPendingTaskByProjectAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = "Deve validar projeto com tarefas não concluídas")]
        public async Task DeleteValidationProjectWithTaskAsync()
        {
            Projeto projeto = new Projeto { Id = 1, Nome = "Projeto Teste" };

            _repositoryMock.Setup(r => r.GetByIdAsync(projeto.Id)).ReturnsAsync(projeto);
            _tarefaRepositoryMock.Setup(r => r.ExistPendingTaskByProjectAsync(projeto.Id)).ReturnsAsync(true);

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteAsync(projeto.Id));

            _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Projeto>()), Times.Never);
            _tarefaRepositoryMock.Verify(r => r.ExistPendingTaskByProjectAsync(It.IsAny<int>()), Times.Once);

            Assert.Contains("É necessário finalizar ou remover todas as tarefas do projeto antes de excui-lo.", ex.Erros);
        }
    }
}
