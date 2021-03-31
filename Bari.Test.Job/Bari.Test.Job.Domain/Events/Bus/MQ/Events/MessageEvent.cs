using MediatR;

namespace Bari.Test.Job.Domain.Events.Bus.MQ.Events
{
    public class MessageEvent : IRequest<bool>
    {
        public string MessageType { get; protected set; }

        protected MessageEvent() => MessageType = GetType().Name;
    }
}
