using SimpleSagaExample.Application.DTO;

namespace SimpleSagaExample.Infrastructure
{
    // In memory storage just to make example simple
    public static class SagaStorage
    {
        // should be a real database
        private static Dictionary<string, SagaState> _store = new();

        public static async Task Save(string sagaId, SagaState state)
        {
            await Task.CompletedTask;

            _store[sagaId] = state;

            Console.WriteLine("Saved: " + sagaId);
        }

        public static async Task<SagaState> Load(string sagaId)
        {
            await Task.CompletedTask;

            _store.TryGetValue(sagaId, out var state);

            Console.WriteLine(state == null ? "No saga found: " + sagaId : "Loaded: " + sagaId);

            return state;
        }

        public static async Task Delete(string sagaId)
        {
            await Task.CompletedTask;

            _store.Remove(sagaId);

            Console.WriteLine("Deleted: " + sagaId);
        }
    }
}
