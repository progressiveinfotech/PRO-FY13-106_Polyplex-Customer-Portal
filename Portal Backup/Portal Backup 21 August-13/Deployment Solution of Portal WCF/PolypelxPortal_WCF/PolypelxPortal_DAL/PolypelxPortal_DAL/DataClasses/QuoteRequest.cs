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
using Newtonsoft.Json;
using System.Xml;

namespace PolypelxPortal_DAL.DataClasses
{
    public class QuoteRequest
    {
        #region *****************************Variables**************************************
        SqlCommand cmd;
        Common com = new Common();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();
        public DataTable dtQuoteRequestLineItems;
        #endregion

        #region *****************************Functions**************************************

        /// <summary>
        /// Created By: Satish
        /// Created Date: 2 August 2013
        /// Function for get Quote Request Unallocated Stock and Status details depend upon the user login.
        /// </summary>
        public string GetQuoteUnallocatedAndStatusDetails(Stream Parameterdetails)
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
                string PlantId = objdictionary["PlantId"].ToString();
                string WareHouseId = objdictionary["WareHouseId"].ToString();

                #region Fill Unallocated Stock Grid Records

                DataTable dtUnallStkRecord = new DataTable();
                cmd = new SqlCommand();
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
                cmd.Parameters.Add("@CustomerGroupId", SqlDbType.VarChar).Value = CustomerGroupId;
                cmd.Parameters.Add("@CustomerAccountId", SqlDbType.VarChar).Value = CustomerAccountId;
                cmd.Parameters.Add("@PlantId", SqlDbType.VarChar).Value = PlantId;
                cmd.Parameters.Add("@WareHouseId", SqlDbType.VarChar).Value = WareHouseId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
                
                cmd.CommandText = "SP_GetSORequestUnallocatedStock";



                dtUnallStkRecord = objdatacommon.GetDataTableWithProc(cmd);
                

                List<Dictionary<string, object>> ObjListDicUnallStk = new List<Dictionary<string, object>>();
                if (dtUnallStkRecord.Rows.Count > 0)
                {
                    Dictionary<string, object> objDicUnallStk = null;
                    foreach (DataRow dr1 in dtUnallStkRecord.Rows)
                    {
                        objDicUnallStk = new Dictionary<string, object>();
                        foreach (DataColumn col in dtUnallStkRecord.Columns)
                        {
                            objDicUnallStk.Add(col.ColumnName, dr1[col]);
                        }
                        ObjListDicUnallStk.Add(objDicUnallStk);
                    }
                }

                string tblInStrOfUnallStk = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                tblInStrOfUnallStk = serializer.Serialize(ObjListDicUnallStk).ToString();
                
                #endregion

                #region Fill Quote Status Header Details.

                DataTable dtQuoteStaHeadRecord = new DataTable();
                cmd = new SqlCommand();
                cmd.Parameters.Add("@CustomerGroupId", SqlDbType.VarChar).Value = CustomerGroupId;
                cmd.Parameters.Add("@CustomerAccountId", SqlDbType.VarChar).Value = CustomerAccountId;
                cmd.Parameters.Add("@WareHouseId", SqlDbType.VarChar).Value = WareHouseId;

                cmd.CommandText = "SP_GetQuoteRequestAllRecords";
                dtQuoteStaHeadRecord = objdatacommon.GetDataTableWithProc(cmd);
              
                List<Dictionary<string, object>> ObjListDicQuoteStaHead = new List<Dictionary<string, object>>();
                if (dtQuoteStaHeadRecord.Rows.Count > 0)
                {
                    Dictionary<string, object> objDicQuoteStaHead = null;
                    foreach (DataRow dr1 in dtQuoteStaHeadRecord.Rows)
                    {
                        objDicQuoteStaHead = new Dictionary<string, object>();
                        foreach (DataColumn col in dtQuoteStaHeadRecord.Columns)
                        {
                            objDicQuoteStaHead.Add(col.ColumnName, dr1[col]);
                        }
                        ObjListDicQuoteStaHead.Add(objDicQuoteStaHead);
                    }
                }

                string tblInStrOfQuoteStaHead = "";
                serializer = new JavaScriptSerializer();
                tblInStrOfQuoteStaHead = serializer.Serialize(ObjListDicQuoteStaHead).ToString();

