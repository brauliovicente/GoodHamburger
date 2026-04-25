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

        [HttpPost("criar")]
        public async Task<IActionResult> Criar([FromBody] CriarItemCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

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

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var command = new RemoverItemCommand { ItemId = id };

            var result = await _mediator.Send(command);

            if (!result.Sucesso)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var result = await _mediator.Send(new ListarItemQuery());

            return Ok(result);
        }

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