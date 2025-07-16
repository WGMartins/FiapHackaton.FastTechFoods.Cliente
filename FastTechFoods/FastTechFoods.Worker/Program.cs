using Domain.Interfaces;
using Infrastructure.RabbitMq;
using Infrastructure.RabbitMQ;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UseCase.CardapioUseCase.AtualizarCardapio;
using UseCase.Interfaces;
using UseCase.PedidoUseCase.PedidoConferido;
using Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<PedidoConferidoWorkerService>();
builder.Services.AddHostedService<CardapioWorkerService>();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetSection("ConnectionStrings")["ConnectionString"]);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ICardapioRepository, CardapioRepository>();

builder.Services.AddScoped<IPedidoConferidoUseCase, PedidoConferidoUseCase>();
builder.Services.AddScoped<IAtualizarCardapioUseCase, AtualizarCardapioUseCase>();

#region RabbitMQ

builder.Services.Configure<RabbitMqSettings>("PedidoConsumer", builder.Configuration.GetSection("RabbitMQ:Pedido"));
builder.Services.Configure<RabbitMqSettings>("CardapioConsumer", builder.Configuration.GetSection("RabbitMQ:Cardapio"));


builder.Services.AddSingleton<IMessageConsumer<PedidoConferidoDto>>(sp =>
{
    var settings = sp.GetRequiredService<IOptionsMonitor<RabbitMqSettings>>().Get("PedidoConsumer");
    var factory = new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password,
        VirtualHost = settings.VirtualHost
    };

    return new RabbitMQMessageConsumer<PedidoConferidoDto>(() => factory.CreateConnectionAsync(), settings);
});

builder.Services.AddSingleton<IMessageConsumer<CardapioAtualizadoDto>>(sp =>
{
    var settings = sp.GetRequiredService<IOptionsMonitor<RabbitMqSettings>>().Get("CardapioConsumer");
    var factory = new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password,
        VirtualHost = settings.VirtualHost
    };

    return new RabbitMQMessageConsumer<CardapioAtualizadoDto>(() => factory.CreateConnectionAsync(), settings);
});

#endregion

var host = builder.Build();
host.Run();
