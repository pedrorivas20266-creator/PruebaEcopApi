using api_prueba_ecop.src.Application.Models.Requests;
using FluentValidation;

namespace api_prueba_ecop.src.Application.Validators;
public class PedidoValidator : AbstractValidator<PedidoRequest>
{
    public PedidoValidator()
    {
        RuleFor(x => x.NumPedido)
            .NotEmpty().WithMessage("El número de pedido es requerido")
            .MaximumLength(55).WithMessage("El número de pedido no puede exceder 55 caracteres");

        RuleFor(x => x.Fecha)
            .NotNull().WithMessage("La fecha es requerida")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha no puede ser futura");

        RuleFor(x => x.CodUsuario)
            .GreaterThan(0).WithMessage("El código de usuario debe ser mayor a 0");

        RuleFor(x => x.CodCliente)
            .GreaterThan(0).WithMessage("El código de cliente debe ser mayor a 0");

        RuleFor(x => x.CodMoneda)
            .GreaterThan(0).WithMessage("El código de moneda debe ser mayor a 0");

        RuleFor(x => x.Detalles)
            .NotNull().WithMessage("Los detalles del pedido son requeridos")
            .NotEmpty().WithMessage("Debe incluir al menos un detalle en el pedido");

        RuleForEach(x => x.Detalles)
            .SetValidator(new PedidoDetalleValidator());
    }
}