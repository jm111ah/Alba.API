using System.Collections.Generic;
using System.Linq;
using AlbaAPI.Repository.Entities;
using AlbaAPI.Repository.Interfaces;

namespace AlbaAPI.Repository
{
    // ========== Repository 계층: Entity 단위로 저장소 접근 ==========
    // IRepository 구현. 실제로는 DB/파일/외부 API 등과 연동.
    // 아키텍처 이해를 위해 메모리 목록으로 동작 (PC/Mobile/App 동일 데이터).

    /// <summary>
    /// Repository 계층: Entity 단위로 데이터 접근 (DB/저장소 연동 지점)
    /// </summary>
    public class SampleRepository : IRepository<SampleEntity>
    {
        private static readonly List<SampleEntity> _store = new List<SampleEntity>
        {
            new SampleEntity { Id = 1, Name = "항목1" },
            new SampleEntity { Id = 2, Name = "항목2" },
            new SampleEntity { Id = 3, Name = "항목3" }
        };

        public SampleEntity GetById(int id)
        {
            return _store.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<SampleEntity> GetAll()
        {
            return _store;
        }

        public void Add(SampleEntity entity)
        {
            _store.Add(entity);
        }

        public void Update(SampleEntity entity)
        {
            var existing = _store.FirstOrDefault(e => e.Id == entity.Id);
            if (existing != null) existing.Name = entity.Name;
        }

        public void Remove(SampleEntity entity)
        {
            _store.RemoveAll(e => e.Id == entity.Id);
        }
    }
}
