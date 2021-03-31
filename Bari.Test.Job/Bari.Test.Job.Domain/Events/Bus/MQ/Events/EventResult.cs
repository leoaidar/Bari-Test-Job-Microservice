namespace Bari.Test.Job.Domain.Events.Bus.MQ.Events
{
    public class EventResult : IEventResult
    {
        public bool Success { get; set; }
    }
}
