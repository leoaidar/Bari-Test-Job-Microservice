using Bari.Test.Job.Domain.Queries.Contracts;
using MediatR;

namespace Wooza.Gateway.Tim.Domain.Queries
{
    public class MessageGetAllQuery : IQuery,
                                               IRequest<IQueryResult>
    {
        public MessageGetAllQuery() { }

        //public static Expression<Func<Client, bool>> Constraint()
        //{
        //    return x => x.ClientId != 0;
        //}
    }
}