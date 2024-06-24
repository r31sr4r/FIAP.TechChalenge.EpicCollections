using FIAP.TechChalenge.EpicCollections.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FIAP.TechChalenge.EpicCollections.Api.Filters
{
    public class ValidateUserIdFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.GetUserId();
            if (userId == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            context.HttpContext.Items["UserId"] = userId.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
