using System.Collections.Generic;
using System.Threading.Tasks;
using CopaFilmes.Api.Domain;

namespace CopaFilmes.Api.Infra.Repository
{
    public interface IFilmesRepository
    {
        Task<IEnumerable<Filme>> ObterFilmesAsync();
    }
}