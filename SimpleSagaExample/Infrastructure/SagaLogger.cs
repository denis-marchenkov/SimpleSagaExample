using SimpleSagaExample.Application.DTO;

namespace SimpleSagaExample.Infrastructure
{
    public static class SagaLogger
    {
        // should be a real database
        private static List<SagaLogEntry> _log = new();

        // log action before executing it
        public static async Task LogIntent(string sagaId, string action, string idempotencyKey)
        {
            var entry = new SagaLogEntry
            {
                SagaId = sagaId,
                Action = action,
                IdempotencyKey = idempotencyKey,
                Completed = false
            };

            _log.Add(entry);

            Console.WriteLine("Logged intent: " + entry.ToString());

            await Task.CompletedTask;
        }

        public static async Task MarkAsCompleted(string sagaId, string action)
        {
            var entry = _log.Find(e => e.SagaId == sagaId && e.Action == action);
            if (entry != null)
            {
                entry.Completed = true;

                Console.WriteLine("Marked as completed: " + entry.ToString());
            }

            await Task.CompletedTask;
        }

        public static async Task<List<SagaLogEntry>> GetPendingActions(string sagaId)
        {
            return await Task.FromResult(_log.FindAll(e => e.SagaId == sagaId && !e.Completed));
        }

        public static async Task ClearLogs(string sagaId)
        {
            _log.RemoveAll(e => e.SagaId == sagaId);

            await Task.CompletedTask;
        }
    }
}
