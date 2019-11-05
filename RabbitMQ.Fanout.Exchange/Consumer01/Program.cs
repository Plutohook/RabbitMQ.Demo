using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("消费者01 is Start!!!!");

            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Password = "guest";
            factory.UserName = "guest";
            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();
            channel.ExchangeDeclare("admin.test", "fanout");
            channel.QueueDeclare(
                queue: "admin.test.A",
                durable: false, //是否持久化
                exclusive: false, // 是否独占
                autoDelete: false, // 是否自定删除
                null // 队列设置参数
            );
            channel.QueueBind("admin.test.A", "admin.test", "");
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                Console.WriteLine($"从队列admin.test.A:接受到生产者消息：{message}");
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queue: "admin.test.A", autoAck: false, consumer: consumer);
            Console.ReadLine();
        }
    }
}
