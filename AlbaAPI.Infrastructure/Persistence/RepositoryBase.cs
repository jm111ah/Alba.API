using System.Collections.Generic;
using AlbaAPI.Domain.Entities;
using AlbaAPI.Domain.Repositories;

namespace AlbaAPI.Infrastructure.Persistence
{
    /// <summary>
    /// 리포지토리 기본 구현 (실제 저장소 연동은 DB/파일 등으로 확장)
    /// </summary>
    public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        public virtual T GetById(int id)
        {
            return default(T);
        }

        public virtual IEnumerable<T> GetAll()
        {
            yield break;
        }

        public virtual void Add(T entity) { }

        public virtual void Update(T entity) { }

        public virtual void Remove(T entity) { }
    }
}
