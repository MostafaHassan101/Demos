using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");
try
{
    var factory = new ConnectionFactory() { HostName = "localhost" }; // Use your RabbitMQ server hostname
    using (var connection = await factory.CreateConnectionAsync())
    using (var channel = await connection.CreateChannelAsync())
    {
        await channel.QueueDeclareAsync(queue: "demoQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        string message = "Hello, RabbitMQ - 4!!!";
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "", routingKey: "demoQueue", body: body, cancellationToken: CancellationToken.None);
        Console.WriteLine($" [x] Sent {message}");
    }

    Console.WriteLine(" Press [enter] to exit.");
	Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
