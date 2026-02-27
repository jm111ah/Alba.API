using System.Web.Http;
using Unity;
using Unity.Lifetime;
using AlbaAPI.Common.Logging;
using AlbaAPI.Repository.Interfaces;
using AlbaAPI.Repository.Entities;
using AlbaAPI.Repository.Repositories;
using AlbaAPI.Service.Interfaces;
using AlbaAPI.Service.Services;

namespace AlbaAPI.Mobile
{
    /// <summary>
    /// 의존성 주입 설정: Unity Container를 사용하여 Service와 Repository 등록
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Unity Container 생성 및 의존성 등록
        /// </summary>
        public static IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            // Logger 등록 (싱글톤)
            container.RegisterType<ILogger, FileLogger>(new ContainerControlledLifetimeManager());

            // Repository 등록
            container.RegisterType<IRepository<SampleEntity>, SampleRepository>(new ContainerControlledLifetimeManager());

            // Service 등록
            container.RegisterType<ISampleService, SampleService>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}

