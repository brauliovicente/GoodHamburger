using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Enum;

namespace GoodHamburger.Dominio.Tests.Helpers
{
    public static class ItemBuilder
    {
        public static Item Sanduiche(decimal preco = 10)
            => new("X-Burger", preco, Tipo.Sanduiche);

        public static Item Batata(decimal preco = 5)
            => new("Batata", preco, Tipo.Batata);

        public static Item Refrigerante(decimal preco = 5)
            => new("Refri", preco, Tipo.Refrigerante);
    }
}
