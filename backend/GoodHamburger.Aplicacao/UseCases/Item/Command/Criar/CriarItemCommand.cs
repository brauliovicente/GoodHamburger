using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Enum;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Command.Criar
{
    public class CriarItemCommand : IRequest<RespostaGeral<ItemDto>>
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public Tipo Tipo { get; set; }
    }
}