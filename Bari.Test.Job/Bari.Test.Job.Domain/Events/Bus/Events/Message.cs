using MediatR;

namespace Bari.Test.Job.Domain.Events.Bus.Events
{
    public class Message : IRequest<bool>
    {
        public string MessageType { get; protected set; }

        //protected Message()
        //{
        //    MessageType = GetType().Name;
        //}

        protected Message() => MessageType = GetType().Name;
    }
}
