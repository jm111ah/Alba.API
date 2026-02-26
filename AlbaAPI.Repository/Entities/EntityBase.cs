namespace AlbaAPI.Repository.Entities
{
    // ========== Repository 계층: Entity = 저장소와 1:1 모델 ==========
    // Repository가 조회·저장할 때 사용. Service는 Entity를 Dto로 변환해 Controller에 전달.

    /// <summary>
    /// 도메인 엔티티 기본 클래스 (모든 Entity의 공통 필드)
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; set; }
    }
}
