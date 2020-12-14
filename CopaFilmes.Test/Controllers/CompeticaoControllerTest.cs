using System.Threading.Tasks;
using CopaFilmes.Api.Controllers;
using Xunit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using CopaFilmes.Api.Infra.Repository;
using System.Linq;
using System.Collections.Generic;
using CopaFilmes.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using CopaFilmes.Api.Views;

namespace CopaFilmes.Test.Controllers
{
    public class CompeticaoControllerTest
    {
        private List<Filme> _listaFilmes = new List<Filme>(){
                new Filme("1", "Os Incríveis 2", 2000, 8.5M),
                new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 6.7M),
                new Filme("3", "Oito Mulheres e um Segredo", 2000, 6.3M),
                new Filme("4", "Hereditário", 2000, 7.8M),
                new Filme("5", "Vingadores: Guerra Infinita", 2000, 8.8M),
                new Filme("6", "Deadpool 2", 2000, 8.1M),
                new Filme("7", "Han Solo: Uma História Star Wars", 2000, 7.2M),
                new Filme("8", "Thor: Ragnarok", 2000, 7.9M),
                new Filme("9", "Te Peguei!", 2000, 6.9M),
                new Filme("10", "A Barraca do Beijo", 2000, 6.8M),
            };

        [Fact]
        public void CompeticaoController_FilmesRepositoryNulo_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CompeticaoController(null));

        }


        #region "Metodo EnviarFilmesCompetidoresAsync"

        [Theory]
        [InlineData(null, "A lista não deve ser nula")] //Lista nulo
        [InlineData(new string[] { }, "A lista deve possuir 8 filmes")] // Lista vazia
        [InlineData(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" }, "A lista deve possuir 8 filmes")] // Lista com mais de 8 itens
        [InlineData(new string[] { "1", "1", "2", "3", "4", "5", "6", "7" }, "A lista não deve possuir filmes duplicados")] //Lista com duplicidade de itens
        [InlineData(new string[] { "IdInvalido", "1", "2", "3", "4", "5", "6", "7" }, "A lista deve possuir 8 filmes validos")] //Lista com ids invalidos
        public async Task EnviarFilmesCompetidoresAsync_ListasInvalidas_BadRequest(string[] idFilmesEnviado, string erroEsperado)
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Returns(_listaFilmes);

            var competicaoController = new CompeticaoController(filmesRepository);

            var resultado = await competicaoController.EnviarFilmesCompetidoresAsync(idFilmesEnviado);

            Assert.IsType<BadRequestObjectResult>(resultado.Result);
            string mensagemErro = (resultado.Result as BadRequestObjectResult).Value.ToString();
            Assert.Equal(erroEsperado, mensagemErro);
        }

        [Fact]
        public async Task EnviarFilmesCompetidoresAsync_ListasCom8Filmes_Ok()
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Returns(_listaFilmes);

            var competicaoController = new CompeticaoController(filmesRepository);

            var idFilmesEnviado = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var resultado = await competicaoController.EnviarFilmesCompetidoresAsync(idFilmesEnviado);

            Assert.IsType<OkObjectResult>(resultado.Result);
            FinalistasView finalistasView = (resultado.Result as OkObjectResult).Value as FinalistasView;

            Assert.Equal("Vingadores: Guerra Infinita", finalistasView.Campeao);
            Assert.Equal("Os Incríveis 2", finalistasView.ViceCampeao);
        }


        [Fact]
        public async Task EnviarFilmesCompetidoresAsync_ErroInesperado_InternalServerError()
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Throws(new Exception("Erro inesperado"));

            var competicaoController = new CompeticaoController(filmesRepository);

            var idFilmesEnviado = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var resultado = await competicaoController.EnviarFilmesCompetidoresAsync(idFilmesEnviado);

            Assert.IsType<ObjectResult>(resultado.Result);
            int? statusCode = (resultado.Result as ObjectResult).StatusCode;

            Assert.Equal(500, statusCode);
        }


        [Fact]
        public async Task EnviarFilmesCompetidoresAsync_RetornoVazio_NotFound()
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Returns(Enumerable.Empty<Filme>());

            var competicaoController = new CompeticaoController(filmesRepository);

            var idFilmesEnviado = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var resultado = await competicaoController.EnviarFilmesCompetidoresAsync(idFilmesEnviado);

            Assert.IsType<ObjectResult>(resultado.Result);
            int? statusCode = (resultado.Result as ObjectResult).StatusCode;

            Assert.Equal(500, statusCode);
        }


        #endregion

        #region "Metodo ObterFilmesCompetidoresAsync"

        [Fact]
        public async Task ObterFilmesCompetidoresAsync_RetornoComSucesso_Ok()
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Returns(_listaFilmes);

            var competicaoController = new CompeticaoController(filmesRepository);

            var resultado = await competicaoController.ObterFilmesCompetidoresAsync();

            Assert.IsType<OkObjectResult>(resultado.Result);
            IEnumerable<FilmeView> filmesView = (resultado.Result as OkObjectResult).Value as IEnumerable<FilmeView>;

            Assert.NotEmpty(filmesView);
        }

        [Fact]
        public async Task ObterFilmesCompetidoresAsync_RetornoVazio_NotFound()
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Returns(Enumerable.Empty<Filme>());

            var competicaoController = new CompeticaoController(filmesRepository);

            var resultado = await competicaoController.ObterFilmesCompetidoresAsync();

            Assert.IsType<NotFoundResult>(resultado.Result);
        }

        [Fact]
        public async Task ObterFilmesCompetidoresAsync_ErroInesperado_InternalServerError()
        {
            var filmesRepository = Substitute.For<IFilmesRepository>();
            filmesRepository.ObterFilmesAsync().Throws(new Exception("Erro inesperado"));

            var competicaoController = new CompeticaoController(filmesRepository);

            var resultado = await competicaoController.ObterFilmesCompetidoresAsync();

            Assert.IsType<ObjectResult>(resultado.Result);
            int? statusCode = (resultado.Result as ObjectResult).StatusCode;

            Assert.Equal(500, statusCode);
        }


        #endregion

    }


}