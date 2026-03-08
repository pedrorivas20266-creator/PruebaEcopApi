using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Data;
using api_prueba_ecop.src.Infraestructure.Entities;
using api_prueba_ecop.src.Infraestructure.Predicate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_prueba_ecop.src.Infraestructure.Repository;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Producto?> ObtenerProducto(int codigoProducto, CancellationToken token)
    {
        return await _context.Producto.AsNoTracking().Where(x => x.CodProducto == codigoProducto).FirstOrDefaultAsync(token);
    }

    public async Task<List<Producto>?> ObtenerProductosActivos(CancellationToken token)
    {
        return await _context.Producto.AsNoTracking().Where(x => x.Activo).ToListAsync(token);
    }

    public async Task<List<ProductoQuery>?> ObtenerProductosPorFiltro(
        string? busqueda, int limite, CancellationToken token)
    {
        var queryProductos = _context.Producto.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var filtro = ProductoPredicate.Build(busqueda);
            queryProductos = queryProductos.Where(filtro);
        }

        var query = from prod in queryProductos
                    join um in _context.UnidadMedida.AsNoTracking() on prod.CodUnidadMedida equals um.CodUnidadMedida into umJoin
                    from um in umJoin.DefaultIfEmpty()
                    join cat in _context.Categoria.AsNoTracking() on prod.CodCategoria equals cat.CodCategoria into catJoin
                    from cat in catJoin.DefaultIfEmpty()
                    select new { prod, um, cat };

        var productos = await query
            .OrderBy(x => x.prod.CodProducto)
            .Take(limite)
            .Select(x => new ProductoQuery
            {
                CodProducto = x.prod.CodProducto,
                NumProducto = x.prod.NumProducto,
                CodigoBarra = x.prod.CodigoBarra,
                DesProducto = x.prod.DesProducto,
                CodCategoria = x.prod.CodCategoria,
                CodUnidadMedida = x.prod.CodUnidadMedida,
                CodIva = x.prod.CodIva,
                Activo = x.prod.Activo,
                DesUnidadMedida = x.um.DesUnidadMedida ?? string.Empty,
                DesCategoria = x.cat.DesCategoria ?? string.Empty,
                PrecioVenta = _context.Precio
                    .Where(p => p.CodProducto == x.prod.CodProducto && p.CodTipoPrecio == 1 && p.Activo)
                    .Select(p => p.PrecioVenta)
                    .FirstOrDefault() ?? 0m,
                CostoPromedio = x.prod.CostoPromedio,
                CostoUltimo = x.prod.CostoUltimo
            })
            .ToListAsync(token);

        return productos;
    }

    public async Task<Producto> InsertarProductoNuevo(Producto producto, CancellationToken token)
    {
        var productoCreado = await _context.Producto.AddAsync(producto, token);
        await _context.SaveChangesAsync(token);
        return productoCreado.Entity;
    }

    public async Task<Producto?> ActualizarProducto(int id, ProductoQuery productoActualizado, CancellationToken token)
    {
        var producto = await _context.Producto.FindAsync(new object[] { id }, token);
        if (producto == null) return null;

        producto.NumProducto = productoActualizado.NumProducto;
        producto.CodigoBarra = productoActualizado.CodigoBarra;
        producto.DesProducto = productoActualizado.DesProducto;
        producto.CodCategoria = productoActualizado.CodCategoria;
        producto.CodUnidadMedida = productoActualizado.CodUnidadMedida;
        producto.CodIva = productoActualizado.CodIva;
        producto.FechaIngreso = productoActualizado.FechaIngreso;
        producto.CostoPromedio = productoActualizado.CostoUltimo;
        producto.CostoUltimo = productoActualizado.CostoUltimo;
        producto.Activo = productoActualizado.Activo;
        producto.DescuentaStock = productoActualizado.DescuentaStock;

        await _context.SaveChangesAsync(token);
        return producto;
    }

    public async Task<bool> EliminarProducto(int id, CancellationToken token)
    {
        var producto = await _context.Producto.FindAsync(new object[] { id }, token);
        if (producto == null) return false;

        producto.Activo = false;
        await _context.SaveChangesAsync(token);
        return true;
    }

    public async Task<List<Precio>?> ObtenerPreciosDeProducto(int codigoProducto, CancellationToken token)
    {
        return await _context.Precio.AsNoTracking().Include(p => p.TipoPrecio).Where(x => x.CodProducto == codigoProducto && x.Activo).ToListAsync(token);
    }

    public async Task<Precio?> ObtenerPrecio(int codigoPrecio, CancellationToken token)
    {
        return await _context.Precio.AsNoTracking().Include(p => p.TipoPrecio).Where(x => x.CodPrecio == codigoPrecio).FirstOrDefaultAsync(token);
    }

    public async Task InsertarPrecios(List<Precio> precios, CancellationToken token)
    {
        await _context.Precio.AddRangeAsync(precios, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task<Precio?> ActualizarPrecio(int id, Precio precioActualizado, CancellationToken token)
    {
        var precio = await _context.Precio.FindAsync(new object[] { id }, token);
        if (precio == null) return null;

        precio.PrecioVenta = precioActualizado.PrecioVenta;
        await _context.SaveChangesAsync(token);
        return precio;
    }

    public async Task ActualizarPrecioGenericoProducto(int codigoProducto, decimal precioVentaNeto, CancellationToken token)
    {
        var precio = await _context
            .Precio
            .Where(x => x.CodProducto == codigoProducto && x.CodTipoPrecio == 1)
            .FirstOrDefaultAsync(token);

        if (precio != null)
        {
            precio.PrecioVenta = precioVentaNeto;
            await _context.SaveChangesAsync(token);
        }
    }

    public async Task<Iva?> ObtenerIva(int codigoIva, CancellationToken token)
    {
        return await _context.Iva.AsNoTracking().Where(x => x.CodIva == codigoIva && x.Activo).FirstOrDefaultAsync(token);
    }

    public async Task<List<Iva>?> ObtenerIvasActivos(CancellationToken token)
    {
        return await _context.Iva.AsNoTracking().Where(x => x.Activo).ToListAsync(token);
    }

    public async Task<Dictionary<int, Iva>> ObtenerIvasPorCodigos(List<int> codigosIva, CancellationToken token)
    {
        var data = await _context.Iva.AsNoTracking().Where(x => codigosIva.Contains(x.CodIva) && x.Activo).ToDictionaryAsync(x => x.CodIva, x => x, token);
        return data;
    }

    public async Task<IDbContextTransaction> IniciarTransaccion(CancellationToken token)
    {
        return await _context.Database.BeginTransactionAsync(token);
    }
}
