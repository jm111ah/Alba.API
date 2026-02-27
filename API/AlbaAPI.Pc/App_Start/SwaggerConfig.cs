using System.Web.Http;
using AlbaAPI.Common.Configuration;

namespace AlbaAPI.Pc
{
    /// <summary>
    /// Swagger API 문서화 설정
    /// Swashbuckle 패키지 설치 후 활성화: Install-Package Swashbuckle
    /// </summary>
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (!AppSettings.EnableSwagger)
            {
                return;
            }

            // Swashbuckle 패키지 설치 후 아래 주석 해제
            /*
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Alba API - PC")
                        .Description("PC 플랫폼용 RESTful API")
                        .Contact(cc => cc
                            .Name("Alba API Support")
                            .Email("support@alba.com"));

                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("Alba API - PC Documentation");
                });
            */
        }

        private static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\AlbaAPI.Pc.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}

