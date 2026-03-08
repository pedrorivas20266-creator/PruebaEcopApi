using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Services;
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;
    private readonly ILogger<CategoriaService> _logger;

    public CategoriaService(ICategoriaRepository repository, ILogger<CategoriaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Categoria>> ObtenerCategoriasActivas(CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de categorias activas ###");

        var categorias = await _repository.ObtenerCategoriasActivas(token);

        if (categorias == null || categorias.Count == 0)
        {
            _logger.LogWarning("No se encontraron categorias activas");
            throw new DomainException("No se encontraron categorias activas");
        }

        _logger.LogInformation("### Categorias activas obtenidas correctamente. Total: {Count} ###", categorias.Count);
        return categorias;
    }

    public async Task<Categoria> ObtenerCategoria(int codigoCategoria, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de la categoria: {@CodigoCategoria} ###", codigoCategoria);

        var categoria = await _repository.ObtenerCategoria(codigoCategoria, token);

        if (categoria == null)
        {
            _logger.LogWarning("Categoria no encontrada para el codigo: {CodigoCategoria}", codigoCategoria);
            throw new DomainException("Categoria no encontrada");
        }

        _logger.LogInformation("### Categoria obtenida correctamente: {@Categoria} ###", categoria);
        return categoria;
    }

    public async Task<List<Categoria>> ObtenerCategoriasPorFiltro(string? busqueda, int? limite, CancellationToken token)
    {
        var limiteFinal = (limite.HasValue && limite.Value > 0) ? limite.Value : 20;

        _logger.LogInformation("### Iniciando proceso de obtencion de categorias por filtro. Busqueda: {Busqueda}, Limite: {Limite} ###", busqueda, limiteFinal);

        var categorias = await _repository.ObtenerCategoriasPorFiltro(busqueda, limiteFinal, token);

        if (categorias == null || categorias.Count == 0)
        {
            _logger.LogWarning("No se encontraron categorias con los filtros. Busqueda: {Busqueda}", busqueda);
            throw new DomainException("No se encontraron categorias con los filtros");
        }

        _logger.LogInformation("### Categorias obtenidas correctamente por filtro. Total: {Count} ###", categorias.Count);
        return categorias;
    }

    public async Task<Categoria> NuevaCategoria(Categoria categoria, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de insercion de la nueva categoria: {@Categoria}", categoria);

        var nuevaCategoria = await _repository.InsertarCategoriaNueva(categoria, token);

        if (nuevaCategoria == null)
        {
            _logger.LogWarning("Error al insertar la nueva categoria");
            throw new DomainException("Error al insertar la nueva categoria");
        }

        _logger.LogInformation("### Categoria insertada correctamente ###");
        return nuevaCategoria;
    }

    public async Task<Categoria> ActualizarCategoria(int codigoCategoria, Categoria categoria, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de actualizacion de la categoria: {@Categoria}", categoria);

        var categoriaActualizada = await _repository.ActualizarCategoria(codigoCategoria, categoria, token);

        if (categoriaActualizada == null)
        {
            _logger.LogWarning("Error al actualizar la nueva categoria");
            throw new DomainException("Error al actualizar la nueva categoria");
        }

        _logger.LogInformation("### Categoria actualizada correctamente ###");

        return categoriaActualizada;
    }

    public async Task EliminarCategoria(int codigoCategoria, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de eliminacion de la categoria: {@CodigoCategoria} ###", codigoCategoria);

        var operacion = await _repository.EliminarCategoria(codigoCategoria, token);

        if (!operacion)
        {
            _logger.LogWarning("Error al eliminar la categoria");
            throw new DomainException("Error al eliminar la categoria");
        }

        _logger.LogInformation("### Categoria eliminada correctamente ###");
    }
}