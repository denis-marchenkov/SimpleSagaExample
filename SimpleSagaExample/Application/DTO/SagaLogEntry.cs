namespace SimpleSagaExample.Application.DTO
{
    // This represents intention to perform an action.
    // We log what we want to do before actually doing it.
    public class SagaLogEntry
    {
        public string SagaId { get; set; }
        public string Action { get; set; }
        public string IdempotencyKey { get; set; }
        public bool Completed { get; set; }

        public override string ToString()
        {
            return $"SagaId={SagaId}, Action={Action}, Key={IdempotencyKey}, Done={Completed}";
        }
    }
}
