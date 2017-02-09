using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitTest
{
    public class Producer
    {
        private readonly ConnectionFactory _factory;

        public Producer(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Start(string queueName)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName,
                        true,
                        false,
                        false,
                        null);

                    Console.WriteLine();
                    Console.WriteLine("Type 'exit' to finish");

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    string message = null;
                    while (message != "exit")
                    {
                        message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish("",
                            queueName,
                            properties,
                            body);
                    }



                    Console.ReadLine();
                }
            }
        }
    }
}