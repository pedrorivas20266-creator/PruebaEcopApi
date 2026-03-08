using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Data;
using api_prueba_ecop.src.Infraestructure.Entities;
using api_prueba_ecop.src.Infraestructure.Predicate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_prueba_ecop.src.Infraestructure.Repository;
public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> ObtenerPedido(int codigoPedido, CancellationToken token)
    {
        return await _context.Pedido.AsNoTracking().Include(p => p.Cliente).Include(p => p.Detalles!).ThenInclude(d => d.Producto).Where(x => x.CodPedido == codigoPedido).FirstOrDefaultAsync(token);
    }

    public async Task<(List<Pedido> pedidos, int totalCount)> ObtenerPedidosPorFiltro(string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, CancellationToken token)
    {
        var filtro = PedidoPredicate.Build(busqueda, fechaDesde, fechaHasta);
        var query = _context.Pedido.AsNoTracking().Include(p => p.Cliente).Where(filtro);
        var totalCount = await query.CountAsync(token);
        var pedidos = await query.OrderByDescending(x => x.Fecha).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(token);
        return (pedidos, totalCount);
    }

    public async Task<Pedido> InsertarPedido(Pedido pedido, CancellationToken token)
    {
        var pedidoCreado = await _context.Pedido.AddAsync(pedido, token);
        await _context.SaveChangesAsync(token);
        return pedidoCreado.Entity;
    }

    public async Task InsertarDetalles(List<PedidoDetalle> detalles, CancellationToken token)
    {
        await _context.PedidoDetalle.AddRangeAsync(detalles, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task<Pedido?> ActualizarPedido(Pedido pedido, CancellationToken token)
    {
        _context.Pedido.Update(pedido);
        await _context.SaveChangesAsync(token);
        return pedido;
    }

    public async Task<IDbContextTransaction> IniciarTransaccion(CancellationToken token)
    {
        return await _context.Database.BeginTransactionAsync(token);
    }

    public Task<Pedido?> AnularPedido(int codigoPedido, string motivoAnulacion, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}