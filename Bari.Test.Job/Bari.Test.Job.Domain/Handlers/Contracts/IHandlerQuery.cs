using Bari.Test.Job.Domain.Queries.Contracts;

namespace Bari.Test.Job.Domain.Handlers.Contracts
{
    public interface IHandlerQuery<Q> : IHandler<Q, IQueryResult>
                        where Q : IQuery
    {
        IQueryResult Handle(Q query);
    }
}