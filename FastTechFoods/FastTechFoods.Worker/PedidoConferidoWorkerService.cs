using Domain.Interfaces;
using UseCase.Interfaces;
using UseCase.PedidoUseCase.PedidoConferido;

namespace Worker
{
    public class PedidoConferidoWorkerService : BackgroundService
    {
        private readonly IMessageConsumer<PedidoConferidoDto> _messageConsumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public PedidoConferidoWorkerService(IMessageConsumer<PedidoConferidoDto> messageConsumer,
                      IServiceScopeFactory scopefactory)
        {
            _messageConsumer = messageConsumer;
            _scopeFactory = scopefactory;

            _messageConsumer.OnMessageReceived += ProcessarMensagem;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync();
        }

        private async Task ProcessarMensagem(PedidoConferidoDto pedidoConferidoDto)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var pedidoConferidoUseCase = scope.ServiceProvider.GetRequiredService<IPedidoConferidoUseCase>();

                pedidoConferidoUseCase.Atualizar(new PedidoConferidoDto
                {
                    Id = pedidoConferidoDto.Id,
                    ClienteId = pedidoConferidoDto.ClienteId,
                    RestauranteId = pedidoConferidoDto.RestauranteId,
                    Status = pedidoConferidoDto.Status,
                });
            }

            await Task.CompletedTask;
        }
    }
}
