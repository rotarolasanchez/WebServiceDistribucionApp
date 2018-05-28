using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;

namespace WebServiceDistribucionApp
{
    /// <summary>
    /// Descripción breve de WebServiceDistribucionApp
    /// </summary>
    [WebService(Namespace = "http://190.12.79.136/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceDistribucionApp : System.Web.Services.WebService
    {
        public static List<Distribucion.RutaCorta> lista = new List<Distribucion.RutaCorta>();
        public static List<Distribucion.RutaCorta> listaclientes = new List<Distribucion.RutaCorta>();
        public static List<Grupos.Elementos> listacantidadclientes = new List<Grupos.Elementos>();
        public static List<Cliente.Elementos> listanuevosclientes = new List<Cliente.Elementos>();
        public static List<Cliente_Analisis.Elementos> cliente_analisis = new List<Cliente_Analisis.Elementos>();
        public static List<Cliente_Analisis.Elementos> cliente_analizado = new List<Cliente_Analisis.Elementos>();
        public static List<Cliente_Agrupado.Elementos> cliente_grupado = new List<Cliente_Agrupado.Elementos>();
        public static List<Cliente_Agrupado.Elementos> cliente_agrupado = new List<Cliente_Agrupado.Elementos>();
        public static List<Cliente_Depurado.Elementos> cliente_depurado = new List<Cliente_Depurado.Elementos>();
        public static List<Grupos.Elementos> lista_grupos = new List<Grupos.Elementos>();
        public static List<Cliente_Agrupado.Elementos> grupo_principal = new List<Cliente_Agrupado.Elementos>();
        public static List<Cliente_Geolocalizado.Elementos> lista_cliente_localizado = new List<Cliente_Geolocalizado.Elementos>();
        public static List<ShipTo.Elementos> lista_shipto = new List<ShipTo.Elementos>();
        public static List<Despacho.Elementos> Despacho = new List<Despacho.Elementos>();
        public static List<Despachos.Elementos> Despachos = new List<Despachos.Elementos>();
        public static List<Despachos.Elementos> Despacho_ordenado = new List<Despachos.Elementos>();

        [WebMethod]
        public String SPM_InicioSesionImei(String cia, String user, String imei)
        {
            String result = "";
            SqlCommand cmd;
            SqlConnection cnxdistribucion = Conexion.cnxdistribucion();
            try
            {
                cmd = new SqlCommand("SPM_InicioSesion_Chofer", cnxdistribucion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cia", cia);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@imei", imei);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(reader.GetOrdinal("msje"));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cnxdistribucion.Close();
            }
            return result;
        }
        [WebMethod]
        public String LoginUsuarioImei(String cia, String user, String imei)
        {
            try
            {
                string msje = "";
                msje = SPM_InicioSesionImei(cia, user, imei);
                return msje;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message + "; user: " + user + "; imei: " + imei;
            }

        }


        [WebMethod]
        public List<Despachos.Elementos> ObtenerDespacho(String company,String vend, String fprog)
        {
            Despachos.Clear();
            int resultado = 0;

            resultado=GetDataSourceDistancia(QueryValidarDistancia(company,vend,fprog));
            //resultado = ValidarDespacho(company,vend, fprog);
            if (resultado.Equals(0))
            {
                ObtenerDataDespacho(company,vend, fprog);
                ValidaDisCliente();
                Despachos = CalcularDistanciaCorta();
                for (int j = 0; j < Despachos.Count;j++)
                {
                    InsertarDespachoOrdenado(
                            Despachos[j].Company,
                            Despachos[j].SalesRepCode,
                            Despachos[j].Date,
                            //Despachos[j].Day,
                            Despachos[j].CustNum,
                            Despachos[j].ShiptoNum,
                            Despachos[j].SalesRepName,
                            Despachos[j].OrderDispatch,
                            Despachos[j].CustId,
                            Despachos[j].Name,
                            Despachos[j].Address_c,
                            Despachos[j].PhoneNum,
                            //Despachos[j].ReservePriorityCode,
                            // Despachos[j].CreditLimit,
                            // Despachos[j].Description,
                            Despachos[j].Latitude_c,
                            Despachos[j].Longitude_c,
                            Despachos[j].FechaDespacho,
                            // Despachos[j].TerritoryCode_c,
                            Despachos[j].chkEntregado_c,
                            Despachos[j].chkAnulado_c,
                            Despachos[j].chkReprogramado_c,
                            Despachos[j].chkPendiente_c,
                            Despachos[j].Ubigeo,
                            Despachos[j].State,
                            Despachos[j].LegalNumber,
                            Despachos[j].PackNum);
                           // Despachos[j].invoicedate,
                           // Despachos[j].Monto); 

                        
                }

            }
            else
            {
                Despachos=ObtenerDespachoOrdenado(company, vend, fprog);
            }

            return Despachos;
        }

        [WebMethod]
        public int ActualizarDespachoOrdenado
        (int entregado,
         int reprogramado, 
         int anulado,
         int pendiente,
         string latitude_c,
         string longitude_c,
         string orderdispatch,
         string vend,
         string fechadespacho,
         string company          
         )
        {
            int resultado = 0;
            SqlCommand cmd;
            SqlConnection cn = Conexion.cnxdistribucion();

            JavaScriptSerializer javaScripSerializer = new JavaScriptSerializer();
            try
            {
                cmd = new SqlCommand("USP_WS_Actualiza_Despacho_Chofer", cn);
                cmd.Parameters.AddWithValue("@chkEntregado_c", entregado);
                cmd.Parameters.AddWithValue("@chkReprogramado_c", reprogramado);
                cmd.Parameters.AddWithValue("@chkAnulado_c", anulado);
                cmd.Parameters.AddWithValue("@chkPendiente_c", pendiente);
                cmd.Parameters.AddWithValue("@latitude_c", latitude_c);
                cmd.Parameters.AddWithValue("@longitude_c", longitude_c);
                cmd.Parameters.AddWithValue("@orderDispatch", orderdispatch);
                cmd.Parameters.AddWithValue("@salesRepCode", vend);
                cmd.Parameters.AddWithValue("@fechadespacho", fechadespacho);
                cmd.Parameters.AddWithValue("@Company", company);
                

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                resultado = 1;
            }


            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                resultado = 0;
            }
            finally
            {
                cn.Close();
            }

            return resultado;
        }
        public List<Despachos.Elementos> ObtenerDespachoOrdenado(String company, String vend, String fprog)
        {
            var JSON = "";
            SqlCommand cmd;
            SqlConnection cn = Conexion.cnxdistribucion();

            JavaScriptSerializer javaScripSerializer = new JavaScriptSerializer();
            try
            {
                cmd = new SqlCommand("USP_WS_Obtener_Despacho_Ordenado", cn);
                cmd.Parameters.AddWithValue("@SalesRepCode", vend);
                cmd.Parameters.AddWithValue("@FechaProg", fprog);
                cmd.Parameters.AddWithValue("@company", company);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Despachos.Elementos d = new Despachos.Elementos()
                    {
                        Company = reader["Company"].ToString(),
                        SalesRepCode = reader["SalesRepCode"].ToString(),
                        Date = reader["Date"].ToString(),
                        //Day = reader["Day"].ToString(),
                        CustNum = reader["CustNum"].ToString(),
                        ShiptoNum = reader["ShiptoNum"].ToString(),
                        SalesRepName = reader["SalesRepName"].ToString(),
                        OrderDispatch = reader["OrderDispatch"].ToString(),
                        CustId = reader["Custid"].ToString(),
                        Name = reader["Name"].ToString(),
                        Address_c = reader["Address_c"].ToString(),
                        PhoneNum = reader["PhoneNum"].ToString(),
                        //ReservePriorityCode = reader["ReservePriorityCode"].ToString(),
                        //CreditLimit = reader["CreditLimit"].ToString(),
                        //Description = reader["Description"].ToString(),
                        Latitude_c = reader["Latitude_c"].ToString(),
                        Longitude_c = reader["Longitude_c"].ToString(),
                        FechaDespacho = reader["FechaDespacho"].ToString(),
                        //TerritoryCode_c = reader["TerritoryCode_c"].ToString(),
                        chkEntregado_c = reader["chkEntregado_c"].ToString(),
                        chkAnulado_c = reader["chkAnulado_c"].ToString(),
                        chkReprogramado_c = reader["chkReprogramado_c"].ToString(),
                        chkPendiente_c = reader["chkPendiente_c"].ToString(),
                        //invoicedate = reader["invoicedate"].ToString(),
                        //Monto = reader["Monto"].ToString()
                        Ubigeo = reader["Ubigeo"].ToString(),
                        State = reader["State"].ToString(),
                        LegalNumber = reader["LegalNumber"].ToString(),
                        PackNum = reader["PackNum"].ToString(),

                    };
                    Despachos.Add(d);
                }
            }


            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }

            return Despachos;

        }
        public int InsertarDespachoOrdenado
            (
            String Company, 
            String SalesRepCode,
            String Date,
           // String Day,
            String CustNum,
            String ShiptoNum,
            String SalesRepName,
            String OrderDispatch,
            String CustId,
            String Name,
            String Address_c,
            String PhoneNum,
           // String ReservePriorityCode,
           //String CreditLimit,
           // String Description,
            String Latitude_c,
            String Longitude_c,
            String FechaDespacho,
            //String TerritoryCode_c,
            String chkEntregado_c,
            String chkAnulado_c,
            String chkReprogramado_c,
            String chkPendiente_c,
            //String invoicedate,
            //String Monto
            String Ubigeo,
            String State,
            String LegalNumber,
            String PackNum)
        {
            int resultado = 0;
           
            SqlCommand cmd;
            SqlConnection cn = Conexion.cnxdistribucion();

            JavaScriptSerializer javaScripSerializer = new JavaScriptSerializer();
            try
            {
                cmd = new SqlCommand("USP_WS_Insertar_Despacho_Chofer", cn);
                cmd.Parameters.AddWithValue("@Company",Company);
                cmd.Parameters.AddWithValue("@SalesRepCode", SalesRepCode);
                cmd.Parameters.AddWithValue("@Date",Date);
                //cmd.Parameters.AddWithValue("@Day",Day);
                cmd.Parameters.AddWithValue("@CustNum",CustNum);
                cmd.Parameters.AddWithValue("@ShiptoNum",ShiptoNum);
                cmd.Parameters.AddWithValue("@SalesRepName", SalesRepName);
                cmd.Parameters.AddWithValue("@OrderDispatch", OrderDispatch);
                cmd.Parameters.AddWithValue("@CustId", CustId);
                cmd.Parameters.AddWithValue("@Name",Name);
                cmd.Parameters.AddWithValue("@Address_c", Address_c);
                cmd.Parameters.AddWithValue("@PhoneNum", PhoneNum);
                //cmd.Parameters.AddWithValue("@ReservePriorityCode",ReservePriorityCode);
                //cmd.Parameters.AddWithValue("@CreditLimi",CreditLimit);
                //cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@Latitude_c","");
                cmd.Parameters.AddWithValue("@Longitude_c","");
                cmd.Parameters.AddWithValue("@FechaDespacho", DateTime.Parse(FechaDespacho));
                cmd.Parameters.AddWithValue("@chkEntregado_c", chkEntregado_c);
                cmd.Parameters.AddWithValue("@chkAnulado_c",chkAnulado_c);
                cmd.Parameters.AddWithValue("@chkReprogramado_c",chkReprogramado_c);
                cmd.Parameters.AddWithValue("@chkPendiente_c",chkPendiente_c);
                //cmd.Parameters.AddWithValue("@invoicedate", invoicedate);
                //cmd.Parameters.AddWithValue("@Monto",Monto);
                cmd.Parameters.AddWithValue("@Ubigeo", Ubigeo);
                cmd.Parameters.AddWithValue("@State",State);
                cmd.Parameters.AddWithValue("@LegalNumber", LegalNumber);
                cmd.Parameters.AddWithValue("@PackNum", PackNum);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                resultado = 1;
            }


            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }


