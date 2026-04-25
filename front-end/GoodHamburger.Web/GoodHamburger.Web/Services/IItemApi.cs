using Refit;
using GoodHamburger.Web.DTOs;

namespace GoodHamburger.Web.Services;

public interface IItemApi
{
    [Get("/api/Item/listar")]
    Task<ApiResponse<List<Item>>> ListarItens();
}
