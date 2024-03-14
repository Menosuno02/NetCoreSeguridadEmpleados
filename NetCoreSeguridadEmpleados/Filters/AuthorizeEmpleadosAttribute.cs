using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetCoreSeguridadEmpleados.Repositories;
using System.Diagnostics;

namespace NetCoreSeguridadEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute :
        AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Por ahora nos da igual quien sea el empleado
            // Simplemente que exista
            var user = context.HttpContext.User;

            // Necesitamos el controlelr y el action de donde hemos pulsado
            // previamente antes de entrar en este filter
            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();
            // Comprobamos dibujando en consola
            Debug.WriteLine("Controller: " + controller);
            Debug.WriteLine("Action: " + action);
            ITempDataProvider provider =
                context.HttpContext.RequestServices
                .GetService<ITempDataProvider>();
            // Esta clase contiene en su interior el tempdata de nuestra app
            // Recuperamos el Tempdata de nuestra App
            var TempData = provider.LoadTempData(context.HttpContext);
            // Guardamos la información en TempData
            TempData["controller"] = controller;
            TempData["action"] = action;
            // Volveamos a guardar los cambios de TempData en la App
            provider.SaveTempData(context.HttpContext, TempData);

            if (!user.Identity.IsAuthenticated)
            {
                // Enviamos a la vista Login
                context.Result = this.GetRoute("Managed", "Login");
            }
            else
            {
                // Quiero comprobar el role del usuario para
                // permitirle acceso
                if (!user.IsInRole("PRESIDENTE")
                    && !user.IsInRole("ANALISTA")
                    && !user.IsInRole("DIRECTOR"))
                {
                    context.Result = this.GetRoute("Managed", "ErrorAcceso");
                }
            }
        }

        // Como tendremos que facilitar múltiples redirecciones, creamos
        // un método para facilitar la redirección
        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary ruta =
                new RouteValueDictionary(
                        new { controller = controller, action = action }
                    );
            RedirectToRouteResult resultado =
                new RedirectToRouteResult(ruta);
            return resultado;
        }
    }
}
