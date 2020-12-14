namespace CopaFilmes.Api.Domain
{
    public class Filme
    {
        public Filme(){
        }
        
        public Filme(string id, string titulo, int ano, decimal nota)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Ano = ano;
            this.Nota = nota;
        }

        public string Id { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public decimal Nota { get; set; }
    }
}