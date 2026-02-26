using System.Collections.Generic;
using System.Web.Http;
using AlbaAPI.Service.Dto;
using AlbaAPI.Service.Interfaces;
using AlbaAPI.Repository;

namespace AlbaAPI.App.Controllers
{
    // ========== App API · 3계층 흐름 ==========
    // Controller(App) → Service → Repository (PC·Mobile과 동일 구조)

    /// <summary>
    /// App용 Controller: 요청 수신 → Service 호출 → Dto 반환
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

        public IEnumerable<SampleDto> Get()
        {
            return _sampleService.GetAll();
        }

        public SampleDto Get(int id)
        {
            return _sampleService.GetById(id);
        }

        public void Post([FromBody] string value) { }

        public void Put(int id, [FromBody] string value) { }

        public void Delete(int id) { }
    }
}
