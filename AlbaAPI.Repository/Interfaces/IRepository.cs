using System.Collections.Generic;
using System.Threading.Tasks;
using AlbaAPI.Repository.Entities;

namespace AlbaAPI.Repository.Interfaces
{
    // Repository 계층: 데이터 접근 계약. Entity 단위 저장/조회. 구현은 SampleRepository 등.

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

        // 비동기 메서드
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}
