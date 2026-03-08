using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Data;
using api_prueba_ecop.src.Infraestructure.Entities;
using api_prueba_ecop.src.Infraestructure.Predicate;
using Microsoft.EntityFrameworkCore;

namespace api_prueba_ecop.src.Infraestructure.Repository;
public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Categoria?> ObtenerCategoria(int codigoCategoria, CancellationToken token)
    {
        var categoria = await _context
            .Categoria
            .AsNoTracking()
            .Where(x => x.CodCategoria == codigoCategoria)
            .FirstOrDefaultAsync(token);

        return categoria;
    }

    public async Task<List<Categoria>?> ObtenerCategoriasActivas(CancellationToken token)
    {
        var categorias = await _context
            .Categoria
            .AsNoTracking()
            .Where(x => x.Activo)
            .ToListAsync(token);

        return categorias;
    }

    public async Task<List<Categoria>?> ObtenerCategoriasPorFiltro(string? busqueda, int limite, CancellationToken token)
    {
        var query = _context
            .Categoria
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var filtro = CategoriaPredicate.Build(busqueda);
            query = query.Where(filtro);
        }

        query = query
            .OrderBy(x => x.CodCategoria)
            .Take(limite);

        var categorias = await query.ToListAsync(token);
        return categorias;
    }

    public async Task<Categoria> InsertarCategoriaNueva(Categoria categoria, CancellationToken token)
    {
        var categoriaCreada = await _context.Categoria.AddAsync(categoria, token);
        await _context.SaveChangesAsync(token);
        return categoriaCreada.Entity;
    }

    public async Task<Categoria?> ActualizarCategoria(int id, Categoria categoriaActualizada, CancellationToken token)
    {
        var categoria = await _context.Categoria.FindAsync(new object[] { id }, token);

        if (categoria == null)
            return null;

        categoria.NumCategoria = categoriaActualizada.NumCategoria;
        categoria.DesCategoria = categoriaActualizada.DesCategoria;
        categoria.Activo = categoriaActualizada.Activo;

        await _context.SaveChangesAsync(token);

        return categoria;
    }

    public async Task<bool> EliminarCategoria(int id, CancellationToken token)
    {
        var categoria = await _context.Categoria.FindAsync(new object[] { id }, token);

        if (categoria == null)
            return false;

        _context.Categoria.Remove(categoria);

        await _context.SaveChangesAsync(token);

        return true;
    }
}