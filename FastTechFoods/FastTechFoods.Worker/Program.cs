using Domain.CardapioAggregate;
using Domain.Interfaces;
using Domain.PedidoAggregate;
using FluentValidation;
using Infrastructure.RabbitMq;
using Infrastructure.RabbitMQ;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
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

//builder.Services.AddScoped<IContatoRepository, ContatoRepository>();

//builder.Services.AddScoped<IAdicionarContatoUseCase, AdicionarContatoUseCase>();
//builder.Services.AddScoped<IValidator<AdicionarContatoDto>, AdicionarContatoValidator>();

#region RabbitMQ

builder.Services.Configure<RabbitMqSettings>("PedidoConsumer", builder.Configuration.GetSection("RabbitMQ:PedidoConsumer"));
builder.Services.Configure<RabbitMqSettings>("CardapioConsumer", builder.Configuration.GetSection("RabbitMQ:CardapioConsumer"));


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

builder.Services.AddSingleton<IMessageConsumer<Cardapio>>(sp =>
{
    var settings = sp.GetRequiredService<IOptionsMonitor<RabbitMqSettings>>().Get("CardapioConsumer");
    var factory = new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password,
        VirtualHost = settings.VirtualHost
    };

    return new RabbitMQMessageConsumer<Cardapio>(() => factory.CreateConnectionAsync(), settings);
});

#endregion

var host = builder.Build();
host.Run();
