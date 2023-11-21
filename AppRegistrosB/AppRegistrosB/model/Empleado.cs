using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppRegistrosB.model
{
    public class Empleado
    {
        [PrimaryKey, AutoIncrement]

        public int IdEmp { get; set; }

        [MaxLength(50)]

        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public double Telefono { get; set; }
        public int Edad { get; set; }
        public string Curp { get; set; }
        public string TipoEmpleado { get; set; }
    }
}
