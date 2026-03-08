using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_prueba_ecop.src.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriaController(ICategoriaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene una lista de categorías filtradas por búsqueda y límite.
    /// </summary>
    /// <param name="busqueda">Término de búsqueda opcional</param>
    /// <param name="limite">Límite de resultados opcional</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de categorías</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Categoria>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Obtener([FromQuery] string? busqueda, [FromQuery] int? limite, CancellationToken token)
    {
        var categorias = await _service.ObtenerCategoriasPorFiltro(busqueda, limite, token);
        return Ok(categorias);
    }

    /// <summary>
    /// Obtiene todas las categorías activas.
    /// </summary>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de categorías activas</returns>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(List<Categoria>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerActivos(CancellationToken token)
    {
        var categorias = await _service.ObtenerCategoriasActivas(token);
        return Ok(categorias);
    }

    /// <summary>
    /// Obtiene una categoría específica por su código.
    /// </summary>
    /// <param name="codigoCategoria">Código de la categoría</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Categoría solicitada</returns>
    [HttpGet("{codigoCategoria:int}")]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerCategoria(int codigoCategoria, CancellationToken token)
    {
        var categoria = await _service.ObtenerCategoria(codigoCategoria, token);
        return Ok(categoria);
    }

    /// <summary>
    /// Crea una nueva categoría.
    /// </summary>
    /// <param name="categoria">Datos de la nueva categoría</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Categoría creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NuevaCategoria([FromBody] Categoria categoria, CancellationToken token)
    {
        var creada = await _service.NuevaCategoria(categoria, token);
        return CreatedAtAction(nameof(ObtenerCategoria), new { codigoCategoria = creada.CodCategoria }, creada);
    }

    /// <summary>
    /// Actualiza una categoría existente.
    /// </summary>
    /// <param name="codigoCategoria">Código de la categoría a actualizar</param>
    /// <param name="categoria">Datos actualizados de la categoría</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Categoría actualizada</returns>
    [HttpPut("{codigoCategoria:int}")]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarCategoria(int codigoCategoria, [FromBody] Categoria categoria, CancellationToken token)
    {
        var actualizada = await _service.ActualizarCategoria(codigoCategoria, categoria, token);
        return Ok(actualizada);
    }

    /// <summary>
    /// Elimina una categoría (eliminación lógica).
    /// </summary>
    /// <param name="codigoCategoria">Código de la categoría a eliminar</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{codigoCategoria:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EliminarCategoria(int codigoCategoria, CancellationToken token)
    {
        await _service.EliminarCategoria(codigoCategoria, token);
        return NoContent();
    }
}