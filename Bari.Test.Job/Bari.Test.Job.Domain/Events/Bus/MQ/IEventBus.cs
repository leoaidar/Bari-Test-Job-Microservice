using Bari.Test.Job.Domain.Events.Bus.MQ.Commands;
using Bari.Test.Job.Domain.Events.Bus.MQ.Events;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Events.Bus.MQ
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
