using FluentValidation;

namespace UseCase.PedidoUseCase.CriarPedido;

public class CriarPedidoValidator : AbstractValidator<AdicionarPedidoDto>
{
    public CriarPedidoValidator()
    {
        RuleFor(x => x.ItensDePedido)
            .NotNull()
            .WithMessage("Itens de pedido não informados")
            .NotEmpty()
            .WithMessage("O pedido deve conter pelo menos um item.");


        RuleForEach(x => x.ItensDePedido).ChildRules(item =>
        {
            item.RuleFor(i => i.ItemDoCardapioId)
                .NotEmpty()
                .WithMessage("Item de Cardapio é obrigatório");

            item.RuleFor(i => i.Quantidade)
                .GreaterThan(0)
                .WithMessage("Quantidade deve ser maior que zero");
        });
    }
}
