using SimpleSagaExample.Application.DTO;

namespace SimpleSagaExample.Application.Interface
{
    public interface ITicketService
    {
        Task<BookingResult> BookTicket(string idempotencyKey);
        Task CancelBooking(string bookingId);
    }
}
