using Bari.Test.Job.Domain.Queries.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Queries
{
    public class GenericQueryResult<T> : IQueryResult
    {
        public GenericQueryResult() { }

        public GenericQueryResult(T entity, IEnumerable<T> entities = null, bool success = true, string message = "", object data = null) : base()
        {
            Success = success;
            Message = message;
            Data = data;
            Entity = entity;
            Entities = entities;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public T Entity { get; set; }
        public IEnumerable<T> Entities { get; set; }

        public static explicit operator GenericQueryResult<T>(Task<IQueryResult> v)
        {
            throw new NotImplementedException();
        }
    }
}