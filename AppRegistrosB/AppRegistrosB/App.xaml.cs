using AppRegistrosB.Helper;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRegistrosB
{
    public partial class App : Application
    {
        static SQLiteHelper db;
        static SQLiteHelperCursos dbc;
        static SQLiteHelperSeguimiento dbs;
        static SQLiteHelperUser dbl;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());
        }

        public static SQLiteHelper SQLiteDB
        {
            get
            {
                if (db == null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Empresa.db3"));
                }
                return db;
            }
        }
        public static SQLiteHelperCursos SQLiteDBc
        {
            get
            {
                if (dbc == null)
                {
                    dbc = new SQLiteHelperCursos(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Curso.db3"));
                }
                return dbc;
            }
        }
        public static SQLiteHelperSeguimiento SQLiteDBs
        {
            get
            {
                if (dbs == null)
                {
                    dbs = new SQLiteHelperSeguimiento(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Seguimiento.db3"));
                }
                return dbs;
            }
        }
        public static SQLiteHelperUser SQLiteDBlogin
        {
            get
            {
                if (dbl == null)
                {
                    dbl = new SQLiteHelperUser(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Usuarios.db3"));
                }
                return dbl;
            }

        }


    }
}
