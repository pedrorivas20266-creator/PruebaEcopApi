using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;

public interface IProductoService
{
    Task<List<Producto>> ObtenerProductosActivos(CancellationToken token);
    Task<List<ProductoQuery>?> ObtenerProductosPorFiltro(string? busqueda, int? limite, CancellationToken token);
    Task<Producto> ObtenerProducto(int codigoProducto, CancellationToken token);
    Task<ProductoConPrecioResponse> NuevoProductoConPrecios(ProductoRequest request, CancellationToken token);
    Task<Producto> ActualizarProducto(int codigoProducto, ProductoQuery producto, CancellationToken token);
    Task EliminarProducto(int codigoProducto, CancellationToken token);
    Task<ProductoConPrecioResponse> ObtenerProductoConPrecios(int codigoProducto, CancellationToken token);
    Task<List<Precio>> ObtenerPreciosDeProducto(int codigoProducto, CancellationToken token);
    Task<List<Precio>> ActualizarPreciosDeProducto(int codigoProducto, ActualizarPreciosRequest request, CancellationToken token);
}