using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCursosB.model
{
    public class CursosModel
    {
        [PrimaryKey, AutoIncrement]
        public int IdCurso { get; set; }

        public string Nombre { get; set; }

        public string Tipo { get; set; }

        public string Descripcion { get; set; }

        public double Horas { get; set; }


    }
}