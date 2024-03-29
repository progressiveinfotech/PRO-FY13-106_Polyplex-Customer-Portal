
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetSalesOrderStatus]') AND type in (N'P', N'PC'))
DROP PROC SP_GetSalesOrderStatus
GO

/****** Object:  StoredProcedure [dbo].[SP_GetSalesOrderStatus]    Script Date: 08/21/2013 20:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetSalesOrderStatus]    

     @UserId nvarchar(50),
	 @CustomerGroupId nvarchar(50),
	 @CustomerAccountId nvarchar(50),
	 @FromDate datetime,
	 @ToDate datetime
 
AS

BEGIN TRANSACTION

	SET NOCOUNT ON;
   
   BEGIN
        
        --CASE 1. WHEN BOTH CUSTOMER GROUP ID AND CUSTOMER ACCOUNT ID ARE 0
        if(@CustomerGroupId ='0' and @CustomerAccountId ='0')
        begin
        
			Select cm.CustomerCode as SOCustomer,b.CustomerOrderNo,
		replace(CONVERT(VARCHAR(11),b.SOCustomerOrderDate ,101),' ','/') AS CustomerOrderDate,
		b.SONo as OrdNo,
		replace(CONVERT(VARCHAR(11),SODate ,101),' ','/') AS SODate,
		(SOUnitPrice*SOReqQuantityInKG) as OrderValueinOrderCurr,
		 replace(CONVERT(VARCHAR(11),b.CommittedETD ,101),' ','/') AS CommittedETD,
		 replace(CONVERT(VARCHAR(11),b.CommittedETA ,101),' ','/') AS CommittedETA,
		 (case when b.Confirmed != 1 then 'Hold' end) as OnHold,
		SOReqQuantityInKG as OrderQty,
		[LineNo] as LnNo,
		a.StatusCode,  
		0.00 as DspQty, 0.00 as RdyQty, 0 as DspNrl, 0 as RdyNrl, 0 as RsvQty,0.00 as InvoiceQty
		     
		from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on 
		(a.SalesOrderId =b.SalesOrderId) inner join Sal_Glb_Customer_Mst c on 
		c.CustomerID = b.SOCustomer inner join Sal_Glb_FinalDestination_Mst as fd on
		b.sofinaldestination = fd.FinalDestinationID inner join Sal_Glb_PaymentTerms_Mst as pt on
		b.soPaymentTerms = pt.PaymentsTermsID inner join Com_DeliveryTo_Mst as dt on
		b.SODeliveryTo = dt.DeliveryToID inner join Sal_Glb_Customer_Mst as cm on 
		b.SOCustomer = cm.CustomerID inner join Proc_UOM_Master as um on b.UOMId = um.AutoId 
		inner join Com_TermsOfDelivery_Mst as td on b.TermsOfDelivery = td.TermsOfDeliveryId
		inner join Sal_Glb_Logistic_Mst as tp on b.SOLogistics = tp.LogisticID  inner join dbo.Com_SalesArea_Mst sa on b.salesareaid = sa.AutoId          
		Inner Join Com_SalesType_Mst s on s.SalesTypeId=b.SalesTypeId
		inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
		where a.ActiveStatus = 1 and b.ActiveStatus = 1 
		AND (SODate BETWEEN @FromDate AND @ToDate )
		AND cm.CustomerCode in(
		select CustomerCode from Sal_Glb_Customer_Mst where CustomerID in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join 
		Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId =@UserId and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct
		A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId =@UserId
		 and A.ActiveStatus =1)))
		
	end
		
		--CASE 2. WHEN BOTH CUSTOMER GROUP ID AND CUSTOMER ACCOUNT ID ARE NOT 0
		else if(@CustomerGroupId !='0' and @CustomerAccountId !='0')
		begin
		
		Select cm.CustomerCode as SOCustomer,b.CustomerOrderNo,
		replace(CONVERT(VARCHAR(11),b.SOCustomerOrderDate ,101),' ','/') AS CustomerOrderDate,
		b.SONo as OrdNo,
		replace(CONVERT(VARCHAR(11),SODate ,101),' ','/') AS SODate,
		(SOUnitPrice*SOReqQuantityInKG) as OrderValueinOrderCurr,
		 replace(CONVERT(VARCHAR(11),b.CommittedETD ,101),' ','/') AS CommittedETD,
		 replace(CONVERT(VARCHAR(11),b.CommittedETA ,101),' ','/') AS CommittedETA,
		(case when b.Confirmed != 1 then 'Hold' end) as OnHold,
		SOReqQuantityInKG as OrderQty,
		[LineNo] as LnNo,
		a.StatusCode,  
		0.00 as DspQty, 0.00 as RdyQty, 0 as DspNrl, 0 as RdyNrl, 0 as RsvQty,0.00 as InvoiceQty
		     
		from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on 
		(a.SalesOrderId =b.SalesOrderId) inner join Sal_Glb_Customer_Mst c on 
		c.CustomerID = b.SOCustomer inner join Sal_Glb_FinalDestination_Mst as fd on
		b.sofinaldestination = fd.FinalDestinationID inner join Sal_Glb_PaymentTerms_Mst as pt on
		b.soPaymentTerms = pt.PaymentsTermsID inner join Com_DeliveryTo_Mst as dt on
		b.SODeliveryTo = dt.DeliveryToID inner join Sal_Glb_Customer_Mst as cm on 
		b.SOCustomer = cm.CustomerID inner join Proc_UOM_Master as um on b.UOMId = um.AutoId 
		inner join Com_TermsOfDelivery_Mst as td on b.TermsOfDelivery = td.TermsOfDeliveryId
		inner join Sal_Glb_Logistic_Mst as tp on b.SOLogistics = tp.LogisticID  inner join dbo.Com_SalesArea_Mst sa on b.salesareaid = sa.AutoId          
		Inner Join Com_SalesType_Mst s on s.SalesTypeId=b.SalesTypeId
		inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
		where a.ActiveStatus = 1 and b.ActiveStatus = 1 
		AND (SODate BETWEEN @FromDate AND @ToDate )
		AND cm.CustomerCode in(
		select CustomerCode from Sal_Glb_Customer_Mst where CustomerID in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join 
		Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId =@UserId and A.CustomerId =@CustomerAccountId and A.ActiveStatus =1
        and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on
        A.CustomerGroupId =B.UserID where A.UserId =@UserId and A.CustomerGroupId=@CustomerGroupId and A.ActiveStatus =1)))
		
		end
		
		--CASE 3. WHEN CUSTOMER GROUP ID IS NOT 0 AND CUSTOMER ACCOUNT ID IS 0
		else if(@CustomerGroupId !='0' and @CustomerAccountId ='0')
		begin
		
		Select cm.CustomerCode as SOCustomer,b.CustomerOrderNo,
		replace(CONVERT(VARCHAR(11),b.SOCustomerOrderDate ,101),' ','/') AS CustomerOrderDate,
		b.SONo as OrdNo,
		replace(CONVERT(VARCHAR(11),SODate ,101),' ','/') AS SODate,
		(SOUnitPrice*SOReqQuantityInKG) as OrderValueinOrderCurr,
		 replace(CONVERT(VARCHAR(11),b.CommittedETD ,101),' ','/') AS CommittedETD,
		 replace(CONVERT(VARCHAR(11),b.CommittedETA ,101),' ','/') AS CommittedETA,
		(case when b.Confirmed != 1 then 'Hold' end) as OnHold,
		SOReqQuantityInKG as OrderQty,
		[LineNo] as LnNo,
		a.StatusCode,  
		0.00 as DspQty, 0.00 as RdyQty, 0 as DspNrl, 0 as RdyNrl, 0 as RsvQty,0.00 as InvoiceQty
		     
		from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on 
		(a.SalesOrderId =b.SalesOrderId) inner join Sal_Glb_Customer_Mst c on 
		c.CustomerID = b.SOCustomer inner join Sal_Glb_FinalDestination_Mst as fd on
		b.sofinaldestination = fd.FinalDestinationID inner join Sal_Glb_PaymentTerms_Mst as pt on
		b.soPaymentTerms = pt.PaymentsTermsID inner join Com_DeliveryTo_Mst as dt on
		b.SODeliveryTo = dt.DeliveryToID inner join Sal_Glb_Customer_Mst as cm on 
		b.SOCustomer = cm.CustomerID inner join Proc_UOM_Master as um on b.UOMId = um.AutoId 
		inner join Com_TermsOfDelivery_Mst as td on b.TermsOfDelivery = td.TermsOfDeliveryId
		inner join Sal_Glb_Logistic_Mst as tp on b.SOLogistics = tp.LogisticID  inner join dbo.Com_SalesArea_Mst sa on b.salesareaid = sa.AutoId          
		Inner Join Com_SalesType_Mst s on s.SalesTypeId=b.SalesTypeId
		inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
		where a.ActiveStatus = 1 and b.ActiveStatus = 1 
		AND (SODate BETWEEN @FromDate AND @ToDate )
		AND cm.CustomerCode in(
		select CustomerCode from Sal_Glb_Customer_Mst where CustomerID in (SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join 
		Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId =@UserId and A.ActiveStatus =1
        and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on
        A.CustomerGroupId =B.UserID where A.UserId =@UserId and A.CustomerGroupId=@CustomerGroupId and A.ActiveStatus =1)))
		
		end
		
   
   END    
 GO
