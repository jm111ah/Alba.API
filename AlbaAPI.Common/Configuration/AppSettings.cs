using System;
using System.Configuration;

namespace AlbaAPI.Common.Configuration
{
    /// <summary>
    /// 애플리케이션 설정 관리: Web.config의 AppSettings를 타입 안전하게 접근
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// 로그 디렉토리 경로 (기본값: Logs)
        /// </summary>
        public static string LogDirectory => GetSetting("LogDirectory", "Logs");

        /// <summary>
        /// API 버전 (기본값: v1)
        /// </summary>
        public static string ApiVersion => GetSetting("ApiVersion", "v1");

        /// <summary>
        /// Swagger 활성화 여부 (기본값: true)
        /// </summary>
        public static bool EnableSwagger => GetSetting("EnableSwagger", "true").Equals("true", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 환경 (Development, Staging, Production)
        /// </summary>
        public static string Environment => GetSetting("Environment", "Development");

        /// <summary>
        /// AppSettings에서 설정값 가져오기
        /// </summary>
        private static string GetSetting(string key, string defaultValue = null)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}

