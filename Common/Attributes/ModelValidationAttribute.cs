using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiDeveloperChallenge.Common.Attributes
{
  public class ModelValidationAttribute : ActionFilterAttribute
  {
    /// <summary>
    /// Check model validation
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult(context.ModelState);
    }
  }
}