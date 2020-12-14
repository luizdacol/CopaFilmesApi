using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Infra.Repository
{
    public class FilmesRepository : IFilmesRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FilmesRepository> _logger;

        public FilmesRepository(IHttpClientFactory httpClientFactory, ILogger<FilmesRepository> logger)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;
        }

        public async Task<IEnumerable<Filme>> ObterFilmes()
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient("FilmesApi"))
            {
                this._logger.LogInformation("Executando GET api/filmes");
                using (HttpResponseMessage responseMessage = await httpClient.GetAsync("api/filmes"))
                {
                    string responseContent = await responseMessage.Content.ReadAsStringAsync();

                    this._logger.LogInformation($"StatusCode retornado: {responseMessage.StatusCode}");
                    this._logger.LogInformation($"Conteudo retornado: {responseContent}");

                    if (responseMessage.IsSuccessStatusCode)
                        return JsonSerializer.Deserialize<IEnumerable<Filme>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    else
                        throw new Exception($"Erro ao consultar filmes");
                }

            }
        }
    }
}