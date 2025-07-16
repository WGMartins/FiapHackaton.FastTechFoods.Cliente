using Domain.Interfaces;
using UseCase.Interfaces;

namespace UseCase.CardapioUseCase.AtualizarCardapio;

public class AtualizarCardapioUseCase : IAtualizarCardapioUseCase
{
    private readonly ICardapioRepository _cardapioRepository;

    public AtualizarCardapioUseCase(ICardapioRepository cardapioRepository)
    {
        _cardapioRepository = cardapioRepository;
    }

    public void Atualizar(CardapioAtualizadoDto cardapioAtualizadoDto)
    {
        var cardapio = _cardapioRepository.ObterPorId(cardapioAtualizadoDto.Id);

        if (cardapio is null || cardapio.RestauranteId != cardapioAtualizadoDto.RestauranteId)
        {
            throw new Exception("Cardapio não encontrado");
        }

        foreach (var item in cardapioAtualizadoDto.ItensDeCardapio)
        {
            var itemDeCardapio = cardapio.ItensDeCardapio.Where(x => x.Id == item.Id).FirstOrDefault();

            if (itemDeCardapio is null)
            {
                cardapio.AdicionarItem(item.Id, cardapio.Id, item.Nome, item.Valor, item.Descricao, item.Tipo);
            }
            else
            {
                cardapio.AtualizarItem(item.Id, item.Nome, item.Valor, item.Descricao, item.Tipo);
            }
        }

        var itensDeCardapioExcluidos = cardapio.ItensDeCardapio.ExceptBy(cardapioAtualizadoDto.ItensDeCardapio.Select(a => a.Id), i => i.Id).ToList();

        foreach (var item in itensDeCardapioExcluidos)
        {
            cardapio.RemoverItem(item);
        }

        _cardapioRepository.Atualizar(cardapio);
    }
}
