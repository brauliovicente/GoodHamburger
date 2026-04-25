using Microsoft.AspNetCore.Components;
using GoodHamburger.Web.Models;
using GoodHamburger.Web.Services;

namespace GoodHamburger.Web.Pages.Pedidos
{
    public partial class Pedidos : ComponentBase
    {
        #region Injects

        [Inject] protected PedidoService PedidoService { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;

        #endregion

        #region State

        protected List<Pedido> ListaPedidos = new();

        protected bool IsLoading = false;
        protected bool MostrarConfirmacao = false;

        protected Guid PedidoParaRemover;

        protected List<string>? ListaErros;
        protected string Mensagem = string.Empty;
        protected string AlertClass = "alert-success";

        protected bool MostrarDetalhes = false;
        protected Pedido? PedidoSelecionado;

        #endregion

        #region Lifecycle

        protected override async Task OnInitializedAsync()
        {
            await CarregarPedidos();
        }

        #endregion

        #region Methods

        protected async Task CarregarPedidos()
        {
            try
            {
                IsLoading = true;
                LimparMensagem();

                var result = await PedidoService.ListarAsync();

                if (result.Sucesso && result.Dados != null)
                {
                    ListaPedidos = result.Dados.ToList();
                }
                else
                {
                    MostrarErro(result.Mensagem ?? "Erro ao carregar pedidos.");
                }
            }
            catch (Exception ex)
            {
                MostrarErro($"Erro inesperado: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void ConfirmarRemocao(Guid id)
        {
            PedidoParaRemover = id;
            MostrarConfirmacao = true;
        }

        protected void CancelarRemocao()
        {
            MostrarConfirmacao = false;
            PedidoParaRemover = Guid.Empty;
        }

        protected async Task Remover()
        {
            try
            {
                IsLoading = true;

                var result = await PedidoService.ApagarAsync(PedidoParaRemover);

                if (result.Sucesso)
                {
                    MostrarSucesso(result.Mensagem ?? "Pedido removido com sucesso!");
                    await CarregarPedidos();
                }
                else
                {
                    MostrarErro(
                        result.Mensagem ?? "Erro ao remover pedido",
                        result.Erros // 👈 vindo direto do backend
                    );
                }
            }
            catch (Exception ex)
            {
                MostrarErro("Erro inesperado", new List<string> { ex.Message });
            }
            finally
            {
                IsLoading = false;
                MostrarConfirmacao = false;
                PedidoParaRemover = Guid.Empty;
            }
        }

        protected void Editar(Guid id)
        {
            Navigation.NavigateTo($"/pedidos/editar/{id}");
        }

        #endregion

        #region Alerts

        protected void MostrarSucesso(string mensagem)
        {
            Mensagem = mensagem;
            AlertClass = "alert-success";
            ListaErros = null;
        }

        protected void MostrarErro(string mensagem, List<string>? erros = null)
        {
            Mensagem = mensagem;
            AlertClass = "alert-danger";
            ListaErros = erros;
        }

        protected void LimparMensagem()
        {
            Mensagem = string.Empty;
        }

        protected void VerDetalhes(Pedido pedido)
        {
            PedidoSelecionado = pedido;
            MostrarDetalhes = true;
        }

        protected void FecharDetalhes()
        {
            MostrarDetalhes = false;
            PedidoSelecionado = null;
        }

        protected string GetTipoDescricao(Tipo tipo)
        {
            return tipo switch
            {
                Tipo.Sanduiche => "Sanduíche",
                Tipo.Batata => "Batata",
                Tipo.Refrigerante => "Refrigerante",
                _ => "Outro"
            };
        }

        #endregion
    }
}