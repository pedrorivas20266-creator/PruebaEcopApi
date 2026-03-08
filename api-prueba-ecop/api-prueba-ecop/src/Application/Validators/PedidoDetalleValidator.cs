using api_prueba_ecop.src.Application.Models.Requests;
using FluentValidation;

namespace api_prueba_ecop.src.Application.Validators;
public class PedidoDetalleValidator : AbstractValidator<PedidoDetalleRequest>
{
    public PedidoDetalleValidator()
    {
        RuleFor(x => x.CodProducto)
            .GreaterThan(0).WithMessage("El código de producto debe ser mayor a 0");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0")
            .LessThanOrEqualTo(9999999.99m).WithMessage("La cantidad no puede exceder 9999999.99");

        RuleFor(x => x.PrecioUnitario)
            .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a 0")
            .LessThanOrEqualTo(9999999999999999.99m).WithMessage("El precio unitario no puede exceder el límite permitido");

        RuleFor(x => x.LineaNumero)
            .GreaterThan(0).WithMessage("El número de línea debe ser mayor a 0");
    }
}