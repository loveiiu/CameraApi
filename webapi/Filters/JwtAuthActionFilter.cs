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

    public class JwtAuthActionFilter : ActionFilterAttribute
    {
        private ISystemConfig config = Ioc.GetConfig();
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var secret = config.Authorization.Key;
            StringValues strValues;
            if (actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out strValues) == false)
            {
                actionContext.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }
            else
            {
                try
                {
                    string expireAt;
                    var jwtObject = JWT.Decode<Dictionary<string, string>>(
                        strValues[0].Replace("Bearer","").Replace(" ",""),
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS256);
                    if (jwtObject.TryGetValue("exp", out expireAt))
                    {
                        if (DateTime.TryParse(expireAt, out DateTime expireTime))
                        {
                            if (expireTime < DateTime.Now)
                            {
                                actionContext.Result = new ObjectResult(new ApiResult
                                {
                                    Code = Enums.ApiResultCode.Error,
                                    Message = "token 過期"
                                });
                                return;
                            }
                        }
                        else
                        {
                            actionContext.Result = new ObjectResult(new ApiResult
                            {
                                Code = Enums.ApiResultCode.Error,
                                Message = "token 時間有誤"
                            });
                            return;
                        }
                    }
                    else
                    {
                        actionContext.Result = new ObjectResult(new ApiResult
                        {
                            Code = Enums.ApiResultCode.Error,
                            Message = "token 有誤"
                        });
                        return;
                    }
                }
                catch (Exception ex)
                {
                    actionContext.Result = new ObjectResult(new ApiResult
                    {
                        Code = Enums.ApiResultCode.Error,
                        Message = ex.Message
                    });
                    return;
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
