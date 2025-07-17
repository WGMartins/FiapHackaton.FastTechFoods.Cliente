using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Auth;
using Infrastructure.RabbitMq;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using UseCase.AuthUseCase.AutenticarUsuario;
using UseCase.CardapioUseCase.ListarItensCardapio;
using UseCase.Interfaces;
using UseCase.PedidoUseCase.AdicionarItemPedido;
using UseCase.PedidoUseCase.CancelarPedido;
using UseCase.PedidoUseCase.CriarPedido;
using UseCase.PedidoUseCase.EnviarPedido;
using UseCase.PedidoUseCase.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

const string serviceName = "FastTechFoods";

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Logging.AddOpenTelemetry(options =>
//{
//    options
//        .SetResourceBuilder(
//            ResourceBuilder.CreateDefault()
//                .AddService(serviceName))
//        .AddConsoleExporter();
//});

//builder.Services.AddOpenTelemetry()
//      .ConfigureResource(resource => resource.AddService(serviceName))
//      .WithTracing(tracing => tracing
//          .AddAspNetCoreInstrumentation()
//          .AddConsoleExporter())
//      .WithMetrics(metrics => metrics
//          .AddAspNetCoreInstrumentation()
//          .AddConsoleExporter());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetSection("ConnectionStrings")["ConnectionString"]);
}, ServiceLifetime.Scoped);

#region Banco de Dados

builder.Services.AddScoped<ICardapioRepository, CardapioRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

#endregion

#region Cardapio

builder.Services.AddScoped<IListarItensCardapioUseCase, ListarItensCardapioUseCase>();

#endregion

#region Pedido

builder.Services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
builder.Services.AddScoped<IValidator<AdicionarPedidoDto>, CriarPedidoValidator>();

builder.Services.AddScoped<IAdicionarItemPedidoUseCase, AdicionarItemPedidoUseCase>();
builder.Services.AddScoped<IValidator<AdicionarItemPedidoDto>, AdicionarItemPedidoValidator>();

builder.Services.AddScoped<ICancelarPedidoUseCase, CancelarPedidoUseCase>();

builder.Services.AddScoped<IEnviarPedidoUseCase, EnviarPedidoUseCase>();

#endregion

#region Autentica��o
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAutenticarUsuarioUseCase, AutenticarUsuarioUseCase>();
builder.Services.AddScoped<IValidator<AutenticarUsuarioDto>, AutenticarUsuarioValidator>();
builder.Services.AddScoped<JwtToken>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]))
        };
    });


#endregion

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FastTechFoods", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT como: Bearer {seu_token_aqui}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region RabbitMQ

builder.Services.Configure<RabbitMqSettings>("ProducerPedido", builder.Configuration.GetSection("RabbitMQ:Pedido"));

builder.Services.AddSingleton<Func<string, IMessagePublisher>>(sp => producerName =>
{
    var settingsMonitor = sp.GetRequiredService<IOptionsMonitor<RabbitMqSettings>>();
    var settings = settingsMonitor.Get(producerName);

    var factory = new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password,
        VirtualHost = settings.VirtualHost
    };

    return new RabbitMqMessagePublisher(
        async () => await factory.CreateConnectionAsync(),
        Options.Create(settings)
    );
});

#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//builder.Services.AddAuthorization();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
