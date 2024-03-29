IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertUpdate_In_tblCustomerUserMapping]') AND type in (N'P', N'PC'))
DROP PROC SP_InsertUpdate_In_tblCustomerUserMapping
GO

/****** Object:  StoredProcedure [dbo].[SP_InsertUpdate_In_tblCustomerUserMapping]    Script Date: 08/21/2013 20:15:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertUpdate_In_tblCustomerUserMapping]    

	 @AutoId AS bigint
	,@UserId as numeric(18,0)
	,@CustomerId as numeric(18,0)
	,@CustomerGroupId numeric(18,0)
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
	        
	        INSERT INTO [tblCustomerUserMapping]
           ([UserId]
           ,[CustomerId]
           ,[CustomerGroupId]
           ,[ActiveStatus]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (@UserId
           ,@CustomerId
           ,@CustomerGroupId
           ,@ActiveStatus
           ,@CreatedBy
           ,GETDATE ())
     END      
	ELSE IF(@AutoId>0)
	BEGIN
	--========UPDATE SECTION================================================================================
	   UPDATE [tblCustomerUserMapping]
	   SET [UserId] = @UserId
		  ,[CustomerId] = @CustomerId
		  ,[CustomerGroupId]=@CustomerGroupId
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