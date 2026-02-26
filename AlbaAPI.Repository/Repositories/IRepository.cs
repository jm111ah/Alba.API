using System.Collections.Generic;
using AlbaAPI.Repository.Entities;

namespace AlbaAPI.Repository.Repositories
{
    // ========== Repository 계층: 데이터 접근 계약 ==========
    // Entity 단위로 저장/조회하는 인터페이스. 구현은 같은 프로젝트의 SampleRepository 등.

    /// <summary>
    /// 리포지토리 기본 인터페이스
    /// </summary>
    public interface IRepository<T> where T : EntityBase
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
