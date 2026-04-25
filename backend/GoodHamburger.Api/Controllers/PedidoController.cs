using GoodHamburger.Aplicacao.UseCases.Pedido.Command.Actualizar;
using GoodHamburger.Aplicacao.UseCases.Pedido.Command.Criar;
using GoodHamburger.Aplicacao.UseCases.Pedido.Command.Remover;
using GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ConsultarPorId;
using GoodHamburger.Aplicacao.UseCases.Pedido.Queries.Listar;
using GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ListarComPaginacao;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly IMediator _mediator;

        public PedidoController(ILogger<PedidoController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Cria um novo pedido
        /// </summary>
        /// <param name="command">Comando para criação do pedido</param>
        /// <returns>Resultado da operação de criação do pedido</returns>
        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                if (response.Sucesso)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido");
                return StatusCode(500, "Erro interno no servidor");
            }
        }

        /// <summary>
        /// Actualiza um pedido existente
        /// </summary>
        /// <param name="command">Comando para atualização do pedido</param>
        /// <returns>Resultado da operação de atualização do pedido</returns>
        [HttpPut]
        [Route("actualizar")]
        public async Task<IActionResult> ActualizarPedido([FromBody] ActualizarPedidoCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                if (response.Sucesso)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pedido");
                return StatusCode(500, "Erro interno no servidor");
            }
        }

        /// <summary>
        /// Remove um pedido
        /// </summary>
        /// <param name="pedidoId">ID do pedido a ser removido</param>
        /// <returns>Resultado da operação de remoção do pedido</returns>
        [HttpDelete]
        [Route("remover/{pedidoId}")]
        public async Task<IActionResult> RemoverPedido(Guid pedidoId)
        {
            try
            {
                var command = new RemoverPedidoCommand { PedidoId = pedidoId };
                var response = await _mediator.Send(command);

                if (response.Sucesso)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover pedido");
                return StatusCode(500, "Erro interno no servidor");
            }
        }

        /// <summary>
        /// Consulta um pedido pelo seu ID
        /// </summary>
        /// <param name="pedidoId">ID do pedido</param>
        /// <returns>Detalhes do pedido</returns>
        [HttpGet]
        [Route("consultar/{pedidoId}")]
        public async Task<IActionResult> ConsultarPedidoPorId(Guid pedidoId)
        {
            try
            {
                var query = new ConsultarPorIdPedidoQuery { PedidoId = pedidoId };
                var response = await _mediator.Send(query);

                if (response.Sucesso)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar pedido");
                return StatusCode(500, "Erro interno no servidor");
            }
        }

        /// <summary>
        /// Lista todos os pedidos
        /// </summary>
        /// <returns>Lista de todos os pedidos</returns>
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarPedidos()
        {
            try
            {
                var query = new ListarPedidoQuery();
                var response = await _mediator.Send(query);

                if (response.Sucesso)
                {
                    return Ok(response);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar pedidos");
                return StatusCode(500, "Erro interno no servidor");
            }
        }


        /// <summary>
        /// Lista os pedidos com paginação, permitindo retornar um número limitado de pedidos por vez.
        /// </summary>
        /// <param name="pagina">Número da página a ser retornada.</param>
        /// <param name="tamanho">Número de itens por página.</param>
        /// <returns>Retorna os pedidos paginados com status 200 OK, ou uma mensagem caso não encontre pedidos.</returns>
        [HttpGet]
        [Route("listar-com-paginacao/{pagina}/{tamanho}")]
        public async Task<IActionResult> ListarPedidosComPaginacao([FromRoute] int pagina, [FromRoute] int tamanho)
        {
            try
            {
                var query = new ListarComPaginacaoPedidoQuery(pagina, tamanho);
                var response = await _mediator.Send(query);

                if (response.Sucesso)
                {
                    return Ok(response);
                }

                return Ok(new { message = "Nenhum pedido encontrado" });
            }
            catch (Exception ex)
            {
                // Logando o erro e retornando um erro interno
                _logger.LogError(ex, "Erro ao listar pedidos");
                return StatusCode(500, "Erro interno no servidor");
            }
        }
    }
}