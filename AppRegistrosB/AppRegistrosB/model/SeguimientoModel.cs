using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppRegistrosB.model
{
    public class SeguimientoModel
    {
        [PrimaryKey, AutoIncrement]
        public int IdRegistro { get; set; }
        public int IdEmple{ get; set; }

        public string NombreEmple { get; set; }
        public string NombreCurso { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Estatus { get; set; }
        public string Calificacion { get; set; }

 
    }
}
