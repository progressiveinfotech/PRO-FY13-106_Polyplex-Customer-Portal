using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using PolypelxPortal_DAL.DataClasses;

namespace PolypelxPortal_WCF
{
    public class PortalService : IPortalService
    {
        #region************************Login And Password Functions**************************

        public string UserLogin(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                LoginAndPassword objLoginAndPassword = new LoginAndPassword();
                JsonStringForSerialized = objLoginAndPassword.UserLogin(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string ForgotPassword(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                LoginAndPassword objLoginAndPassword = new LoginAndPassword();
                JsonStringForSerialized = objLoginAndPassword.ForgotPassword(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string ChangePassword(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                LoginAndPassword objLoginAndPassword = new LoginAndPassword();
                JsonStringForSerialized = objLoginAndPassword.ChangePassword(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion

        #region**************************Header Details Functions****************************

        public string GetHeaderRecord(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                HeaderRecords objHeaderRecords = new HeaderRecords();
                JsonStringForSerialized = objHeaderRecords.GetHeaderRecord(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion

        #region************************Sales Order Status Functions**************************

        public string GetOrderStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                SalesOrderStatus objSalesOrderStatus = new SalesOrderStatus();
                JsonStringForSerialized = objSalesOrderStatus.GetOrderStatus(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string GetOrderDetailsInSOStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                SalesOrderStatus objSalesOrderStatus = new SalesOrderStatus();
                JsonStringForSerialized = objSalesOrderStatus.GetOrderDetailsInSOStatus(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string GetOrderDetailsReasonInSOStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                SalesOrderStatus objSalesOrderStatus = new SalesOrderStatus();
                JsonStringForSerialized = objSalesOrderStatus.GetOrderDetailsReasonInSOStatus(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion

        #region*********************DashBoard Document Access Functions**********************

        public string Dashboard_DocumentAccess(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                DashboardDocumentAccess objDashboardDocumentAccess = new DashboardDocumentAccess();
                JsonStringForSerialized = objDashboardDocumentAccess.Dashboard_DocumentAccess(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string PrintSOReport(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                DashboardDocumentAccess objDashboardDocumentAccess = new DashboardDocumentAccess();
                JsonStringForSerialized = objDashboardDocumentAccess.PrintSOReport(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string PrintPackingList(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                DashboardDocumentAccess objDashboardDocumentAccess = new DashboardDocumentAccess();
                JsonStringForSerialized = objDashboardDocumentAccess.PrintPackingList(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string PrintInvoice(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                DashboardDocumentAccess objDashboardDocumentAccess = new DashboardDocumentAccess();
                JsonStringForSerialized = objDashboardDocumentAccess.PrintInvoice(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string PrintCOA(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                DashboardDocumentAccess objDashboardDocumentAccess = new DashboardDocumentAccess();
                JsonStringForSerialized = objDashboardDocumentAccess.PrintCOA(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public void DeleteDocument(string Parameterdetails)
        {
            try
            {
                DashboardDocumentAccess objDashboardDocumentAccess = new DashboardDocumentAccess();
                objDashboardDocumentAccess.DeleteDocument(Parameterdetails);
            }
            catch { }
        }

        #endregion

        #region**************************Stock Status Functions******************************

        public string GetStockStatus(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                StockStatus objStockStatus = new StockStatus();
                JsonStringForSerialized = objStockStatus.GetStockStatus(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion

        #region************************Account Statement Functions***************************

        public string GetAccountStatement(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                AccountStatement objAccountStatement = new AccountStatement();
                JsonStringForSerialized = objAccountStatement.GetAccountStatement(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion

        #region******************************Aging Functions*********************************

        public string AgingReport(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                Aging objAging = new Aging();
                JsonStringForSerialized = objAging.AgingReport(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        #endregion

        #region***************************Quote Request Functions*******************************

        public string GetQuoteUnallocatedAndStatusDetails(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                QuoteRequest objQuoteRequest = new QuoteRequest();
                JsonStringForSerialized = objQuoteRequest.GetQuoteUnallocatedAndStatusDetails(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string GetQuoteStatusLineItemDetails(Stream Parameterdetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                QuoteRequest objQuoteRequest = new QuoteRequest();
                JsonStringForSerialized = objQuoteRequest.GetQuoteStatusLineItemDetails(Parameterdetails);
            }
            catch { }
            return JsonStringForSerialized;
        }

        public string InsertUpdateQuoteRequest(Stream ParameterDetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                QuoteRequest objQuoteRequest = new QuoteRequest();
                JsonStringForSerialized = objQuoteRequest.InsertUpdateQuoteRequest(ParameterDetails);
            }
            catch { }
            return JsonStringForSerialized;
        }
        public string DeleteQuoteRequest(Stream ParameterDetails)
        {
            string JsonStringForSerialized = "";
            try
            {
                QuoteRequest objQuoteRequest = new QuoteRequest();
                JsonStringForSerialized = objQuoteRequest.DeleteQuoteRequest(ParameterDetails);
            }
            catch { }
            return JsonStringForSerialized;
        }
        #endregion
    }
}
