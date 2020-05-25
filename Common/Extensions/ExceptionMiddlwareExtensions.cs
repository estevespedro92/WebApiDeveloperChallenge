using Microsoft.AspNetCore.Builder;
using WebApiDeveloperChallenge.Common.Middlewares;

namespace WebApiDeveloperChallenge.Common.Extensions
{
  public static class ExceptionMiddlwareExtensions
  {
    /// <summary>
    /// Add custom exception middlware
    /// </summary>
    /// <param name="app"></param>
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
      app.UseMiddleware<ExceptionMiddleware>();
    }
  }
}