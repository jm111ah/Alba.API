using System;

namespace AlbaAPI.Common.Exceptions
{
    /// <summary>
    /// API 예외: 비즈니스 로직 오류나 사용자 입력 오류 시 사용
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>오류 코드</summary>
        public string ErrorCode { get; }

        /// <summary>HTTP 상태 코드</summary>
        public int StatusCode { get; }

        public ApiException(string message, string errorCode = null, int statusCode = 400)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public ApiException(string message, Exception innerException, string errorCode = null, int statusCode = 400)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}