            return resultado;
        }
        public List<Despachos.Elementos> ObtenerDataDespacho(String company,String vend, String fprog)
        {
            var JSON = "";
            SqlCommand cmd;
            SqlConnection cn = Conexion.cnxdistribucion();

            JavaScriptSerializer javaScripSerializer = new JavaScriptSerializer();
            try
            {
                cmd = new SqlCommand("USP_WS_Despacho_Chofer", cn);
                cmd.Parameters.AddWithValue("@SalesRepCode", vend);
                cmd.Parameters.AddWithValue("@FechaProg", fprog);
                cmd.Parameters.AddWithValue("@company", company);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Despachos.Elementos d = new Despachos.Elementos()
                    {
                            Company = reader["Company"].ToString(),
                            SalesRepCode = reader["SalesRepCode"].ToString(),
                            Date = reader["Date"].ToString(),
                            //Day = reader["Day"].ToString(),
                            CustNum = reader["CustNum"].ToString(),
                            ShiptoNum = reader["ShiptoNum"].ToString(),
                            SalesRepName = reader["SalesRepName"].ToString(),
                            OrderDispatch = reader["OrderDispatch"].ToString(),
                            CustId = reader["Custid"].ToString(),
                            Name = reader["Name"].ToString(),
                            Address_c = reader["Address_c"].ToString(),
                            PhoneNum = reader["PhoneNum"].ToString(),
                            //ReservePriorityCode = reader["ReservePriorityCode"].ToString(),
                            //CreditLimit = reader["CreditLimit"].ToString(),
                            //Description = reader["Description"].ToString(),
                            Latitude_c = reader["Latitude_c"].ToString(),
                            Longitude_c = reader["Longitude_c"].ToString(),
                            FechaDespacho = reader["FechaDespacho"].ToString(),
                            //TerritoryCode_c = reader["TerritoryCode_c"].ToString(),
                            chkEntregado_c = reader["chkEntregado_c"].ToString(),
                            chkAnulado_c = reader["chkAnulado_c"].ToString(),
                            chkReprogramado_c = reader["chkReprogramado_c"].ToString(),
                            chkPendiente_c = reader["chkPendiente_c"].ToString(),
                        //invoicedate = reader["invoicedate"].ToString(),
                        //Monto = reader["Monto"].ToString()
                            Ubigeo = reader["Ubigeo"].ToString(),
                            State = reader["State"].ToString(),
                            LegalNumber = reader["LegalNumber"].ToString(),
                            PackNum = reader["PackNum"].ToString(),

                    };
                    Despachos.Add(d);
                }
            }


            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }

            return Despachos;

        }

        public string ValidarDespacho(String company,String vend,String fprog)
        {
            string resultado = "";
            SqlCommand cmd;
            SqlConnection cn = Conexion.cnxdistribucion();

            JavaScriptSerializer javaScripSerializer = new JavaScriptSerializer();
            try
            {
                cmd = new SqlCommand("USP_WS_Validacion_Despacho_Chofer", cn);
                cmd.Parameters.AddWithValue("@company", company);
                cmd.Parameters.AddWithValue("@SalesRepCode", vend);
                cmd.Parameters.AddWithValue("@FechaProg", fprog);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    resultado = reader.GetOrdinal("msje").ToString();
                }
            }


            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }

            return resultado;

        }

        


        private int ValidaDisCliente()
        {

            string cli1 = "";
            string ship1 = "";
            string cli2 = "";
            string ship2 = "";
            string SQLQuery = "";
            string resultado = "";
            string vistony = "13249";
            string shiptonum = "01";
            string latitud = "-11.764080";
            string longitud = "-77.161065";
            int cerradura = 0;
            for (int i = 0; i < Despachos.Count; i++)
            {
                if (cerradura == 0)
                {
                    for (int j = 0; j < Despachos.Count; j++)
                    {
                        if (Despachos[i].CustNum != Despachos[j].CustNum)
                        {
                            cli1 = vistony;
                            cli2 = Convert.ToString(Despachos[j].CustNum);
                            ship1 = shiptonum;
                            ship2 = Convert.ToString(Despachos[j].ShiptoNum);
                            string SQLQueryConsulta = QueryConsultaDistancia(cli1, ship1, cli2, ship2);
                            resultado = Convert.ToString(GetDataSourceDistancia(SQLQueryConsulta));
                            if ((GetDataSourceDistancia(SQLQueryConsulta)) <= 0)
                            {
                                try
                                {
                                    string latclie1 = "";
                                    string lonclie1 = "";
                                    string latclie2 = "";
                                    string lonclie2 = "";
                                    latclie1 = latitud;
                                    lonclie1 = longitud;
                                    latclie2 = Convert.ToString(Despachos[j].Latitude_c);
                                    lonclie2 = Convert.ToString(Despachos[j].Longitude_c);
                                    string url = @"https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + latclie1 + "," + lonclie1 + "&destinations=" + latclie2 + "," + lonclie2 + "&mode=driving&language=fr-FR&avoid=tolls&key=AIzaSyC_k2m7mxljS8M4kXZE41n1t4V_Usgkq_4";
                                    var web_request = (HttpWebRequest)WebRequest.Create(url);
                                    using (var response = web_request.GetResponse())
                                    using (var reader = new StreamReader(response.GetResponseStream()))
                                    {
                                        string resultado2 = reader.ReadToEnd();
                                        string jsonRes = Convert.ToString(resultado2);
                                        RootObject respuesta = JsonConvert.DeserializeObject<RootObject>(jsonRes);
                                        if (respuesta.status == "OK")
                                        {
                                            for (int k = 0; k < respuesta.rows.Length; k++)
                                            {
                                                string distance = respuesta.rows[k].elements[k].distance.value;
                                                string duration = respuesta.rows[k].elements[k].duration.value;

                                                SQLQuery = QueryInsertarDistancia(cli1, cli2, distance, duration, ship1, ship2);
                                                GetDataSourceDistancia(SQLQuery);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    cerradura++;
                }
            
                for (int j = 0; j < Despachos.Count; j++)
                {
                    if (Despachos[i].CustNum != Despachos[j].CustNum)
                    {
                        cli1 = Convert.ToString(Despachos[i].CustNum);
                        cli2 = Convert.ToString(Despachos[j].CustNum);
                        ship1 = Convert.ToString(Despachos[i].ShiptoNum);
                        ship2 = Convert.ToString(Despachos[j].ShiptoNum);
                        string SQLQueryConsulta = QueryConsultaDistancia(cli1,ship1,cli2,ship2);
                        resultado = Convert.ToString(GetDataSourceDistancia(SQLQueryConsulta));
                        if ((GetDataSourceDistancia(SQLQueryConsulta)) <= 0)
                        {
                            try
                            {
                                string latclie1 = "";
                                string lonclie1 = "";
                                string latclie2 = "";
                                string lonclie2 = "";
                                latclie1 = Convert.ToString(Despacho[i].Latitud);
                                lonclie1 = Convert.ToString(Despacho[i].Longitud);
                                latclie2 = Convert.ToString(Despacho[j].Latitud);
                                lonclie2 = Convert.ToString(Despacho[j].Longitud);
                                string url = @"https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + latclie1 + "," + lonclie1 + "&destinations=" + latclie2 + "," + lonclie2 + "&mode=driving&language=fr-FR&avoid=tolls&key=AIzaSyC_k2m7mxljS8M4kXZE41n1t4V_Usgkq_4";
                                var web_request = (HttpWebRequest)WebRequest.Create(url);
                                using (var response = web_request.GetResponse())
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    string resultado2 = reader.ReadToEnd();
                                    string jsonRes = Convert.ToString(resultado2);
                                    RootObject respuesta = JsonConvert.DeserializeObject<RootObject>(jsonRes);
                                    if (respuesta.status == "OK")
                                    {
                                        for (int k = 0; k < respuesta.rows.Length; k++)
                                        {
                                            string distance = respuesta.rows[k].elements[k].distance.value;
                                            string duration = respuesta.rows[k].elements[k].duration.value;

                                            SQLQuery = QueryInsertarDistancia(cli1,cli2,distance, duration, ship1, ship2);
                                            GetDataSourceDistancia(SQLQuery);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                    }
                }
            }

            return 1;
        }
        public string QueryValidarDistancia(string company, string vend, string fprog)
        {
            string QuerySQL = "";
            QuerySQL = " select  COUNT(company) resultado FROM dbo.HojaDespacho " +
                       " where Company = " +"'"+ company +"'"+ " and SalesRepCode = " + vend + "" +
                       " and FechaDespacho =" + "'"+fprog +"'"+ "";
            return QuerySQL;
        }

        public string QueryConsultaDistancia(string cli1, string ship1, string cli2,string ship2)
        {
            string QuerySQL = "";
            QuerySQL = " SELECT codoriDiscli,coddesDiscli,disDiscli,tiemDiscli" +
                       " FROM [DistribucionApp].[dbo].[DisCliente]" +
                       " WHERE codoriDiscli = '" + cli1 + "' and shiporiDiscli = '" + ship1 + "'" +
                       " and coddesDiscli = '" + cli2 + "' and shipdesDiscli = '" + ship2 + "'";
            return QuerySQL;
        }
        public int GetDataSourceDistancia(string SQLCmd)
        {

            int numberRecord;
            try
            {
                SqlConnection cnn;
                cnn = new SqlConnection(("Server=192.168.254.6;uid=SA;pwd=W3bv1st0;Database=DistribucionApp"));
                cnn.Open();
                SqlCommand cmd = new SqlCommand(SQLCmd, cnn);
                SqlDataAdapter daSource = new SqlDataAdapter(SQLCmd, cnn);
                numberRecord = Convert.ToInt32(cmd.ExecuteScalar());
                cnn.Close();
                daSource.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                numberRecord = 0;
            }
            return numberRecord;
        }

        public string QueryInsertarDistancia(string cliori, string clides, string dist, string dura,string ship1,string ship2)
        {
            string QuerySQL = "";
            QuerySQL = " insert into [DistribucionApp].[dbo].[DisCliente]" +
                       " values  ('" + cliori + "','" + clides + "','" + dist + "','" + dura + "','" + ship1 + "','" + ship2 + "')";
            return QuerySQL;
        }

        private List<Despachos.Elementos> CalcularDistanciaCorta()
        {
            
            string clientes = "";
            string NoClientes = "";
            string prueba = "";
            string Codinicio = "";
            Distribucion.RutaCorta RutaCorta = new Distribucion.RutaCorta();
            var ru = new Distribucion.RutaCorta();
            for (int l = 0; l < Despachos.Count; l++)
            {
                if (l == 0)
                {

                    for (int k = 0; k < Despachos.Count; k++)
                    {
                        Codinicio = "13249";
                        if (!(Codinicio.Equals(Despachos[l].CustNum)))
                        {
                            clientes = clientes + "," + Despachos[k].CustNum;

                        }
                    }
                    clientes = clientes.ToString().Substring(1, clientes.Length - 1);
                    var aux = new Distribucion.RutaCorta()
                    {
                        codoridiscli = Codinicio,
                        coddesdiscli = clientes
                    };
                    Buscar(aux);
                }
                else
                {
                    Codinicio = "";
                    clientes = "";
                    NoClientes = "";
                    int contador = l;
                    Codinicio = lista[l-1].coddesdiscli.ToString();

                    for (int n = 0; n < lista.Count; n++)
                    {
                        NoClientes = NoClientes + "," + lista[n].codoridiscli;

                    }
                    NoClientes = NoClientes.ToString().Substring(1, NoClientes.Length - 1);

                    for (int m = 0; m < Despachos.Count; m++)
                    {
                        clientes = clientes + "," + Despachos[m].CustNum;
                    }
                    clientes = clientes.ToString().Substring(1, clientes.Length - 1);
                    var aux2 = new Distribucion.RutaCorta()
                    {
                        codoridiscli = Codinicio,
                        coddesdiscli = clientes,
                        disdiscli = NoClientes
                    };

                    Buscar2(aux2);
                }
            }

            
            int cuenta_cliente = 0;
            string cliente = "";

            for (int xxx = 0; xxx < lista.Count; xxx++)
            {
                int contador = 0;
                
                for (int yyy = 0; yyy < Despachos.Count; yyy++)
                {
                    
                    if (lista[xxx].codoridiscli==Despachos[yyy].CustNum)
                    {
                        contador = 1;
                        cliente = lista[xxx].codoridiscli.ToString();
                    }
                   

                }

                if (contador == 1)
                {
                    cuenta_cliente = 0;
                    for (int aaa = 0; aaa < Despacho_ordenado.Count; aaa++)
                    {
                        if (cliente==Despacho_ordenado[aaa].CustNum)
                        {
                            cuenta_cliente = 1;
                        }

                    }

                    if (cuenta_cliente == 0)
                    {
                        var despacho = new Despachos.Elementos()
                        {
                            CustNum = cliente
                        }
                    ;
                        Despacho_ordenado.Add(despacho);
                    }
                    
                }

                contador = 0;
                for (int zzz = 0; zzz < Despachos.Count; zzz++)
                {
                    
                    if (lista[xxx].codoridiscli==Despachos[zzz].CustNum)
                    {
                        contador = 1;
                    }
                    
                  
                }
                if (contador == 1)
                {
                    cuenta_cliente = 0;
                    for (int bbb = 0; bbb < Despacho_ordenado.Count; bbb++)
                    {
                        if (cliente==Despacho_ordenado[bbb].CustNum)
                        {
                            cuenta_cliente = 1;
                        }

                    }

                    if (cuenta_cliente == 0)
                    {
                        var despacho = new Despachos.Elementos()
                        {
                            CustNum = cliente
                        }
                    ;
                        Despacho_ordenado.Add(despacho);
                    }

                }




            }


            for (int xyz = 0; xyz < Despacho_ordenado.Count; xyz++)
            {
                for (int zyx = 0; zyx < Despachos.Count; zyx++)
                {
                    if (Despacho_ordenado[xyz].CustNum == Despachos[zyx].CustNum)
                    {
                        Despacho_ordenado[xyz].ShiptoNum = Despachos[zyx].ShiptoNum.ToString();
                        Despacho_ordenado[xyz].Latitude_c = Despachos[zyx].Latitude_c.ToString();
                        Despacho_ordenado[xyz].Longitude_c = Despachos[zyx].Longitude_c.ToString();
                        Despacho_ordenado[xyz].CustId = Despachos[zyx].CustId.ToString();
                        Despacho_ordenado[xyz].Name = Despachos[zyx].Name.ToString();
                        Despacho_ordenado[xyz].Address_c = Despachos[zyx].Address_c.ToString();
                        Despacho_ordenado[xyz].Ubigeo = Despachos[zyx].Ubigeo.ToString();
                        Despacho_ordenado[xyz].LegalNumber = Despachos[zyx].LegalNumber.ToString();
                        Despacho_ordenado[xyz].PackNum = Despachos[zyx].PackNum.ToString();
                        Despacho_ordenado[xyz].CustNum = Despachos[zyx].CustNum.ToString();
                        Despacho_ordenado[xyz].Company = Despachos[zyx].Company.ToString();
                        Despacho_ordenado[xyz].State = Despachos[zyx].State.ToString();
                        Despacho_ordenado[xyz].SalesRepCode = Despachos[zyx].SalesRepCode.ToString();
                        Despacho_ordenado[xyz].SalesRepName = Despachos[zyx].SalesRepName.ToString();
                        Despacho_ordenado[xyz].Date = Despachos[zyx].Date.ToString();
                        Despacho_ordenado[xyz].SalesRepName = Despachos[zyx].SalesRepName.ToString();
                        Despacho_ordenado[xyz].OrderDispatch = (xyz+1).ToString();
                        Despacho_ordenado[xyz].PhoneNum = Despachos[zyx].PhoneNum.ToString();
                        Despacho_ordenado[xyz].FechaDespacho = Despachos[zyx].FechaDespacho.ToString();
                        Despacho_ordenado[xyz].chkEntregado_c = Despachos[zyx].chkEntregado_c.ToString();
                        Despacho_ordenado[xyz].chkAnulado_c = Despachos[zyx].chkAnulado_c.ToString();
                        Despacho_ordenado[xyz].chkPendiente_c = Despachos[zyx].chkPendiente_c.ToString();
                        Despacho_ordenado[xyz].chkReprogramado_c = Despachos[zyx].chkReprogramado_c.ToString();
                        Despacho_ordenado[xyz].State = Despachos[zyx].State.ToString();
                    }
                }
            }

            return Despacho_ordenado;
            DataTable table = ConvertListToDataTable(lista);
            //dgvprueba.DataSource = table;
        }

        public static List<Distribucion.RutaCorta> Buscar(Distribucion.RutaCorta ca)
        {
            var cn = new SqlConnection(Conexion2.Cadena);

            
            cn.Open();
            string sql = @"select codoriDiscli, coddesDiscli, disDiscli, tiemDiscli from[DistribucionApp].[dbo].[DisCliente] " +
                        " where disDiscli = ( SELECT MIN(disDiscli) FROM [DistribucionApp].[dbo].[DisCliente] " +
                        " WHERE codoriDiscli =" + ca.codoridiscli + " and coddesDiscli in (" + ca.coddesdiscli + "))";
            
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Distribucion.RutaCorta Ruta = new Distribucion.RutaCorta()
                {
                    codoridiscli = dr["codoriDiscli"].ToString(),
                    coddesdiscli = dr["coddesDiscli"].ToString(),
                    disdiscli = dr["disDiscli"].ToString(),
                    tiemdiscli = dr["tiemDiscli"].ToString()
                };
                lista.Add(Ruta);
            }
            cn.Close();
            return lista;
        }

        public static List<Distribucion.RutaCorta> Buscar2(Distribucion.RutaCorta ca)
        {
            var cn = new SqlConnection(Conexion2.Cadena);

          
            cn.Open();
            string sql = @"select top 1 * from[DistribucionApp].[dbo].[DisCliente] " +
                        " where disDiscli = ( SELECT MIN(disDiscli) FROM [DistribucionApp].[dbo].[DisCliente] " +
                        " WHERE codoriDiscli =" + ca.codoridiscli + " and coddesDiscli in (" + ca.coddesdiscli + ") " +
                        "and coddesDiscli not in (" + ca.disdiscli + ")) and codoriDiscli=" + ca.codoridiscli + "";
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Distribucion.RutaCorta Ruta = new Distribucion.RutaCorta()
                {
                    codoridiscli = dr["codoriDiscli"].ToString(),
                    coddesdiscli = dr["coddesDiscli"].ToString(),
                    disdiscli = dr["disDiscli"].ToString(),
                    tiemdiscli = dr["tiemDiscli"].ToString()
                };

                lista.Add(Ruta);
            }
            cn.Close();
            return lista;
        }
        static DataTable ConvertListToDataTable(List<Distribucion.RutaCorta> list)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Distribucion.RutaCorta));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (Distribucion.RutaCorta item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public class RootObject
        {
            public string[] destination_addresses { get; set; }
            public string[] origin_addresses { get; set; }
            public Rows[] rows { get; set; }
            public string status { get; set; }
        }
        public class Rows
        {
            public Elements[] elements { get; set; }
        }
        public class Elements
        {
            public Duration duration { get; set; }
            public Distance distance { get; set; }
        }
        public class Duration
        {
            public string text { get; set; }
            public string value { get; set; }
        }

        public class Distance
        {
            public string text { get; set; }
            public string value { get; set; }
        }
    }
}
