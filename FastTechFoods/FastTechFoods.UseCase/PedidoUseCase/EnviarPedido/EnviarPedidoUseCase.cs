using Domain.Interfaces;
using Domain.PedidoAggregate;
using UseCase.Interfaces;

namespace UseCase.PedidoUseCase.EnviarPedido
{
    public class EnviarPedidoUseCase : IEnviarPedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMessagePublisher _publisherPedido;

        public EnviarPedidoUseCase(IPedidoRepository pedidoRepository, Func<string, IMessagePublisher> publisherFactory)
        {
            _pedidoRepository = pedidoRepository;
            _publisherPedido = publisherFactory("ProducerPedido");
        }

        public void Enviar(Guid idCliente, Guid id)
        {
            var pedido = _pedidoRepository.ObterPorId(id);

            if (pedido is null || pedido.ClienteId != idCliente)
            {
                throw new Exception("Pedido não encontrado");
            }

            if (pedido.Status != Status.EmAndamento)
            {
                throw new Exception("Pedido não permite envio");
            }

            pedido.AlterarStatus(Status.Enviado);
            
            _pedidoRepository.Atualizar(pedido);

            _publisherPedido.PublishAsync(new PedidoEnviadoDto
            {
                Id = pedido.Id,
                RestauranteId = pedido.RestauranteId,
                ClienteId = pedido.ClienteId,
                FormaDeEntrega = pedido.FormaDeEntrega,
                Status = pedido.Status,
                ValorTotal = pedido.ValorTotal,
                ItensDePedido = pedido.ItensDePedido
                    .Select(i => new ItemDePedidoEnviadoDto
                    {
                        Id = i.Id,
                        Nome = i.Nome,
                        ValorUnitario = i.ValorUnitario,
                        Quantidade = i.Quantidade,
                        ValorTotal = i.ValorTotal
                    })
                    .ToList()
            });
        }
    }
}
