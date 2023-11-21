using AppRegistrosB.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppRegistrosB.Helper
{
    public class SQLiteHelperUser
    {
        SQLiteAsyncConnection db;
        public SQLiteHelperUser(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<UsersModel>().Wait();
        }
        public Task<int> SaveUserModelAsync(UsersModel usr)
        {
            if (usr.UserId != 0)
            {
                return db.UpdateAsync(usr);
            }
            else
            {
                return db.InsertAsync(usr);

            }
        }
        public Task<UsersModel> GetUsersValidate(string email, string password)
        {
            return db.Table<UsersModel>().Where(a => a.Email == email && a.Emailpassword == password).FirstOrDefaultAsync();
            //return db.QueryAsync<UsersModel>("SELECT * FROM Usuarios.db3 WHERE Email = '" + email + "' AND Emailpassword = '" + password + "' ");
        }

    }
}
