using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceDistribucionApp
{
    public class Despacho
    {
        public class Elementos
        {

            public string Salesrepcode { get; set; }
            public string Shiptonum{ get; set; }
            public string Dateshipping { get; set; }
            public string Custid { get; set; }
            public string CustNum { get; set; }
            public string Customername { get; set; }
            public string Address { get; set; }
            public string Ubigeo { get; set; }
            public string LegalNumber { get; set; }
            public string PackNum { get; set; }
            public string State { get; set; }
            public string Latitud { get; set; }
            public string Longitud { get; set; }
            public string OrderShipping { get; set; }



            public Elementos()
            {
                Salesrepcode = "";
                Shiptonum = "";
                Dateshipping = "";
                Custid = "";
                CustNum = "";
                Customername = "";
                Address = "";
                Ubigeo = "";
                LegalNumber = "";
                PackNum = "";
                State = "";
                Latitud = "";
                Longitud = "";
                OrderShipping = "";
            }

            public Elementos
            (string salesrepcode, string shiptonum, string dateshipping, string custid,
             string custnum,string customername,string address, string ubigeo,
             string legalnumber, string packnum, string state, string latitud, 
             string longitud,string ordershipping)
            {
                Salesrepcode = salesrepcode;
                Shiptonum = shiptonum;
                Dateshipping = dateshipping;
                Custid = custid;
                CustNum = custnum;
                Customername = customername;
                Address = address;
                Ubigeo = ubigeo;
                LegalNumber = legalnumber;
                PackNum = packnum;
                State = state;
                Latitud = latitud;
                Longitud = longitud;
                OrderShipping = ordershipping;
            }
        }
    }
}