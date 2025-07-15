namespace Infrastructure.RabbitMq;

public class RabbitMqSettings
{
    public string VirtualHost { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    public string Queue { get; set; }
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}