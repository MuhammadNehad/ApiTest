using ApiTest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Filters
{
    public class TokenFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            if (hasAllowAnonymous)
            { return; }

            var tokenValidator = (JwtValidationInterface)context.HttpContext.RequestServices.GetService(typeof(JwtValidationInterface));
            var headers = context.HttpContext.Request.Headers;
            var queries = context.HttpContext.Request.Query;
            bool result = true;

            if (!headers.ContainsKey("Authorization"))
            {
                result = false;
            }
            if(result)
            {
                string tokenHeader = headers.FirstOrDefault(x => x.Key == "Authorization").Value;
                if(tokenHeader != null)
                {
                    string token = tokenHeader.Split(" ")[1];
                    var queryname = queries.Where(e => e.Key == "name").FirstOrDefault();
                    if(!tokenValidator.validate(queryname.Value,token))
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }

            if(!result)
            {
                context.ModelState.AddModelError("Unauthorized", "You are not authorized");
                context.Result = new UnauthorizedObjectResult(context.ModelState);

            }

        }
    }
    

    
}
