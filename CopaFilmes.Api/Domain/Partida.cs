using System.Linq;
using System;

namespace CopaFilmes.Api.Domain
{
    public class Partida
    {
        private readonly Filme _primeiroCompetidor;
        private readonly Filme _segundoCompetidor;

        public Partida(Filme primeiroCompetidor, Filme segundoCompetidor)
        {
            _primeiroCompetidor = primeiroCompetidor ?? throw new ArgumentNullException("Primeiro Competidor não deve ser nulo");
            _segundoCompetidor = segundoCompetidor ?? throw new ArgumentNullException("Segundo Competidor não deve ser nulo"); ;
        }

        public (Filme Vencedor, Filme Perdedor) RealizarPartida()
        {
            if (this._primeiroCompetidor.Nota > this._segundoCompetidor.Nota)
                return (this._primeiroCompetidor, this._segundoCompetidor);
            else if (this._primeiroCompetidor.Nota < this._segundoCompetidor.Nota)
                return (this._segundoCompetidor, this._primeiroCompetidor);
            else
            {
                int comparacao = String.Compare(this._primeiroCompetidor.Titulo, this._segundoCompetidor.Titulo, StringComparison.CurrentCultureIgnoreCase);
                return (comparacao <= 0) ? (this._primeiroCompetidor, this._segundoCompetidor) : (this._segundoCompetidor, this._primeiroCompetidor);
            }

        }
    }
}