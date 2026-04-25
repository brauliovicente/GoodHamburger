namespace GoodHamburger.Web.Models
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public List<Item> Items { get; set; }

        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class CriarPedido
    {
        public List<Guid> ItensId { get; set; }
    }

    public class ActualizarPedido
    {
        public Guid PedidoId { get; set; }
        public List<Guid> ItensId { get; set; } = new();
    }
}
