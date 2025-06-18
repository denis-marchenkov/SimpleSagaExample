using SimpleSagaExample.Application.DTO;
using SimpleSagaExample.Application.Interface;

namespace SimpleSagaExample.Application.Service
{
    public class HotelService : IHotelService
    {
        private readonly Random _random = new();

        public async Task<BookingResult> BookHotel(string idempotencyKey)
        {
            await Task.Delay(100);

            // randomly fail booking
            if (_random.NextDouble() < 0.5)
            {
                Console.WriteLine("Hotel booking failed");

                return new BookingResult { Success = false };
            }

            Console.WriteLine("Hotel booked");
            return new BookingResult { Success = true, BookingId = "HOTEL_" + Guid.NewGuid() };
        }

        public async Task CancelBooking(string bookingId)
        {
            await Task.Delay(100);

            Console.WriteLine("Hotel canceled: " + bookingId);
        }
    }
}
