namespace SimpleSagaExample
{
    public static class SagaRunner
    {
        // run booking saga until both services successfully execute booking
        public static async Task RunSagaUntilSuccess(Func<Task<bool>> startSagaAsync)
        {
            int attempt = 1;

            while (true)
            {
                Console.WriteLine("\nAttempt #" + attempt++);
                bool success = await startSagaAsync();

                if (success)
                {
                    Console.WriteLine("All bookings completed successfully!");

                    break;
                }

                Console.WriteLine("Retrying due to partial failure...");

                await Task.Delay(500);
            }
        }
    }
}
