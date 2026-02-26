namespace AlbaAPI.Service.Dto
{
    // ========== Dto 역할: Controller ↔ Service 간 데이터 전달 ==========
    // API 요청/응답에 사용. Entity와 분리해 API 스펙을 안정적으로 유지.
    // (Entity 구조가 바뀌어도 Dto만 조정하면 API 계약은 유지 가능)

    /// <summary>
    /// Controller ↔ Service 간 데이터 전달용 DTO (API 응답/요청에 사용)
    /// </summary>
    public class SampleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
