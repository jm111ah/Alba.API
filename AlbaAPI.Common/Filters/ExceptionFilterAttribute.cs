using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using AlbaAPI.Common.Exceptions;
using AlbaAPI.Common.Logging;
using AlbaAPI.Common.Models;

namespace AlbaAPI.Common.Filters
{
    /// <summary>
    /// 전역 예외 처리 필터: 모든 Controller의 예외를 일관되게 처리
    /// </summary>
    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            var request = context.Request;
            var actionName = context.ActionContext?.ActionDescriptor?.ActionName ?? "Unknown";
            var controllerName = context.ActionContext?.ControllerContext?.ControllerDescriptor?.ControllerName ?? "Unknown";

            // ApiException인 경우
            if (exception is ApiException apiException)
            {
                _logger.Warning($"API 예외 발생: {controllerName}.{actionName} - {apiException.Message} (ErrorCode: {apiException.ErrorCode})");

                context.Response = request.CreateResponse(
                    (HttpStatusCode)apiException.StatusCode,
                    ApiResponse<object>.ErrorResponse(apiException.Message, apiException.ErrorCode)
                );
            }
            // 일반 예외인 경우
            else
            {
                _logger.Error($"예기치 않은 오류 발생: {controllerName}.{actionName}", exception);

                // 프로덕션 환경에서는 상세 오류 정보를 숨김
                var errorMessage = "서버 오류가 발생했습니다. 관리자에게 문의하세요.";
                var errorCode = "INTERNAL_SERVER_ERROR";

                context.Response = request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(errorMessage, errorCode)
                );
            }
        }
    }
}

