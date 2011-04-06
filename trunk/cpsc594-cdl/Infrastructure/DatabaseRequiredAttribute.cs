using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.Portal.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DatabaseRequiredAttribute : ActionFilterAttribute
    {
        public override void  OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (DatabaseAccessor.Connection())
                base.OnActionExecuting(filterContext);
            else
            {
                filterContext.Result = new RedirectResult("/Error/DBConnection");
            }
        }
    }
}