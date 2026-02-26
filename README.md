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
