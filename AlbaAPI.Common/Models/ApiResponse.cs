using System;

namespace AlbaAPI.Common.Models
{
    /// <summary>
    /// API 응답 표준 형식: 모든 API 응답을 일관된 형식으로 래핑
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>성공 여부</summary>
        public bool Success { get; set; }

        /// <summary>응답 데이터</summary>
        public T Data { get; set; }

        /// <summary>메시지</summary>
        public string Message { get; set; }

        /// <summary>타임스탬프</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>오류 코드 (실패 시)</summary>
        public string ErrorCode { get; set; }

        /// <summary>성공 응답 생성</summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "성공")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>실패 응답 생성</summary>
        public static ApiResponse<T> ErrorResponse(string message, string errorCode = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default(T),
                Message = message,
                ErrorCode = errorCode,
                Timestamp = DateTime.Now
            };
        }
    }

    /// <summary>
    /// 데이터가 없는 API 응답 (void 메서드용)
    /// </summary>
    public class ApiResponse
    {
        /// <summary>성공 여부</summary>
        public bool Success { get; set; }

        /// <summary>메시지</summary>
        public string Message { get; set; }

        /// <summary>타임스탬프</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>오류 코드 (실패 시)</summary>
        public string ErrorCode { get; set; }

        /// <summary>성공 응답 생성</summary>
        public static ApiResponse SuccessResponse(string message = "성공")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>실패 응답 생성</summary>
        public static ApiResponse ErrorResponse(string message, string errorCode = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Timestamp = DateTime.Now
            };
        }
    }
}

