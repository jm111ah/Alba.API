using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaAPI.Common.Logging;
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
        private readonly ILogger _logger;

        public SampleService(IRepository<SampleEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
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

        // 비동기 메서드 구현
        public async Task<SampleDto> GetByIdAsync(int id)
        {
            try
            {
                _logger.Debug($"GetByIdAsync 호출: id={id}");
                var entity = await _repository.GetByIdAsync(id);
                
                if (entity == null)
                {
                    _logger.Warning($"GetByIdAsync: id={id}에 해당하는 엔티티를 찾을 수 없습니다.");
                    return null;
                }

                _logger.Info($"GetByIdAsync 성공: id={id}, name={entity.Name}");
                return ToDto(entity);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByIdAsync 오류: id={id}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<SampleDto>> GetAllAsync()
        {
            try
            {
                _logger.Debug("GetAllAsync 호출");
                var entities = await _repository.GetAllAsync();
                var result = entities.Select(ToDto).ToList();
                
                _logger.Info($"GetAllAsync 성공: {result.Count}개 항목 반환");
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error("GetAllAsync 오류", ex);
                throw;
            }
        }

        private static SampleDto ToDto(SampleEntity entity)
        {
            return new SampleDto { Id = entity.Id, Name = entity.Name };
        }
    }
}
