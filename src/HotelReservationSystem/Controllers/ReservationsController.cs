using HotelReservationSystemTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HotelReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private static readonly Random _random = new Random();

        // GET: api/Reservations
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            await Task.Delay(100); // Simulate async operation
            return Array.Empty<string>(); // Return an empty array
        }

        // GET: api/Reservations/ReservationID
        [HttpGet("{reservationid}", Name = "GetReservation")]
        public async Task<CustomerReservation> Get(int reservationid)
        {
            await Task.Delay(500); // Simulate async work

            return new CustomerReservation
            {
                ReservationID = reservationid,
                CustomerID = $"{_random.Next()}",
                HotelID = $"Hotel {_random.Next()}",
                Checkin = DateTime.Now.AddDays(10),
                Checkout = DateTime.Now.AddDays(10 + _random.NextDouble() * 100),
                NumberOfGuests = _random.Next(1, 6),
                ReservationComments = GetRandomString(_random.Next(100, 500)) // Generate a random string of length 100-500
            };
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            await Task.Delay(100); // Simulate async work

            // Return status code 201 Created with a mock reservation ID
            return CreatedAtAction(nameof(Get), new { reservationid = _random.Next(1, 1000) }, value);
        }

        // PUT: api/Reservations/ReservationID
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            await Task.Delay(100); // Simulate async work

            // Return a 204 No Content response (standard for updates without returning data)
            return NoContent();
        }

        // DELETE: api/Reservations/ReservationID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Task.Delay(50); // Simulate async work

            // Return a 204 No Content response (standard for deletions without returning data)
            return NoContent();
        }

        /// <summary>
        /// Generates a secure random string of a given length using Base64 encoding.
        /// </summary>
        private string GetRandomString(int stringLength)
        {
            int byteCount = (stringLength * 3) / 4; // Base64 encoding expands by 4/3
            byte[] bytes = RandomNumberGenerator.GetBytes(byteCount);
            return Convert.ToBase64String(bytes);
        }
    }
}
