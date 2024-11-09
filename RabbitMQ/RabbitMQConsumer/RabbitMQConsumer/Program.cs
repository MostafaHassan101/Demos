using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

		var consumer = new AsyncEventingBasicConsumer(channel);
		consumer.ReceivedAsync += async (model, ea) =>
		{
			var body = ea.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);
			Console.WriteLine($" [x] Received {message}");

			// Simulate an asynchronous operation
			await Task.Yield(); // Ensures the method is asynchronous
		};
		await channel.BasicConsumeAsync(queue: "demoQueue",
							 autoAck: true,
							 consumer: consumer);

		Console.WriteLine(" Press [enter] to exit.");
		Console.ReadLine();
	}
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
