using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class SalesOrderStatus
    {
        #region *****************************Variables**************************************

        SqlCommand cmd;
        Common com = new Common();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();

        #endregion

        #region *****************************Functions**************************************

        /// <summary>
        /// Function for get Sales Order Status.
        /// Created By: Satish
        /// Created Date: 26july 2013
        /// </summary>
        public string GetOrderStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(Parameterdetails);

                DateTime dtFrom = Common.UnixTimeStampToDateTime(com.STRToDBL(objdictionary["FromDate"].ToString()));
                DateTime dtTo = Common.UnixTimeStampToDateTime(com.STRToDBL(objdictionary["ToDate"].ToString()));

                string FromDate = dtFrom.ToString("MM/dd/yyyy");
                string ToDate = dtTo.ToString("MM/dd/yyyy");

                string UserId = objdictionary["UserId"].ToString();
                string CustomerGroupId = objdictionary["CustomerGroupId"].ToString();
                string CustomerAccountId = objdictionary["CustomerAccountId"].ToString();

                #region Fill Grid Records

                DataTable dtgridrecord = new DataTable();
                cmd = new SqlCommand();
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
                cmd.Parameters.Add("@CustomerGroupId", SqlDbType.VarChar).Value = CustomerGroupId;
                cmd.Parameters.Add("@CustomerAccountId", SqlDbType.VarChar).Value = CustomerAccountId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;

                cmd.CommandText = "SP_GetSalesOrderStatus";
                dtgridrecord = objdatacommon.GetDataTableWithProc(cmd);

                if (dtgridrecord.Rows.Count > 0)
                {
                    for (int i = 0; i < dtgridrecord.Rows.Count; i++)
                    {
                        DataTable dt1 = new DataTable();
                        cmd = new SqlCommand();
                        cmd.Parameters.Add("@ordno", SqlDbType.VarChar).Value = dtgridrecord.Rows[i]["OrdNo"].ToString();
                        cmd.Parameters.Add("@lnno", SqlDbType.VarChar).Value = dtgridrecord.Rows[i]["LnNo"].ToString();
                        cmd.CommandText = "SP_Sal_MPSQry";
                        dt1 = objdatacommon.GetDataTableWithProc(cmd);

                        dtgridrecord.Rows[i]["RdyQty"] = 0;
                        dtgridrecord.Rows[i]["DspQty"] = 0;
                        dtgridrecord.Rows[i]["DspNrl"] = 0;
                        dtgridrecord.Rows[i]["RdyNrl"] = 0;
                        dtgridrecord.Rows[i]["RsvQty"] = 0;
                        dtgridrecord.Rows[i]["InvoiceQty"] = 0;

                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            if (dt1.Rows[j]["trn_type"].ToString() == "D")
                            {
                                dtgridrecord.Rows[i]["DspNrl"] = (com.STRToInt(dtgridrecord.Rows[i]["DspNrl"].ToString()) + com.STRToInt(dt1.Rows[j]["Nrl"].ToString()));
                                dtgridrecord.Rows[i]["DspQty"] = (com.STRToDCML(dtgridrecord.Rows[i]["DspQty"].ToString()) + com.STRToDCML(dt1.Rows[j]["qty"].ToString()));
                            }
                            else
                            {
                                dtgridrecord.Rows[i]["RdyNrl"] = (Convert.ToInt16(dtgridrecord.Rows[i]["RdyNrl"].ToString()) + com.STRToInt(dt1.Rows[j]["Nrl"].ToString()));
                                dtgridrecord.Rows[i]["RdyQty"] = (com.STRToDCML(dtgridrecord.Rows[i]["RdyQty"].ToString()) + com.STRToDCML(dt1.Rows[j]["qty"].ToString()));
                                dtgridrecord.Rows[i]["RsvQty"] = (com.STRToDCML(dtgridrecord.Rows[i]["RsvQty"].ToString()) + com.STRToDCML(dt1.Rows[j]["RsvQty"].ToString()));
                                dtgridrecord.Rows[i]["InvoiceQty"] = (com.STRToDCML(dtgridrecord.Rows[i]["InvoiceQty"].ToString()) + com.STRToDCML(dt1.Rows[j]["RsvQty"].ToString()));
                            }
                        }
                        dtgridrecord.AcceptChanges();
                        dt1 = null;
                    }
                }

                #endregion

                List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                Dictionary<string, object> row1 = null;
                foreach (DataRow dr1 in dtgridrecord.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dtgridrecord.Columns)
                    {
                        row1.Add(col.ColumnName, dr1[col]);
                    }
                    rows1.Add(row1);
                }

                string SerializedTable = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                SerializedTable = serializer.Serialize(rows1).ToString();

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("OrderStatusTable", typeof(string));
                dt2.Columns.Add("RecordStatus", typeof(string));
                DataRow dr = dt2.NewRow();
                dr["OrderStatusTable"] = SerializedTable;
                if (dtgridrecord.Rows.Count > 0)
                {
                    dr["RecordStatus"] = "Exist";
                }
                else
                {
                    dr["RecordStatus"] = "NotExist";
                }
                dt2.Rows.Add(dr);
                dt2.AcceptChanges();
                dtgridrecord = null;

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr2 in dt2.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt2.Columns)
                    {
                        row.Add(col.ColumnName, dr2[col]);
                    }
                    rows.Add(row);
                }
                serializer = new JavaScriptSerializer();
                JsonStringForSerialized = serializer.Serialize(rows).ToString();
            }
            catch { }
            return JsonStringForSerialized;
        }

        /// <summary>
        /// Function for get Sales Order Details.
        /// Created By: Satish
        /// Created Date: 30 July 2013
        /// </summary>
        public string GetOrderDetailsInSOStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(Parameterdetails);

                string SONo = objdictionary["SONo"].ToString();
                string LineNo = objdictionary["LineNo"].ToString();
                string Type = objdictionary["Type"].ToString();

                #region Fill Grid Records

                DataTable dtgridrecord = new DataTable();
                cmd = new SqlCommand();
                cmd.Parameters.Add("@SONo", SqlDbType.VarChar).Value = SONo;
                cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = com.STRToInt(LineNo);
                cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = Type;

                cmd.CommandText = "SP_GetOrderDetailsInSOStatus";
                dtgridrecord = objdatacommon.GetDataTableWithProc(cmd);

                #endregion

                List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                Dictionary<string, object> row1 = null;
                foreach (DataRow dr1 in dtgridrecord.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dtgridrecord.Columns)
                    {
                        row1.Add(col.ColumnName, dr1[col]);
                    }
                    rows1.Add(row1);
                }

                string SerializedTable = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                SerializedTable = serializer.Serialize(rows1).ToString();

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("TableRecords", typeof(string));
                dt2.Columns.Add("RecordStatus", typeof(string));
                DataRow dr = dt2.NewRow();
                dr["TableRecords"] = SerializedTable;
                if (dtgridrecord.Rows.Count > 0)
                {
                    dr["RecordStatus"] = "Exist";
                }
                else
                {
                    dr["RecordStatus"] = "NotExist";
                }
                dt2.Rows.Add(dr);
                dt2.AcceptChanges();
                dtgridrecord = null;

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr2 in dt2.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt2.Columns)
                    {
                        row.Add(col.ColumnName, dr2[col]);
                    }
                    rows.Add(row);
                }
                serializer = new JavaScriptSerializer();
                JsonStringForSerialized = serializer.Serialize(rows).ToString();
            }
            catch { }
            return JsonStringForSerialized;
        }

        /// <summary>
        /// Function for get Sales Order Details Reason.
        /// Created By: Satish
        /// Created Date: 31 July 2013
        /// </summary>
        public string GetOrderDetailsReasonInSOStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(Parameterdetails);
                string SONo = objdictionary["SONo"].ToString();
                string CustomerGroupId = objdictionary["CustomerGroupId"].ToString();

                DataTable dt = new DataTable();
                cmd = new SqlCommand();
                cmd.CommandText = @"select UserName,EmailID from Com_User_Mst where UserID =(select top 1 A.UserId from tblCustomerUserMapping
                                    as A inner join Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId in (select UserID from
                                    Com_User_Mst where UserTypeId =9 and ActiveStatus =1) and A.CustomerGroupId ='" + CustomerGroupId + "')";
                cmd.CommandText += " and ActiveStatus =1";

                dt = objdatacommon.GetDataTableWithQuery(cmd);
                Dictionary<string, string> objdic = new Dictionary<string, string>();
                if (dt.Rows.Count > 0)
                {
                    objdic.Add("Status", "Please contact the project owner: " + dt.Rows[0]["UserName"].ToString() + ".His Email-Id is: " + dt.Rows[0]["EmailID"].ToString() + ".");
                }
                else
                {
                    objdic.Add("Status", "Please contact the project owner.");
                }

                List<Dictionary<string, string>> objdicinList = new List<Dictionary<string, string>>();
                objdicinList.Add(objdic);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                JsonStringForSerialized = serializer.Serialize(objdicinList).ToString();
                dt = null;
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion
    }
}
