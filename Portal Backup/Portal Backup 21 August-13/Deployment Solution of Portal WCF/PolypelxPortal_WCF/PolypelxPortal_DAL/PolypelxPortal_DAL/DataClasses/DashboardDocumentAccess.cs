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
    public class DashboardDocumentAccess
    {
        #region*******************************Variables**************************************

        Common com = new Common();
        SqlCommand cmd;
        SqlDataAdapter _Obj_SDA;
        private DataSet ds = new DataSet();
        SqlConnection _Obj_Conn = new SqlConnection();
        private DataSet dsCOA = new PolypelxPortal_DAL.PortalDataSet.JumboDs();
        SqlConnection ObjCon = new SqlConnection();
        SqlDataReader _ObjSdr;
        PolypelxPortal_DAL.ConnectionClass.Connection con = new PolypelxPortal_DAL.ConnectionClass.Connection();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();
        
        private static string AppPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        private string xCoa, Sql, stemp;
        private int xRec, xUoM, xCatg, dRec;
        ReportDocument rptDoc = new ReportDocument();

        string ReportpathSO = AppPath + WebConfigurationManager.AppSettings["ReportPath"].ToString();
        string ReportpathPackList = AppPath + WebConfigurationManager.AppSettings["ReportpathPackList"].ToString();
        string ReportpathInvPrt2 = AppPath + WebConfigurationManager.AppSettings["ReportpathInvPrt2"].ToString();
        string ReportpathInvPrt3 = AppPath + WebConfigurationManager.AppSettings["ReportPathInvPrt3"].ToString();
        string ReportpathInvPrtsl = AppPath + WebConfigurationManager.AppSettings["ReportpathInvPrtsl"].ToString();
        string ReportpathInvPrt1 = AppPath + WebConfigurationManager.AppSettings["ReportpathInvPrt1"].ToString();
        string ReportpathCoa = AppPath + WebConfigurationManager.AppSettings["ReportpathCoa"].ToString();
        string FileToSavepath = AppPath + WebConfigurationManager.AppSettings["FileToSavepath"].ToString();
        string FileAccessPath = WebConfigurationManager.AppSettings["FileAccessPath"].ToString();
        string _FlName;
        double Trc, tamount;

        #endregion

        #region*********************DashBoard Document Access Functions**********************

        /// <summary>
        /// Function for Document Access Deshboard.
        /// Created By: Lalit
        /// Created Date: 22july 2013
        /// </summary>
        public string Dashboard_DocumentAccess(Stream Parameterdetails)
        {
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

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ds = new DataSet();
                _Obj_Conn.ConnectionString = con.ConnectionString;
                con.OpenConnection();
                cmd = new SqlCommand();
                cmd.Connection = _Obj_Conn;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"Select distinct (select CustomerCode from Sal_Glb_Customer_Mst where CustomerID=Obj_H.SOCustomer) as [CusAcc#],
                                    Obj_H.CustomerOrderNo as [Cust Order#],CONVERT(nvarchar(50), Obj_H.SOCustomerOrderDate,101) as [Cust Order Date],
                                    REPLACE(Obj_H.SONo,('SO'+Obj_H.Year),'') as [PolyPlex SO#],CONVERT(nvarchar(50),Obj_H.CreatedOn,101) as [Polyplex So Date],
                                    Obj_H.SONo as [SO Confirmation#],v.InvNo as [Packing List],Obj_PackLst.InvoiceNo as [InvoiceNo]
                                    ,Obj_Coa.CoaNo as [COA] From Sal_Glb_OrderInformations Obj_H Inner Join Sal_Glb_OrderLineItem Obj_D 
                                     On Obj_H.SalesOrderId=Obj_D.SalesOrderId left Outer Join Sal_Glb_Invoice_Tran Obj_PackLst On Obj_H.SalesOrderId=Obj_PackLst.SalesOrderId
                                     left Outer Join View_PackingLst as V on V.SalesOrdNo=Obj_H.SONo
                                     left Outer Join TblCOA as Obj_Coa on Obj_Coa.SONo=obj_H.SONo
                                     Where Obj_H.ActiveStatus=1 and Obj_D.ActiveStatus=1 and Obj_PackLst.ActiveStatus=1";
                cmd.CommandText += @" AND (Obj_H.SODate BETWEEN '" + FromDate + "' AND '" + ToDate + "' )";

                if (CustomerGroupId == "0" && CustomerAccountId == "0")
                {
                    cmd.CommandText += " AND Obj_H.SOCustomer in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + UserId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + UserId + "' and A.ActiveStatus =1))";
                    
                }
                else if (CustomerGroupId != "0" && CustomerAccountId == "0")
                {
                    cmd.CommandText += " AND Obj_H.SOCustomer in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + UserId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + UserId + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1))";
                }
                else
                {
                    cmd.CommandText += " AND Obj_H.SOCustomer in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join";
                    cmd.CommandText += " Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId ='" + UserId + "' and A.CustomerId ='" + CustomerAccountId + "'";
                    cmd.CommandText += " and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join ";
                    cmd.CommandText += " Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId ='" + UserId + "' and A.CustomerGroupId='" + CustomerGroupId + "'";
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
                ds.Dispose();
                serializer.MaxJsonLength = Int32.MaxValue;
                return serializer.Serialize(rows);
            }
            catch (Exception ex) { };
            return "";
        }

        /// <summary>
        /// Function for Printing Sales Order Report on the basis of Sales Order.
        /// Created By: Lalit
        /// Created Date: 19july 2013
        /// </summary>        
        public string PrintSOReport(Stream Parameterdetails)
        {
            string File = "";
            string[] Ht = new string[2];
            try
            {
                SalesOrder _ObjSalesOrd = new SalesOrder();
                _ObjSalesOrd = JsonHelper.DeserializeObj<SalesOrder>(Parameterdetails);
                string _SalesOrdNo = _ObjSalesOrd.SalesOrderNo;
                DataSet _DSNew = new PolypelxPortal_DAL.PortalDataSet.SODS1();
                _Obj_Conn.ConnectionString = con.ConnectionString;
                con.OpenConnection();
                cmd = new SqlCommand();
                cmd.Connection = _Obj_Conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * From View_SalesOrder1 Where OrdNo = '" + _SalesOrdNo + "'";
                _Obj_SDA = new SqlDataAdapter(cmd);
                _Obj_SDA.Fill(_DSNew, "View_SalesOrder1");
                if (_DSNew.Tables[0].Rows.Count > 0)
                {
                    rptDoc.Load(ReportpathSO);
                    rptDoc.SetDataSource(_DSNew);
                    ExportOptions exportOpts = new ExportOptions();
                    PdfRtfWordFormatOptions pdfOpts = ExportOptions.CreatePdfRtfWordFormatOptions();
                    exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOpts.ExportFormatOptions = pdfOpts;
                    _FlName = Guid.NewGuid().ToString() + ".pdf";
                    rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, (FileToSavepath + _FlName));
                    Ht[0] = FileAccessPath;
                    Ht[1] = _FlName;
                    File = JsonHelper.JsonSerializer<string[]>(Ht);
                   
                    rptDoc.Dispose();
                }
                con.CloseConnection();
                con.DisposeConnection();
                _Obj_SDA.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex) {};

            return File;
        }

        /// <summary>
        /// Function for Printing a Packing List Report. This function will take 2 parameter CustomerNo,PackingListNo.
        /// Created By: Lalit
        /// Created Date: 19july 2013
        /// </summary>
        public string PrintPackingList(Stream PackingListParm)
        {
            string File = "";
            try
            {
                string[] Ht = new string[2];
                PackingList _ObjPackingList = new PackingList();
                _ObjPackingList = JsonHelper.DeserializeObj<PackingList>(PackingListParm);
                _Obj_Conn.ConnectionString = con.ConnectionString;
                string _CustNo = _ObjPackingList.CustomerNo;
                string _PckLstNo = _ObjPackingList.PackingListNo;
                cmd = new SqlCommand();
                Sql = @"Select * from View_PackingLst where InvNo = '" + _PckLstNo + "' and CustomerCode ='" + _CustNo + "' Order By Type, Micron, Width";
                _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);
                _Obj_SDA.Fill(ds, "View_PackingLst");
                Sql = "select distinct VrYr + COANo as CoaNo From Dbo.TblCOa Where InvNo = '" + ds.Tables["View_PackingLst"].Rows[0]["RrNumber"].ToString().Trim() + "'";
                DataTable xDt = objdatacommon.ExecuteSqlQry(Sql);
                if (xDt != null && xDt.Rows.Count > 0)
                {
                    xCoa = "";
                    for (int x = 0; x < xDt.Rows.Count; x++)
                    {
                        if (xCoa == "")
                        {
                            xCoa = xDt.Rows[x]["COANo"].ToString();
                        }
                        else
                        {
                            xCoa += ", " + xDt.Rows[x]["COANo"].ToString();
                        }
                    }
                }
                else
                {
                    xCoa = "";
                }
                int xUom = Convert.ToInt16(ds.Tables["View_PackingLst"].Rows[0]["UOMId"]);
                if (xUom > 101)
                {
                    for (int x = 0; x < ds.Tables["View_PackingLst"].Rows.Count; x++)
                    {
                        if (xUom == 180)
                        {
                            ds.Tables["View_PackingLst"].Rows[x]["Lbs"] = Convert.ToDecimal(ds.Tables["View_PackingLst"].Rows[x]["NoOfRolls"].ToString());
                        }
                        else
                        {
                            ds.Tables["View_PackingLst"].Rows[x]["Lbs"] = Convert.ToDecimal(ds.Tables["View_PackingLst"].Rows[x]["OuQty"].ToString());
                        }
                    }
                    ds.Tables["View_PackingLst"].AcceptChanges();
                }

                Sql = "select '' as DeliveryToName,'' as DlvAddress,'' as DlvCity,'' as DlvStateName,'' as DlvZip,'' as DlvCountryName,'' as City,* from Com_SalesOrgnization_Mst where SalesOrgCode = 'PCL-US'";
                _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);
                _Obj_SDA.Fill(ds, "Com_SalesOrgnization_Mst");

                Sql = @"Select d.DeliveryToName, d.address, d.City, d.StateName, d.Zip, d.CountryName 
                    From dbo.Sal_Glb_OrderInformations o inner join view_deliveryto d 
                    on d.DeliveryToID = o.SODeliveryTo 
                    Where sono = '" + ds.Tables["View_PackingLst"].Rows[0]["SalesOrdNo"] + "'";

                _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);
                int i = _Obj_SDA.Fill(ds, "DlvMst");

                if (i > 0)
                {
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["DeliveryToName"] = ds.Tables["DlvMst"].Rows[0]["DeliveryToName"];
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["DlvAddress"] = ds.Tables["DlvMst"].Rows[0]["address"];
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["DlvCity"] = ds.Tables["DlvMst"].Rows[0]["City"];
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["DlvStateName"] = ds.Tables["DlvMst"].Rows[0]["StateName"].ToString().Trim();
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["DlvZip"] = ", " + ds.Tables["DlvMst"].Rows[0]["Zip"];
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["DlvCountryName"] = ds.Tables["DlvMst"].Rows[0]["CountryName"];
                    ds.Tables["Com_SalesOrgnization_Mst"].Rows[0]["City"] = xCoa.Trim();
                    ds.Tables["Com_SalesOrgnization_Mst"].AcceptChanges();
                }
                if (ds.Tables["Com_SalesOrgnization_Mst"].Rows.Count > 0)
                {
                    rptDoc.Load(ReportpathPackList);
                    rptDoc.SetDataSource(ds);
                    ExportOptions exportOpts = new ExportOptions();
                    PdfRtfWordFormatOptions pdfOpts = ExportOptions.CreatePdfRtfWordFormatOptions();
                    exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOpts.ExportFormatOptions = pdfOpts;
                    _FlName = Guid.NewGuid().ToString() + ".pdf";
                    rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, (FileToSavepath + _FlName));
                    Ht[0] = FileAccessPath;
                    Ht[1] = _FlName;
                    File = JsonHelper.JsonSerializer<string[]>(Ht);
                    rptDoc.Dispose();
                }
                con.CloseConnection();
                con.DisposeConnection();
                _Obj_SDA.Dispose();
                cmd.Dispose();
                ds.Dispose();
            }
            catch (Exception ex) {  }
            return File;
        }

        /// <summary>
        /// Function for Printing a Invoice Report. This function will take 3 parameters InvoiceNo,Unit,PrintType.
        /// Created By: Lalit
        /// Created Date: 20july 2013
        /// </summary>
        public string PrintInvoice(Stream InvoiceParam)
        {
            string File = "";
            string[] Ht = new string[2];
            try
            {
                Invoice _ObjInvoice = new Invoice();
                _ObjInvoice = JsonHelper.DeserializeObj<Invoice>(InvoiceParam);
                string _PrintType = _ObjInvoice.PrintType = "Normal";
                string input, s;
                bool xFlag = true;
                int i = 0;
                input = _ObjInvoice.InvoiceNo;
                _Obj_Conn.ConnectionString = con.ConnectionString;
                _Obj_Conn.Open();
                ds = new PolypelxPortal_DAL.PortalDataSet.MstLst();
                Sql = @"Select InvoiceNo as InvNo, InvoiceDate as InvDt, Name as PName, Address as cAdd1, 
                        isnull(StateName,'') as cAdd2, isnull(CountryName,'') as cCity, ZipCode as cZip, DeliveryToName as SName, 
                        Dlv_address as Sadd1, isnull(Dlv_StateName,'') as SCity, Dlv_Zip as sZip,  
                        isnull(Dlv_CountryName,'') as sAdd2, TermsOfPayment as Pterm, CustomerOrderNo as CPO, LogisticName as ShpNo, 
                        ShippingLine as Carrier, InvoiceDate as ShpDt, (InvoiceDate+SOCreditDays) as DueDt, 0 as STax, isnull(TotalPrice,0) as tot, 
                        isnull(TotalPrice,0) as GTot, '' as Wamt, 1 as MpsFps, SpecialInstructions as Rmk, 0 as oFrt,  
                        case insurance when '' then 0 else cast(isnull(Insurance,0) as Decimal(18,2)) end as InsAmt, 1 as FIFlag, TermsOfDeliveryName as DTerms, '' as PAPOs, isnull(UOM,'Kg') as UOM, 
                        isnull(um,101) as Um, DATEADD(day, EarlyPaymentDays, InvoiceDate) as ErlyPymt, 0 as LFlag, 0 as Sal, 0 as Gst, 
                        cast(Freight as decimal(18,2)) as Frt, '' as CAgent, TaxId, Curncy, CentD, InvoiceNo2 as Whouse, invoicetype 
                         From View_invoiceheaderRpt where invoiceno='" + _ObjInvoice.InvoiceNo + "'";

                _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);

                Trc = _Obj_SDA.Fill(ds, "InvPrtH");

                Sql = @"Select InvNo, Type, Micron, isnull(WidthInInch,round((width/25.4),4)) as Width, Width as WidMM, Gauge, QtyInKg as Qty, QtyIn_Lbs as Lbs, 
                        SoUnitPrice as Srate, case sUom when 101 then Round((QtyIn_Lbs*SoUnitPrice),2) When 81 then Round((QtyInKg*SoUnitPrice),2) 
                        When 180 then Round((NoOfRolls*SoUnitPrice),2) else Round((OuQty*SOUnitPrice),2) end as Ival, PrdDsc, MxDsc, 
                        Lngft, IsNull(NoOfRolls,0) as Norls, 0 as RRate, case sUom when 180 then NoOfRolls else OuQty end as OthQty, FCatgId, PrdSpec,POrd, sku, sUom  
                        from View_InvoiceDetailPrint where InvNo='" + _ObjInvoice.InvoiceNo + "'";

                _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);

                dRec = _Obj_SDA.Fill(ds, "InvPrtD");

                xCoa = "";
                if (dRec > 0)
                {
                    string xRsv = "";
                    string Sqly = "Select Top 1 rrNumber From View_PackingLst Where InvNo = '" + ds.Tables["InvPrtD"].Rows[0]["InvNo"].ToString() + "'";
                    DataTable yDt = objdatacommon.ExecuteSqlQry(Sqly);
                    if (yDt != null && yDt.Rows.Count > 0)
                    {
                        xRsv = yDt.Rows[0]["RrNumber"].ToString();
                    }
                    string Sqlx = "select distinct vryr+COANo as CoaNo From Dbo.TblCOa Where InvNo = '" + xRsv.Trim() + "'";
                    DataTable xDt = objdatacommon.ExecuteSqlQry(Sqlx);
                    if (xDt != null && xDt.Rows.Count > 0)
                    {
                        xCoa = "";
                        for (int x = 0; x < xDt.Rows.Count; x++)
                        {
                            if (xCoa == "")
                            {
                                xCoa = xDt.Rows[x]["COANo"].ToString();
                            }
                            else
                            {
                                xCoa += ", " + xDt.Rows[x]["COANo"].ToString();
                            }
                        }
                    }
                    else
                    {
                        xCoa = "";
                    }
                    ds.Tables["InvPrtH"].Rows[0]["SName"] = xCoa.Trim();
                }

                if (ds.Tables["InvPrtH"].Rows[0]["InvoiceType"].ToString() != "1")
                {
                    Sql = "Select VoucherNo From View_GlTrnRpt where voucherno = '" + _ObjInvoice.InvoiceNo + "'";
                    _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);
                    int xty = _Obj_SDA.Fill(ds, "abc");
                    if (xty == 0)
                    {                        
                        xFlag = false;
                    }
                }
                if (dRec == 0)
                {
                    Sql = @"Select InvNo, FType as Type, Micron, Width, Widmm, Gauge, QtyInKg as Qty, QtyIn_Lbs as Lbs, SRate, IVal, PrdDsc, MxDsc, LngFt, Norls, RRate, 0 as OthQty, 
                        FCatgId, '' as PrdSpec, '101' as SUom From View_OtherInvoices Where InvNo='" + _ObjInvoice.InvoiceNo + "'";
                    _Obj_SDA = new SqlDataAdapter(Sql, _Obj_Conn);
                    xRec = _Obj_SDA.Fill(ds, "InvPrtD");
                    if (xRec == 0)
                    {
                       
                        xFlag = false;
                    }
                    Sql = "Select isnull(deliverycode, '') as Dcode From dbo.Sal_Glb_Invoice_Tran where invoiceno = '" + ds.Tables["InvPrtH"].Rows[0]["InvNo"].ToString() + "'";
                    DataTable dt = objdatacommon.ExecuteSqlQry(Sql);
                    string dd = "";
                    if (dt.Rows.Count > 0)
                    {
                        dd = dt.Rows[0]["DCode"].ToString();
                    }
                    Sql = @"Select deliverytoname, Address as Dlv_Address, city as Dlv_City, zip as Dlv_Zip, statename as Dlv_StateName, countryname as Dlv_CountryName 
                            from View_Deliveryto where deliverytoid = '" + dd + "'";
                    DataTable xdt = objdatacommon.ExecuteSqlQry(Sql);
                    if (xdt.Rows.Count > 0)
                    {
                        ds.Tables["InvPrtH"].Rows[0]["SName"] = xdt.Rows[0]["DeliveryToName"].ToString();
                        ds.Tables["InvPrtH"].Rows[0]["Sadd1"] = xdt.Rows[0]["Dlv_Address"].ToString();
                        ds.Tables["InvPrtH"].Rows[0]["SCity"] = xdt.Rows[0]["Dlv_City"].ToString();
                        ds.Tables["InvPrtH"].Rows[0]["sZip"] = xdt.Rows[0]["Dlv_Zip"].ToString();
                        ds.Tables["InvPrtH"].Rows[0]["sAdd2"] = xdt.Rows[0]["Dlv_CountryName"].ToString();
                        ds.Tables["InvPrtH"].AcceptChanges();
                    }
                    for (int h = 0; h < ds.Tables["InvPrtD"].Rows.Count; h++)
                    {
                        if (_ObjInvoice.Unit == "MKS")
                        {
                            ds.Tables["InvPrtD"].Rows[h]["SUom"] = 81;
                        }
                    }
                }
                else
                {
                    for (int h = 0; h < ds.Tables["InvPrtD"].Rows.Count; h++)
                    {
                        int zz = Convert.ToInt16(ds.Tables["InvPrtD"].Rows[h]["sUoM"].ToString());
                        if (zz == 180)
                        {
                            ds.Tables["InvPrtD"].Rows[h]["RRate"] = Convert.ToDecimal(ds.Tables["InvPrtD"].Rows[h]["SRate"]);

                            decimal uu = Convert.ToDecimal(ds.Tables["InvPrtD"].Rows[h]["IVal"]);
                            decimal uq = Convert.ToDecimal(ds.Tables["InvPrtD"].Rows[h]["OthQty"]);

                            if (uq > 0)
                            {
                                ds.Tables["InvPrtD"].Rows[h]["SRate"] = Math.Round((uu / uq), 4);
                            }
                        }
                        else
                        {
                            decimal uu = Convert.ToDecimal(ds.Tables["InvPrtD"].Rows[h]["IVal"]);
                            decimal uq = Convert.ToDecimal(ds.Tables["InvPrtD"].Rows[h]["Norls"]);
                            if (uq > 0)
                            {
                                ds.Tables["InvPrtD"].Rows[h]["RRate"] = Math.Round((uu / uq), 4);
                            }
                        }
                    }
                }
                ds.Tables["InvPrtD"].AcceptChanges();
                xUoM = Convert.ToInt32(ds.Tables["InvPrtH"].Rows[0]["Um"].ToString());
                xCatg = 0;
                if (xFlag)
                {
                    xCatg = Convert.ToInt32(ds.Tables["InvPrtD"].Rows[0]["FCatgId"]);
                }
                Trc = ds.Tables["InvPrtD"].Rows.Count - 1;
                tamount = 0;
                for (i = 0; i <= Trc; i++)
                {
                    s = Convert.ToString(ds.Tables["InvPrtD"].Rows[i]["IVal"]);
                    tamount += Convert.ToDouble(s);
                    if (xUoM.ToString().Trim() == "180" && xFlag)
                    {
                    }
                }
                ds.Tables["InvPrtH"].Rows[0]["Tot"] = Convert.ToString(tamount);
                tamount += Convert.ToDouble(ds.Tables["InvPrtH"].Rows[0]["STax"]) + Convert.ToDouble(ds.Tables["InvPrtH"].Rows[0]["GST"]) + Convert.ToDouble(ds.Tables["InvPrtH"].Rows[0]["FRT"]) - Convert.ToDouble(ds.Tables["InvPrtH"].Rows[0]["Sal"]);
                ds.Tables["InvPrtH"].Rows[0]["GTot"] = Convert.ToString(tamount);
                cmd = new SqlCommand();
                cmd.Connection = _Obj_Conn;
                cmd.CommandText = "select dbo.figureinwords(" + Convert.ToString(tamount) + ") as amt";
                _ObjSdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                stemp = "";
                if (_ObjSdr.Read())
                {
                    stemp = Convert.ToString(_ObjSdr["amt"]);
                }
                _ObjSdr.Close();
                string crn = ds.Tables["InvPrtH"].Rows[0]["Curncy"].ToString();
                string cnt = ds.Tables["InvPrtH"].Rows[0]["CentD"].ToString();
                if (tamount > 0)
                {
                    stemp = crn.Trim() + " " + stemp;
                    Int64 xx = Convert.ToInt64((tamount - Math.Truncate(tamount)));
                    if (xx > 0)
                    {
                        stemp = stemp + " " + cnt + " Only";
                    }
                    else
                    {
                        stemp = stemp + " Only";
                    }
                }
                else
                {
                    stemp = crn.Trim() + " Zero Only";
                }
                ds.Tables["InvPrtH"].Rows[0]["WAmt"] = stemp;
                ds.Tables["InvPrtH"].Rows[0]["MpsFps"] = 0;
                ds.Tables["InvPrtD"].AcceptChanges();
                ds.Tables["InvPrtH"].AcceptChanges();

                if (xFlag)       // this will be true if invoice is found
                {
                    if (dRec == 0)
                    {
                        if (_ObjInvoice.PrintType == "Normal")
                        {

                            rptDoc.Load(ReportpathInvPrt2);
                        }
                        else
                        {
                            rptDoc.Load(ReportpathInvPrt3);
                        }
                    }
                    else
                    {
                        if (xCatg == 17 && xUoM > 101)
                        {
                            rptDoc.Load(ReportpathInvPrtsl);
                        }
                        else
                        {
                           
                            rptDoc.Load(ReportpathInvPrt1);
                        }
                    }
                    rptDoc.SetDataSource(ds);
                    ExportOptions exportOpts = new ExportOptions();
                    PdfRtfWordFormatOptions pdfOpts = ExportOptions.CreatePdfRtfWordFormatOptions();
                    exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOpts.ExportFormatOptions = pdfOpts;
                    _FlName = Guid.NewGuid().ToString() + ".pdf";
                    string file = FileToSavepath + _FlName;
                    rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, (FileToSavepath + _FlName));
                    Ht[0] = FileAccessPath;
                    Ht[1] = _FlName;
                    File = JsonHelper.JsonSerializer<string[]>(Ht);
                    rptDoc.Dispose();
                    _Obj_Conn.Close();
                    _Obj_Conn.Dispose();
                    _Obj_SDA.Dispose();
                    cmd.Dispose();
                    ds.Dispose();
                }
            }
            catch (Exception ex)
            {}
            return File;
        }

        /// <summary>
        /// Function for Printing a COA Report. This function will take COA no as parameter.
        /// Created By: Lalit
        /// Created Date: 25july 2013
        /// </summary>
        public string PrintCOA(Stream COA)
        {

            string File = "";
            string[] Ht = new string[2];
            try
            {

                COA _ObjCOA = new COA();
                _ObjCOA = JsonHelper.DeserializeObj<COA>(COA);
                _Obj_Conn.ConnectionString = con.ConnectionString;
                string _COANo = _ObjCOA.COANo;
                cmd = new SqlCommand();
                cmd.Connection = _Obj_Conn;
                _Obj_Conn.Open();
                string str = @"Select a.vryr + a.CoaNo as CoaNo, a.CoaDate, a.SoNo, a.InvNO, a.FType, a.Thickness, a.Coapr, a.CoasPr, a.TestM, a.Units, a.Typical, a.Tmin, a.Tmax, a.REmarks, 
                                o.CustomerOrderNo, c.Name, f.FilmTypeName, a.ChkSno
                           from tblcoa as a Inner Join dbo.Sal_Glb_OrderInformations o on a.Sono = o.sono Inner Join Sal_Glb_Customer_Mst c on o.SOCustomer = c.CustomerId 
                                Inner Join Com_FilmType_Mst f on a.FType = f.FilmTypeCode
                            Where a.CoaNo = '" + _COANo + "' order by a.autoid";
                _Obj_SDA = new SqlDataAdapter(str, _Obj_Conn);
                int i = _Obj_SDA.Fill(dsCOA, "COA");

                if (i > 0)
                {
                    int sno = 0;
                    for (int jj = 0; jj < dsCOA.Tables["COA"].Rows.Count; jj++)
                    {
                        if (dsCOA.Tables["COA"].Rows[jj]["COAPR"].ToString().Trim().Length > 0 && Convert.ToBoolean(dsCOA.Tables["COA"].Rows[jj]["ChkSno"]))
                        {
                            sno += 1;
                            dsCOA.Tables["COA"].Rows[jj]["Remarks"] = sno.ToString();
                        }
                        if (dsCOA.Tables["COA"].Rows[jj]["COAPR"].ToString().Trim() == "Average Gauge Micron/Gauge")
                        {
                            if (dsCOA.Tables["COA"].Rows[jj]["Typical"].ToString() != "")
                            {
                                int xGa = Convert.ToInt32((Convert.ToDouble(dsCOA.Tables["COA"].Rows[jj]["Typical"].ToString()) * 4));
                                string xyz = dsCOA.Tables["COA"].Rows[jj]["Typical"].ToString().Trim() + "/" + xGa.ToString();
                                dsCOA.Tables["COA"].Rows[jj]["Typical"] = xyz;
                            }
                        }
                        if (dsCOA.Tables["COA"].Rows[jj]["COAPR"].ToString().Trim() == "Tensile Strength")
                        {
                            if (dsCOA.Tables["COA"].Rows[jj]["Typical"].ToString() != "")
                            {
                                decimal xts = Convert.ToInt32((Convert.ToDouble(dsCOA.Tables["COA"].Rows[jj]["Typical"].ToString()) * 0.0142234));
                                string xyz = dsCOA.Tables["COA"].Rows[jj]["Typical"].ToString().Trim() + "/" + xts.ToString();
                                dsCOA.Tables["COA"].Rows[jj]["Typical"] = xyz;
                            }
                        }

                    }
                    dsCOA.Tables["COA"].AcceptChanges();
                }

                if (dsCOA.Tables["COA"].Rows.Count > 0)
                {
                    rptDoc.Close();
                    rptDoc.Load(ReportpathCoa);
                    rptDoc.SetDataSource(dsCOA);
                    ExportOptions exportOpts = new ExportOptions();
                    PdfRtfWordFormatOptions pdfOpts = ExportOptions.CreatePdfRtfWordFormatOptions();
                    exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOpts.ExportFormatOptions = pdfOpts;
                    _FlName = Guid.NewGuid().ToString() + ".pdf";
                    rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, (FileToSavepath + _FlName));
                    Ht[0] = FileAccessPath;
                    Ht[1] = _FlName;
                    File = JsonHelper.JsonSerializer<string[]>(Ht);
                    rptDoc.Dispose();
                }
                _Obj_Conn.Close();
                _Obj_Conn.Dispose();
                _Obj_SDA.Dispose();
                cmd.Dispose();
                dsCOA.Dispose();
            }
            catch (Exception ex)
            {
               
            }
            return File;
        }

        /// <summary>
        /// Function for Deleting document.
        /// Created By: Lalit
        /// Created Date: 23july 2013
        /// </summary>
        /// 
        public void DeleteDocument(string Filename)
        {
           
            // string File = JsonHelper.JsonDeserialize<String>(Filename);
            DeleteFile(FileToSavepath, Filename);
        }

        /// <summary>
        /// Function for deleting a file from a Directory.
        /// Created By: Lalit
        /// Created Date: 19july 2013
        /// </summary>
        public void DeleteFile(string _Folder, string _FlName)
        {
            try
            {
                string[] Files = Directory.GetFiles(_Folder);
                foreach (string file in Files)
                {                   
                    if (file.ToUpper().Contains(_FlName.ToUpper()))
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex) {  };
        }

        /// <summary>
        /// Function for getting file from a Directory.
        /// Created By: Lalit
        /// Created Date: 19july 2013
        /// </summary>
        public byte[] GetFile(string filename)
        {
            byte[] binFile = null;
            try
            {
                BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
                binReader.BaseStream.Position = 0;
                binFile = binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
                binReader.Close();
                return binFile;
            }
            catch (Exception ex) { };
            return binFile;
        }

        #endregion
    }
}
