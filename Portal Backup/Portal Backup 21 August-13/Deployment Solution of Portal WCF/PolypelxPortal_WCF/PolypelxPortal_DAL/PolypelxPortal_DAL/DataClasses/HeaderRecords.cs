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
    public class HeaderRecords
    {
        #region *****************************Variables**************************************

        SqlCommand cmd;
        Common com = new Common();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();

        #endregion

        #region *****************************Functions**************************************

        /// <summary>
        /// Created By: Satish
        /// Created Date: 25 July 2013
        /// Function for get header customer group,customer account and warehouse location depend upon the user login.
        /// </summary>
        public string GetHeaderRecord(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                List<CustomerGroup> objListCustomerGroup = new List<CustomerGroup>();
                List<CustomerAccount> objListCustomerAccount = new List<CustomerAccount>();
                List<WarehouseLocation> objListWarehouseLocation = new List<WarehouseLocation>();

                StreamReader objStreamReader = new StreamReader(Parameterdetails);
                string JsonStringForDeSerialized = objStreamReader.ReadToEnd();
                objStreamReader.Dispose();

                string UserId = JsonHelper.JsonDeserialize<String>(JsonStringForDeSerialized);

                #region Fill Customer Group

                DataTable dtcustgrp = new DataTable();
                cmd = new SqlCommand();
                cmd.CommandText = @"SELECT distinct A.CustomerGroupId,B.UserName FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on
                                A.CustomerGroupId =B.UserID where A.UserId ='" + UserId + "' and A.ActiveStatus =1";

                dtcustgrp = objdatacommon.GetDataTableWithQuery(cmd);

                if (dtcustgrp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcustgrp.Rows.Count; i++)
                    {
                        CustomerGroup objCustomerGroup = new CustomerGroup();
                        objCustomerGroup.CustomerGroupId = dtcustgrp.Rows[i]["CustomerGroupId"].ToString();
                        objCustomerGroup.UserName = dtcustgrp.Rows[i]["UserName"].ToString();
                        objListCustomerGroup.Add(objCustomerGroup);
                    }
                }

                #endregion

                #region Fill Customer Account

                DataTable dtcustacc = new DataTable();
                cmd = new SqlCommand();
                cmd.CommandText = @"SELECT distinct A.CustomerId,B.Name,A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Sal_Glb_Customer_Mst as B on
                                A.CustomerId =B.CustomerID where A.UserId ='" + UserId + "' and A.ActiveStatus =1";

                dtcustacc = objdatacommon.GetDataTableWithQuery(cmd);
                if (dtcustacc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcustacc.Rows.Count; i++)
                    {
                        CustomerAccount objCustomerAccount = new CustomerAccount();
                        objCustomerAccount.CustomerId = dtcustacc.Rows[i]["CustomerId"].ToString();
                        objCustomerAccount.CustomerName = dtcustacc.Rows[i]["Name"].ToString();
                        objCustomerAccount.CustomerGroupId = dtcustacc.Rows[i]["CustomerGroupId"].ToString();
                        objListCustomerAccount.Add(objCustomerAccount);
                    }
                }

                #endregion

                #region Fill WareHouse Location

                DataTable dtwareloca = new DataTable();
                cmd = new SqlCommand();
                cmd.CommandText = @"SELECT distinct A.WareHouseId,B.Location,A.CustomerId FROM tblWarehouseMapping AS A inner join
                                    Prod_StorageLocation_Mst as B on A.WareHouseId =B.autoid where A.CustomerId in (SELECT A.CustomerId
                                    FROM tblCustomerUserMapping AS A inner join Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID
                                    where A.UserId ='" + UserId + "' and A.ActiveStatus =1) and A.ActiveStatus =1";

                dtwareloca = objdatacommon.GetDataTableWithQuery(cmd);

                if (dtwareloca.Rows.Count > 0)
                {
                    for (int i = 0; i < dtwareloca.Rows.Count; i++)
                    {
                        WarehouseLocation objWarehouseLocation = new WarehouseLocation();
                        objWarehouseLocation.WareHouseId = dtwareloca.Rows[i]["WareHouseId"].ToString();
                        objWarehouseLocation.WareHouseName = dtwareloca.Rows[i]["Location"].ToString();
                        objWarehouseLocation.CustomerId = dtwareloca.Rows[i]["CustomerId"].ToString();
                        objListWarehouseLocation.Add(objWarehouseLocation);
                    }
                }

                #endregion

                string JsonStringcustgrp = JsonHelper.JsonSerializer<List<CustomerGroup>>(objListCustomerGroup);
                string JsonStringcustacc = JsonHelper.JsonSerializer<List<CustomerAccount>>(objListCustomerAccount);
                string JsonStringwareloc = JsonHelper.JsonSerializer<List<WarehouseLocation>>(objListWarehouseLocation);

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("CustomerGroup", typeof(string));
                dt2.Columns.Add("CustomerAccount", typeof(string));
                dt2.Columns.Add("WarehouseLocation", typeof(string));
                dt2.Columns.Add("CustGrpStatus", typeof(string));
                dt2.Columns.Add("CustAccSatus", typeof(string));
                dt2.Columns.Add("WarehLocSatus", typeof(string));
                DataRow dr = dt2.NewRow();
                dr["CustomerGroup"] = JsonStringcustgrp;
                dr["CustomerAccount"] = JsonStringcustacc;
                dr["WarehouseLocation"] = JsonStringwareloc;
                if (dtcustgrp.Rows.Count > 0)
                {
                    dr["CustGrpStatus"] = "Exist";
                }
                else
                {
                    dr["CustGrpStatus"] = "NotExist";
                }
                if (dtcustacc.Rows.Count > 0)
                {
                    dr["CustAccSatus"] = "Exist";
                }
                else
                {
                    dr["CustAccSatus"] = "NotExist";
                }
                if (dtwareloca.Rows.Count > 0)
                {
                    dr["WarehLocSatus"] = "Exist";
                }
                else
                {
                    dr["WarehLocSatus"] = "NotExist";
                }
                dt2.Rows.Add(dr);
                dt2.AcceptChanges();

                dtcustgrp = null;
                dtcustacc = null;
                dtwareloca = null;

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr1 in dt2.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt2.Columns)
                    {
                        row.Add(col.ColumnName, dr1[col]);
                    }
                    rows.Add(row);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                JsonStringForSerialized = serializer.Serialize(rows).ToString();
            }
            catch
            {
                JsonStringForSerialized = JsonHelper.JsonSerializer<String>("Error");
            }
            return JsonStringForSerialized;
        }

        #endregion
    }
}
