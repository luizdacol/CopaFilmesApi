namespace CopaFilmes.Api.Views
{
    public class FilmeView
    {
        public FilmeView(string id, string titulo, int ano)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Ano = ano;
        }

        public string Id { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
    }
}