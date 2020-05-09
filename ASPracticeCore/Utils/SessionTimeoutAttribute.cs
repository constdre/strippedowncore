using ASPracticeCore.DAL;
using ASPracticeCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Utils
{
    public class SessionValidateAttribute : ActionFilterAttribute
    {
        //Checks whether session is already expired thus null or if it can't be found in the database
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = new HttpContextAccessor().HttpContext;
            string guid = httpContext.User.Identity.Name;

            //instantiate db context, prepare options 
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-DKG8Q1TH\\SQLEXPRESS;Initial Catalog=ASPracticeCore;Integrated Security=True").UseLazyLoadingProxies();
            using (var dbContext = new ApplicationContext(optionsBuilder.Options))
            {
                //check whether cookie is already expired or is different
                if (!UserSessionHelper.IsSessionValid(guid, dbContext))
                {
                    //remove session in the db
                    filterContext.Result = new RedirectToActionResult("Login", "Access", new { Area = "Accounts" });
                    return;
                }

            }
            base.OnActionExecuting(filterContext);
        }
    }
}
