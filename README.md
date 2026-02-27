## AlbaAPI 아키텍처 개요

AlbaAPI 솔루션은 **클래식 .NET Framework 4.8 기반의 3계층(Web API · Service · Repository) 구조**로 설계되어 있으며,
여러 클라이언트(Pc, Mobile, App)를 위한 공통 비즈니스 로직과 인프라를 공유합니다.

- **API 레이어**: `AlbaAPI.Pc`, `AlbaAPI.Mobile`, `AlbaAPI.App`
- **Service 레이어**: `AlbaAPI.Service`
- **Repository 레이어**: `AlbaAPI.Repository`
- **공통 인프라**: `AlbaAPI.Common`

각 API 프로젝트는 Web API Host 역할만 담당하고, 실제 비즈니스/데이터 접근은 Service/Repository 레이어로 위임합니다.

---

## 솔루션 구조

- **API**
  - `API/AlbaAPI.Pc`
  - `API/AlbaAPI.Mobile`
  - `API/AlbaAPI.App`
  - 역할: 클라이언트별(PC, 모바일, 기타 앱) Web API 엔드포인트 제공
  - 주요 구성
    - `Controllers/*Controller.cs` : HTTP 요청을 받아 Service 호출 후 표준 응답 형태로 반환
    - `App_Start/WebApiConfig.cs` : 라우팅, 전역 필터, Swagger 설정
    - `App_Start/UnityConfig.cs` : Unity Container를 이용한 DI 설정(Service, Repository, Logger 등록)
    - `DependencyResolution/UnityDependencyResolver.cs` : Web API에 Unity DI 연결

- **Service (`AlbaAPI.Service`)**
  - 역할: **비즈니스 로직 + Entity ↔ DTO 변환 계층**
  - 참조: `AlbaAPI.Common`, `AlbaAPI.Repository`
  - 주요 구성
    - `Dto/*Dto.cs` : 외부(API 응답)에 노출되는 DTO 정의
    - `Interfaces/*Service.cs` : 서비스 인터페이스(비즈니스 계약)
    - `Services/*Service.cs` : 실제 비즈니스 구현
      - Repository에서 Entity를 조회/저장
      - 필요 시 비즈니스 규칙 적용
      - Entity를 DTO로 변환하여 Controller로 반환

- **Repository (`AlbaAPI.Repository`)**
  - 역할: **데이터 접근 계층**
  - 참조: `AlbaAPI.Common`
  - 주요 구성
    - `Entities/*Entity.cs` : 데이터 저장소와 1:1로 매핑되는 Entity
    - `Interfaces/IRepository.cs` : 제네릭 Repository 계약(동기/비동기 CRUD 메서드)
    - `Repositories/*Repository.cs` : 실제 데이터 접근 구현(현재는 샘플 구현 중심)

- **Common (`AlbaAPI.Common`)**
  - 역할: **공통 인프라 및 크로스컷팅 concerns**
  - 주요 구성
    - `Configuration/AppSettings.cs`
      - `Web.config`의 `appSettings`를 타입 안전하게 래핑
      - 예: `ApiVersion`, `LogDirectory`, `Environment`, `EnableSwagger` 등
    - `Models/ApiResponse.cs`
      - 모든 API 응답을 **표준 형식**(`Success`, `Data`, `Message`, `ErrorCode`, `Timestamp`)으로 래핑
      - `SuccessResponse`, `ErrorResponse` 정적 메서드 제공
    - `Exceptions/ApiException.cs`
      - 비즈니스/도메인 오류를 표현하는 커스텀 예외
      - HTTP StatusCode, ErrorCode, Message를 포함
    - `Filters/ExceptionFilterAttribute.cs`
      - 전역 예외 처리 필터
      - `ApiException`은 정의된 상태코드/에러코드로 매핑
      - 일반 예외는 500(서버 오류) + 표준 에러 응답으로 변환
    - `Filters/ValidateModelAttribute.cs`
      - `ModelState`가 유효하지 않은 경우, 즉시 400 + 표준 에러 응답 반환
    - `Logging/ILogger.cs`, `Logging/FileLogger.cs`, `Logging/LogLevel.cs`
      - 파일 기반 로깅 인터페이스 및 구현
      - `AppSettings.LogDirectory` 기준으로 날짜/레벨별 로그 파일 생성

---

## 계층 간 흐름

### 1. 전체 요청 흐름 (3계층)

1. **클라이언트 → API 레이어**
   - 예: PC 클라이언트가 `GET /api/v1/values` 호출
   - 요청은 `AlbaAPI.Pc`의 `ValuesController`로 진입

2. **API 레이어(Controller) → Service 레이어**
   - Controller는 **의존성 주입(DI)** 으로 받은 `ISampleService` 등의 Service 인터페이스만 호출
   - 요청/응답 포맷을 `ApiResponse<T>`로 래핑하여 반환

3. **Service 레이어 → Repository 레이어**
   - Service는 `IRepository<T>` 인터페이스를 통해 데이터 접근을 수행
   - Entity를 조회/저장한 뒤 DTO로 변환하여 Controller에 반환

