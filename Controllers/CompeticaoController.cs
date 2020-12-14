using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Infra.Repository;
using Microsoft.AspNetCore.Mvc;
using Views;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Controllers
{
    [Route("competicao")]
    public class CompeticaoController : ControllerBase
    {
        private readonly IFilmesRepository _filmesRepository;
        private readonly ILogger<CompeticaoController> _logger;

        public CompeticaoController(IFilmesRepository filmesRepository, ILogger<CompeticaoController> logger)
        {
            this._filmesRepository = filmesRepository;
            this._logger = logger;
        }

        [HttpGet("filmes")]
        public async Task<ActionResult<IEnumerable<FilmeView>>> ObterFilmesCompetidoresAsync()
        {
            try
            {
                var filmes = await this._filmesRepository.ObterFilmes();
                return Ok(filmes.Select(f => new FilmeView(f.Id, f.Titulo, f.Ano)));
            }
            catch (System.Exception ex)
            {
                this._logger.LogInformation($"Ocorreu um erro na api. Detalhes: {ex.Message.ToString()}");
                return StatusCode(500, $"Ocorreu um erro na api. Detalhes: {ex.Message}");
            }
        }

    }
}