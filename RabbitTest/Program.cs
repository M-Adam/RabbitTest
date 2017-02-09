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
            const string queueName = "tasks_queue";
            const string directExchangeName = "log";
            const string topicExchangeName = "log_topics";

            Console.WriteLine("1. Producer \n2. Consumer \n\n3. Log emitter \n4. Log reciever \n\n5. Topic emitter \n6. Topic reciever");
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
                    new LogEmitter(factory).Start(directExchangeName);
                    break;
                case '4':
                    new LogReciever(factory).Start(directExchangeName);
                    break;
                case '5':
                    new TopicEmitter(factory).Start(topicExchangeName);
                    break;
                case '6':
                    new TopicReciever(factory).Start(topicExchangeName);
                    break;
            }

            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}