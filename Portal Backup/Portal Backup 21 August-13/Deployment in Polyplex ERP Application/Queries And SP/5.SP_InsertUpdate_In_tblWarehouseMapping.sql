IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertUpdate_In_tblWarehouseMapping]') AND type in (N'P', N'PC'))
DROP PROC SP_InsertUpdate_In_tblWarehouseMapping
GO

/****** Object:  StoredProcedure [dbo].[SP_InsertUpdate_In_tblWarehouseMapping]    Script Date: 08/21/2013 20:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertUpdate_In_tblWarehouseMapping]    

	 @AutoId AS bigint	
	,@CustomerId as numeric(18,0)
	,@PlantId AS numeric(18,0)
	,@WareHouseId AS numeric(18,0)
	,@ActiveStatus as bit
	,@CreatedBy as  numeric(18,0)
	,@ModifiedBy as  numeric(18,0)	
    ,@ErrorStatus as nvarchar(100) OUTPUT
 
AS

BEGIN TRANSACTION

	SET NOCOUNT ON;
		
   IF(@AutoId=0)
	     BEGIN
	     
	        --========INSERT SECTION================================================================================
	        
	        INSERT INTO [tblWarehouseMapping]
           ([CustomerId]
           ,[PlantId]
           ,[WareHouseId]
           ,[ActiveStatus]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (@CustomerId
           ,@PlantId
           ,@WareHouseId
           ,@ActiveStatus
           ,@CreatedBy
           ,GETDATE ())
     END      
	ELSE IF(@AutoId>0)
	BEGIN
	--========UPDATE SECTION================================================================================
	   UPDATE [tblWarehouseMapping]
	   SET [CustomerId] = @CustomerId
		  ,[PlantId] = @PlantId
		  ,[WareHouseId] = @WareHouseId
		  ,[ActiveStatus] = @ActiveStatus
		  ,[ModifiedBy] = @ModifiedBy
		  ,[ModifiedOn] = GETDATE ()
	 WHERE AutoId =@AutoId
	END	
	
	IF @@ERROR <> 0 
		    BEGIN
			  SET @ErrorStatus=@@ERROR 
			  ROLLBACK TRANSACTION
		    END
			ELSE
		    BEGIN	
		      SET @ErrorStatus =@@ERROR		            
		      COMMIT TRANSACTION
		    END
GO