                #endregion                

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("tblInStrOfUnallStk", typeof(string));
                dt2.Columns.Add("RecordStatusUnallStk", typeof(string));
                dt2.Columns.Add("tblInStrOfQuoteStaHead", typeof(string));
                dt2.Columns.Add("RecordStatusQuoteStaHead", typeof(string));
                DataRow dr = dt2.NewRow();
                dr["tblInStrOfUnallStk"] = tblInStrOfUnallStk;
                if (dtUnallStkRecord.Rows.Count > 0)
                {
                    dr["RecordStatusUnallStk"] = "Exist";
                }
                else
                {
                    dr["RecordStatusUnallStk"] = "NotExist";
                }
                dr["tblInStrOfQuoteStaHead"] = tblInStrOfQuoteStaHead;
                if (dtQuoteStaHeadRecord.Rows.Count > 0)
                {
                    dr["RecordStatusQuoteStaHead"] = "Exist";
                }
                else
                {
                    dr["RecordStatusQuoteStaHead"] = "NotExist";
                }
                dt2.Rows.Add(dr);
                dt2.AcceptChanges();
                dtUnallStkRecord = null;
                dtQuoteStaHeadRecord = null;

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
            catch(Exception ex) { }
            return JsonStringForSerialized;
        }

        /// <summary>
        /// Created By: Satish
        /// Created Date: 3 August 2013
        /// Function for get Quote Status LineItem Details depend upon the selected quote no.
        /// </summary>
        public string GetQuoteStatusLineItemDetails(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(Parameterdetails);
                
                string QuoteHeadAutoId = objdictionary["QuoteHeadAutoId"].ToString();
                string DetailType = objdictionary["DetailType"].ToString();//ForShow,ForSave

                #region Fill Quote Status LineItem Details.

                DataTable dtQuoteStaDetailRecord = new DataTable();
                cmd = new SqlCommand();
                cmd.Parameters.Add("@QuoteHeadAutoId", SqlDbType.VarChar).Value = QuoteHeadAutoId;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = DetailType;

                cmd.CommandText = "SP_GetQuoteRequestDetails";
                dtQuoteStaDetailRecord = objdatacommon.GetDataTableWithProc(cmd);

                List<Dictionary<string, object>> ObjListDicQuoteStaDetail = new List<Dictionary<string, object>>();
                if (dtQuoteStaDetailRecord.Rows.Count > 0)
                {
                    Dictionary<string, object> objDicQuoteStaDetail = null;
                    foreach (DataRow dr1 in dtQuoteStaDetailRecord.Rows)
                    {
                        objDicQuoteStaDetail = new Dictionary<string, object>();
                        foreach (DataColumn col in dtQuoteStaDetailRecord.Columns)
                        {
                            objDicQuoteStaDetail.Add(col.ColumnName, dr1[col]);
                        }
                        ObjListDicQuoteStaDetail.Add(objDicQuoteStaDetail);
                    }
                }

                string tblInStrOfQuoteStaDetail = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                tblInStrOfQuoteStaDetail = serializer.Serialize(ObjListDicQuoteStaDetail).ToString();

                #endregion

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("tblInStrOfQuoteStaDetail", typeof(string));
                dt2.Columns.Add("RecordStatusQuoteStaDetail", typeof(string));
                DataRow dr = dt2.NewRow();
                
                dr["tblInStrOfQuoteStaDetail"] = tblInStrOfQuoteStaDetail;
                if (dtQuoteStaDetailRecord.Rows.Count > 0)
                {
                    dr["RecordStatusQuoteStaDetail"] = "Exist";
                }
                else
                {
                    dr["RecordStatusQuoteStaDetail"] = "NotExist";
                }
                dt2.Rows.Add(dr);
                dt2.AcceptChanges();
                dtQuoteStaDetailRecord = null;

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
            catch (Exception ex) { }
            return JsonStringForSerialized;
        }

