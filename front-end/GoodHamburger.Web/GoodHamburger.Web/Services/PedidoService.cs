using System.Net.Http.Json;
using GoodHamburger.Web.Models;

namespace GoodHamburger.Web.Services
{
    public class PedidoService
    {
        private readonly HttpClient _http;

        public PedidoService(HttpClient http)
        {
            _http = http;
        }

        // Criar pedido
        public async Task<RespostaGeral<Pedido>> CriarAsync(CriarPedido command)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/pedido/criar", command);

                var result = await LerResposta<Pedido>(response);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar o pedido", ex);
            }
        }

        // Atualizar pedido
        public async Task<RespostaGeral<Pedido>> ActualizarAsync(ActualizarPedido command)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("api/pedido/actualizar", command);

                var result = await LerResposta<Pedido>(response);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao de conexão com o servidor", ex);
            }
        }

        // Apagar pedido
        public async Task<RespostaGeral<Pedido>> ApagarAsync(Guid id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/pedido/remover/{id}");

                var result = await LerResposta<Pedido>(response);

                return result;
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro de conexão com  o servidor", ex);
            }
        }

        // Listar pedidos
        public async Task<RespostaGeral<IEnumerable<Pedido>>> ListarAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<RespostaGeral<IEnumerable<Pedido>>>("api/pedido/listar");

                if (result == null)
                {
                    throw new Exception("Erro ao carregar os pedidos");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao listar os pedidos", ex);
            }
        }

        public async Task<RespostaGeral<Pedido>> ConsultarPorIdAsync(Guid id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<RespostaGeral<Pedido>>($"api/pedido/consultar/{id}");

                if (result == null)
                {
                    throw new Exception("Erro ao carregar o item");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao consultar o item", ex);
            }
        }

        // Listar pedidos com paginação
        public async Task<RespostaGeral<PaginacaoResultado<Pedido>>> ListarComPaginacaoAsync(int pagina, int tamanho)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<RespostaGeral<PaginacaoResultado<Pedido>>>($"api/pedido/listar-com-paginacao/{pagina}/{tamanho}");

                if (result == null)
                {
                    throw new Exception("Erro ao carregar os pedidos com paginação");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao listar pedidos com paginação", ex);
            }
        }

        private async Task<RespostaGeral<T>> LerResposta<T>(HttpResponseMessage response)
        {
            try
            {
                var conteudo = await response.Content.ReadFromJsonAsync<RespostaGeral<T>>();

                if (conteudo != null)
                    return conteudo;

                return RespostaGeral<T>.Falha("Resposta inválida do servidor");
            }
            catch
            {
                return RespostaGeral<T>.Falha("Erro ao interpretar resposta do servidor");
            }
        }
    }


}