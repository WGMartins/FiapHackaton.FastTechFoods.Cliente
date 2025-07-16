﻿using Domain.UsuarioAggregate;

namespace Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task<Usuario?> BuscarClientePorCpfOuEmailAsync(string? cpf, string? email);
    }
}
