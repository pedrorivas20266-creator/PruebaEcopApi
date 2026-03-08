using api_prueba_ecop.src.Infraestructure.Entities;
using System.Linq.Expressions;

namespace api_prueba_ecop.src.Infraestructure.Predicate;

public static class ProductoPredicate
{
    public static Expression<Func<Producto, bool>> Build(string? busqueda)
    {
        if (string.IsNullOrWhiteSpace(busqueda))
            return x => true;

        var term = busqueda.Trim().ToLower();

        return x =>
            (x.NumProducto != null && x.NumProducto.ToLower().Contains(term)) ||
            (x.CodigoBarra != null && x.CodigoBarra.ToLower().Contains(term)) ||
            (x.DesProducto != null && x.DesProducto.ToLower().Contains(term)) ||
            (x.CodProducto.ToString().Equals(term));
    }
}