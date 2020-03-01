using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuthorizedAttribute:AuthorizeAttribute
    {
        public string ViewName { get; set; }

        public CustomAuthorizedAttribute()
        {
            ViewName = "AuthorizeFailed";
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }
        void IsUserAuthorized(AuthorizationContext filterContext)
        {
            //User is authorized
            if (filterContext.Result == null)
                return;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewDataDictionary dic = new ViewDataDictionary();
                dic.Add("Message","You don't have sufficinet privileges for this operation!");
                var result = new ViewResult() { ViewName = this.ViewName, ViewData = dic };
                filterContext.Result = result;
            }


        }

    }
}