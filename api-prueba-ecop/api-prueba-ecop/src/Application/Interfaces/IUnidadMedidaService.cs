using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IUnidadMedidaService
{
    Task<List<UnidadMedida>> ObtenerUnidadesMedidasActivas(CancellationToken token);
    Task<List<UnidadMedida>> ObtenerUnidadesMedidasPorFiltro(string? busqueda, int? limite, CancellationToken token);
    Task<UnidadMedida> ObtenerUnidadMedida(int codigoUnidadMedida, CancellationToken token);
    Task<UnidadMedida> NuevaUnidadMedida(UnidadMedida unidadMedida, CancellationToken token);
    Task<UnidadMedida> ActualizarUnidadMedida(int codigoUnidadMedida, UnidadMedida unidadMedida, CancellationToken token);
    Task EliminarUnidadMedida(int codigoUnidadMedida, CancellationToken token);
}