using AppCursosB.model;
using AppRegistrosB.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppRegistrosB.Helper
{
    public class SQLiteHelperSeguimiento
    {
        SQLiteAsyncConnection db;
        public SQLiteHelperSeguimiento(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<SeguimientoModel>().Wait();
        }

        public Task<int> SaveSeguimientoAsync(SeguimientoModel seg)
        {
            if (seg.IdRegistro != 0)
            {
                return db.UpdateAsync(seg);
            }
            else
            {
                return db.InsertAsync(seg);
            }
        }

        public Task<List<SeguimientoModel>> GetSeguimientosAsync()
        {
            return db.Table<SeguimientoModel>().ToListAsync();
        }

        public Task<SeguimientoModel> GetSeguimientoByIdAsync(int Id)
        {
            return db.Table<SeguimientoModel>().Where(a => a.IdRegistro == Id).FirstOrDefaultAsync();
        }

        public Task<int> DeleteRegistroAsync(SeguimientoModel seg)
        {
            return db.DeleteAsync(seg);
        }
    }
}
