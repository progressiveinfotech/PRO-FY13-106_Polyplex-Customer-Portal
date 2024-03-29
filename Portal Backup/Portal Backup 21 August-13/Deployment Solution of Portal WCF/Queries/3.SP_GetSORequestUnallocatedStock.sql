
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetSORequestUnallocatedStock]') AND type in (N'P', N'PC'))
DROP PROC SP_GetSORequestUnallocatedStock
GO

/****** Object:  StoredProcedure [dbo].[SP_GetSORequestUnallocatedStock]    Script Date: 08/21/2013 20:49:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetSORequestUnallocatedStock]    

     @UserId nvarchar(50),
	 @CustomerGroupId nvarchar(50),
	 @CustomerAccountId nvarchar(50),
	 @PlantId nvarchar(50),
	 @WareHouseId nvarchar(50),
	 @FromDate datetime,
	 @ToDate datetime
 
AS

	SET NOCOUNT ON;
   
        BEGIN
        
        --CASE 1. WHEN BOTH CUSTOMER GROUP ID AND CUSTOMER ACCOUNT ID ARE 0
        if(@CustomerGroupId ='0' and @CustomerAccountId ='0')
        begin
        
        select Type,Actual_Micron as Thickness,Gauge,Width as 'WidthInMM' ,WidthInInch,Whouse,Core
		,(select Location from Prod_StorageLocation_Mst where StorageLocCode =Whouse) as Whousec
		,SUM(Actual_Length) as LengthinMtr,sum(Actual_Length_Ft) as LengthInFt,
		sum(ActualQtyInKg) as QtyInKg,sum(ActualQtyIn_Lbs) as QtyInLbs,sum(NoOfRolls) as NoOfRolls from Prod_RollSlitting_Details
		
		where (SalesOrdNo is null or SalesOrdNo ='') and NoOfRolls >0
		and Whouse in (select StorageLocCode from Prod_StorageLocation_Mst where PlantId =@PlantId and Status =1 and autoid in (select WareHouseId from tblWarehouseMapping 
		where PlantId =@PlantId and CustomerId in 
		(SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join 
		Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId =@UserId and A.ActiveStatus =1 and A.CustomerGroupId in (SELECT distinct
		A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on A.CustomerGroupId =B.UserID where A.UserId =@UserId
		and A.ActiveStatus =1)))) and CreatedOn between @FromDate and @ToDate
		group by Type,Actual_Micron,Gauge,Width,WidthInInch,Whouse,Actual_Length,Actual_Length_Ft,ActualQtyInKg,ActualQtyIn_Lbs,NoOfRolls,Core        
		
		
	    end
		
		--CASE 2. WHEN BOTH CUSTOMER GROUP ID AND CUSTOMER ACCOUNT ID ARE NOT 0
		else if(@CustomerGroupId !='0' and @CustomerAccountId !='0')
		begin		
		
		select Type,Actual_Micron as Thickness,Gauge,Width as 'WidthInMM' ,WidthInInch,Whouse,Core
		,(select Location from Prod_StorageLocation_Mst where StorageLocCode =Whouse) as Whousec
		,SUM(Actual_Length) as LengthinMtr,sum(Actual_Length_Ft) as LengthInFt,
		sum(ActualQtyInKg) as QtyInKg,sum(ActualQtyIn_Lbs) as QtyInLbs,sum(NoOfRolls) as NoOfRolls from Prod_RollSlitting_Details
		
		where (SalesOrdNo is null or SalesOrdNo ='') and NoOfRolls >0
		and Whouse in (select StorageLocCode from Prod_StorageLocation_Mst where PlantId =@PlantId and Status =1 and autoid in (select WareHouseId from tblWarehouseMapping 
		where PlantId =@PlantId and CustomerId in 
		(SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join 
		Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId =@UserId and A.CustomerId =@CustomerAccountId and A.ActiveStatus =1
        and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on
        A.CustomerGroupId =B.UserID where A.UserId =@UserId and A.CustomerGroupId=@CustomerGroupId and A.ActiveStatus =1))))
		 and CreatedOn between @FromDate and @ToDate
		group by Type,Actual_Micron,Gauge,Width,WidthInInch,Whouse,Actual_Length,Actual_Length_Ft,ActualQtyInKg,ActualQtyIn_Lbs,NoOfRolls,Core	 
				
		end
		
		--CASE 3. WHEN CUSTOMER GROUP ID IS NOT 0 AND CUSTOMER ACCOUNT ID IS 0
		else if(@CustomerGroupId !='0' and @CustomerAccountId ='0')
		begin
		
		select Type,Actual_Micron as Thickness,Gauge,Width as 'WidthInMM' ,WidthInInch,Whouse,Core
		,(select Location from Prod_StorageLocation_Mst where StorageLocCode =Whouse) as Whousec
		,SUM(Actual_Length) as LengthinMtr,sum(Actual_Length_Ft) as LengthInFt,
		sum(ActualQtyInKg) as QtyInKg,sum(ActualQtyIn_Lbs) as QtyInLbs,sum(NoOfRolls) as NoOfRolls from Prod_RollSlitting_Details
		
		where (SalesOrdNo is null or SalesOrdNo ='') and NoOfRolls >0
		and Whouse in (select StorageLocCode from Prod_StorageLocation_Mst where PlantId =@PlantId and Status =1 and autoid in (select WareHouseId from tblWarehouseMapping 
		where PlantId =@PlantId and CustomerId in 
		(SELECT distinct A.CustomerId FROM tblCustomerUserMapping AS A inner join 
		Sal_Glb_Customer_Mst as B on A.CustomerId =B.CustomerID where A.UserId =@UserId and A.ActiveStatus =1
        and A.CustomerGroupId in (SELECT distinct A.CustomerGroupId FROM tblCustomerUserMapping AS A inner join Com_User_Mst as B on
        A.CustomerGroupId =B.UserID where A.UserId =@UserId and A.CustomerGroupId=@CustomerGroupId and A.ActiveStatus =1))))
		 and CreatedOn between @FromDate and @ToDate
		group by Type,Actual_Micron,Gauge,Width,WidthInInch,Whouse,Actual_Length,Actual_Length_Ft,ActualQtyInKg,ActualQtyIn_Lbs,NoOfRolls,Core	
		
		end
		
   
   END    
GO