using Domain.PedidoAggregate;

namespace UseCase.PedidoUseCase.EnviarPedido;

public class PedidoEnviadoDto
{
    public Guid Id { get; set; }
    public Guid RestauranteId { get; set; }
    public Guid ClienteId { get; set; }
    public Status Status { get; set; }
    public FormaDeEntrega FormaDeEntrega { get; set; }
    public decimal ValorTotal { get; set; }
    public IList<ItemDePedidoEnviadoDto> ItensDePedido { get; set; } = [];
}
