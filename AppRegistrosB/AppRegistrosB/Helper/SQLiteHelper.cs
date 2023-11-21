using System;
using System.Collections.Generic;
using System.Text;
using AppRegistrosB.model;
using System.Threading.Tasks;
using SQLite;

namespace AppRegistrosB.Helper
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public  SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Empleado>().Wait();


        }

        public Task<int> SaveEmpleadosAsync(Empleado emple)
        {
            if (emple.IdEmp != 0)
            {
                return db.UpdateAsync(emple);
            }
            else
            {
                return db.InsertAsync(emple);
            }
        }

        public Task<List<Empleado>>GetEmpleadosAsync()
        {
            return db.Table<Empleado>().ToListAsync();
        }
        public Task<Empleado> GetEmpleadoByIdAsync(int IdEmp)
        {
            return db.Table<Empleado>().Where(a => a.IdEmp == IdEmp).FirstOrDefaultAsync();
        }

        public Task<int> DeleteEmpleadoAsync(Empleado empleado)
        {
            return db.DeleteAsync(empleado);
        }
    }
}
