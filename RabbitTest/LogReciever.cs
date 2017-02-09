using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitTest
{
    public class LogReciever
    {
        private readonly ConnectionFactory _factory;

        public LogReciever(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Start(string exchangeName)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                Console.WriteLine("Specify severity level(s). For example '1' or '1,2'.");
                var severityLevels = Console.ReadLine().Trim().Split(',');

                foreach (var severityLevel in severityLevels)
                {
                    channel.QueueBind(queue: queueName,
                                  exchange: exchangeName,
                                  routingKey: severityLevel);
                }

                Console.WriteLine("Waiting for logs.\n");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                };
                channel.BasicConsume(queueName, true, consumer);

                Console.ReadLine();
            }
        }
    }
}