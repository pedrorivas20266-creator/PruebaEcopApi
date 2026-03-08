using api_prueba_ecop.src.Infraestructure.Entities;
using System.Linq.Expressions;

namespace api_prueba_ecop.src.Infraestructure.Predicate;

public static class UnidadMedidaPredicate
{
    public static Expression<Func<UnidadMedida, bool>> Build(string? busqueda)
    {
        if (string.IsNullOrWhiteSpace(busqueda))
            return x => true;

        var term = busqueda.Trim().ToLower();

        return x =>
            (x.NumUnidadMedida != null && x.NumUnidadMedida.ToLower().Contains(term)) ||
            (x.DesUnidadMedida != null && x.DesUnidadMedida.ToLower().Contains(term));
    }
}