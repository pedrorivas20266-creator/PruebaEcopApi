using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Services;
public class UnidadMedidaService : IUnidadMedidaService
{
    private readonly IUnidadMedidaRepository _repository;
    private readonly ILogger<UnidadMedidaService> _logger;

    public UnidadMedidaService(IUnidadMedidaRepository repository, ILogger<UnidadMedidaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<UnidadMedida>> ObtenerUnidadesMedidasActivas(CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de unidades de medida activas ###");

        var unidadesMedidas = await _repository.ObtenerUnidadesMedidasActivas(token);

        if (unidadesMedidas == null || unidadesMedidas.Count == 0)
        {
            _logger.LogWarning("No se encontraron unidades de medida activas");
            throw new DomainException("No se encontraron unidades de medida activas");
        }

        _logger.LogInformation("### Unidades de medida activas obtenidas correctamente. Total: {Count} ###", unidadesMedidas.Count);
        return unidadesMedidas;
    }

    public async Task<UnidadMedida> ObtenerUnidadMedida(int codigoUnidadMedida, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de la unidad de medida: {@CodigoUnidadMedida} ###", codigoUnidadMedida);

        var unidadMedida = await _repository.ObtenerUnidadMedida(codigoUnidadMedida, token);

        if (unidadMedida == null)
        {
            _logger.LogWarning("Unidad de medida no encontrada para el codigo: {CodigoUnidadMedida}", codigoUnidadMedida);
            throw new DomainException("Unidad de medida no encontrada");
        }

        _logger.LogInformation("### Unidad de medida obtenida correctamente: {@UnidadMedida} ###", unidadMedida);
        return unidadMedida;
    }

    public async Task<List<UnidadMedida>> ObtenerUnidadesMedidasPorFiltro(string? busqueda, int? limite, CancellationToken token)
    {
        var limiteFinal = (limite.HasValue && limite.Value > 0) ? limite.Value : 20;

        _logger.LogInformation("### Iniciando proceso de obtencion de unidades de medida por filtro. Busqueda: {Busqueda}, Limite: {Limite} ###", busqueda, limiteFinal);

        var unidadesMedidas = await _repository.ObtenerUnidadesMedidasPorFiltro(busqueda, limiteFinal, token);

        if (unidadesMedidas == null || unidadesMedidas.Count == 0)
        {
            _logger.LogWarning("No se encontraron unidades de medida con los filtros. Busqueda: {Busqueda}", busqueda);
            throw new DomainException("No se encontraron unidades de medida con los filtros");
        }

        _logger.LogInformation("### Unidades de medida obtenidas correctamente por filtro. Total: {Count} ###", unidadesMedidas.Count);
        return unidadesMedidas;
    }

    public async Task<UnidadMedida> NuevaUnidadMedida(UnidadMedida unidadMedida, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de insercion de la nueva unidad de medida: {@UnidadMedida}", unidadMedida);

        var nuevaUnidadMedida = await _repository.InsertarUnidadMedidaNueva(unidadMedida, token);

        if (nuevaUnidadMedida == null)
        {
            _logger.LogWarning("Error al insertar la nueva unidad de medida");
            throw new DomainException("Error al insertar la nueva unidad de medida");
        }

        _logger.LogInformation("### Unidad de medida insertada correctamente ###");
        return nuevaUnidadMedida;
    }

    public async Task<UnidadMedida> ActualizarUnidadMedida(int codigoUnidadMedida, UnidadMedida unidadMedida, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de actualizacion de la unidad de medida: {@UnidadMedida}", unidadMedida);

        var unidadMedidaActualizada = await _repository.ActualizarUnidadMedida(codigoUnidadMedida, unidadMedida, token);

        if (unidadMedidaActualizada == null)
        {
            _logger.LogWarning("Error al actualizar la unidad de medida");
            throw new DomainException("Error al actualizar la unidad de medida");
        }

        _logger.LogInformation("### Unidad de medida actualizada correctamente ###");

        return unidadMedidaActualizada;
    }

    public async Task EliminarUnidadMedida(int codigoUnidadMedida, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de eliminacion de la unidad de medida: {@CodigoUnidadMedida} ###", codigoUnidadMedida);

        var operacion = await _repository.EliminarUnidadMedida(codigoUnidadMedida, token);

        if (!operacion)
        {
            _logger.LogWarning("Error al eliminar la unidad de medida");
            throw new DomainException("Error al eliminar la unidad de medida");
        }

        _logger.LogInformation("### Unidad de medida eliminada correctamente ###");
    }
}