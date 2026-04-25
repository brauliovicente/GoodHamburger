using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Entidade;
using Mapster;

namespace GoodHamburger.Aplicacao.Mapeadores
{
    internal class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //Item -> ItemDto
            config.NewConfig<Item, ItemDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Nome, src => src.Nome)
                .Map(dest => dest.Preco, src => src.Preco)
                .Map(dest => dest.Tipo, src => src.Tipo);

            //Pedido -> PedidoDto
            config.NewConfig<Pedido, PedidoDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Items, src => src.Itens)
                .Map(dest => dest.SubTotal, src => src.SubTotal)
                .Map(dest => dest.Total, src => src.Total);
        }
    }
}