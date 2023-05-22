using Jose;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WebApi.Core;
using WebApi.Models;

namespace WebApi.Filters
{

    public class DefaultHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            StringValues strValues;
            if (actionContext.HttpContext.Request.Headers.TryGetValue("Content-Type", out strValues) == false)
            {
                actionContext.HttpContext.Request.Headers.Add("Content-Type", "application/json");
            }
            else if(string.IsNullOrEmpty(strValues[0].ToString()))
            {
                actionContext.HttpContext.Request.Headers.Add("Content-Type", "application/json");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
