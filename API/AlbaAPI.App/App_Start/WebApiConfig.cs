using System.Web.Http;
using Unity;
using AlbaAPI.Common.Configuration;
using AlbaAPI.Common.Filters;
using AlbaAPI.Common.Logging;
using AlbaAPI.App.DependencyResolution;

namespace AlbaAPI.App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 의존성 주입 설정
            var container = UnityConfig.RegisterComponents();
            config.DependencyResolver = new UnityDependencyResolver(container);

            // 전역 필터 등록
            var logger = container.Resolve<ILogger>();
            config.Filters.Add(new ExceptionFilterAttribute(logger));
            config.Filters.Add(new ValidateModelAttribute());

            // API 버전 관리 라우팅 설정
            config.MapHttpAttributeRoutes();
            
            // 버전별 라우팅
            config.Routes.MapHttpRoute(
                name: "VersionedApi",
                routeTemplate: $"api/{AppSettings.ApiVersion}/{{controller}}/{{id}}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 기본 라우팅 (하위 호환성)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // JSON 포맷터 설정 (기본 설정 사용)
        }
    }
}
