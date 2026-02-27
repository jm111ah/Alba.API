using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AlbaAPI.Common.Exceptions;
using AlbaAPI.Common.Logging;
using AlbaAPI.Common.Models;
using AlbaAPI.Service.Dto;
using AlbaAPI.Service.Interfaces;

namespace AlbaAPI.Mobile.Controllers
{
    // ========== Mobile API · 3계층 흐름 ==========
    // Controller(Mobile) → Service → Repository (PC와 동일한 Service·Repository 사용)

    /// <summary>
    /// Mobile용 Controller: 요청 수신 → Service 호출 → Dto 반환
    /// </summary>
    public class ValuesController : ApiController
    {
        private readonly ISampleService _sampleService;
        private readonly ILogger _logger;

        /// <summary>
        /// 의존성 주입: Unity Container가 자동으로 ISampleService와 ILogger를 주입
        /// </summary>
        public ValuesController(ISampleService sampleService, ILogger logger)
        {
            _sampleService = sampleService;
            _logger = logger;
        }

        /// <summary>GET api/values → Service.GetAllAsync() → Dto 목록 반환 (비동기)</summary>
        public async Task<ApiResponse<IEnumerable<SampleDto>>> Get()
        {
            _logger.Info("GET api/values 요청 수신 (Mobile)");
            var result = await _sampleService.GetAllAsync();
            var resultList = result.ToList();
            _logger.Info($"GET api/values 응답 (Mobile): {resultList.Count}개 항목");
            return ApiResponse<IEnumerable<SampleDto>>.SuccessResponse(result, $"{resultList.Count}개 항목 조회 성공");
        }

        /// <summary>GET api/values/5 → Service.GetByIdAsync(5) → Dto 반환 (비동기)</summary>
        public async Task<ApiResponse<SampleDto>> Get(int id)
        {
            _logger.Info($"GET api/values/{id} 요청 수신 (Mobile)");
            var result = await _sampleService.GetByIdAsync(id);
            
            if (result == null)
            {
                _logger.Warning($"GET api/values/{id} 응답 (Mobile): 항목을 찾을 수 없음");
                throw new ApiException($"ID {id}에 해당하는 항목을 찾을 수 없습니다.", "NOT_FOUND", 404);
            }
            
            _logger.Info($"GET api/values/{id} 응답 (Mobile): id={result.Id}, name={result.Name}");
            return ApiResponse<SampleDto>.SuccessResponse(result, "항목 조회 성공");
        }

        public void Post([FromBody] string value) { }

        public void Put(int id, [FromBody] string value) { }

        public void Delete(int id) { }
    }
}
