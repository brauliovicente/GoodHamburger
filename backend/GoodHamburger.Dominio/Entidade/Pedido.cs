using GoodHamburger.Dominio.Enum;
using GoodHamburger.Dominio.Comum;
using GoodHamburger.Dominio.Exceptions;

namespace GoodHamburger.Dominio.Entidade
{
    public class Pedido : EntidadeBase
    {
        private readonly List<Item> _itens = new();

        public IReadOnlyCollection<Item> Itens => _itens;

        public decimal SubTotal { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal Total { get; private set; }

        public void AdicionarItem(Item item)
        {
            var novosItens = _itens.Append(item).ToList();

            ValidarRegras(novosItens);

            _itens.Add(item);

            Recalcular();
        }

        private void ValidarItemDuplicado(Item item)
        {
            var existeTipo = _itens.Any(i => i.Tipo == item.Tipo);

            if (existeTipo)
                throw new DomainException($"Já existe um item do tipo {item.Tipo} no pedido.");
        }


        private void Recalcular()
        {
            SubTotal = CalcularSubTotal();
            Desconto = CalcularDesconto();
            Total = SubTotal - Desconto;
        }

        private decimal CalcularSubTotal()
           => _itens.Sum(i => i.Preco);

        private decimal CalcularDesconto()
        {
            bool temSanduiche = _itens.Any(i => i.Tipo == Tipo.Sanduiche);
            bool temBatata = _itens.Any(i => i.Tipo == Tipo.Batata);
            bool temRefrigerante = _itens.Any(i => i.Tipo == Tipo.Refrigerante);

            var subtotal = SubTotal;

            if (temSanduiche && temBatata && temRefrigerante)
                return subtotal * 0.20m;

            if (temSanduiche && temRefrigerante)
                return subtotal * 0.15m;

            if (temSanduiche && temBatata)
                return subtotal * 0.10m;

            return 0;
        }

        public void SincronizarItens(List<Item> novosItens)
        {
            ValidarRegras(novosItens);

            _itens.Clear();
            _itens.AddRange(novosItens);

            Recalcular();
        }

        private void ValidarRegras(IEnumerable<Item> itens)
        {
            var sanduiches = itens.Count(i => i.Tipo == Tipo.Sanduiche);
            var batatas = itens.Count(i => i.Tipo == Tipo.Batata);
            var refrigerantes = itens.Count(i => i.Tipo == Tipo.Refrigerante);

            if (sanduiches > 1)
                throw new DomainException("O pedido não pode conter mais de um sanduíche.");

            if (batatas > 1)
                throw new DomainException("O pedido não pode conter mais de uma batata.");

            if (refrigerantes > 1)
                throw new DomainException("O pedido não pode conter mais de um refrigerante.");
        }

        public void LimparItens()
        {
            _itens.Clear();
            Recalcular();
        }


    }
}
