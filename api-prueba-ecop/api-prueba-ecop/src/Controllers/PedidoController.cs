using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_prueba_ecop.src.Controllers;

[ApiController]
[Route("api/pedidos")]
[Authorize]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidoController(IPedidoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene una lista paginada de pedidos con filtros opcionales.
    /// </summary>
    /// <param name="busqueda">Término de búsqueda opcional</param>
    /// <param name="fechaDesde">Fecha desde para filtrar pedidos</param>
    /// <param name="fechaHasta">Fecha hasta para filtrar pedidos</param>
    /// <param name="pageNumber">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista paginada de pedidos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerPedidos([FromQuery] string? busqueda, [FromQuery] DateTime? fechaDesde, [FromQuery] DateTime? fechaHasta, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken token = default)
    {
        var pedidos = await _service.ObtenerPorFiltro(busqueda, fechaDesde, fechaHasta, pageNumber, pageSize, token);
        return Ok(pedidos);
    }

    /// <summary>
    /// Obtiene un pedido específico con sus detalles.
    /// </summary>
    /// <param name="codigoPedido">Código del pedido</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Pedido con sus detalles</returns>
    [HttpGet("{codigoPedido:int}")]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerPedido(int codigoPedido, CancellationToken token)
    {
        var pedido = await _service.Obtener(codigoPedido, token);
        return Ok(pedido);
    }

    /// <summary>
    /// Crea un nuevo pedido con sus detalles.
    /// </summary>
    /// <param name="request">Datos del nuevo pedido y sus detalles</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Pedido creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearPedido([FromBody] PedidoRequest request, CancellationToken token)
    {
        var creado = await _service.Nuevo(request, token);
        return CreatedAtAction(nameof(ObtenerPedido), new { codigoPedido = creado.CodPedido }, creado);
    }

    /// <summary>
    /// Anula un pedido existente.
    /// </summary>
    /// <param name="codigoPedido">Código del pedido a anular</param>
    /// <param name="request">Datos de anulación (motivo)</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Pedido anulado</returns>
    [HttpPut("{codigoPedido:int}/anular")]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AnularPedido(int codigoPedido, [FromBody] AnularPedidoRequest request, CancellationToken token)
    {
        var anulado = await _service.Anulacion(codigoPedido, request, token);
        return Ok(anulado);
    }
}