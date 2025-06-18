using SimpleSagaExample.Application.DTO;
using SimpleSagaExample.Application.Interface;
using SimpleSagaExample.Infrastructure;


namespace SimpleSagaExample.Application.Service
{
    public class BookingSagaService
    {
        private readonly string _sagaId = Guid.NewGuid().ToString();
        private readonly ITicketService _ticketService;
        private readonly IHotelService _hotelService;

        public BookingSagaService(ITicketService ticketService, IHotelService hotelService)
        {
            _ticketService = ticketService;
            _hotelService = hotelService;
        }

        public async Task<bool> StartOrResumeAsync()
        {
            Console.WriteLine("Starting or resuming saga: " + _sagaId);

            var pendingActions = await SagaLogger.GetPendingActions(_sagaId);

            foreach (var action in pendingActions)
            {
                Console.WriteLine("Recovering uncompleted action: " + action.Action);

                if (action.Action == "BookTicket")
                    await ResumeTicketBooking(action.IdempotencyKey);

                else if (action.Action == "BookHotel")
                    await ResumeHotelBooking(action.IdempotencyKey);
            }

            var state = await SagaStorage.Load(_sagaId) ?? new SagaState();

            try
            {
                if (!state.TicketBooked)
                {
                    state.TicketIdempotencyKey = "TICKET_KEY_" + _sagaId;
                    await SagaLogger.LogIntent(_sagaId, "BookTicket", state.TicketIdempotencyKey);

                    var ticketResult = await _ticketService.BookTicket(state.TicketIdempotencyKey);
                    state.TicketBooked = ticketResult.Success;
                    state.TicketBookingId = ticketResult.BookingId;

                    await SagaLogger.MarkAsCompleted(_sagaId, "BookTicket");
                    await SagaStorage.Save(_sagaId, state);
                }

                if (!state.HotelBooked)
                {
                    state.HotelIdempotencyKey = "HOTEL_KEY_" + _sagaId;
                    await SagaLogger.LogIntent(_sagaId, "BookHotel", state.HotelIdempotencyKey);

                    var hotelResult = await _hotelService.BookHotel(state.HotelIdempotencyKey);
                    state.HotelBooked = hotelResult.Success;
                    state.HotelBookingId = hotelResult.BookingId;

                    await SagaLogger.MarkAsCompleted(_sagaId, "BookHotel");
                    await SagaStorage.Save(_sagaId, state);
                }

                if (state.TicketBooked && state.HotelBooked)
                {
                    state.IsCompleted = true;

                    await SagaStorage.Save(_sagaId, state);
                    await SagaLogger.ClearLogs(_sagaId);

                    Console.WriteLine("Both bookings completed successfully.");

                    return true;
                }

                await CompensateAsync(state);
                await SagaStorage.Delete(_sagaId);

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saga interrupted: " + ex.Message);
                await SagaStorage.Save(_sagaId, state);

                return false;
            }
        }

        private async Task ResumeTicketBooking(string idempotencyKey)
        {
            Console.WriteLine("Resuming Ticket Booking with key: " + idempotencyKey);

            var result = await _ticketService.BookTicket(idempotencyKey);

            Console.WriteLine(result.Success ? "Ticket re-booked" : "Ticket still failed");
        }

        private async Task ResumeHotelBooking(string idempotencyKey)
        {
            Console.WriteLine("Resuming Hotel Booking with key: " + idempotencyKey);

            var result = await _hotelService.BookHotel(idempotencyKey);

            Console.WriteLine(result.Success ? "Hotel re-booked" : "Hotel still failed");
        }

        private async Task CompensateAsync(SagaState state)
        {
            Console.WriteLine("Starting compensation...");

            if (state.TicketBooked)
                await _ticketService.CancelBooking(state.TicketBookingId);

            if (state.HotelBooked)
                await _hotelService.CancelBooking(state.HotelBookingId);
        }
    }
}
