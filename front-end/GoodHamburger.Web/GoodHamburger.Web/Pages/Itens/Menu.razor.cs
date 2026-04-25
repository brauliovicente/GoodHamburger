using GoodHamburger.Web.Models;
using GoodHamburger.Web.Services;
using Microsoft.AspNetCore.Components;

namespace GoodHamburger.Web.Pages.Itens
{
    public partial class Menu : ComponentBase
    {
        [Inject] public ItemService ItemService { get; set; }
        [Inject] public PedidoService PedidoService { get; set; }

        protected List<Item> Itens = new();
        protected Dictionary<Guid, bool> ItemSelecionado = new();

        protected bool IsLoading = true;
        protected bool IsSubmitting = false;

        protected string Mensagem = string.Empty;
        protected bool Sucesso = false;
        protected List<string>? Erros;

        protected override async Task OnInitializedAsync()
        {
            await CarregarItens();
        }

        protected async Task CarregarItens()
        {
            try
            {
                IsLoading = true;

                var result = await ItemService.ListarAsync();

                if (!result.Sucesso || result.Dados == null)
                {
                    MostrarErro(
                        result.Mensagem ?? "Erro ao carregar itens",
                        result.Erros
                    );
                    return;
                }

                Itens = result.Dados.ToList();
                ItemSelecionado = Itens.ToDictionary(x => x.Id, _ => false);
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

        protected async Task FazerPedido()
        {
            var selecionados = Itens
                .Where(x => ItemSelecionado[x.Id])
                .Select(x => x.Id)
                .ToList();

            if (!selecionados.Any())
            {
                MostrarErro("Selecione pelo menos um item");
                return;
            }

            try
            {
                IsSubmitting = true;

                var result = await PedidoService.CriarAsync(new CriarPedido
                {
                    ItensId = selecionados
                });

                if (result.Sucesso)
                {
                    MostrarSucesso(result.Mensagem ?? "Pedido criado com sucesso!");
                    LimparSelecao();
                }
                else
                {
                    MostrarErro(
                        result.Mensagem ?? "Erro ao criar pedido",
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
                IsSubmitting = false;
            }
        }

        protected void LimparSelecao()
        {
            foreach (var key in ItemSelecionado.Keys.ToList())
                ItemSelecionado[key] = false;
        }

        protected void ToggleItem(Guid id)
        {
            ItemSelecionado[id] = !ItemSelecionado[id];
        }

        protected int ItensSelecionadosCount =>
            ItemSelecionado.Count(x => x.Value);

        protected decimal TotalSelecionado =>
            Itens.Where(x => ItemSelecionado[x.Id]).Sum(x => x.Preco);

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

        protected void LimparMensagem()
        {
            Mensagem = string.Empty;
        }
    }
}