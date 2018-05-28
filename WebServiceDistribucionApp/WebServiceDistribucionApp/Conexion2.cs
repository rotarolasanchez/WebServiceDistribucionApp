using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceDistribucionApp
{
    public class Conexion2
    {
        
            // public static string Cadena = @"Data Source=PCVIS0097-PC;Initial Catalog=Libreria;Integrated Security=True";
            // public static string Cadena = @"Data source=SERVER06;DataBase=ASISTENCIA;Integrated Security=SSPI";
            public static string Cadena = @"Data Source=192.168.254.6;Initial Catalog=DistribucionApp; Persist Security Info=True; User ID=SA; Password=W3bv1st0";
       
        public class ConexionEpicor
        {
            // public static string Cadena = @"Data Source=PCVIS0097-PC;Initial Catalog=Libreria;Integrated Security=True";
            public static string Cadena = @"Data source=Server11;DataBase=EPICOR905;Integrated Security=SSPI";
            // public static string Cadena = @"Data Source=SERVER06,1433;Initial Catalog=ASISTENCIA; Persist Security Info=True; User ID=sa; Password=W3bv1st0";
        }
    }
}