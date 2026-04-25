namespace GoodHamburger.Web.Models;

public class Item
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public Tipo Tipo { get; set; }
}

public class CriarItem
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public Tipo Tipo { get; set; }
}

public class ActualizarItem
{
    public Guid ItemId { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public Tipo Tipo { get; set; }
}

