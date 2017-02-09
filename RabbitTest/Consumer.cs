using System;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitTest
{
    public class Consumer
    {
        private readonly ConnectionFactory _factory;

        public Consumer(ConnectionFactory factory)
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

                    channel.BasicQos(0, 1, false);

                    Console.WriteLine("\nRecieving...\n");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, eventArgs) =>
                    {
                        var body = eventArgs.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.Write($"Messsage: {message}");

                        int dots = message.Count(x => x == '.');
                        Thread.Sleep(dots * 1000);

                        Console.Write(" - done.");

                        channel.BasicAck(eventArgs.DeliveryTag, false);
                        Console.Write("Ack sent.");

                        Console.WriteLine();
                    };

                    channel.BasicConsume(queueName, false, consumer);
                    Console.ReadLine();
                }
            }
        }
    }
}