using CopaFilmes.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace CopaFilmes.Test.Domain
{
    public class CompeticaoTest
    {
        [Fact]
        public void Competicao_ListaNulo_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Competicao(null));
        }

        [Fact]
        public void Competicao_ListaVazia_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Competicao(Enumerable.Empty<Filme>()));
        }

        [Fact]
        public void Competicao_ListaComMaisDe8Itens_ArgumentException()
        {
            var filmes = Enumerable.Repeat(new Filme("IdFilme", "TituloFilme", 2020, 8.5M), 10);

            Assert.Throws<ArgumentException>(() => new Competicao(filmes));
        }

        [Fact]
        public void Competicao_ListaCom8Itens_ListaOrdenada()
        {
            var filmes = new List<Filme>(8);
            filmes.Add(new Filme("1", "Os Incríveis 2", 2000, 8.0M));
            filmes.Add(new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 8.0M));
            filmes.Add(new Filme("3", "Oito Mulheres e um Segredo", 2000, 8.0M));
            filmes.Add(new Filme("4", "Hereditário", 2000, 8.0M));
            filmes.Add(new Filme("5", "Vingadores: Guerra Infinita", 2000, 8.0M));
            filmes.Add(new Filme("6", "Deadpool 2", 2000, 8.0M));
            filmes.Add(new Filme("7", "Han Solo: Uma História Star Wars", 2000, 8.0M));
            filmes.Add(new Filme("8", "Thor: Ragnarok", 2000, 8.0M));

            var competicao = new Competicao(filmes);

            Assert.Collection(competicao.FilmesParticipantes,
                                filme => Assert.Equal("Deadpool 2", filme.Titulo),
                                filme => Assert.Equal("Han Solo: Uma História Star Wars", filme.Titulo),
                                filme => Assert.Equal("Hereditário", filme.Titulo),
                                filme => Assert.Equal("Jurassic World: Reino Ameaçado", filme.Titulo),
                                filme => Assert.Equal("Oito Mulheres e um Segredo", filme.Titulo),
                                filme => Assert.Equal("Os Incríveis 2", filme.Titulo),
                                filme => Assert.Equal("Thor: Ragnarok", filme.Titulo),
                                filme => Assert.Equal("Vingadores: Guerra Infinita", filme.Titulo));
        }


        [Fact]
        public void RealizarCompeticao_ListaCom8Itens_Campeao()
        {
            var filmes = new List<Filme>(8);
            filmes.Add(new Filme("1", "Os Incríveis 2", 2000, 8.5M));
            filmes.Add(new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 6.7M));
            filmes.Add(new Filme("3", "Oito Mulheres e um Segredo", 2000, 6.3M));
            filmes.Add(new Filme("4", "Hereditário", 2000, 7.8M));
            filmes.Add(new Filme("5", "Vingadores: Guerra Infinita", 2000, 8.8M));
            filmes.Add(new Filme("6", "Deadpool 2", 2000, 8.1M));
            filmes.Add(new Filme("7", "Han Solo: Uma História Star Wars", 2000, 7.2M));
            filmes.Add(new Filme("8", "Thor: Ragnarok", 2000, 7.9M));

            var competicao = new Competicao(filmes);
            var resultado = competicao.RealizarCompeticao();

            Assert.Equal("Vingadores: Guerra Infinita", resultado.Campeao.Titulo);
        }

        [Fact]
        public void RealizarCompeticao_ListaCom8Itens_ViceCampeao()
        {
            var filmes = new List<Filme>(8);
            filmes.Add(new Filme("1", "Os Incríveis 2", 2000, 8.5M));
            filmes.Add(new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 6.7M));
            filmes.Add(new Filme("3", "Oito Mulheres e um Segredo", 2000, 6.3M));
            filmes.Add(new Filme("4", "Hereditário", 2000, 7.8M));
            filmes.Add(new Filme("5", "Vingadores: Guerra Infinita", 2000, 8.8M));
            filmes.Add(new Filme("6", "Deadpool 2", 2000, 8.1M));
            filmes.Add(new Filme("7", "Han Solo: Uma História Star Wars", 2000, 7.2M));
            filmes.Add(new Filme("8", "Thor: Ragnarok", 2000, 7.9M));

            var competicao = new Competicao(filmes);
            var resultado = competicao.RealizarCompeticao();

            Assert.Equal("Os Incríveis 2", resultado.ViceCampeao.Titulo);
        }
    }
}