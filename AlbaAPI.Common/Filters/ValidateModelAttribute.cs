using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using AlbaAPI.Common.Models;

namespace AlbaAPI.Common.Filters
{
    /// <summary>
    /// 모델 유효성 검증 필터: ModelState가 유효하지 않으면 자동으로 오류 응답 반환
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var errorMessage = string.Join("; ", errors);

                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    ApiResponse<object>.ErrorResponse($"입력값이 유효하지 않습니다: {errorMessage}", "VALIDATION_ERROR")
                );
            }
        }
    }
}

