using api_prueba_ecop.src.Infraestructure.Entities;
using System.Linq.Expressions;

namespace api_prueba_ecop.src.Infraestructure.Predicate;

public static class PedidoPredicate
{
    public static Expression<Func<Pedido, bool>> Build(string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta)
    {
        return pedido =>
            (string.IsNullOrWhiteSpace(busqueda) ||
                (pedido.NumPedido != null && pedido.NumPedido.ToLower().Contains(busqueda.ToLower())) ||
                (pedido.Cliente != null && pedido.Cliente.Nombres != null && pedido.Cliente.Nombres.ToLower().Contains(busqueda.ToLower())) ||
                (pedido.Cliente != null && pedido.Cliente.Apellidos != null && pedido.Cliente.Apellidos.ToLower().Contains(busqueda.ToLower())) ||
                (pedido.Cliente != null && pedido.Cliente.NumeroDocumento != null && pedido.Cliente.NumeroDocumento.ToLower().Contains(busqueda.ToLower()))) &&
            (!fechaDesde.HasValue || pedido.Fecha >= fechaDesde.Value) &&
            (!fechaHasta.HasValue || pedido.Fecha <= fechaHasta.Value);
    }
}