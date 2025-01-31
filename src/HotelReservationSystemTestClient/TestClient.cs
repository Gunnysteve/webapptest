using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using HotelReservationSystemTypes;

namespace HotelReservationSystemTestClient
{
    class TestClient
    {
        private readonly string clientName;
        private readonly string reservationsServiceURI;
        private readonly string reservationsServiceCollection;
        private readonly HttpClient client;

        // Constructor with IConfiguration injection
        internal TestClient(string clientName, IConfiguration configuration)
        {
            this.clientName = clientName;
            this.reservationsServiceURI = configuration["AppSettings:ReservationsServiceURI"];
            this.reservationsServiceCollection = configuration["AppSettings:ReservationsServiceCollection"];
            this.client = new HttpClient();
        }

        internal async Task DoWork(CancellationToken token)
        {
            Random rnd = new Random();

            while (!token.IsCancellationRequested) // Stop when cancellation is requested
            {
                try
                {
                    int reservationID = rnd.Next();
                    Console.WriteLine($"Client {clientName} making reservation {reservationID}");
                    client.BaseAddress = new Uri(this.reservationsServiceURI);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Make a dummy reservation
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.reservationsServiceCollection);
                    string json = JsonConvert.SerializeObject("dummy data");
                    request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.SendAsync(request, token);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error making reservation: {e.Message}");
                }

                try
                {
                    // Do a query
                    var data = await client.GetStringAsync($"{this.reservationsServiceCollection}/{rnd.Next()}");
                    var reservation = JsonConvert.DeserializeObject<CustomerReservation>(data);
                    Console.WriteLine($"Client {clientName} querying reservation: {reservation}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error retrieving reservation: {e.Message}");
                }

                await Task.Delay(1000, token); // Add a delay to prevent excessive looping
            }

            Console.WriteLine($"Client {clientName} stopping due to cancellation.");
        }
    }
}
