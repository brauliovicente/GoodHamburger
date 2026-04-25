using GoodHamburger.Dominio.Enum;
using GoodHamburger.Dominio.Comum;

namespace GoodHamburger.Dominio.Entidade
{
    public class Item:EntidadeBase
    {
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public Tipo Tipo { get; private set; }

        public Item(string nome, decimal preco, Tipo tipo)
        {
            Nome = nome;
            Preco = preco;
            Tipo = tipo;
        }

        public Item(Guid id,string nome, decimal preco, Tipo tipo)
        {
            Id= id;
            Nome = nome;
            Preco = preco;
            Tipo = tipo;
        }

        public void SetNome(string nome)
        {
            Nome = nome;
        }
        public void SetPreco(decimal preco)
        {
            Preco = preco;
        }
        public void SetTipo(Tipo tipo)
        {
            Tipo = tipo;
        }

    }
}
