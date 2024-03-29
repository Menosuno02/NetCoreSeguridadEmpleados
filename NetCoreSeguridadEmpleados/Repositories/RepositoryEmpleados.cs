﻿using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public async Task<Empleado> LoginEmpleadoAsync
            (string apellido, int idEmpleado)
        {
            Empleado empleado = await this.context.Empleados
                .FirstOrDefaultAsync(e => e.Apellido == apellido
                 && e.IdEmpleado == idEmpleado);
            return empleado;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idempleado)
        {
            return await this.context.Empleados
                .FirstOrDefaultAsync(e => e.IdEmpleado == idempleado);
        }

        public async Task<List<Empleado>>
            GetEmpleadosDepartamentoAsync(int iddepart)
        {
            return await this.context.Empleados
                .Where(e => e.Departamento == iddepart)
                .ToListAsync();
        }

        public async Task UpdateSalarioEmpleadosDepartamentoAsync
            (int iddepart, int incremento)
        {
            List<Empleado> empleados =
                await this.GetEmpleadosDepartamentoAsync(iddepart);
            foreach (Empleado emp in empleados)
            {
                emp.Salario += incremento;
            }
            await this.context.SaveChangesAsync();
        }
    }
}
