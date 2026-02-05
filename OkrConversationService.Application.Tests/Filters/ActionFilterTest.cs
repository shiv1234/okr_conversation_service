using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using OkrConversationService.Application.Filters;
using System.Collections.Generic;
using Xunit;

namespace OkrConversationService.Application.Tests.Filters
{
    public class ActionFilterTest
    {
        [Fact]
        public void ActionFilter_IsSuccess()
        {
            // Arrange
            string headersName = "aheadername";
            // Create a default ActionContext (depending on our case-scenario)
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
            var objIFilterMetadata = new List<IFilterMetadata>();
            var mockIActionResult = new Mock<IActionResult>();


            var objActionExecutedContext = new ActionExecutedContext(actionContext, objIFilterMetadata, mockIActionResult.Object);
            // Act
            var objActionFilter = new ActionFilter();
            objActionFilter.OnActionExecuted(objActionExecutedContext);
            // Assert
            Assert.Equal(0, objActionExecutedContext.HttpContext.Response.Headers.Count);
            Assert.False(objActionExecutedContext.HttpContext.Response.Headers.ContainsKey(headersName));
        }
    }
}
