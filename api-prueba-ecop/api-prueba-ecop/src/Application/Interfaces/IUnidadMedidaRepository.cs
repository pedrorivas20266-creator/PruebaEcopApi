using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IUnidadMedidaRepository
{
    Task<UnidadMedida?> ObtenerUnidadMedida(int codigoUnidadMedida, CancellationToken token);
    Task<List<UnidadMedida>?> ObtenerUnidadesMedidasActivas(CancellationToken token);
    Task<List<UnidadMedida>?> ObtenerUnidadesMedidasPorFiltro(string? busqueda, int limite, CancellationToken token);
    Task<UnidadMedida> InsertarUnidadMedidaNueva(UnidadMedida unidadMedida, CancellationToken token);
    Task<UnidadMedida?> ActualizarUnidadMedida(int id, UnidadMedida unidadMedidaActualizada, CancellationToken token);
    Task<bool> EliminarUnidadMedida(int id, CancellationToken token);
}