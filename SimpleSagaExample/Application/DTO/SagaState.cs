namespace SimpleSagaExample.Application.DTO
{
    // This represents a current state of a running saga.
    // Contrary to the intent (SagaLogEntry) - this represents what already happened.
    public class SagaState
    {
        public string TicketBookingId { get; set; }
        public string HotelBookingId { get; set; }
        public bool TicketBooked { get; set; }
        public bool HotelBooked { get; set; }
        public bool IsCompleted { get; set; }

        public string TicketIdempotencyKey { get; set; }
        public string HotelIdempotencyKey { get; set; }
    }
}
