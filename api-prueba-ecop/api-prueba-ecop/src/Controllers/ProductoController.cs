using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_prueba_ecop.src.Controllers;

[ApiController]
[Route("api/productos")]
[Authorize]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductoController(IProductoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene una lista de productos filtrados por búsqueda y límite.
    /// </summary>
    /// <param name="busqueda">Término de búsqueda opcional</param>
    /// <param name="limite">Límite de resultados opcional</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de productos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductoQuery>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Obtener([FromQuery] string? busqueda, [FromQuery] int? limite, CancellationToken token)
    {
        var productos = await _service.ObtenerProductosPorFiltro(busqueda, limite, token) ?? new List<ProductoQuery>();
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene todos los productos activos.
    /// </summary>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de productos activos</returns>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerActivos(CancellationToken token)
    {
        var productos = await _service.ObtenerProductosActivos(token);
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto específico por su código.
    /// </summary>
    /// <param name="codigoProducto">Código del producto</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Producto solicitado</returns>
    [HttpGet("{codigoProducto:int}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerProducto(int codigoProducto, CancellationToken token)
    {
        var producto = await _service.ObtenerProducto(codigoProducto, token);
        return Ok(producto);
    }

    /// <summary>
    /// Obtiene un producto con todos sus precios asociados.
    /// </summary>
    /// <param name="codigoProducto">Código del producto</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Producto con sus precios</returns>
    [HttpGet("{codigoProducto:int}/con-precios")]
    [ProducesResponseType(typeof(ProductoConPrecioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerProductoConPrecios(int codigoProducto, CancellationToken token)
    {
        var producto = await _service.ObtenerProductoConPrecios(codigoProducto, token);
        return Ok(producto);
    }

    /// <summary>
    /// Crea un nuevo producto con sus precios.
    /// </summary>
    /// <param name="request">Datos del nuevo producto y sus precios</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Producto creado con sus precios</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoConPrecioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NuevoProductoConPrecios([FromBody] ProductoRequest request, CancellationToken token)
    {
        var creado = await _service.NuevoProductoConPrecios(request, token);
        return CreatedAtAction(nameof(ObtenerProducto), new { codigoProducto = creado.CodProducto }, creado);
    }

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    /// <param name="codigoProducto">Código del producto a actualizar</param>
    /// <param name="producto">Datos actualizados del producto</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Producto actualizado</returns>
    [HttpPut("{codigoProducto:int}")]
    [ProducesResponseType(typeof(ProductoQuery), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarProducto(int codigoProducto, [FromBody] ProductoQuery producto, CancellationToken token)
    {
        var actualizado = await _service.ActualizarProducto(codigoProducto, producto, token);
        return Ok(actualizado);
    }

    /// <summary>
    /// Elimina un producto (eliminación lógica).
    /// </summary>
    /// <param name="codigoProducto">Código del producto a eliminar</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{codigoProducto:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EliminarProducto(int codigoProducto, CancellationToken token)
    {
        await _service.EliminarProducto(codigoProducto, token);
        return NoContent();
    }

    /// <summary>
    /// Obtiene todos los precios de un producto específico.
    /// </summary>
    /// <param name="codigoProducto">Código del producto</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de precios del producto</returns>
    [HttpGet("{codigoProducto:int}/precios")]
    [ProducesResponseType(typeof(List<Precio>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerPreciosDeProducto(int codigoProducto, CancellationToken token)
    {
        var precios = await _service.ObtenerPreciosDeProducto(codigoProducto, token);
        return Ok(precios);
    }

    /// <summary>
    /// Actualiza los precios de un producto específico.
    /// </summary>
    /// <param name="codigoProducto">Código del producto</param>
    /// <param name="request">Datos de los precios a actualizar</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de precios actualizados</returns>
    [HttpPut("{codigoProducto:int}/precios")]
    [ProducesResponseType(typeof(List<Precio>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarPreciosDeProducto(int codigoProducto, [FromBody] ActualizarPreciosRequest request, CancellationToken token)
    {
        var preciosActualizados = await _service.ActualizarPreciosDeProducto(codigoProducto, request, token);
        return Ok(preciosActualizados);
    }
}