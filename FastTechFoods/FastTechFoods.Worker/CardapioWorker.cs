using Domain.CardapioAggregate;
using Domain.Interfaces;
using Domain.PedidoAggregate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UseCase.Interfaces;

namespace Worker
{
    public class CardapioWorkerService : BackgroundService
    {
        private readonly IMessageConsumer<Cardapio> _messageConsumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public CardapioWorkerService(IMessageConsumer<Cardapio> messageConsumer,
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
        private async Task ProcessarMensagem(Cardapio contato)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var adicionarContatoUseCase = scope.ServiceProvider.GetRequiredService<ICriarPedidoUseCase>();

                //adicionarContatoUseCase.Adicionar(new AdicionarContatoDto
                //{
                //    Id = contato.Id,
                //    Nome = contato.Nome,
                //    Email = contato.Email,
                //    Telefone = contato.Telefone,
                //    RegionalId = contato.RegionalId,
                //});
            }

            await Task.CompletedTask;
        }
    }
}