        /// <summary>
        /// Created By: Lalit
        /// Created Date: 14 August 2013
        /// Function to insert Quote Request.
        /// </summary>
        public string InsertUpdateQuoteRequest(Stream ParameterDetails)
        {
            string Message="";
            try
            {
                StreamReader Str = new StreamReader(ParameterDetails);
                string Param = Str.ReadToEnd();
                Str.Close();
                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(Param);
                DataTable DtHeaderDetails = dataSet.Tables["HeaderDetails"];
                if (DtHeaderDetails != null && DtHeaderDetails.Rows.Count > 0)
                {                   
                    string AutoId, CustomerGroup, CustomerAccNo, WareHouseId, OrderStatus, Remarks, CreatedBy, ModifiedBy;
                    AutoId = DtHeaderDetails.Rows[0]["AutoId"].ToString();
                    CustomerGroup = DtHeaderDetails.Rows[0]["CustomerGroup"].ToString();
                    CustomerAccNo = DtHeaderDetails.Rows[0]["CustomerAccNo"].ToString();
                    WareHouseId = DtHeaderDetails.Rows[0]["WareHouseId"].ToString();
                    OrderStatus = DtHeaderDetails.Rows[0]["OrderStatus"].ToString();
                    Remarks = DtHeaderDetails.Rows[0]["Remarks"].ToString();
                    CreatedBy = DtHeaderDetails.Rows[0]["CreatedBy"].ToString();
                    ModifiedBy = DtHeaderDetails.Rows[0]["ModifiedBy"].ToString();
                    
                    DataTable DtLineDetails = dataSet.Tables["LineItemDetails"];
                    
                    if (DtLineDetails != null && DtLineDetails.Rows.Count > 0)
                    {
                        cmd = new SqlCommand();
                        cmd.Parameters.Add("@AutoId", SqlDbType.Int).Value = com.STRToInt(AutoId);
                        cmd.Parameters.Add("@CustomerGroup", SqlDbType.Int).Value = com.STRToInt(CustomerGroup);
                        cmd.Parameters.Add("@CustomerAccNo", SqlDbType.Int).Value = com.STRToInt(CustomerAccNo);
                        cmd.Parameters.Add("@WareHouseId", SqlDbType.Int).Value = com.STRToInt(WareHouseId);
                        cmd.Parameters.Add("@OrderStatus", SqlDbType.VarChar).Value = OrderStatus;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = Remarks;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = com.STRToInt(CreatedBy);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = com.STRToInt(ModifiedBy);
                        cmd.Parameters.AddWithValue("@dtLineitems", DtLineDetails);
                        cmd.Parameters.Add(new SqlParameter("@ErrorStatus", SqlDbType.VarChar, 10));
                        cmd.Parameters["@ErrorStatus"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@NewQuoteNo", SqlDbType.VarChar, 20));
                        cmd.Parameters["@NewQuoteNo"].Direction = ParameterDirection.Output;
                       
                        cmd.CommandText = "SP_InsertAndUpdate_In_QuoteRequest";
                        cmd = objdatacommon.ExecuteSqlProcedure(cmd);
                        string ErrorStatus = cmd.Parameters["@ErrorStatus"].Value.ToString();
                        string NewQuoteNo = cmd.Parameters["@NewQuoteNo"].Value.ToString();
                        
                        if (ErrorStatus == "0")
                        {
                            Message = "Saved";
                        }
                        else
                        {
                            Message = "Not Saved";
                        }
                    }
                    else
                    {
                        Message = "No LineItem Data recieved from Portal";
                    }
                }
                else
                {
                    Message = "No Header Data recieved from Portal";
                }
            }
            catch (Exception ex) {  }
            return Message;
        }

        /// <summary>
        /// Created By: Lalit
        /// Created Date: 19 August 2013
        /// Function to delete Quote Request.
        /// </summary>
        public string DeleteQuoteRequest(Stream ParameterDetails)
        {
            string Message = "";
            cmd = new SqlCommand();
            try
            {
                Dictionary<string, object> objdictionary = JsonHelper.DeserializeJsonStream<Dictionary<string, object>>(ParameterDetails);
                string QuoteHeadAutoId = objdictionary["QuoteHeadAutoId"].ToString();
                cmd.CommandText = "Update tbl_QuoteRequestHeader set ActiveStatus=0 where AutoId='" + QuoteHeadAutoId + "'";
                cmd = objdatacommon.ExecuteSqlProcedure(cmd);
                Message = "Saved";                
            }
            catch (Exception ex)
            {
                Message = "Not Saved";
                
            }
            return Message;
        } 

        #endregion
    }
}
