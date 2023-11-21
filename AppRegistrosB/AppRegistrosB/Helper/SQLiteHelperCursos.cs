using System;
using System.Collections.Generic;
using System.Text;
using AppCursosB.model;
using System.Threading.Tasks;
using SQLite;

namespace AppRegistrosB.Helper
{
    public class SQLiteHelperCursos
    {
        SQLiteAsyncConnection db;
        public SQLiteHelperCursos(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<AppCursosB.model.CursosModel>().Wait();

        }

        public Task<int> SaveCursosAsync(AppCursosB.model.CursosModel curso)
        {
            if (curso.IdCurso != 0)
            {
                return db.UpdateAsync(curso);
            }
            else
            {
                return db.InsertAsync(curso);
            }
        }

        public Task<List<CursosModel>> GetCursosAsync()
        {
            return db.Table<CursosModel>().ToListAsync();
        }

        public Task<CursosModel> GetCursoByIdAsync(int IdCurso)
        {
            return db.Table<CursosModel>().Where(a => a.IdCurso == IdCurso).FirstOrDefaultAsync();
        }

        public Task<int> DeleteCursoAsync(CursosModel curso)
        {
            return db.DeleteAsync(curso);
        }
    }
}