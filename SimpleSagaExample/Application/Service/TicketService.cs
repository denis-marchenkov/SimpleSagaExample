using SimpleSagaExample.Application.DTO;
using SimpleSagaExample.Application.Interface;

namespace SimpleSagaExample.Application.Service
{
    public class TicketService : ITicketService
    {
        private readonly Random _random = new();

        public async Task<BookingResult> BookTicket(string idempotencyKey)
        {
            await Task.Delay(100);

            // randomly fail booking
            if (_random.NextDouble() < 0.5)
            {
                Console.WriteLine("Ticket booking failed");

                return new BookingResult { Success = false };
            }

            Console.WriteLine("Ticket booked");

            return new BookingResult { Success = true, BookingId = "TICKET_" + Guid.NewGuid() };
        }

        public async Task CancelBooking(string bookingId)
        {
            await Task.Delay(100);

            Console.WriteLine("Ticket canceled: " + bookingId);
        }
    }
}
