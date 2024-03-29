IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetQuoteRequestAllRecords]') AND type in (N'P', N'PC'))
DROP PROC SP_GetQuoteRequestAllRecords
GO

/****** Object:  StoredProcedure [dbo].[SP_GetQuoteRequestAllRecords]    Script Date: 08/21/2013 20:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetQuoteRequestAllRecords]    

	 @CustomerGroupId nvarchar(50),
	 @CustomerAccountId nvarchar(50),
	 @WareHouseId nvarchar(50)
 
AS

   SET NOCOUNT ON;
   BEGIN
        
        --CASE 1. WHEN CUSTOMER GROUP ID,CUSTOMER ACCOUNT ID AND WAREHOUSEID ARE 0
        if(@CustomerGroupId !='0' and @CustomerAccountId !='0' and @WareHouseId !='0')
        begin
        SELECT [AutoId],[QuoteRequestNo],[PINo],[OrderConfNo],replace(CONVERT(VARCHAR(11),[OrderDate] ,101),' ','/') AS OrderDate,[OrderStatus]
       ,[Remarks] FROM [tbl_QuoteRequestHeader] where [ActiveStatus]=1 and [CustomerGroup] =@CustomerGroupId and [CustomerAccNo]=@CustomerAccountId
        and [WareHouseId]=@WareHouseId 		
	    end
   END    
 
GO