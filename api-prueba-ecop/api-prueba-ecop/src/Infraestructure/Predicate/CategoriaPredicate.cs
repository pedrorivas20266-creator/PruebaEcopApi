using api_prueba_ecop.src.Infraestructure.Entities;
using System.Linq.Expressions;

namespace api_prueba_ecop.src.Infraestructure.Predicate;

public static class CategoriaPredicate
{
    public static Expression<Func<Categoria, bool>> Build(string? busqueda)
    {
        if (string.IsNullOrWhiteSpace(busqueda))
            return x => true;

        var term = busqueda.Trim().ToLower();

        return x =>
            (x.NumCategoria != null && x.NumCategoria.ToLower().Contains(term)) ||
            (x.DesCategoria != null && x.DesCategoria.ToLower().Contains(term));
    }
}