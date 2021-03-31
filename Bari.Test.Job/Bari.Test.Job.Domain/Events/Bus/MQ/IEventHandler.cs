using Bari.Test.Job.Domain.Events.Bus.MQ.Events;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Events.Bus.MQ
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }

}
