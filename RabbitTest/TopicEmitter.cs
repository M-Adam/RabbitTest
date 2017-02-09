using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitTest
{
    public class TopicEmitter
    {
        private readonly ConnectionFactory _factory;

        public TopicEmitter(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Start(string exchangeName)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

                    Console.WriteLine();

                    Console.WriteLine($"Send messages in format 'localization.severity:message'");
                    Console.WriteLine("Type 'exit' to finish");

                    string message = null;
                    while (message != "exit")
                    {
                        message = Console.ReadLine();
                        var afterSplit = message.Split(':');
                        var body = Encoding.UTF8.GetBytes(afterSplit[1]);

                        channel.BasicPublish(exchangeName,
                            afterSplit[0],
                            null,
                            body);
                    }

                    Console.ReadLine();
                }
            }
        }
    }
}