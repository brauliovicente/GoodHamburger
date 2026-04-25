using System.Net.Http.Json;
using GoodHamburger.Web.Models;
using System.Net.Http;

namespace GoodHamburger.Web.Services
{
    public class ItemService
    {
        private readonly HttpClient _http;

        public ItemService(HttpClient http)
        {
            _http = http;
        }

        // Criar um item
        public async Task<RespostaGeral<Item>> CriarAsync(CriarItem command)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/item/criar", command);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RespostaGeral<Item>>()!;
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<RespostaGeral<Item>>()!;
                    return RespostaGeral<Item>.Falha(errorResponse.Mensagem);
                }
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao criar o item", ex);
            }
        }

        // Atualizar um item
        public async Task<RespostaGeral<Item>> AtualizarAsync(ActualizarItem command)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/item/actualizar/{command.ItemId}", command);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RespostaGeral<Item>>()!;
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<RespostaGeral<Item>>()!;
                    return RespostaGeral<Item>.Falha(errorResponse.Mensagem);
                }
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao atualizar o item", ex);
            }
        }

        // Remover um item
        public async Task<RespostaGeral<Item>> RemoverAsync(Guid id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/item/remover/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RespostaGeral<Item>>()!;
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<RespostaGeral<Item>>()!;
                    return RespostaGeral<Item>.Falha(errorResponse.Mensagem);
                }
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao remover o item", ex);
            }
        }

        // Listar todos os itens
        public async Task<RespostaGeral<IEnumerable<Item>>> ListarAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<RespostaGeral<IEnumerable<Item>>>("api/item/listar");

                if (result == null)
                {
                    throw new Exception("Erro ao carregar os itens");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Adiciona log para o erro
                throw new Exception("Erro ao listar os itens", ex);
            }
        }

        // Consultar um item pelo ID
        public async Task<RespostaGeral<Item>> ConsultarPorIdAsync(Guid id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<RespostaGeral<Item>>($"api/item/consultar-pelo-id/{id}");

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
    }
}