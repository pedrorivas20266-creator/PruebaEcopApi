using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IPedidoService
{
    Task<PedidoResponse> Obtener(int codigoPedido, CancellationToken token);
    Task<PaginatedResponse<PedidoResponse>> ObtenerPorFiltro(string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, CancellationToken token);
    Task<PedidoResponse> Nuevo(PedidoRequest request, CancellationToken token);
    Task<PedidoResponse> Anulacion(int codigoPedido, AnularPedidoRequest request, CancellationToken token);
}