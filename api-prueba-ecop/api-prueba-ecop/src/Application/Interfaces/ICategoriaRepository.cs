using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface ICategoriaRepository
{
    Task<Categoria?> ObtenerCategoria(int codigoCategoria, CancellationToken token);
    Task<List<Categoria>?> ObtenerCategoriasActivas(CancellationToken token);
    Task<List<Categoria>?> ObtenerCategoriasPorFiltro(string? busqueda, int limite, CancellationToken token);
    Task<Categoria> InsertarCategoriaNueva(Categoria categoria, CancellationToken token);
    Task<Categoria?> ActualizarCategoria(int id, Categoria categoriaActualizada, CancellationToken token);
    Task<bool> EliminarCategoria(int id, CancellationToken token);
}