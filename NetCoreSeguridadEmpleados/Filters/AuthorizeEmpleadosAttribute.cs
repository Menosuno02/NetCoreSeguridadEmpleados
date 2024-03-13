using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            if (!user.Identity.IsAuthenticated)
            {
                // Enviamos a la vista Login
                context.Result = this.GetRoute("Managed", "Login");
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
