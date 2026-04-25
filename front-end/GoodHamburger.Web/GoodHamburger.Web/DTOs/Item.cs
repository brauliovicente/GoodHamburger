namespace GoodHamburger.Web.DTOs
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Tipo { get; set; }
    }
}
