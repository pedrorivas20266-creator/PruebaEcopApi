using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Data;
using api_prueba_ecop.src.Infraestructure.Entities;
using api_prueba_ecop.src.Infraestructure.Predicate;
using Microsoft.EntityFrameworkCore;

namespace api_prueba_ecop.src.Infraestructure.Repository;
public class UnidadMedidaRepository : IUnidadMedidaRepository
{
    private readonly AppDbContext _context;

    public UnidadMedidaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UnidadMedida?> ObtenerUnidadMedida(int codigoUnidadMedida, CancellationToken token)
    {
        var unidadMedida = await _context
            .UnidadMedida
            .AsNoTracking()
            .Where(x => x.CodUnidadMedida == codigoUnidadMedida)
            .FirstOrDefaultAsync(token);

        return unidadMedida;
    }

    public async Task<List<UnidadMedida>?> ObtenerUnidadesMedidasActivas(CancellationToken token)
    {
        var unidadesMedidas = await _context
            .UnidadMedida
            .AsNoTracking()
            .Where(x => x.Activo)
            .ToListAsync(token);

        return unidadesMedidas;
    }

    public async Task<List<UnidadMedida>?> ObtenerUnidadesMedidasPorFiltro(string? busqueda, int limite, CancellationToken token)
    {
        var query = _context
            .UnidadMedida
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var filtro = UnidadMedidaPredicate.Build(busqueda);
            query = query.Where(filtro);
        }

        query = query
            .OrderBy(x => x.CodUnidadMedida)
            .Take(limite);

        var unidadesMedidas = await query.ToListAsync(token);
        return unidadesMedidas;
    }

    public async Task<UnidadMedida> InsertarUnidadMedidaNueva(UnidadMedida unidadMedida, CancellationToken token)
    {
        var unidadMedidaCreada = await _context.UnidadMedida.AddAsync(unidadMedida, token);
        await _context.SaveChangesAsync(token);
        return unidadMedidaCreada.Entity;
    }

    public async Task<UnidadMedida?> ActualizarUnidadMedida(int id, UnidadMedida unidadMedidaActualizada, CancellationToken token)
    {
        var unidadMedida = await _context.UnidadMedida.FindAsync(new object[] { id }, token);

        if (unidadMedida == null)
            return null;

        unidadMedida.NumUnidadMedida = unidadMedidaActualizada.NumUnidadMedida;
        unidadMedida.DesUnidadMedida = unidadMedidaActualizada.DesUnidadMedida;
        unidadMedida.Activo = unidadMedidaActualizada.Activo;

        await _context.SaveChangesAsync(token);

        return unidadMedida;
    }

    public async Task<bool> EliminarUnidadMedida(int id, CancellationToken token)
    {
        var unidadMedida = await _context.UnidadMedida.FindAsync(new object[] { id }, token);

        if (unidadMedida == null)
            return false;

        _context.UnidadMedida.Remove(unidadMedida);

        await _context.SaveChangesAsync(token);

        return true;
    }
}