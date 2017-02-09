using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitTest
{
    public class TopicReciever
    {
        private readonly ConnectionFactory _factory;

        public TopicReciever(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Start(string exchangeName)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                Console.WriteLine("Specify binding pattern(s). For example: \n'service.*' \n'*.critical' \n'repository.info' \n'#'");
                var bindingKeys = Console.ReadLine().Trim().Split(',');

                foreach (var key in bindingKeys)
                {
                    channel.QueueBind(queue: queueName,
                        exchange: exchangeName,
                        routingKey: key);
                }

                Console.WriteLine("Waiting for logs.\n");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"{ea.RoutingKey}:{message}");
                };
                channel.BasicConsume(queueName, true, consumer);

                Console.ReadLine();
            }
        }
    }
}