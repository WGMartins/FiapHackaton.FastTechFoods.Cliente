using Domain.Interfaces;
using UseCase.CardapioUseCase.AtualizarCardapio;
using UseCase.Interfaces;

namespace Worker
{
    public class CardapioWorkerService : BackgroundService
    {
        private readonly IMessageConsumer<CardapioAtualizadoDto> _messageConsumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public CardapioWorkerService(IMessageConsumer<CardapioAtualizadoDto> messageConsumer,
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
        private async Task ProcessarMensagem(CardapioAtualizadoDto cardapioAtualizadoDto)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var atualizarCardapioUseCase = scope.ServiceProvider.GetRequiredService<IAtualizarCardapioUseCase>();

                atualizarCardapioUseCase.Atualizar(new CardapioAtualizadoDto
                {
                    Id = cardapioAtualizadoDto.Id,
                    RestauranteId = cardapioAtualizadoDto.RestauranteId,
                    ItensDeCardapio = cardapioAtualizadoDto.ItensDeCardapio
                        .Select(i => new ItemDeCardapioAtualizadoDto
                        {
                            Id = i.Id,
                            Nome = i.Nome,
                            Valor = i.Valor,
                            Descricao = i.Descricao,
                            Tipo = i.Tipo,
                        }).ToList()
                });
            }

            await Task.CompletedTask;
        }
    }
}
