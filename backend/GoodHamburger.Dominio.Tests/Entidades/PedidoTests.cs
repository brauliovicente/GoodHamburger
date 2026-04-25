using FluentAssertions;
using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Enum;
using GoodHamburger.Dominio.Exceptions;
using GoodHamburger.Dominio.Tests.Helpers;

namespace GoodHamburger.Dominio.Tests.Entidades;

public class PedidoTests
{
    [Fact]
    public void Deve_adicionar_item_e_recalcular_totais()
    {
        var pedido = new Pedido();
        var item = ItemBuilder.Sanduiche(10);

        pedido.AdicionarItem(item);

        pedido.Itens.Should().ContainSingle();
        pedido.SubTotal.Should().Be(10);
        pedido.Desconto.Should().Be(0);
        pedido.Total.Should().Be(10);
    }

    [Theory]
    [InlineData(Tipo.Sanduiche)]
    [InlineData(Tipo.Batata)]
    [InlineData(Tipo.Refrigerante)]
    public void Nao_deve_permitir_itens_duplicados_por_tipo(Tipo tipo)
    {
        var pedido = new Pedido();

        var item1 = new Item("Item1", 10, tipo);
        var item2 = new Item("Item2", 10, tipo);

        pedido.AdicionarItem(item1);

        Action act = () => pedido.AdicionarItem(item2);

        act.Should()
           .Throw<DomainException>()
           .WithMessage("*não pode conter mais de um*");
    }

    [Fact]
    public void Deve_aplicar_20_porcento_quando_combo_completo()
    {
        var pedido = new Pedido();

        pedido.AdicionarItem(ItemBuilder.Sanduiche(10));
        pedido.AdicionarItem(ItemBuilder.Batata(5));
        pedido.AdicionarItem(ItemBuilder.Refrigerante(5));

        pedido.SubTotal.Should().Be(20);
        pedido.Desconto.Should().Be(4);
        pedido.Total.Should().Be(16);
    }

    [Fact]
    public void Deve_aplicar_15_porcento_quando_sanduiche_e_refrigerante()
    {
        var pedido = new Pedido();

        pedido.AdicionarItem(ItemBuilder.Sanduiche(10));
        pedido.AdicionarItem(ItemBuilder.Refrigerante(10));

        pedido.SubTotal.Should().Be(20);
        pedido.Desconto.Should().Be(3);
        pedido.Total.Should().Be(17);
    }

    [Fact]
    public void Deve_aplicar_10_porcento_quando_sanduiche_e_batata()
    {
        var pedido = new Pedido();

        pedido.AdicionarItem(ItemBuilder.Sanduiche(10));
        pedido.AdicionarItem(ItemBuilder.Batata(10));

        pedido.SubTotal.Should().Be(20);
        pedido.Desconto.Should().Be(2);
        pedido.Total.Should().Be(18);
    }

    [Fact]
    public void Nao_deve_aplicar_desconto_quando_itens_nao_formam_combo()
    {
        var pedido = new Pedido();

        pedido.AdicionarItem(ItemBuilder.Batata(10));

        pedido.Desconto.Should().Be(0);
        pedido.Total.Should().Be(10);
    }

    [Fact]
    public void Deve_actualizar_itens_e_recalcular()
    {
        var pedido = new Pedido();

        var sanduiche = ItemBuilder.Sanduiche(10);
        var batata = ItemBuilder.Batata(5);

        pedido.AdicionarItem(sanduiche);
        pedido.AdicionarItem(batata);

        pedido.SincronizarItens(new List<Item> { sanduiche });

        pedido.Itens.Should().ContainSingle();
        pedido.SubTotal.Should().Be(10);
        pedido.Desconto.Should().Be(0);
        pedido.Total.Should().Be(10);
    }

    [Fact]
    public void Deve_limpar_itens_e_zerar_valores()
    {
        var pedido = new Pedido();

        pedido.AdicionarItem(ItemBuilder.Sanduiche(10));

        pedido.LimparItens();

        pedido.Itens.Should().BeEmpty();
        pedido.SubTotal.Should().Be(0);
        pedido.Desconto.Should().Be(0);
        pedido.Total.Should().Be(0);
    }
}