using System;
using RabbitMQ.Client;

namespace RabbitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            var queueName = "tasks_queue";
            var exchangeName = "log";

            Console.WriteLine("1. Producer \n 2. Consumer \n\n 3. Log emitter \n 4. Log reciever \n\n 5. Topic emitter \n 6. Topic reciever");
            Console.WriteLine();

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    new Producer(factory).Start(queueName);
                    break;
                case '2':
                    new Consumer(factory).Start(queueName);
                    break;
                case '3':
                    new LogEmitter(factory).Start(exchangeName);
                    break;
                case '4':
                    new LogReciever(factory).Start(exchangeName);
                    break;
                case '5':
                    new TopicEmitter(factory).Start(exchangeName);
                    break;
                case '6':
                    new TopicReciever(factory).Start(exchangeName);
                    break;
            }

            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}