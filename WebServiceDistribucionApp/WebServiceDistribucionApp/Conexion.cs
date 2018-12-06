using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebServiceDistribucionApp
{
    public class Conexion
    {
        public static SqlConnection cnxPrueba()
        {
            SqlConnection cnx = new SqlConnection("Data Source=192.168.254.6; Initial Catalog=ERP10DB; User ID=User_Movil;Password=M0v1l$.");
            cnx.Open();
            return cnx;
        }
        public static SqlConnection cnx()
        {
            SqlConnection cnx = new SqlConnection("Data Source=192.168.254.7; Initial Catalog=ERP10DB; User ID=User_Movil;Password=M0v1l$.");
            cnx.Open();
            return cnx;
        }
        public static SqlConnection cnx2()
        {
            SqlConnection cnx = new SqlConnection("Data Source=192.168.254.7; Initial Catalog=BDVistonyApp; User ID=User_Movil;Password=M0v1l$.");
            cnx.Open();
            return cnx;
        }
        public static SqlConnection cnxantigua()
        {
            SqlConnection cnxantigua = new SqlConnection("Server=192.168.254.6;DataBase=VISTONYDATA;User Id=User_Movil;password=M0v1l$.");
            cnxantigua.Open();
            return cnxantigua;
        }
        public static SqlConnection cnxdistribucion()
        {
            SqlConnection cnxdistribucion = new SqlConnection("Server=192.168.254.6;DataBase=DistribucionApp;User Id=report;password=Report01");
            cnxdistribucion.Open();
            return cnxdistribucion;
        }

        public static SqlConnection cadena()
        {
            // public static string Cadena = @"Data Source=PCVIS0097-PC;Initial Catalog=Libreria;Integrated Security=True";
            // public static string Cadena = @"Data source=SERVER06;DataBase=ASISTENCIA;Integrated Security=SSPI";
            SqlConnection cadena = new SqlConnection("Server=PCFVIS-0098;DataBase=DistribucionApp;User Id=SA;password=Vistony01");
            cadena.Open();
            return cadena;
        }
    }
}