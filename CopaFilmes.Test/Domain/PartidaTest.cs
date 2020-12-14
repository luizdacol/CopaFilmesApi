using Xunit;
using System;
using CopaFilmes.Api.Domain;
using System.Collections.Generic;

namespace CopaFilmes.Test.Domain
{
    public class PartidaTest
    {
        [Fact]
        public void Partida_PrimeiroCompetidorNulo_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Partida(null, new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 6.7M)));
        }

        [Fact]
        public void Partida_SegundoCompetidorNulo_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Partida(new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 6.7M), null));
        }

        [Theory]
        [MemberData(nameof(PartidaParameters))]
        public void RealizarPartida_ValidaVencedor(Filme primeiroCompetidor, Filme segundoCompetidor, string vencedorEsperado)
        {
            var partida = new Partida(primeiroCompetidor, segundoCompetidor);

            var resultado = partida.RealizarPartida();

            Assert.Equal(vencedorEsperado, resultado.Vencedor.Titulo);
        }


        public static IEnumerable<object[]> PartidaParameters()
        {
            // Cenario 1: Primeiro competidor com nota maior
            // Vencedor Esperado: Primeiro competidor
            yield return new object[]{
                new Filme("2", "Jurassic World: Reino Ameaçado", 2000, 6.7M),
                new Filme("3", "Oito Mulheres e um Segredo", 2000, 6.3M),
                "Jurassic World: Reino Ameaçado"
                };

            // Cenario 2: Segundo competidor com nota maior
            // Vencedor Esperado: Segundo competidor
            yield return new object[]{
                new Filme("6", "Deadpool 2", 2000, 8.1M),
                new Filme("5", "Vingadores: Guerra Infinita", 2000, 8.8M),
                "Vingadores: Guerra Infinita"
                };

            // Cenario 3: Os dois competidores com notas iguais
            // Vencedor Esperado: Primeiro competidor, pelo critério de desempate
            yield return new object[]{
                new Filme("7", "Han Solo: Uma História Star Wars", 2000, 7.2M),
                new Filme("8", "Thor: Ragnarok", 2000, 7.2M),
                "Han Solo: Uma História Star Wars"
                };

            // Cenario 4: Os dois competidores com notas iguais
            // Vencedor Esperado: Segundo competidor, pelo critério de desempate
            yield return new object[]{
                new Filme("1", "Os Incríveis 2", 2000, 8.5M),
                new Filme("4", "Hereditário", 2000, 8.5M),
                "Hereditário"
                };
        }
    }
}