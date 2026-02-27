using System.Collections.Generic;
using System.Threading.Tasks;
using AlbaAPI.Service.Dto;

namespace AlbaAPI.Service.Interfaces
{
    // Service 계약: Controller는 이 인터페이스로만 Service를 사용 (구현은 SampleService).

    /// <summary>
    /// Controller 계층에서 호출하는 Service 인터페이스
    /// </summary>
    public interface ISampleService
    {
        SampleDto GetById(int id);
        IEnumerable<SampleDto> GetAll();

        // 비동기 메서드
        Task<SampleDto> GetByIdAsync(int id);
        Task<IEnumerable<SampleDto>> GetAllAsync();
    }
}
