using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BiblioConnect.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        //sobreescribe un determinado metodo
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}