using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Infra.Repository
{
    public interface IFilmesRepository
    {
        Task<IEnumerable<Filme>> ObterFilmes();
    }
}