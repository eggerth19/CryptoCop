using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace PaymentService
{
    class PaymentService
    {
        static bool ValidateCreditCard(string cardNumber)
        {
            int[] cardInt = new int[cardNumber.Length];

            for( int i = 0; i < cardNumber.Length; i++)
            {
                cardInt[i] = (int)(cardNumber[i] - '0');
            }

            for (int i = cardInt.Length - 2; i >= 0; i = i -2)
            {
                int tempValue = cardInt[i];
                tempValue = tempValue * 2;
                if (tempValue > 9)
                {
                    tempValue = tempValue % 10 + 1;
                }
                cardInt[i] = tempValue;
            }

            int total = 0;
            for (int i = 0; i < cardInt.Length; i++)
            {
                total += cardInt[i];
            }

            if (total % 10 == 0)
            {
                return true;
            }

            return false;
        }
        
        public static void Main()
        {
            string hostName = Environment.GetEnvironmentVariable("PRODUCTION_ENV");
            if (hostName == null) { hostName = "localhost"; }
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = hostName;
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Direct, durable: true);

                var queueName = "payment_queue";

                channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                
                channel.QueueBind(queue: queueName,
                                exchange: "order_exchange",
                                routingKey: "create_order");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var jo = JObject.Parse(message);
                    if(ValidateCreditCard(jo["CreditCard"].ToString()) == true)
                    {
                        Console.WriteLine("CreditCard is valid");
                    }
                    else{
                        Console.WriteLine("CreditCard is invalid");
                    }
                };
                channel.BasicConsume(queue: queueName,
                                    autoAck: true,
                                    consumer: consumer);
                Console.ReadLine();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
