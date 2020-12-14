using System.Collections.Generic;
using System.Linq;
using System;

namespace CopaFilmes.Api.Domain
{
    public class Competicao
    {
        public IEnumerable<Filme> FilmesParticipantes { get; }
        public Competicao(IEnumerable<Filme> filmes)
        {
            if (filmes == null) throw new ArgumentNullException();
            if (filmes.Count() != 8) throw new ArgumentException("A lista de filmes não possui 8 itens");

            this.FilmesParticipantes = this.OrdenarFilmes(filmes);
        }

        private IEnumerable<Filme> OrdenarFilmes(IEnumerable<Filme> filmes) => filmes.OrderBy(f => f.Titulo, StringComparer.CurrentCultureIgnoreCase);

        public (Filme Campeao, Filme ViceCampeao) RealizarCompeticao()
        {
            var partidas = this.DefinirPartidas(this.FilmesParticipantes);
            while (partidas.Count() > 1)
            {
                var listaVencedores = partidas.Select(p => p.RealizarPartida().Vencedor);
                partidas = this.DefinirPartidas(listaVencedores);
            }

            return partidas.Single().RealizarPartida();
        }

        private IEnumerable<Partida> DefinirPartidas(IEnumerable<Filme> filmes)
        {
            int quantidadeFilmes = filmes.Count();
            List<Partida> partidas = new List<Partida>(quantidadeFilmes / 2);

            for (int posicao = 0; posicao < (quantidadeFilmes / 2); posicao++)
            {
                int posicaoSegundoCompetidor = quantidadeFilmes - 1 - posicao;
                partidas.Add(new Partida(filmes.ElementAt(posicao), filmes.ElementAt(posicaoSegundoCompetidor)));
            }

            return partidas;
        }
    }
}