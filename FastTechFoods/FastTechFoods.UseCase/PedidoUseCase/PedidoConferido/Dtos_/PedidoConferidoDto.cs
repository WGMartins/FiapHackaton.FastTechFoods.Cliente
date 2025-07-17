using Domain.PedidoAggregate;

namespace UseCase.PedidoUseCase.PedidoConferido;

public class PedidoConferidoDto
{
    public Guid Id { get; set; }
    public Guid RestauranteId { get; set; }
    public Status Status { get; set; }
}
