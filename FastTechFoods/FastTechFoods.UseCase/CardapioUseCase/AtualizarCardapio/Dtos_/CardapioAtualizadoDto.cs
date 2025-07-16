﻿namespace UseCase.CardapioUseCase.AtualizarCardapio;

public class CardapioAtualizadoDto
{
    public Guid Id { get; set; }
    public Guid RestauranteId { get; set; }
    public required IList<ItemDeCardapioAtualizadoDto> ItensDeCardapio { get; set; } = [];
}
