using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Handlers
{
    public abstract class Handler : Notifiable
    {
        private readonly IRepository<Entity> _cacheEntityRepository;

        protected bool INVALIDATE_ONE_CACHE
        {
            get
            {
                var cacheTask = _cacheEntityRepository.GetBy<string, bool>("INVALIDATE_ONE_CACHE");
                return cacheTask.Result;
            }
            set
            {
                _cacheEntityRepository.Bind<bool>(value, "INVALIDATE_ONE_CACHE");
            }

        }
        
        protected bool INVALIDATE_ALL_CACHE
        {
            get
            {
                var cacheTask = _cacheEntityRepository.GetBy<string, bool>("INVALIDATE_ALL_CACHE");
                return cacheTask.Result;
            }
            set
            {
                _cacheEntityRepository.Bind<bool>(value, "INVALIDATE_ALL_CACHE");
            }

        }

        public Handler()
        {
        }

        public Handler(IRepository<Entity> repository) : base()
        {
            //_cacheEntityRepository = repositories.FirstOrDefault(repo => repo.GetType().Name == "EntityCacheRepository");
            _cacheEntityRepository = repository;
        }

    }
}
