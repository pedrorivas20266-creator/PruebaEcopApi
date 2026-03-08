using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api_prueba_ecop.src.Infraestructure.Predicate;

public static class ClientePredicate
{
    public static Expression<Func<Cliente, bool>> Build(string? busqueda)
    {
        if (string.IsNullOrWhiteSpace(busqueda))
            return x => true;

        var term = $"%{busqueda.Trim()}%";

        return x =>
            (x.Nombres != null && EF.Functions.Like(x.Nombres, term)) ||
            (x.Apellidos != null && EF.Functions.Like(x.Apellidos, term)) ||
            (x.Nombres != null && x.Apellidos != null && EF.Functions.Like(x.Nombres + " " + x.Apellidos, term)) ||
            (x.NumeroDocumento != null && EF.Functions.Like(x.NumeroDocumento, term)) ||
            (x.NumCliente != null && EF.Functions.Like(x.NumCliente, term)) ||
            (x.CodCliente.ToString() == busqueda.Trim());
    }
}