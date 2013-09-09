using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Text;
using System.IO;

namespace PolypelxPortal_WCF
{
    [ServiceContract]
    public interface IPortalService
    {
        #region************************Login And Password Functions**************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "UserLogin/")]
        string UserLogin(Stream LoginDetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ForgotPassword/")]
        string ForgotPassword(Stream PasswordDetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ChangePassword/")]
        string ChangePassword(Stream ChangePasswordDetails);

        #endregion

        #region**************************Header Details Functions****************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetHeaderRecord/")]
        string GetHeaderRecord(Stream Parameterdetails);

        #endregion

        #region************************Sales Order Status Functions**************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOrderStatus/")]
        string GetOrderStatus(Stream Parameterdetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOrderDetailsInSOStatus/")]
        string GetOrderDetailsInSOStatus(Stream Parameterdetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetOrderDetailsReasonInSOStatus/")]
        string GetOrderDetailsReasonInSOStatus(Stream Parameterdetails);

        #endregion

        #region*********************DashBoard Document Access Functions**********************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DashboardDocumentAccess/")]
        string Dashboard_DocumentAccess(Stream Params);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SalesOrderReport/")]
        string PrintSOReport(Stream SO);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PackingListReport/")]
        string PrintPackingList(Stream PackingLst);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InvoiceReport/")]
        string PrintInvoice(Stream InvDetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "PrintCOA/")]
        string PrintCOA(Stream COA);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteDocument/")]
        void DeleteDocument(string filename);

        #endregion        

        #region***************************Stock Status Functions*****************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "StockStatus/")]
        string GetStockStatus(Stream Params);

        #endregion

        #region************************Account Statement Functions***************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AccountStatement/")]
        string GetAccountStatement(Stream Params);

        #endregion

        #region****************************Aging Functions***********************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AgingReport/")]
        string AgingReport(Stream Params);

        #endregion

        #region*************************Quote Request Functions******************************

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuoteUnallocatedAndStatusDetails/")]
        string GetQuoteUnallocatedAndStatusDetails(Stream Parameterdetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetQuoteStatusLineItemDetails/")]
        string GetQuoteStatusLineItemDetails(Stream Parameterdetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertUpdateQuoteRequest/")]
        string InsertUpdateQuoteRequest(Stream Parameterdetails);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteQuoteRequest/")]
        string DeleteQuoteRequest(Stream Parameterdetails);

        #endregion

    }
    
}
