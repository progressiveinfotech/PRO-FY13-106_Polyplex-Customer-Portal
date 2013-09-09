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
    public class AccountStatement
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
        /// Function for Account Statement Access Deshboard.
        /// Created By: Lalit
        /// Created Date: 30july 2013
        /// </summary>
        public string GetAccountStatement(Stream SearchParams)
        {
            string JsonStringForSerialized = "";
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(SearchParams);
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


                cmd.CommandText = @"Select distinct Obj_D.GLCode as DocumentNo,Convert(nvarchar(50),Obj_H.VoucherDate,101) as DocumentDate,Obj_H.VoucherNumber,
                                    Obj_SL.SLDescr as Narration,Obj_D.DebitAmount,
                                  (Select CreditAmount from FA_Glb_JournalVoucher_Details_trans where VoucherNo=Obj_H.VoucherNumber and [LineNo]=10) as [CreditAmount],
                                  Obj_D.ChequeNo,Obj_D.ChequeDate as [Date] from Sal_Glb_Invoice_Tran as Obj_Inv inner join FA_Glb_JournalVoucher_Header_trans as Obj_H on Obj_Inv.InvoiceNo=Obj_H.VoucherNumber
                                  Inner join FA_Glb_JournalVoucher_Details_trans Obj_D on Obj_H.VoucherNumber=Obj_D.VoucherNo
                                  Inner Join FA_Glb_GLMaster_Mst Obj_GL on Obj_GL.GeneralLedgerCode=Obj_D.GLCode
                                  inner join TblDebtAdj Obj_DebAdj on Obj_DebAdj.InvoiceNo=Obj_H.VoucherNumber
                                  Inner Join FA_GLB_TBLSubGL Obj_SL on Obj_SL.SubCode=Obj_D.SubGLCode  where Obj_H.Active=1  ";
                cmd.CommandText += @" and Obj_Inv.CreatedOn between '" + DtFrom + "' AND '" + DtTo + "'";

                if (CustomerGroupId == "0" && CustId == "0")
                {
                    cmd.CommandText += " AND Obj_Inv.CustomerId in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.ActiveStatus =1))";
                }
                else if (CustomerGroupId != "0" && CustId == "0")
                {
                    cmd.CommandText += " AND Obj_Inv.CustomerId in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1))";
                }
                else
                {
                    cmd.CommandText += " AND Obj_Inv.CustomerId in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "' and A.CustomerId ='" + CustId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1))";
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
                
               // return serializer.Serialize(rows);
                string SerializedTable = "";
                serializer = new JavaScriptSerializer();
                SerializedTable = serializer.Serialize(rows).ToString();

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("AccountSummeryTable", typeof(string));
                dt2.Columns.Add("RecordStatus", typeof(string));
                DataRow dr1 = dt2.NewRow();
                dr1["AccountSummeryTable"] = SerializedTable;
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
            catch (Exception ex) { };
            return JsonStringForSerialized;
        }

        #endregion

    }
}
