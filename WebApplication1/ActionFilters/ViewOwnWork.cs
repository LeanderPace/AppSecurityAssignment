using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ActionFilters
{
    public class ViewOwnWork : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var currentLoggedInUser = context.HttpContext.User.Identity.Name;

                ISubmissionsService subService = (ISubmissionsService)context.HttpContext.RequestServices.GetService(typeof(ISubmissionsService));
                var submissions = subService.GetSubmissionsForStudent(currentLoggedInUser);
                foreach(var sub in submissions)
                {
                    if (sub.email != currentLoggedInUser)
                    {
                        context.Result = new UnauthorizedObjectResult("Access Denied");
                    }
                }
                
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult("Bad Request");
            }
        }
    }
}
