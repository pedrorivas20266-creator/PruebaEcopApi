using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_prueba_ecop.src.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene una lista de clientes filtrados por búsqueda y límite.
    /// </summary>
    /// <param name="busqueda">Término de búsqueda opcional</param>
    /// <param name="limite">Límite de resultados opcional</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de clientes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Cliente>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Obtener([FromQuery] string? busqueda, [FromQuery] int? limite, CancellationToken token)
    {
        var clientes = await _service.ObtenerClientesPorFiltro(busqueda, limite, token);
        return Ok(clientes);
    }

    /// <summary>
    /// Obtiene todos los clientes activos.
    /// </summary>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de clientes activos</returns>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(List<Cliente>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerActivos(CancellationToken token)
    {
        var clientes = await _service.ObtenerClientesActivos(token);
        return Ok(clientes);
    }

    /// <summary>
    /// Obtiene un cliente específico por su código.
    /// </summary>
    /// <param name="codigoCliente">Código del cliente</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Cliente solicitado</returns>
    [HttpGet("{codigoCliente:int}")]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerCliente(int codigoCliente, CancellationToken token)
    {
        var cliente = await _service.ObtenerCliente(codigoCliente, token);
        return Ok(cliente);
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    /// <param name="cliente">Datos del nuevo cliente</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Cliente creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NuevoCliente([FromBody] Cliente cliente, CancellationToken token)
    {
        var creado = await _service.NuevoCliente(cliente, token);
        return CreatedAtAction(nameof(ObtenerCliente), new { codigoCliente = creado.CodCliente }, creado);
    }

    /// <summary>
    /// Actualiza un cliente existente.
    /// </summary>
    /// <param name="codigoCliente">Código del cliente a actualizar</param>
    /// <param name="cliente">Datos actualizados del cliente</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Cliente actualizado</returns>
    [HttpPut("{codigoCliente:int}")]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarCliente(int codigoCliente, [FromBody] Cliente cliente, CancellationToken token)
    {
        var actualizado = await _service.ActualizarCliente(codigoCliente, cliente, token);
        return Ok(actualizado);
    }

    /// <summary>
    /// Elimina un cliente (eliminación lógica).
    /// </summary>
    /// <param name="codigoCliente">Código del cliente a eliminar</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{codigoCliente:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EliminarCliente(int codigoCliente, CancellationToken token)
    {
        await _service.EliminarCliente(codigoCliente, token);
        return NoContent();
    }
}