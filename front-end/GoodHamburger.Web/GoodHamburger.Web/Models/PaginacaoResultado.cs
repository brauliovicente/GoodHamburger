namespace GoodHamburger.Web.Models
{
    public class PaginacaoResultado<T>
    {
        public T Dados { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalRegistos { get; set; }
    }
}
