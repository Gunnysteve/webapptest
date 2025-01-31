using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelReservationSystemTestClient
{
    public class TestClientsDriver
    {
        private readonly int numClients;
        private readonly IConfiguration configuration;

        public TestClientsDriver(int numClients)
        {
            this.numClients = numClients;

            // Load configuration from appsettings.json
            this.configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public async Task RunAsync(CancellationToken token)
        {
            var tasks = new List<Task>();

            for (int clientNum = 0; clientNum < this.numClients; clientNum++)
            {
                string clientName = $"Client{clientNum}";
                var client = new TestClient(clientName, configuration); // Pass configuration

                // Ensure DoWork() matches the correct signature
                tasks.Add(Task.Run(() => client.DoWork(token), token));
            }

            // Wait for all tasks to complete or until cancellation is requested
            await Task.WhenAll(tasks);
        }

        public void WaitForEnter(CancellationTokenSource tokenSource)
        {
            Console.WriteLine("Press Enter to stop clients...");
            Console.ReadLine();
            tokenSource.Cancel();  // Cancel the operation when the user presses Enter
        }
    }
}
