using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitTest
{
    public class LogEmitter
    {
        private readonly ConnectionFactory _factory;

        public LogEmitter(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Start(string exchangeName)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

                    Console.WriteLine();

                    Console.WriteLine("Prepend messages with '1', '2' or '3' to set severity level.");

                    Console.WriteLine("Type 'exit' to finish");

                    string message = null;
                    while (message != "exit")
                    {
                        message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchangeName,
                            message.Trim()[0].ToString(),
                            null,
                            body);
                    }



                    Console.ReadLine();
                }
            }
        }
    }
}