using System.Collections.Generic;
using System.Web.Http;
using AlbaAPI.Service.Dto;
using AlbaAPI.Service.Interfaces;
using AlbaAPI.Repository;

namespace AlbaAPI.Pc.Controllers
{
    // ========== PC API · 3계층 흐름 ==========
    // Controller(PC) → Service → Repository
    // 1. Controller: 요청 수신 후 Service만 호출하고, 반환받은 Dto를 그대로 응답.
    // 2. Service: Repository에서 Entity 조회 후 Dto로 변환해 반환.
    // 3. Repository: Entity 저장/조회 (IRepository 구현). PC/Mobile/App 동일 구조.

    /// <summary>
    /// PC용 Controller: 요청 수신 → Service 호출 → Dto 반환
    /// </summary>
    public class ValuesController : ApiController
    {
        private readonly ISampleService _sampleService;

        public ValuesController()
        {
            _sampleService = new SampleService(new SampleRepository());
        }

        public ValuesController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        /// <summary>GET api/values → Service.GetAll() → Dto 목록 반환</summary>
        public IEnumerable<SampleDto> Get()
        {
            return _sampleService.GetAll();
        }

        /// <summary>GET api/values/5 → Service.GetById(5) → Dto 반환</summary>
        public SampleDto Get(int id)
        {
            return _sampleService.GetById(id);
        }

        public void Post([FromBody] string value) { }

        public void Put(int id, [FromBody] string value) { }

        public void Delete(int id) { }
    }
}
