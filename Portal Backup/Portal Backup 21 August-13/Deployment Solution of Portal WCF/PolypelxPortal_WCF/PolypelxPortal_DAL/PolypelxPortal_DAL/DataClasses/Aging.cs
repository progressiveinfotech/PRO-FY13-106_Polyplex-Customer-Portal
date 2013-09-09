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
    public class Aging
    {
        #region *****************************Variables**************************************

        SqlCommand cmd;
        Common com = new Common();
        private DataSet ds = new DataSet();
        SqlDataAdapter _Obj_SDA;
        SqlDataReader _ObjSdr;
        SqlConnection _Obj_Conn = new SqlConnection();
        PolypelxPortal_DAL.ConnectionClass.Connection con = new PolypelxPortal_DAL.ConnectionClass.Connection();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();

        #endregion

        #region *****************************Functions**************************************

        /// <summary>
        /// Function for Aging Report.
        /// Created By: Lalit
        /// Created Date: 31july 2013
        /// </summary>

        public string AgingReport(Stream Params)
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
                DataRow newrow = null;
                DataTable abc1 = null;
                decimal fabal, gTotal;
                string pk = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ds = new DataSet();
                _Obj_Conn.ConnectionString = con.ConnectionString;
                con.OpenConnection();
                cmd = new SqlCommand();
                cmd.Connection = _Obj_Conn;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @" Select a.Customer as [CustCode], b.Name as [CustName],case b.CreditLimit when '0.00' then b.EximLimit when 'Named' then b.EximLimit else 0.00
                                     end as SecuredCL,case b.CreditLimit when 'DCL' then b.EximLimit else 0.00 end as DCLCL, b.InternalLimit as InternalCL,
                                    (b.EximLimit + b.InternalLimit) as TotalCL, isnull(s.PaymentTermDesc,'') as PaymentTerms,s.CustomerOrderNo, a.PTLInv as [OurInvoice],
                                     CONVERT(nvarchar(50),a.InvDt,101) as [InvDate],(a.InvAmt - dbo.Recd_Amt_FromTo(a.PTLInv,'" + FromDate + "','" + ToDate + "')) as BalDue,";
                cmd.CommandText += @"CONVERT(nvarchar(50),a.DueDate,101) as [DueDate], p.Description as ProfitCenter,dateDiff(Day, a.DueDate, '" + ToDate + "') as DaysOld,Space(50) as [Status],";
                cmd.CommandText += @" Space(50) as Aging, b.CustomerId From Fa_BillOS_Trans a inner join Sal_Glb_Customer_Mst b on a.Customer = b.CustomerCode Inner Join 
                                     FA_Glb_ProfitCenter_Mst p on a.PCenter = p.ProfitCenterCode left Outer Join Sal_Glb_Invoice_Tran i on a.PTLInv = i.InvoiceNo Left Outer Join 
                                     View_SalesOrder s on i.SalesOrderId = s.SalesOrderId left join Com_CreditAgency CA on CA.autoid=b.CreditAgencyNo";
                cmd.CommandText += @" Where  a.InvDt  between '" + FromDate + "' AND '" + ToDate + "'";

                if (CustomerGroupId == "0" && CustId == "0")
                {
                    cmd.CommandText += " AND b.CustomerId in(SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.ActiveStatus =1))";
                }
                else if (CustomerGroupId != "0" && CustId == "0")
                {
                    cmd.CommandText += " AND b.CustomerId in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1))";
                }
                else
                {
                    cmd.CommandText += " AND b.CustomerId in(SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + CurrentUser + "' and A.CustomerId ='" + CustId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + CurrentUser + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1))";
                }
                _Obj_SDA = new SqlDataAdapter(cmd);

                _Obj_SDA.Fill(ds, "Rcv");
                DateTime Adt = Convert.ToDateTime(ToDate);
                abc1 = ds.Tables["Rcv"].Clone();
                int l2 = 0; 
                if (ds.Tables["Rcv"].Rows.Count > 0)
                {
                    while (l2 <= (ds.Tables["Rcv"].Rows.Count - 1))
                    {
                        pk = Convert.ToString(ds.Tables["Rcv"].Rows[l2]["CustCode"]).Trim();
                        decimal XNOTDUE = Convert.ToDecimal(ds.Tables["Rcv"].Rows[l2]["TotalCL"].ToString());
                        decimal BALDUE = 0; fabal = 0;
                        gTotal = 0;
                        while (l2 <= (ds.Tables["Rcv"].Rows.Count - 1) && ds.Tables["Rcv"].Rows[l2]["CustCode"].ToString().Trim() == pk)
                        {
                            DateTime Fdt = Convert.ToDateTime(ds.Tables["Rcv"].Rows[l2]["DueDate"]);
                            if (Fdt >= Adt || Convert.ToDecimal(ds.Tables["Rcv"].Rows[l2]["BalDue"].ToString()) < 0)
                            {
                                ds.Tables["Rcv"].Rows[l2]["Status"] = "Not Due";
                                if (Fdt.Date > Adt.Date && Fdt.Date <= Adt.Date.AddDays(7))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "Due in Next 7 Days";
                                }
                                if (Fdt.Date > Adt.Date.AddDays(7) && Fdt.Date <= Adt.Date.AddDays(15))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "Due in Next 15 Days";
                                }
                                if (Fdt.Date > Adt.Date.AddDays(15) && Fdt.Date <= Adt.Date.AddDays(30))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "Due in Next 30 Days";
                                }
                            }
                            else
                            {
                                ds.Tables["Rcv"].Rows[l2]["Status"] = "Overdue";
                                if (Fdt.Date < Adt.Date && Fdt.Date >= Adt.Date.AddDays(-7))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "00-07 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-7) && Fdt.Date >= Adt.Date.AddDays(-15))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "07-15 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-15) && Fdt.Date >= Adt.Date.AddDays(-30))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "15-30 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-30) && Fdt.Date >= Adt.Date.AddDays(-45))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "30-45 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-45) && Fdt.Date >= Adt.Date.AddDays(-55))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "45-55 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-55) && Fdt.Date >= Adt.Date.AddDays(-60))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "55-60 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-60) && Fdt.Date >= Adt.Date.AddDays(-90))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "60-90 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-90) && Fdt.Date >= Adt.Date.AddDays(-180))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "90-180 Days";
                                }
                                if (Fdt.Date < Adt.Date.AddDays(-180))
                                {
                                    ds.Tables["Rcv"].Rows[l2]["Aging"] = "Above 180 Days";
                                }
                            }
                            newrow = abc1.NewRow();
                            newrow["CustCode"] = pk;
                            newrow["CustName"] = ds.Tables["Rcv"].Rows[l2]["CustName"].ToString();
                            newrow["SecuredCL"] = ds.Tables["Rcv"].Rows[l2]["SecuredCL"].ToString();

                            newrow["DCLCL"] = ds.Tables["Rcv"].Rows[l2]["DCLCL"].ToString();
                            newrow["InternalCL"] = ds.Tables["Rcv"].Rows[l2]["InternalCL"].ToString();
                            newrow["TotalCL"] = ds.Tables["Rcv"].Rows[l2]["TotalCL"].ToString();

                            newrow["PaymentTerms"] = ds.Tables["Rcv"].Rows[l2]["PaymentTerms"].ToString();
                            newrow["OurInvoice"] = ds.Tables["Rcv"].Rows[l2]["OurInvoice"].ToString();
                            newrow["InvDate"] = ds.Tables["Rcv"].Rows[l2]["InvDate"].ToString();
                            newrow["BalDue"] = ds.Tables["Rcv"].Rows[l2]["BalDue"].ToString();
                            newrow["DueDate"] = ds.Tables["Rcv"].Rows[l2]["DueDate"].ToString();
                            newrow["ProfitCenter"] = ds.Tables["Rcv"].Rows[l2]["ProfitCenter"].ToString();
                            newrow["DaysOld"] = ds.Tables["Rcv"].Rows[l2]["DaysOld"].ToString();
                            newrow["Status"] = ds.Tables["Rcv"].Rows[l2]["Status"].ToString();
                            newrow["Aging"] = ds.Tables["Rcv"].Rows[l2]["Aging"].ToString();
                            newrow["CustomerOrderNo"] = ds.Tables["Rcv"].Rows[l2]["CustomerOrderNo"].ToString();
                            BALDUE += Convert.ToDecimal(ds.Tables["Rcv"].Rows[l2]["BalDue"].ToString());
                            abc1.Rows.Add(newrow);
                            l2 += 1;
                         
                        }
                        gTotal += BALDUE;
                        fabal = 0;
                        cmd.Connection = _Obj_Conn;
                        cmd.Connection.Open();
                        cmd.CommandText = @"Select dbo.pa_slacopg('240000','" + pk + "','" + ToDate + "', 0) as FaClg";
                        _ObjSdr = cmd.ExecuteReader(CommandBehavior.SingleResult);

                        if (_ObjSdr.Read())
                        {
                            fabal = com.STRToDCML(_ObjSdr["FaClg"].ToString());
                        }
                        _ObjSdr.Close();
                        cmd.Connection.Close();
                        newrow = abc1.NewRow();
                        abc1.Rows.Add(newrow);
                        newrow = abc1.NewRow();
                        newrow["OurInvoice"] = "Total :";
                        if (fabal != BALDUE)
                        {
                            newrow["DCLCL"] = fabal.ToString();
                        }
                        newrow["TotalCL"] = (XNOTDUE - BALDUE).ToString();
                        newrow["BalDue"] = BALDUE.ToString();
                        abc1.Rows.Add(newrow);
                        newrow = abc1.NewRow();
                        abc1.Rows.Add(newrow);

                        //newrow = abc1.NewRow();
                        //abc1.Rows.Add(newrow);
                        newrow = abc1.NewRow();
                        newrow["OurInvoice"] = "Grand Total:";
                        newrow["BalDue"] = gTotal.ToString();
                        abc1.Rows.Add(newrow);
                        newrow = abc1.NewRow();
                        abc1.Rows.Add(newrow);
                        ds.Tables["Rcv"].AcceptChanges();

                    }       
                    con.CloseConnection();
                    con.DisposeConnection();
                    _Obj_SDA.Dispose();
                    cmd.Dispose();
                }
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();                
                if (abc1.Rows.Count > 0)
                {
                    Dictionary<string, object> row = null;
                    foreach (DataRow dr in abc1.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in abc1.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                }
                ds.Clear();
                ds.Dispose();
                serializer.MaxJsonLength = Int32.MaxValue;
                string SerializedTable = "";
                serializer = new JavaScriptSerializer();
                SerializedTable = serializer.Serialize(rows).ToString();
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("AgingTable", typeof(string));
                dt2.Columns.Add("RecordStatus", typeof(string));
                DataRow dr1 = dt2.NewRow();
                dr1["AgingTable"] = SerializedTable;
                if (abc1.Rows.Count > 0)
                {
                    dr1["RecordStatus"] = "Exist";
                }
                else
                {
                    dr1["RecordStatus"] = "NotExist";
                }
                dt2.Rows.Add(dr1);
                dt2.AcceptChanges();
                abc1 = null;
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
            catch (Exception ex)
            {
                JsonStringForSerialized = "error";
            }
            return JsonStringForSerialized;
        }

        #endregion
    }
}
