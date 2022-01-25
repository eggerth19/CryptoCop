using Cryptocop.Software.API.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService
    {
        public void PublishMessage(string routingKey, object body)
        {
        string hostName = Environment.GetEnvironmentVariable("PRODUCTION_ENV");
        if (hostName == null) { hostName = "localhost"; }
        string jsonString = JsonConvert.SerializeObject(body);
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = "guest";
        factory.Password = "guest";
        factory.HostName= hostName;
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Direct, durable: true);

            var byteBody = Encoding.UTF8.GetBytes(jsonString);
            channel.BasicPublish(exchange: "order_exchange",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: byteBody);
        }
        }
    }
}