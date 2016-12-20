using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rationarum_v3.Infrastructure
{
    public class AllExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // we can add filtering and redirection
            //ex  && filterContext.Exception is ArgumentNullException
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new RedirectResult("~/Content/ErrorPages/AllExceptionError.html");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}