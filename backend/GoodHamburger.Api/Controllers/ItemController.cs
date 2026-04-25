using Microsoft.AspNetCore.Mvc;
using MediatR;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Criar;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Actualizar;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Remover;
using GoodHamburger.Aplicacao.UseCases.Item.Queries.Listar;
using GoodHamburger.Aplicacao.UseCases.Item.Queries.ConsultarPorId;

namespace GoodHamburger.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IMediator mediator, ILogger<ItemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo item.
        /// </summary>
        /// <param name="command">Comando contendo os dados do item a ser criado.</param>
        /// <returns>Retorna 200 OK se a criação for bem-sucedida, ou 400 BadRequest se houver erros.</returns>
        [HttpPost("criar")]
        public async Task<IActionResult> Criar([FromBody] CriarItemCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Actualiza um item existente.
        /// </summary>
        /// <param name="id">ID do item a ser atualizado.</param>
        /// <param name="command">Comando contendo os dados atualizados do item.</param>
        /// <returns>Retorna 200 OK se a atualização for bem-sucedida, ou 400 BadRequest se o ID da URL não coincidir com o ID do comando.</returns>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarItemCommand command)
        {
            if (id != command.ItemId)
                return BadRequest("ID da rota diferente do corpo");

            var result = await _mediator.Send(command);

            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }


        /// <summary>
        /// Remove um item pelo ID.
        /// </summary>
        /// <param name="id">ID do item a ser removido.</param>
        /// <returns>Retorna 200 OK se a remoção for bem-sucedida, ou 404 NotFound se o item não for encontrado.</returns>
        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var command = new RemoverItemCommand { ItemId = id };

            var result = await _mediator.Send(command);

            if (!result.Sucesso)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os itens disponíveis no cardápio.
        /// </summary>
        /// <returns>Retorna uma lista de itens com status 200 OK.</returns>

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var result = await _mediator.Send(new ListarItemQuery());

            return Ok(result);
        }

        /// <summary>
        /// Consulta um item pelo ID.
        /// </summary>
        /// <param name="id">ID do item a ser consultado.</param>
        /// <returns>Retorna 200 OK com o item encontrado, ou 404 NotFound se o item não for encontrado.</returns>
        [HttpGet("consultar-pelo-id/{id}")]
        public async Task<IActionResult> ConsultarPorId(Guid id)
        {
            var result = await _mediator.Send(new ConsultarPorIdItemQuery { ItemId = id });

            if (!result.Sucesso)
                return NotFound(result);

            return Ok(result);
        }
    }
}