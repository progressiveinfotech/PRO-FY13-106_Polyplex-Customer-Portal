using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Specialized;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using PolypelxPortal_BAL.BasicClasses;
using PolypelxPortal_BAL.PortalClasses;

namespace PolypelxPortal_DAL.DataClasses
{
    public class StockStatus
    {
        #region *****************************Variables**************************************

        SqlCommand cmd;
        Common com = new Common();
        private DataSet ds = new DataSet();
        SqlDataAdapter _Obj_SDA;
        SqlConnection _Obj_Conn = new SqlConnection();
        PolypelxPortal_DAL.ConnectionClass.Connection con = new PolypelxPortal_DAL.ConnectionClass.Connection();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();

        #endregion

        #region *****************************Functions**************************************

        /// <summary>
        /// Function for Stock Status.
        /// Created By: Lalit
        /// Created Date: 30july 2013
        /// </summary>
        public string GetStockStatus(Stream Params)
        {
            string JsonStringForSerialized = "";
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(Params);
                DateTime DtFrom = Common.UnixTimeStampToDateTime(com.STRToDBL(objdictionary["FromDate"].ToString()));
                DateTime DtTo = Common.UnixTimeStampToDateTime(com.STRToDBL(objdictionary["ToDate"].ToString()));

                string FromDate = DtFrom.ToString("MM/dd/yyyy");
                string ToDate = DtTo.ToString("MM/dd/yyyy");
                string CurrentUser = objdictionary["UserId"].ToString();
                string CustomerGroupId = objdictionary["CustomerGroupId"].ToString(); ;
                string CustId = objdictionary["CustomerAccountId"].ToString();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ds = new DataSet();
                _Obj_Conn.ConnectionString = con.ConnectionString;
                con.OpenConnection();
                cmd = new SqlCommand();
                cmd.Connection = _Obj_Conn;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"Select distinct b.SONo as [Order],replace(CONVERT(VARCHAR(11),SODate ,101),' ','/') AS [Date]
                                    ,a.[LineNo] as [LineNo], cm.CustomerCode as AccountNo, f.FilmTypeCode as [Type],a.Thickness, a.socore as Core, a.SOWidthInInch as WidthInch,
                                     a.SOWidthInMM as WidthMm,CAST(SOLengthInMtr as int) as LengthMtr, CAST(solengthinft as int)as LengthFt,SOReqQuantityInKG as Qtykg,
                                     SOReqQuantityInLBS as QtyLbs,sonoofrolls as [RequiredRolls],Obj_St.Location as Warehouse From Sal_Glb_OrderLineItem a 
                                     Inner join Sal_Glb_OrderInformations b on (a.SalesOrderId =b.SalesOrderId)
                                     Inner join Sal_Glb_Customer_Mst as cm on b.SOCustomer = cm.CustomerID 
                                     Inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
                                     right outer join Prod_RollSlitting_Details Obj_Prod on Obj_Prod.SalesOrdNo=b.SONo
                                     Inner join Prod_StorageLocation_Mst as Obj_St on Obj_St.StorageLocCode=Obj_Prod.Whouse
                                     where a.ActiveStatus = 1 and b.ActiveStatus = 1 and Obj_Prod.SalesOrdLineItemNo=a.[LineNo]";
                cmd.CommandText += @" and b.SODate between '" + DtFrom + "' AND '" + DtTo + "'";

                if (CustomerGroupId == "0" && CustId == "0")
                {
                    cmd.CommandText += " AND cm.CustomerCode in(Select CustomerCode from Sal_Glb_Customer_Mst where CustomerID in(SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.ActiveStatus =1)))";
                }
                else if (CustomerGroupId != "0" && CustId == "0")
                {
                    cmd.CommandText += " AND cm.CustomerCode in(Select CustomerCode from Sal_Glb_Customer_Mst where CustomerID in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1)))";
                }
                else
                {
                    cmd.CommandText += " AND cm.CustomerCode in(Select CustomerCode from Sal_Glb_Customer_Mst where CustomerID in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "' and A.CustomerId ='" + CustId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1)))";
                }

                _Obj_SDA = new SqlDataAdapter(cmd);
                _Obj_SDA.Fill(ds);
                con.CloseConnection();
                con.DisposeConnection();
                _Obj_SDA.Dispose();
                cmd.Dispose();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                ds.Clear();
                ds.Dispose();
                serializer.MaxJsonLength = Int32.MaxValue;
                
                string SerializedTable = "";
                serializer = new JavaScriptSerializer();
                SerializedTable = serializer.Serialize(rows).ToString();

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("StockTable", typeof(string));
                dt2.Columns.Add("RecordStatus", typeof(string));
                DataRow dr1 = dt2.NewRow();
                dr1["StockTable"] = SerializedTable;
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dr1["RecordStatus"] = "Exist";
                }
                else
                {
                    dr1["RecordStatus"] = "NotExist";
                }
                dt2.Rows.Add(dr1);
                dt2.AcceptChanges();
                List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                Dictionary<string, object> row1 = null;
                foreach (DataRow dr2 in dt2.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt2.Columns)
                    {
                        row1.Add(col.ColumnName, dr2[col]);
                    }
                    rows1.Add(row1);
                }
                serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                JsonStringForSerialized = serializer.Serialize(rows1).ToString();
                
            }
            catch (Exception ex) {  };
            return JsonStringForSerialized;
        }

        #endregion
    }
}
