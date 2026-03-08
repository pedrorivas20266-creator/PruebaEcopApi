using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface ICategoriaService
{
    Task<List<Categoria>> ObtenerCategoriasActivas(CancellationToken token);
    Task<List<Categoria>> ObtenerCategoriasPorFiltro(string? busqueda, int? limite, CancellationToken token);
    Task<Categoria> ObtenerCategoria(int codigoCategoria, CancellationToken token);
    Task<Categoria> NuevaCategoria(Categoria categoria, CancellationToken token);
    Task<Categoria> ActualizarCategoria(int codigoCategoria, Categoria categoria, CancellationToken token);
    Task EliminarCategoria(int codigoCategoria, CancellationToken token);
}