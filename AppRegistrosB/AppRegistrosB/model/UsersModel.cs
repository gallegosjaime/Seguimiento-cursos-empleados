using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppRegistrosB.model
{
    public class UsersModel
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Emailpassword { get; set; }

        public string NombreCompleto { get; set; }

        public int Edad { get; set; }

        public string FechaCreacion { get; set; }

    }
}
