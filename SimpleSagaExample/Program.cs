using SimpleSagaExample;
using SimpleSagaExample.Application.Service;

Console.WriteLine("Starting glorious saga of the hotel booking...\n");

await SagaRunner.RunSagaUntilSuccess(async () =>
{
    var saga = new BookingSagaService(new TicketService(), new HotelService());
    return await saga.StartOrResumeAsync();
});

Console.WriteLine("\nProcess complete.");