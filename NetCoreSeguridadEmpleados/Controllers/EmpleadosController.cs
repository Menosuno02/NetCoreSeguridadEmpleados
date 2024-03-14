using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados =
                await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details
            (int idempleado)
        {
            Empleado empleado = await this.repo.FindEmpleadoAsync(idempleado);
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> PerfilEmpleado()
        {
            return View();
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {
            string dato = HttpContext.User.FindFirst("Departamento").Value;
            int idDepartamento = int.Parse(dato);
            List<Empleado> empleados = await
                this.repo.GetEmpleadosDepartamentoAsync(idDepartamento);
            return View(empleados);
        }

        [AuthorizeEmpleados]
        [HttpPost]
        public async Task<IActionResult> Compis(int incremento)
        {
            string depart = HttpContext.User.FindFirst("Departamento").Value;
            int idDepart = int.Parse(depart);
            await this.repo.UpdateSalarioEmpleadosDepartamentoAsync(idDepart, incremento);
            List<Empleado> empleados = await
                this.repo.GetEmpleadosDepartamentoAsync(idDepart);
            return View(empleados);
        }
    }
}
