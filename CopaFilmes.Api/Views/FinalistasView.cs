using CopaFilmes.Api.Domain;

namespace CopaFilmes.Api.Views
{
    public class FinalistasView
    {
        public FinalistasView(Filme campeao, Filme viceCampeao)
        {
            this.Campeao = campeao.Titulo;
            this.ViceCampeao = viceCampeao.Titulo;
        }

        public string Campeao { get; }
        public string ViceCampeao { get; }
    }
}