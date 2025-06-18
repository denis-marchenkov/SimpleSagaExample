using SimpleSagaExample.Application.DTO;

namespace SimpleSagaExample.Application.Interface
{
    public interface IHotelService
    {
        Task<BookingResult> BookHotel(string idempotencyKey);
        Task CancelBooking(string bookingId);
    }
}
