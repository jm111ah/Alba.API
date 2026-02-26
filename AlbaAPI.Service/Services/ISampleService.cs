using System.Collections.Generic;
using AlbaAPI.Service.Dto;

namespace AlbaAPI.Service.Services
{
    // Service 계약: Controller는 이 인터페이스로만 Service를 사용 (구현은 SampleService).

    /// <summary>
    /// Controller 계층에서 호출하는 Service 인터페이스
    /// </summary>
    public interface ISampleService
    {
        SampleDto GetById(int id);
        IEnumerable<SampleDto> GetAll();
    }
}
