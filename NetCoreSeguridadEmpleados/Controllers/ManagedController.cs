using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryEmpleados repo;

        public ManagedController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login
            (string username, string password)
        {
            int idEmpleado = int.Parse(password);
            Empleado empleado = await this.repo.LoginEmpleadoAsync
                (username, idEmpleado);
            if (empleado != null)
            {
                // Seguridad
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                // Creamos Claim para nombre
                Claim claimName = new Claim(ClaimTypes.Name, empleado.Apellido);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString());
                Claim claimOficio = new Claim(ClaimTypes.Role, empleado.Oficio);
                Claim claimSalario = new Claim("Salario", empleado.Salario.ToString());
                Claim claimDepartamento = new Claim("Departamento", empleado.Departamento.ToString());
                identity.AddClaim(claimName);
                identity.AddClaim(claimId);
                identity.AddClaim(claimOficio);
                identity.AddClaim(claimSalario);
                identity.AddClaim(claimDepartamento);
                // Como no vamos a usar roles no los incluimos
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);
                // Lo vamos a llevar a una vista con la info que nos devuelve el
                // filter en TempData
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                return RedirectToAction(action, controller);
                return RedirectToAction("PerfilEmpleado", "Empleados");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
