IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetQuoteRequestDetails]') AND type in (N'P', N'PC'))
DROP PROC SP_GetQuoteRequestDetails
GO

/****** Object:  StoredProcedure [dbo].[SP_GetQuoteRequestDetails]    Script Date: 08/21/2013 20:51:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetQuoteRequestDetails]    

	 @QuoteHeadAutoId nvarchar(50),
	 @Type nvarchar(50)
 
AS
	SET NOCOUNT ON;
   
   BEGIN
        
        IF(@Type='ForShow')
        BEGIN
        --CASE 1. WHEN QuoteHeadAutoId IS NOT 0
        if(@QuoteHeadAutoId !='0')
        begin
        
        SELECT [Type],[Thickness],[Gauge],[WidthInMM],[WidthInInch],(select Location from Prod_StorageLocation_Mst WHERE autoid =A.WarehouseId ) AS Warehouse
        ,Core,'' as Whousec,
        [LengthInMtr],[LengthInFt],[QtyInKg],[QtyInLbs],[NoOfRolls]
     
        FROM [tbl_QuoteRequestDetail] AS A WHERE ActiveStatus =1 AND [QuoteRequestId]=@QuoteHeadAutoId		
		
	     end
	     
	  END
	  ELSE IF(@Type='ForSave')
        BEGIN
       
         SELECT [Type],[Thickness],[Gauge],[WidthInMM],[WidthInInch],'' as Whouse,Core,'' as Whousec,
         [LengthInMtr],[LengthInFt],[QtyInKg],[QtyInLbs],[NoOfRolls]
        FROM [tbl_QuoteRequestDetail] AS A WHERE ActiveStatus =1 AND [QuoteRequestId]=@QuoteHeadAutoId	
	     
	  END
   
   END   

 GO
