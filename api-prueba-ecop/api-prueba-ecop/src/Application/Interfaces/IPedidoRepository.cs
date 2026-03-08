using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_prueba_ecop.src.Application.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObtenerPedido(int codigoPedido, CancellationToken token);
    Task<(List<Pedido> pedidos, int totalCount)> ObtenerPedidosPorFiltro(string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, CancellationToken token);
    Task<Pedido> InsertarPedido(Pedido pedido, CancellationToken token);
    Task InsertarDetalles(List<PedidoDetalle> detalles, CancellationToken token);
    Task<Pedido?> ActualizarPedido(Pedido pedido, CancellationToken token);
    Task<IDbContextTransaction> IniciarTransaccion(CancellationToken token);
}