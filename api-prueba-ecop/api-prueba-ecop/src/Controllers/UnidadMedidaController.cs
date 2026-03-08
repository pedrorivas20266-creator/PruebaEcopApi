using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_prueba_ecop.src.Controllers;

[ApiController]
[Route("api/unidades-medida")]
[Authorize]
public class UnidadMedidaController : ControllerBase
{
    private readonly IUnidadMedidaService _service;

    public UnidadMedidaController(IUnidadMedidaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene una lista de unidades de medida filtradas por búsqueda y límite.
    /// </summary>
    /// <param name="busqueda">Término de búsqueda opcional</param>
    /// <param name="limite">Límite de resultados opcional</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de unidades de medida</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<UnidadMedida>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Obtener([FromQuery] string? busqueda, [FromQuery] int? limite, CancellationToken token)
    {
        var unidadesMedidas = await _service.ObtenerUnidadesMedidasPorFiltro(busqueda, limite, token);
        return Ok(unidadesMedidas);
    }

    /// <summary>
    /// Obtiene todas las unidades de medida activas.
    /// </summary>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Lista de unidades de medida activas</returns>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(List<UnidadMedida>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerActivos(CancellationToken token)
    {
        var unidadesMedidas = await _service.ObtenerUnidadesMedidasActivas(token);
        return Ok(unidadesMedidas);
    }

    /// <summary>
    /// Obtiene una unidad de medida específica por su código.
    /// </summary>
    /// <param name="codigoUnidadMedida">Código de la unidad de medida</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Unidad de medida solicitada</returns>
    [HttpGet("{codigoUnidadMedida:int}")]
    [ProducesResponseType(typeof(UnidadMedida), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerUnidadMedida(int codigoUnidadMedida, CancellationToken token)
    {
        var unidadMedida = await _service.ObtenerUnidadMedida(codigoUnidadMedida, token);
        return Ok(unidadMedida);
    }

    /// <summary>
    /// Crea una nueva unidad de medida.
    /// </summary>
    /// <param name="unidadMedida">Datos de la nueva unidad de medida</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Unidad de medida creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UnidadMedida), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NuevaUnidadMedida([FromBody] UnidadMedida unidadMedida, CancellationToken token)
    {
        var creada = await _service.NuevaUnidadMedida(unidadMedida, token);
        return CreatedAtAction(nameof(ObtenerUnidadMedida), new { codigoUnidadMedida = creada.CodUnidadMedida }, creada);
    }

    /// <summary>
    /// Actualiza una unidad de medida existente.
    /// </summary>
    /// <param name="codigoUnidadMedida">Código de la unidad de medida a actualizar</param>
    /// <param name="unidadMedida">Datos actualizados de la unidad de medida</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Unidad de medida actualizada</returns>
    [HttpPut("{codigoUnidadMedida:int}")]
    [ProducesResponseType(typeof(UnidadMedida), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarUnidadMedida(int codigoUnidadMedida, [FromBody] UnidadMedida unidadMedida, CancellationToken token)
    {
        var actualizada = await _service.ActualizarUnidadMedida(codigoUnidadMedida, unidadMedida, token);
        return Ok(actualizada);
    }

    /// <summary>
    /// Elimina una unidad de medida (eliminación física).
    /// </summary>
    /// <param name="codigoUnidadMedida">Código de la unidad de medida a eliminar</param>
    /// <param name="token">Token de cancelación</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{codigoUnidadMedida:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EliminarUnidadMedida(int codigoUnidadMedida, CancellationToken token)
    {
        await _service.EliminarUnidadMedida(codigoUnidadMedida, token);
        return NoContent();
    }
}