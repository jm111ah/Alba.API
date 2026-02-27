using System;

namespace AlbaAPI.Common.Logging
{
    /// <summary>
    /// 로깅 인터페이스: 모든 레이어에서 공통 사용
    /// </summary>
    public interface ILogger
    {
        /// <summary>디버그 로그 기록</summary>
        void Debug(string message);

        /// <summary>정보 로그 기록</summary>
        void Info(string message);

        /// <summary>경고 로그 기록</summary>
        void Warning(string message);

        /// <summary>오류 로그 기록</summary>
        void Error(string message);

        /// <summary>예외 로그 기록</summary>
        void Error(string message, Exception exception);

        /// <summary>지정된 레벨로 로그 기록</summary>
        void Log(LogLevel level, string message);

        /// <summary>지정된 레벨로 예외 로그 기록</summary>
        void Log(LogLevel level, string message, Exception exception);
    }
}

