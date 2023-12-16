using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Consumer

var factory = new ConnectionFactory{ HostName = "localhost"};
using var connection = factory.CreateConnection();  
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "secondexchange", type: ExchangeType.Fanout);

channel.QueueDeclare(queue: "letterbox1");
channel.QueueBind("letterbox1", "secondexchange", "");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received new message: {message}");
};

channel.BasicConsume(queue: "letterbox1", autoAck: true, consumer: consumer);

Console.WriteLine("Consuming");
Console.ReadKey();