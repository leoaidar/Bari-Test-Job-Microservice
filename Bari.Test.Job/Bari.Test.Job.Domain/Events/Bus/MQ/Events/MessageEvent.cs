using MediatR;

namespace Bari.Test.Job.Domain.Events.Bus.MQ.Events
{
    public class MessageEvent : IRequest<bool>
    {
        public string MessageType { get; protected set; }

        //protected Message()
        //{
        //    MessageType = GetType().Name;
        //}

        protected MessageEvent() => MessageType = GetType().Name;
    }
}
