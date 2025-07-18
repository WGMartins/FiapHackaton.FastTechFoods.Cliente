﻿using Domain.Interfaces;
using UseCase.Interfaces;

namespace UseCase.PedidoUseCase.PedidoConferido;

public class PedidoConferidoUseCase : IPedidoConferidoUseCase
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoConferidoUseCase(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public void Atualizar(PedidoConferidoDto pedidoConferidoDto)
    {
        var pedido = _pedidoRepository.ObterPorId(pedidoConferidoDto.Id);

        if (pedido is null || pedido.RestauranteId != pedidoConferidoDto.RestauranteId)
        {
            throw new Exception("Pedido não encontrado");
        }

        pedido.AlterarStatus(pedidoConferidoDto.Status);

        _pedidoRepository.Atualizar(pedido);
    }
}
