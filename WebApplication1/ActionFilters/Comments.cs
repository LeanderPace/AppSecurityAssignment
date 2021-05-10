using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Utility;

namespace WebApplication1.ActionFilters
{
    public class Comments : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(context.ActionArguments["id"].ToString());
                Guid decId = Guid.Parse(urlEnc);

                var currentLoggedInUser = context.HttpContext.User.Identity.Name;

                ISubmissionsService subService = (ISubmissionsService)context.HttpContext.RequestServices.GetService(typeof(ISubmissionsService));
                var submission = subService.GetSubmission(decId);
                if (submission.email != currentLoggedInUser && submission.task.email != currentLoggedInUser)
                {
                    context.Result = new UnauthorizedObjectResult("Access Denied");
                }
               
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult("Bad Request");
            }
        }
    }
}
