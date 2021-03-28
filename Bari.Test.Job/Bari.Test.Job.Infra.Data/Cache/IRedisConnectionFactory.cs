using StackExchange.Redis;

namespace Bari.Test.Job.Infra.Data.Cache
{
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }
}
