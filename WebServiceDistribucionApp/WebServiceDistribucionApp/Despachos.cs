using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceDistribucionApp
{
    public class Despachos
    {
        public class Elementos
        {

            public string Company { get; set; }
            public string SalesRepCode { get; set; }
            public string Date { get; set; }
            //public string Day { get; set; }
            public string CustNum { get; set; }
            public string ShiptoNum { get; set; }
            public string SalesRepName { get; set; }
            public string OrderDispatch { get; set; }
            public string CustId { get; set; }
            public string Name { get; set; }
            public string Address_c { get; set; }
            public string PhoneNum { get; set; }
           // public string ReservePriorityCode { get; set; }
            //public string CreditLimit { get; set; }
            //public string Description { get; set; }
            public string Latitude_c { get; set; }
            public string Longitude_c { get; set; }
            public string FechaDespacho { get; set; }
            //public string TerritoryCode_c { get; set; }
            public string chkEntregado_c { get; set; }
            public string chkAnulado_c { get; set; }
            public string chkReprogramado_c { get; set; }
            public string chkPendiente_c { get; set; }
            //public string invoicedate { get; set; }
            //public string Monto { get; set; }
            public string Ubigeo { get; set; }
            public string State { get; set; }
            public string LegalNumber{ get; set; }
            public string PackNum { get; set; }



            public Elementos()
            {
             Company = "";
             SalesRepCode="";
             Date="";
             //Day="";
             CustNum="";
             ShiptoNum="";
             SalesRepName="";
             OrderDispatch="";
             CustId="";
             Name="";
             Address_c="";
             PhoneNum="";
            // ReservePriorityCode="";
            // CreditLimit="";
            // Description="";
             Latitude_c="";
             Longitude_c="";
             FechaDespacho="";
            // TerritoryCode_c="";
             chkEntregado_c="";
             chkAnulado_c="";
             chkReprogramado_c="";
             chkPendiente_c="";
             //invoicedate="";
             // Monto = "";
             Ubigeo = "";
             State = "";
             LegalNumber = "";
             PackNum = "";


            }

        public Elementos
            ( string Company_2,
             string SalesRepCode_2,
             string Date_2,
             //string Day_2,
             string CustNum_2,
             string ShiptoNum_2,
             string SalesRepName_2,
             string OrderDispatch_2,
             string CustId_2,
             string Name_2,
             string Address_c_2,
             string PhoneNum_2,
             //string ReservePriorityCode_2,
             //string CreditLimit_2,
             string Description_2,
             string Latitude_c_2,
             string Longitude_c_2,
             string FechaDespacho_2,
             //string TerritoryCode_c_2,
             string chkEntregado_c_2,
             string chkAnulado_c_2,
             string chkReprogramado_c_2,
             string chkPendiente_c_2,
             //string invoicedate_2,
             //string Monto_2
             string Ubigeo_2,
             string State_2,
             string LegalNumber_2,
             string PackNum_2
             
             )
            {
                Company = Company_2;
                SalesRepCode = SalesRepCode_2;
                Date = Date_2;
                //Day = Day_2;
                CustNum = CustNum_2;
                ShiptoNum = ShiptoNum_2;
                SalesRepName = SalesRepName_2;
                OrderDispatch = OrderDispatch_2;
                CustId = CustId_2;
                Name = Name_2;
                Address_c = Address_c_2;
                PhoneNum = PhoneNum_2;
                //ReservePriorityCode = ReservePriorityCode_2;
                //CreditLimit = CreditLimit_2;
                //Description = Description_2;
                Latitude_c = Latitude_c_2;
                Longitude_c = Longitude_c_2;
                FechaDespacho = FechaDespacho_2;
                //TerritoryCode_c = TerritoryCode_c_2;
                chkEntregado_c = chkEntregado_c_2;
                chkAnulado_c = chkAnulado_c_2;
                chkReprogramado_c = chkReprogramado_c_2;
                chkPendiente_c = chkPendiente_c_2;
                //invoicedate = invoicedate_2;
                //Monto = Monto_2;
                Ubigeo = Ubigeo_2;
                State = State_2;
                LegalNumber = LegalNumber_2;
                PackNum = PackNum_2;
            }
        }
    }
}