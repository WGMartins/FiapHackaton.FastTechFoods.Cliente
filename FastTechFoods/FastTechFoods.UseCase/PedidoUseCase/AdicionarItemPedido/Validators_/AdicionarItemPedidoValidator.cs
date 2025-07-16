using FluentValidation;
using UseCase.PedidoUseCase.Shared;

namespace UseCase.PedidoUseCase.AdicionarItemPedido;

public class AdicionarItemPedidoValidator : AbstractValidator<AdicionarItemPedidoDto>
{
    public AdicionarItemPedidoValidator()
    {
        RuleFor(x => x.ItemDoCardapioId)
            .NotEmpty()
            .WithMessage("Item de Cardapio é obrigatório");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0)
            .WithMessage("Quantidade deve ser maior que zero");
    }
}