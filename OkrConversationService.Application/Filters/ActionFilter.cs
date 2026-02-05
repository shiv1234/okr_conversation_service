using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace OkrConversationService.Application.Filters
{
    public class ActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}
