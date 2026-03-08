using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_prueba_ecop.src.Application.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> ObtenerProducto(int codigoProducto, CancellationToken token);
    Task<List<Producto>?> ObtenerProductosActivos(CancellationToken token);
    Task<List<ProductoQuery>?> ObtenerProductosPorFiltro(string? busqueda, int limite, CancellationToken token);
    Task<Producto> InsertarProductoNuevo(Producto producto, CancellationToken token);
    Task<Producto?> ActualizarProducto(int id, ProductoQuery productoActualizado, CancellationToken token);
    Task<bool> EliminarProducto(int id, CancellationToken token);
    Task<List<Precio>?> ObtenerPreciosDeProducto(int codigoProducto, CancellationToken token);
    Task<Precio?> ObtenerPrecio(int codigoPrecio, CancellationToken token);
    Task InsertarPrecios(List<Precio> precios, CancellationToken token);
    Task<Precio?> ActualizarPrecio(int id, Precio precioActualizado, CancellationToken token);
    Task<Iva?> ObtenerIva(int codigoIva, CancellationToken token);
    Task<List<Iva>?> ObtenerIvasActivos(CancellationToken token);
    Task<Dictionary<int, Iva>> ObtenerIvasPorCodigos(List<int> codigosIva, CancellationToken token);
    Task<IDbContextTransaction> IniciarTransaccion(CancellationToken token);
    Task ActualizarPrecioGenericoProducto(int codigoProducto, decimal precioVentaNeto, CancellationToken token);
}
