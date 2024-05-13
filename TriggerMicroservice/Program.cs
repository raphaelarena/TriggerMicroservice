using RabbitMQ.Client;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "trigger",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            while (true)
            {
                Console.WriteLine("Pressione 'I' para iniciar ou 'S' para sair.");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.I)
                {
                    Console.WriteLine("\nDigite o endereço:");
                    Console.Write("Logradouro(Rua/Avenida,Numero): ");
                    string rua = Console.ReadLine();

                    Console.Write("CEP: ");
                    string cep = Console.ReadLine();

                    Console.Write("Cidade: ");
                    string cidade = Console.ReadLine();

                    Console.Write("Estado: ");
                    string estado = Console.ReadLine();

                    string message = $"{rua}, {cep}, {cidade}, {estado}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "trigger",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine("\nTrigger sent: {0}", message);
                }
                else if (key.Key == ConsoleKey.S)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nOpção inválida.");
                }
            }
        }
    }
}
