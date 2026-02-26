using System.Collections.Generic;
using System.Linq;
using AlbaAPI.Repository.Entities;
using AlbaAPI.Repository.Interfaces;
using AlbaAPI.Service.Dto;
using AlbaAPI.Service.Interfaces;

namespace AlbaAPI.Service.Services
{
    // ========== Service 역할: Repository(Entity) ↔ Controller(Dto) 중간 계층 ==========
    // 1. Repository에서 Entity를 가져옴 (IRepository 사용)
    // 2. 비즈니스 로직 적용 가능
    // 3. Entity → Dto 변환 후 Controller에 전달 (API에는 Dto만 노출)

    /// <summary>
    /// Repository(Entity) ↔ Controller(Dto) 중간 계층. Entity → Dto 변환 담당.
    /// </summary>
    public class SampleService : ISampleService
    {
        private readonly IRepository<SampleEntity> _repository;

        public SampleService(IRepository<SampleEntity> repository)
        {
            _repository = repository;
        }

        public SampleDto GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : ToDto(entity);
        }

        public IEnumerable<SampleDto> GetAll()
        {
            return _repository.GetAll().Select(ToDto);
        }

        private static SampleDto ToDto(SampleEntity entity)
        {
            return new SampleDto { Id = entity.Id, Name = entity.Name };
        }
    }
}
