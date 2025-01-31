using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelReservationSystemTestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int numClients = 5; // Adjust the number of clients as needed
            var tokenSource = new CancellationTokenSource();

            Console.WriteLine("Starting test clients...");
            var driver = new TestClientsDriver(numClients);

            var runTask = driver.RunAsync(tokenSource.Token);
            driver.WaitForEnter(tokenSource); // Wait for user to press Enter

            await runTask; // Ensure all tasks finish before exiting
            Console.WriteLine("All clients stopped.");
        }
    }
}
