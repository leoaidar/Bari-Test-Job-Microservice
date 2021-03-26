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

        public MessageQueryHandler(IEnumerable<IRepository<Message>> repositories) 
        {
            _repository = repositories.FirstOrDefault(repo => repo.GetType().Name == "MessageRepository");
            _cacheRepository = repositories.FirstOrDefault(repo => repo.GetType().Name == "MessageCacheRepository");
            _genericErrorText = "Ops, parece que os dados da Mensagem estão errados!";
            _genericSuccessText = "Mensagens retornados com sucesso!";
        }

        public async Task<IQueryResult> Handle(MessageGetAllQuery query, CancellationToken cancellationToken)
        {
            try
            {
                //pega do cache (Redis)
                var cached = await _cacheRepository.GetAll();

                //avalia se trouxe e o cache nao ta desatualizado
                if (cached != null && !INVALIDATE_ALL_CACHE)
                    return await Task.FromResult<IQueryResult>(new GenericQueryResult<IEnumerable<Message>>(cached, success: true, message: _genericSuccessText));

                //pega do banco de dados (SQLServer)
                var messages = await _repository.GetAll();

                //avalia se trouxe e o cache ta desatualizado
                if ((messages != null) && (cached == null || INVALIDATE_ALL_CACHE))
                    await _cacheRepository.Bind<IEnumerable<Message>>(messages, "Messages");

                //valida o cache
                INVALIDATE_ONE_CACHE = false;
                INVALIDATE_ALL_CACHE = false;

                //await _mediator.Publish(new PessoaAlteradaNotification { Id = pessoa.Id, Nome = pessoa.Nome, Idade = pessoa.Idade, Sexo = pessoa.Sexo, IsEfetivado = true });

                return await Task.FromResult<IQueryResult>(new GenericQueryResult<IEnumerable<Message>>(messages, success: true, message: _genericSuccessText));
            }
            catch (Exception ex)
            {
                //await _mediator.Publish(new PessoaAlteradaNotification { Id = pessoa.Id, Nome = pessoa.Nome, Idade = pessoa.Idade, Sexo = pessoa.Sexo, IsEfetivado = false });
                //await _mediator.Publish(new ErroNotification { Excecao = ex.Message, PilhaErro = ex.StackTrace });

                return await Task.FromResult<IQueryResult>(new GenericQueryResult<IEnumerable<Message>>(null, success: false, message: ex.Message));
            }

        }

    }
}