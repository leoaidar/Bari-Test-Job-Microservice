using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Queries;
using Bari.Test.Job.Domain.Queries.Contracts;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessageQueryHandler : Handler,
                                                IRequestHandler<MessageGetAllQuery, IQueryResult>
    {
        private readonly IRepository<Message> _repository;
        private readonly IRepository<Message> _cacheRepository;
        private readonly string _genericErrorText;
        private readonly string _genericSuccessText;

        public MessageQueryHandler(IEnumerable<IRepository<Message>> repositories, IRepository<Entity> entityRepository) : base(entityRepository)
        {
            _repository = repositories.FirstOrDefault(repo => repo.GetType().Name == "MessageRepository");
            _cacheRepository = repositories.FirstOrDefault(repo => repo.GetType().Name == "MessageCacheRepository");
            _genericErrorText = "Ops, parece que houve algum problema com a Mensagem!";
            _genericSuccessText = "Mensagens retornados com sucesso!";
        }

        public async Task<IQueryResult> Handle(MessageGetAllQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var cached = await _cacheRepository.GetAll();

                if (cached != null && !INVALIDATE_ALL_CACHE)
                    return await Task.FromResult<IQueryResult>(new QueryResult<IEnumerable<Message>>(cached, success: true, message: _genericSuccessText));

                var messages = await _repository.GetAll();

                if ((messages != null) && (cached == null || INVALIDATE_ALL_CACHE))
                    await _cacheRepository.Bind<IEnumerable<Message>>(messages, "Messages");

                INVALIDATE_ONE_CACHE = false;
                INVALIDATE_ALL_CACHE = false;

                return await Task.FromResult<IQueryResult>(new QueryResult<IEnumerable<Message>>(messages, success: true, message: _genericSuccessText));
            }
            catch (Exception ex)
            {
                return await Task.FromResult<IQueryResult>(new QueryResult<IEnumerable<Message>>(null, success: false, message: _genericErrorText + "|" + ex.Message + "|" + ex.StackTrace));
            }
        }

    }
}