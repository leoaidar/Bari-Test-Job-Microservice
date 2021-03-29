using System.Threading.Tasks;

namespace Bari.Test.Job.Api.Jobs
{
    public interface IMessageJob
    {
        public Task SendMessage();
    }
}