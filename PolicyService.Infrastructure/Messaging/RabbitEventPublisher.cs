namespace PolicyService.Infrastructure.Messaging
{    
    using System.Text;
    
    using RabbitMQ.Client;
    using Newtonsoft.Json;

    using Microsoft.Extensions.Options;
    
    using MicroservicesPOC.Shared.Messaging;
    
    using PolicyService.Application.Common.Interfaces;

    public class RabbitEventPublisher : IEventPublisher
    {
        private readonly RabbitMQConfigurations _options;

        public RabbitEventPublisher(IOptions<RabbitMQConfigurations> configuration) => this._options = configuration.Value;

        public void PublishMessage<T>(T message)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = this._options.Hostname, UserName = this._options.Username, Password = this._options.Password };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string routingKey = typeof(T).Name.ToLower();

            channel.ExchangeDeclare(exchange: this._options.Exchange.Name, "topic", durable: true, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: $"{this._options.Queue.Prefix}{routingKey}", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: $"{this._options.Queue.Prefix}{routingKey}", exchange: this._options.Exchange.Name, routingKey: routingKey, arguments: null);

            channel.BasicPublish(exchange: this._options.Exchange.Name, routingKey: routingKey, basicProperties: null, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }
    }
}
