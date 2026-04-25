using GoodHamburger.Web.Models;
using GoodHamburger.Web.Services;
using Microsoft.AspNetCore.Components;

namespace GoodHamburger.Web.Pages.Pedidos
{
    public partial class PedidosEditar : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }

        [Inject] public PedidoService PedidoService { get; set; }
        [Inject] public ItemService ItemService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        protected List<Item> Itens = new();
        protected Dictionary<Guid, bool> ItemSelecionado = new();

        protected bool IsLoading = true;
        protected bool IsSaving = false;

        protected string Mensagem = string.Empty;
        protected bool Sucesso = false;
        protected List<string>? Erros;

        protected IEnumerable<IGrouping<Tipo, Item>> ItensAgrupados =>
            Itens.GroupBy(x => x.Tipo);

        protected override async Task OnInitializedAsync()
        {
            await Carregar();
        }

        protected async Task Carregar()
        {
            try
            {
                IsLoading = true;

                var itens = await ItemService.ListarAsync();
                var pedido = await PedidoService.ConsultarPorIdAsync(Id);

                if (!itens.Sucesso || !pedido.Sucesso)
                {
                    MostrarErro(
                        "Erro ao carregar dados",
                        itens.Erros ?? pedido.Erros
                    );
                    return;
                }

                Itens = itens.Dados.ToList();

                var selecionados = pedido.Dados.Items
                    .Select(x => x.Id)
                    .ToHashSet();

                ItemSelecionado = Itens.ToDictionary(
                    x => x.Id,
                    x => selecionados.Contains(x.Id)
                );
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void ToggleItem(Guid id)
        {
            ItemSelecionado[id] = !ItemSelecionado[id];
        }

        protected async Task Actualizar()
        {
            try
            {
                IsSaving = true;

                var selecionados = ItemSelecionado
                    .Where(x => x.Value)
                    .Select(x => x.Key)
                    .ToList();

                var result = await PedidoService.ActualizarAsync(new ActualizarPedido
                {
                    PedidoId = Id,
                    ItensId = selecionados
                });

                if (result.Sucesso)
                {
                    MostrarSucesso(result.Mensagem ?? "Pedido atualizado com sucesso");
                }
                else
                {
                    MostrarErro(
                        result.Mensagem ?? "Erro ao atualizar pedido",
                        result.Erros
                    );
                }
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
            finally
            {
                IsSaving = false;
            }
        }

        protected void Voltar()
        {
            Navigation.NavigateTo("/pedidos");
        }

        protected void MostrarSucesso(string msg)
        {
            Sucesso = true;
            Mensagem = msg;
            Erros = null;
        }

        protected void MostrarErro(string msg, List<string>? erros = null)
        {
            Sucesso = false;
            Mensagem = msg;
            Erros = erros;
        }

        protected string GetTipoDescricao(Tipo tipo) => tipo switch
        {
            Tipo.Sanduiche => "Sanduíches",
            Tipo.Batata => "Batatas",
            Tipo.Refrigerante => "Refrigerantes",
            _ => "Outros"
        };

        protected void LimparMensagem()
        {
            Mensagem = string.Empty;
            Erros = null;
        }
    }
}