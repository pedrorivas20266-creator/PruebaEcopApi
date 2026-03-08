using api_prueba_ecop.src.Application.Models.Requests;
using FluentValidation;

namespace api_prueba_ecop.src.Application.Validators;
public class AnularPedidoValidator : AbstractValidator<AnularPedidoRequest>
{
    public AnularPedidoValidator()
    {
        RuleFor(x => x.MotivoAnulacion)
            .NotEmpty().WithMessage("El motivo de anulación es requerido")
            .MinimumLength(10).WithMessage("El motivo de anulación debe tener al menos 10 caracteres")
            .MaximumLength(255).WithMessage("El motivo de anulación no puede exceder 255 caracteres");
    }
}