4. **Repository 레이어 → 데이터 저장소**
   - 실제 구현에서는 DB 또는 외부 시스템 호출이 위치
   - 현재는 샘플 엔티티/리포지토리 구조를 기준으로 설계되어 있음

5. **공통 인프라(Common)**
   - 위 모든 과정에서 로깅, 예외 처리, 모델 검증, 설정 조회가 공통으로 사용됨

### 2. PC API 예시 흐름 (`ValuesController`)

1. 클라이언트가 `GET /api/v1/values` 호출
2. `AlbaAPI.Pc.Controllers.ValuesController.Get()` 진입
   - 생성자에서 DI로 주입된 `ISampleService`, `ILogger` 사용
   - `_sampleService.GetAllAsync()` 호출
   - 결과(DTO 목록)를 `ApiResponse<IEnumerable<SampleDto>>.SuccessResponse(...)`로 감싸서 반환
3. `SampleService`는 내부에서 `IRepository<SampleEntity>` 를 이용해 Entity를 조회 후 `SampleDto`로 변환
4. 예외 발생 시:
   - 비즈니스 예외(`ApiException`) → `ExceptionFilterAttribute`에서 적절한 HTTP 상태코드 + 에러 응답으로 변환
   - 기타 예외 → 500 내부 서버 오류 + 표준 에러 응답
5. 모든 단계에서 `FileLogger`를 통해 파일 로그가 남습니다.

---

## DI(의존성 주입)와 구성

- **컨테이너**: Unity (`Unity.Container`, `Unity.Abstractions`)
- **등록 위치**: 각 API 프로젝트의 `App_Start/UnityConfig.cs`
  - 예: `ILogger → FileLogger`, `IRepository<SampleEntity> → SampleRepository`, `ISampleService → SampleService`
- **Web API와 연동**: `UnityDependencyResolver`를 통해 `HttpConfiguration.DependencyResolver`에 연결
- **전역 필터 등록**: `WebApiConfig.Register` 에서
  - `ExceptionFilterAttribute`
  - `ValidateModelAttribute`
  를 DI 컨테이너에서 생성해 전역 필터로 추가

---

## 라우팅 및 버전 관리

- 기본적으로 **Attribute Routing + 전통적인 Web API 라우팅**을 함께 사용
- 예시(`AlbaAPI.Pc.App_Start.WebApiConfig`):
  - `config.MapHttpAttributeRoutes();`
  - 버전 경로 포함 라우트: `api/{ApiVersion}/{controller}/{id}`
    - `ApiVersion` 값은 `AppSettings.ApiVersion` (기본 `v1`)
  - 하위 호환을 위한 기본 라우트: `api/{controller}/{id}`

---

## Swagger 및 환경 설정

- `AppSettings.EnableSwagger` 값에 따라 Swagger UI 활성화 가능 (실제 세부 설정은 각 API 프로젝트의 `SwaggerConfig` 참고)
- `AppSettings.Environment` 값으로 실행 환경(Development, Staging, Production)을 구분하여
  - 로깅/예외 메시지 노출 정도 등 정책을 분리할 수 있도록 설계되어 있습니다.

---

## 요약

- **AlbaAPI**는
  - 다수의 API Host(PC, Mobile, App)를
  - 공통 **Service/Repository/Common** 레이어 위에 올린
  - **표준화된 응답 형식 + 전역 예외 처리 + 파일 로깅 + 설정 관리**를 갖춘 3계층 아키텍처입니다.
- 제3자가 이 README만 읽어도
  - **요청이 어느 프로젝트/계층을 거쳐 처리되는지**
  - **공통 기능이 어디에서 제공되는지**
  를 빠르게 이해할 수 있도록 구성했습니다.

# Alba.API

Controller - Service - Repository 3계층 구조의 .NET Framework 4.8 Web API (PC / Mobile / App).

## 폴더 구조 (솔루션과 동일)

```
AlbaAPI.Sln/
├── AlbaAPI.sln
├── API/                    # API (진입점)
│   ├── AlbaAPI.Pc/
│   ├── AlbaAPI.Mobile/
│   └── AlbaAPI.App/
├── AlbaAPI.Service/        # Service (비즈니스·Dto 변환)
└── AlbaAPI.Repository/     # Repository (Entity·IRepository·구현)
```

## 다른 환경에서 이어서 개발하기

### 1. 저장소 복제

```bash
git clone https://github.com/jm111ah/Alba.API.git
cd Alba.API
```

### 2. 솔루션 열기

- `AlbaAPI.sln` 더블클릭 또는 Visual Studio에서 열기

### 3. NuGet 패키지 복원

- Visual Studio: 솔루션 우클릭 → **NuGet 패키지 복원**
- 또는 패키지 복원이 자동으로 실행되도록 설정된 경우 빌드 시 자동 복원

### 4. 빌드 및 실행

- 시작 프로젝트 설정 (AlbaAPI.Pc / Mobile / App 중 선택)
- F5 또는 Ctrl+F5로 실행

### 5. 이후 작업 흐름

```bash
git pull                    # 최신 반영
# ... 개발 ...
git add .
git commit -m "메시지"
git push
```

## 요구 사항

- Visual Studio 2017 이상 (또는 .NET Framework 4.8 빌드 환경)
- .NET Framework 4.8
