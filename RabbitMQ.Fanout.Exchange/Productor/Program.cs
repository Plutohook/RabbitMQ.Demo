using System;
using System.Security.Authentication;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Productor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("生产者 is Start!!!!");

            var factory =new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Password = "guest";
            factory.UserName = "guest";
            string message;
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                //channel.QueueDeclare(queue: "admin.test",
                //    durable: true,
                //    exclusive: false,
                //    autoDelete: false,
                //    arguments: null);

                do
                {
                    message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "admin.test",
                        routingKey: "",
                        basicProperties: properties,
                        body: body);
                } while (message?.Trim().ToLower() != "exit");
            }

            Console.ReadLine();
        }
    }
}
