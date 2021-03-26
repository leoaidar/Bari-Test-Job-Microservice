using Bari.Test.Job.Domain.Queries.Contracts;
using MediatR;

namespace Bari.Test.Job.Domain.Queries
{
    public class MessageGetAllQuery : IQuery,
                                               IRequest<IQueryResult>
    {
        public MessageGetAllQuery() { }
    }
}