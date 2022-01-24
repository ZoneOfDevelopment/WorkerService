using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace WorkerService.Service
{
    public class ReadMessage : IReadMessage
    {
        public void Read()
        {
            // definition of Connection 
            var _rabbitMQServer = new ConnectionFactory() { HostName = "localhost", Password = "guest", UserName = "guest" };

            // create connection
            using var connection = _rabbitMQServer.CreateConnection();

            // create channel
            using var channel = connection.CreateModel();

            StartReading(channel, "TestWorkService");
        }

        private void StartReading(IModel channel, string queueName)
        {
            // connect to the queue
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Consumer definition
            var consumer = new EventingBasicConsumer(channel);

            // Definition of event when the Consumer gets a message
            consumer.Received += (sender, e) =>
            {
                ManageMessage(e);
            };

            // Start pushing messages to our consumer
            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine("Consumer is running");
            Console.ReadLine();
        }

        private void ManageMessage(BasicDeliverEventArgs e)
        {
            // Obviously for this demo, we just print the message...
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);

            using StreamWriter file = new("MessagesRead.txt", append: true);
            file.WriteLine(message);
        }
    }
}
