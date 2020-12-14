using System.Collections.Generic;
using System.Threading.Tasks;
using CopaFilmes.Api.Domain;
using CopaFilmes.Api.Infra.Repository;
using Microsoft.AspNetCore.Mvc;
using CopaFilmes.Api.Views;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace CopaFilmes.Api.Controllers
{
    [Route("competicao")]
    public class CompeticaoController : ControllerBase
    {
        private readonly IFilmesRepository _filmesRepository;
        private readonly ILogger<CompeticaoController> _logger;

        public CompeticaoController(IFilmesRepository filmesRepository, ILogger<CompeticaoController> logger = null)
        {
            this._filmesRepository = filmesRepository ?? throw new ArgumentNullException("filmesRepository não deve ser nulo");
            this._logger = logger;
        }

        [HttpGet("filmes")]
        public async Task<ActionResult<IEnumerable<FilmeView>>> ObterFilmesCompetidoresAsync()
        {
            try
            {
                var filmes = await this._filmesRepository.ObterFilmesAsync();
                if (!filmes.Any())
                    return NotFound();

                return Ok(filmes.Select(f => new FilmeView(f.Id, f.Titulo, f.Ano)));
            }
            catch (System.Exception ex)
            {
                this._logger?.LogInformation($"Ocorreu um erro na api. Detalhes: {ex.Message.ToString()}");
                return StatusCode(500, $"Ocorreu um erro na api. Detalhes: {ex.Message}");
            }
        }

        [HttpPost("filmes")]
        public async Task<ActionResult<FinalistasView>> EnviarFilmesCompetidoresAsync([FromBody] IEnumerable<string> idfilmesEnviados)
        {
            try
            {
                if (!this.ValidarFilmesEnviados(idfilmesEnviados, out string mensagemErro))
                    return BadRequest(mensagemErro);

                var filmesSelecionados = await this.ObterInformacoesFilmes(idfilmesEnviados);
                if (filmesSelecionados.Count() != 8)
                    return BadRequest("A lista deve possuir 8 filmes validos");

                var competicao = new Competicao(filmesSelecionados);
                var resultado = competicao.RealizarCompeticao();

                return Ok(new FinalistasView(resultado.Campeao, resultado.ViceCampeao));
            }
            catch (System.Exception ex)
            {
                this._logger?.LogInformation($"Ocorreu um erro no envio dos filmes. Detalhes: {ex.Message.ToString()}");
                return StatusCode(500, $"Ocorreu um erro no envio dos filmes. Detalhes: {ex.Message}");
            }
        }

        private bool ValidarFilmesEnviados(IEnumerable<string> filmesSelecionados, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (filmesSelecionados == null)
                mensagemErro = "A lista não deve ser nula";
            else if (filmesSelecionados.Count() != 8)
                mensagemErro = "A lista deve possuir 8 filmes";
            else if (filmesSelecionados.Distinct(StringComparer.CurrentCultureIgnoreCase).Count() != 8)
                mensagemErro = "A lista não deve possuir filmes duplicados";

            return string.IsNullOrEmpty(mensagemErro) ? true : false;
        }

        private async Task<IEnumerable<Filme>> ObterInformacoesFilmes(IEnumerable<string> idFilmesEnviados)
        {
            IEnumerable<Filme> filmes = await this._filmesRepository.ObterFilmesAsync();
            if(!filmes.Any())
                throw new Exception("Não foram encontrados filmes cadastrados");

            return filmes.Join(idFilmesEnviados, filme => filme.Id, id => id, (filme, ids) => filme);
        }

    }
}