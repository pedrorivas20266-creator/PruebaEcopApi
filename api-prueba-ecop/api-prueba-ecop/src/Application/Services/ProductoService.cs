using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using api_prueba_ecop.src.Infraestructure.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace api_prueba_ecop.src.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;
    private readonly ILogger<ProductoService> _logger;
    private readonly IMapper _mapper;

    public ProductoService(IProductoRepository repository, ILogger<ProductoService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    #region "Obtener Productos"

    public async Task<List<Producto>> ObtenerProductosActivos(CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de productos activos ###");

        var productos = await ConsultarProductosActivos(token);
        ValidarProductosEncontrados(productos, "No se encontraron productos activos");

        _logger.LogInformation("### Productos activos obtenidos correctamente. Total: {Count} ###", productos.Count);
        return productos;
    }

    public async Task<Producto> ObtenerProducto(int codigoProducto, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion del producto: {@CodigoProducto} ###", codigoProducto);

        var producto = await ConsultarProducto(codigoProducto, token);
        ValidarProductoExiste(producto, codigoProducto);

        _logger.LogInformation("### Producto obtenido correctamente ###");
        return producto!;
    }

    public async Task<List<ProductoQuery>?> ObtenerProductosPorFiltro(string? busqueda, int? limite, CancellationToken token)
    {
        var limiteFinal = NormalizarLimite(limite);
        _logger.LogInformation("### Iniciando proceso de obtencion de productos por filtro. Busqueda: {Busqueda}, Limite: {Limite} ###", busqueda, limiteFinal);

        var productos = await ConsultarProductosPorFiltro(busqueda, limiteFinal, token);

        _logger.LogInformation("### Productos obtenidos correctamente por filtro. Total: {Count} ###", productos?.Count);
        return productos;
    }

    public async Task<ProductoConPrecioResponse> ObtenerProductoConPrecios(int codigoProducto, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion del producto con precios: {@CodigoProducto} ###", codigoProducto);

        var producto = await ConsultarProducto(codigoProducto, token);
        ValidarProductoExiste(producto, codigoProducto);

        var precios = await ConsultarPreciosDeProducto(codigoProducto, token);
        var response = ConstruirProductoConPreciosResponse(producto!, precios);

        _logger.LogInformation("### Producto con precios obtenido correctamente ###");
        return response;
    }

    public async Task<List<Precio>> ObtenerPreciosDeProducto(int codigoProducto, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de precios del producto: {@CodigoProducto} ###", codigoProducto);

        var precios = await ConsultarPreciosDeProducto(codigoProducto, token);
        ValidarPreciosEncontrados(precios, codigoProducto);

        _logger.LogInformation("### Precios del producto obtenidos correctamente. Total: {Count} ###", precios.Count);
        return precios;
    }

    private async Task<List<Producto>?> ConsultarProductosActivos(CancellationToken token)
    {
        return await _repository.ObtenerProductosActivos(token);
    }

    private async Task<Producto?> ConsultarProducto(int codigoProducto, CancellationToken token)
    {
        return await _repository.ObtenerProducto(codigoProducto, token);
    }

    private async Task<List<ProductoQuery>?> ConsultarProductosPorFiltro(string? busqueda, int limite, CancellationToken token)
    {
        return await _repository.ObtenerProductosPorFiltro(busqueda, limite, token);
    }

    private async Task<List<Precio>?> ConsultarPreciosDeProducto(int codigoProducto, CancellationToken token)
    {
        return await _repository.ObtenerPreciosDeProducto(codigoProducto, token);
    }

    private int NormalizarLimite(int? limite)
    {
        return (limite.HasValue && limite.Value > 0) ? limite.Value : 20;
    }

    private void ValidarProductosEncontrados(dynamic productos, string mensaje)
    {
        if (productos == null || productos?.Count == 0)
        {
            _logger.LogWarning(mensaje);
            throw new DomainException(mensaje);
        }
    }

    private void ValidarProductoExiste(Producto? producto, int codigoProducto)
    {
        if (producto == null)
        {
            _logger.LogWarning("Producto no encontrado para el codigo: {CodigoProducto}", codigoProducto);
            throw new DomainException("Producto no encontrado");
        }
    }

    private void ValidarPreciosEncontrados(List<Precio>? precios, int codigoProducto)
    {
        if (precios == null || precios.Count == 0)
        {
            _logger.LogWarning("No se encontraron precios para el producto: {CodigoProducto}", codigoProducto);
            throw new DomainException("No se encontraron precios para el producto");
        }
    }

    private ProductoConPrecioResponse ConstruirProductoConPreciosResponse(Producto producto, List<Precio>? precios)
    {
        return new ProductoConPrecioResponse
        {
            CodProducto = producto.CodProducto,
            NumProducto = producto.NumProducto,
            CodigoBarra = producto.CodigoBarra,
            DesProducto = producto.DesProducto,
            CodCategoria = producto.CodCategoria,
            CodUnidadMedida = producto.CodUnidadMedida,
            CodIva = producto.CodIva,
            FechaIngreso = producto.FechaIngreso,
            CostoPromedio = producto.CostoPromedio,
            CostoUltimo = producto.CostoUltimo,
            Activo = producto.Activo,
            DescuentaStock = producto.DescuentaStock,
            FecGra = producto.FecGra,
            Precios = precios?.Select(p => new PrecioResponse
            {
                CodPrecio = p.CodPrecio,
                CodTipoPrecio = p.CodTipoPrecio,
                DesTipoPrecio = p.TipoPrecio?.DesTipoPrecio,
                PrecioVenta = p.PrecioVenta,
                Activo = p.Activo
            }).ToList()
        };
    }

    #endregion

    #region "Insertar Producto Con Precios"

    public async Task<ProductoConPrecioResponse> NuevoProductoConPrecios(ProductoRequest request, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de insercion del nuevo producto con precios: {@Request} ###", JsonSerializer.Serialize(request));

        var producto = PrepararProductoParaInsercion(request);
        var productoConPrecios = await EjecutarInsercionConTransaccion(producto, request.Precios, token);

        _logger.LogInformation("### Producto con precios insertado correctamente. Codigo: {CodProducto} ###", productoConPrecios.CodProducto);
        return productoConPrecios;
    }

    private Producto PrepararProductoParaInsercion(ProductoRequest request)
    {
        return new Producto
        {
            NumProducto = request.NumProducto,
            CodigoBarra = request.CodigoBarra,
            DesProducto = request.DesProducto,
            CodCategoria = request.CodCategoria,
            CodUnidadMedida = request.CodUnidadMedida,
            CodIva = request.CodIva,
            FechaIngreso = request.FechaIngreso ?? DateTime.Now,
            CostoPromedio = request.CostoUltimo,
            CostoUltimo = request.CostoUltimo,
            DescuentaStock = request.DescuentaStock,
            Activo = true,
            FecGra = DateTime.Now
        };
    }

    private async Task<ProductoConPrecioResponse> EjecutarInsercionConTransaccion(Producto producto, List<PrecioRequest>? preciosRequest, CancellationToken token)
    {
        using var transaction = await IniciarTransaccion(token);

        try
        {
            var productoCreado = await InsertarProductoEnBaseDatos(producto, token);

            if (TienePreciosParaInsertar(preciosRequest))
            {
                await InsertarPreciosDelProducto(productoCreado.CodProducto, preciosRequest!, token);
            }

            await ConfirmarTransaccion(transaction, token);
            return await ObtenerProductoConPrecios(productoCreado.CodProducto, token);
        }
        catch
        {
            await RevertirTransaccion(transaction, token);
            throw new DomainException("Error al crear el producto con precios");
        }
    }

    private async Task<IDbContextTransaction> IniciarTransaccion(CancellationToken token)
    {
        return await _repository.IniciarTransaccion(token);
    }

    private async Task<Producto> InsertarProductoEnBaseDatos(Producto producto, CancellationToken token)
    {
        _logger.LogInformation("Insertando producto en base de datos");
        return await _repository.InsertarProductoNuevo(producto, token);
    }

    private bool TienePreciosParaInsertar(List<PrecioRequest>? precios)
    {
        return precios != null && precios.Count > 0;
    }

    private async Task InsertarPreciosDelProducto(int codigoProducto, List<PrecioRequest> preciosRequest, CancellationToken token)
    {
        _logger.LogInformation("Insertando {Count} precios del producto", preciosRequest.Count);
        var precios = PrepararPreciosParaInsercion(codigoProducto, preciosRequest);
        await _repository.InsertarPrecios(precios, token);
    }

    private List<Precio> PrepararPreciosParaInsercion(int codigoProducto, List<PrecioRequest> preciosRequest)
    {
        return preciosRequest.Select(p => new Precio
        {
            CodProducto = codigoProducto,
            CodTipoPrecio = p.CodTipoPrecio,
            PrecioVenta = p.PrecioVenta,
            Activo = true,
            FecGra = DateTime.Now
        }).ToList();
    }

    private async Task ConfirmarTransaccion(IDbContextTransaction transaction, CancellationToken token)
    {
        await transaction.CommitAsync(token);
    }

    private async Task RevertirTransaccion(IDbContextTransaction transaction, CancellationToken token)
    {
        await transaction.RollbackAsync(token);
        _logger.LogError("Error al insertar producto con precios. Se realizó rollback de la transacción");
    }

    #endregion

    #region "Actualizar Producto"

    public async Task<Producto> ActualizarProducto(int codigoProducto, ProductoQuery producto, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de actualizacion del producto: {@Producto} ###", producto);

        var productoActualizado = await EjecutarActualizacionProducto(codigoProducto, producto, token);

        ValidarActualizacionProducto(productoActualizado);

        _logger.LogInformation("### Producto actualizado correctamente ###");
        return productoActualizado!;
    }

    private async Task<Producto?> EjecutarActualizacionProducto(int codigoProducto, ProductoQuery producto, CancellationToken token)
    {
        var productoActualizado = await _repository.ActualizarProducto(codigoProducto, producto, token);

        if (producto.PrecioVenta > 0)
        {
            await ActualizarPrecioGenericoProducto(codigoProducto, producto.PrecioVenta, token);
        }

        return productoActualizado;
    }

    private async Task ActualizarPrecioGenericoProducto(int codigoProducto, decimal precioVenta, CancellationToken token)
    {
        _logger.LogInformation("Actualizando precio generico del producto: {CodigoProducto} a PrecioVenta: {PrecioVenta}", codigoProducto, precioVenta);
        await _repository.ActualizarPrecioGenericoProducto(codigoProducto, precioVenta, token);
    }

    private void ValidarActualizacionProducto(Producto? producto)
    {
        if (producto == null)
        {
            _logger.LogWarning("Error al actualizar el producto");
            throw new DomainException("Error al actualizar el producto");
        }
    }

    #endregion

    #region "Eliminar Producto"

    public async Task EliminarProducto(int codigoProducto, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de eliminacion del producto: {@CodigoProducto} ###", codigoProducto);

        var operacion = await EjecutarEliminacionProducto(codigoProducto, token);
        ValidarEliminacionProducto(operacion);

        _logger.LogInformation("### Producto eliminado correctamente ###");
    }

    private async Task<bool> EjecutarEliminacionProducto(int codigoProducto, CancellationToken token)
    {
        return await _repository.EliminarProducto(codigoProducto, token);
    }

    private void ValidarEliminacionProducto(bool operacion)
    {
        if (!operacion)
        {
            _logger.LogWarning("Error al eliminar el producto");
            throw new DomainException("Error al eliminar el producto");
        }
    }

    #endregion

    #region "Actualizar Precios De Producto"

    public async Task<List<Precio>> ActualizarPreciosDeProducto(int codigoProducto, ActualizarPreciosRequest request, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de actualizacion de precios del producto: {CodigoProducto} ###", codigoProducto);

        await ValidarProductoExisteParaActualizar(codigoProducto, token);
        await ActualizarListaDePrecios(request.Precios, token);
        var preciosActualizados = await ConsultarPreciosDeProducto(codigoProducto, token);

        _logger.LogInformation("### Precios actualizados correctamente. Total: {Count} ###", preciosActualizados?.Count ?? 0);
        return preciosActualizados ?? new List<Precio>();
    }

    private async Task ValidarProductoExisteParaActualizar(int codigoProducto, CancellationToken token)
    {
        var producto = await ConsultarProducto(codigoProducto, token);
        ValidarProductoExiste(producto, codigoProducto);
    }

    private async Task ActualizarListaDePrecios(List<ActualizarPrecioItem> precios, CancellationToken token)
    {
        foreach (var precio in precios)
        {
            await ActualizarPrecioIndividual(precio, token);
        }
    }

    private async Task ActualizarPrecioIndividual(ActualizarPrecioItem item, CancellationToken token)
    {
        _logger.LogInformation("Actualizando precio: CodPrecio={CodPrecio}, NuevoPrecio={PrecioVenta}", item.CodPrecio, item.PrecioVenta);

        var precioActualizado = new Precio { PrecioVenta = item.PrecioVenta };
        var resultado = await _repository.ActualizarPrecio(item.CodPrecio, precioActualizado, token);

        if (resultado == null)
        {
            _logger.LogWarning("No se pudo actualizar el precio: {CodPrecio}", item.CodPrecio);
            throw new DomainException($"No se pudo actualizar el precio con código {item.CodPrecio}");
        }
    }

    #endregion
}