
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertUpdate_In_Com_UserAndLogin_Mst]') AND type in (N'P', N'PC'))
DROP PROC SP_InsertUpdate_In_Com_UserAndLogin_Mst
GO

/****** Object:  StoredProcedure [dbo].[SP_InsertUpdate_In_Com_UserAndLogin_Mst]    Script Date: 08/21/2013 20:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_InsertUpdate_In_Com_UserAndLogin_Mst]    

	 @UserID as numeric(18,0)
	,@UserName as nvarchar(50)
	,@LocationID as numeric(18,0)
	,@EmailID as nvarchar(50)
	,@LoginID as nvarchar(50)
	,@UserTypeId as nvarchar(50)
	,@ActiveStatus as bit
	,@CreatedBy as numeric(18,0)
	,@ModifiedBy AS numeric(18,0)
	,@dtLineItemsOfGroup AS Com_GroupId readonly
	,@ErrorStatus as nvarchar(100) OUTPUT 
	,@NewEmployeeCode  as nvarchar(50)  OUTPUT
AS

BEGIN TRANSACTION

	SET NOCOUNT ON;
  
  IF(@UserID=0)
  BEGIN
         
         DECLARE @NewUserId numeric(18, 0) 
         DECLARE @Series numeric(18, 0)
	     DECLARE @UserCode nvarchar(50)
	     
	     SET @Series = (select MAX(UserID) from Com_User_Mst)+1
	     IF(@Series IS NOT NULL)
			 BEGIN
			   SET @UserCode= 'EMP'+cast(REPLACE(STR(@Series, 4), SPACE(1), '0') as varchar(50))
			 END
	     ELSE
			 BEGIN
				set @UserCode= 'EMP'+'0001'
				set @Series=1
			 END
  
  INSERT INTO [Com_User_Mst]
           ([UserCode]
           ,[UserName]
           ,[LocationID]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[EmailID]
           ,UserTypeId
           ,[ActiveStatus])
     VALUES
           (@UserCode
           ,@UserName
           ,@LocationID
           ,@CreatedBy
           ,GETDATE()
           ,@EmailID
           ,@UserTypeId
           ,@ActiveStatus)
           
           IF @@ERROR <> 0 
  BEGIN
    SET @NewEmployeeCode=''	
    SET @ErrorStatus =@@ERROR	  
	ROLLBACK TRANSACTION
  END
  
  SET @NewUserId=@@IDENTITY
  
  INSERT INTO [Com_Login_Mst]
           ([UserID]
           ,[LoginID]
           ,[Password]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (@NewUserId
           ,@LoginID
           ,'usa@123'
           ,@CreatedBy
           ,GETDATE ())           
           
           IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END
           
           DECLARE @GroupId numeric(18, 0)		  
           DECLARE My_cursor CURSOR FAST_FORWARD FOR SELECT GroupId	from @dtLineItemsOfGroup
           Open My_cursor
           FETCH NEXT FROM My_cursor INTO @GroupId
           WHILE @@FETCH_STATUS=0		   
	       BEGIN
	          
	          INSERT INTO [Com_UserGroupMapping_Mst]
           ([GroupID]
           ,[UserID]
           ,[ActiveStatus]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (@GroupID
           ,@NewUserId
           ,@ActiveStatus
           ,@CreatedBy
           ,GETDATE ())
           
           IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END
	          
	          FETCH NEXT FROM My_cursor INTO @GroupId	
	    		
		   IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END
	    END	  
	    CLOSE My_cursor
        DEALLOCATE My_cursor
  
  IF @@ERROR <> 0 
  BEGIN	
    SET @NewEmployeeCode=''
    SET @ErrorStatus =@@ERROR	  
	ROLLBACK TRANSACTION
  END
	ELSE
    BEGIN	
      SET @NewEmployeeCode=@UserCode
      SET @ErrorStatus =@@ERROR		      
      COMMIT TRANSACTION
    END
  
  END
  
  ELSE IF(@UserID > 0)
  BEGIN
  
  UPDATE [Com_User_Mst] SET [UserName]=@UserName,LocationID =@LocationID,EmailID =@EmailID,
  ModifiedBy =@ModifiedBy,ModifiedOn =GETDATE (),UserTypeId=@UserTypeId,ActiveStatus=@ActiveStatus
  WHERE UserID =@UserID 
  
  IF @@ERROR <> 0 
  BEGIN	
    SET @NewEmployeeCode=''
    SET @ErrorStatus =@@ERROR	  
	ROLLBACK TRANSACTION
  END
  
  UPDATE [Com_Login_Mst] SET [LoginID]=@LoginID WHERE UserID =@UserID
  IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END 
  
  DELETE FROM [Com_UserGroupMapping_Mst] WHERE UserID =@UserID
  IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END
  
  DECLARE My_cursor CURSOR FAST_FORWARD FOR SELECT GroupId	from @dtLineItemsOfGroup
           Open My_cursor
           FETCH NEXT FROM My_cursor INTO @GroupId
           WHILE @@FETCH_STATUS=0		   
	       BEGIN
	          
	       INSERT INTO [Com_UserGroupMapping_Mst]
           ([GroupID]
           ,[UserID]
           ,[ActiveStatus]
           ,[CreatedBy]
           ,[CreatedOn])
     VALUES
           (@GroupID
           ,@UserId
           ,@ActiveStatus
           ,@CreatedBy
           ,GETDATE ())
           
           IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END
	          
	          FETCH NEXT FROM My_cursor INTO @GroupId	
	    		
		   IF @@ERROR <> 0 
			  BEGIN	
			    SET @NewEmployeeCode=''
			    SET @ErrorStatus =@@ERROR	  
				ROLLBACK TRANSACTION
			  END
	    END	  
	    CLOSE My_cursor
        DEALLOCATE My_cursor
  
  
  IF @@ERROR <> 0 
  BEGIN	
    SET @NewEmployeeCode=''
    SET @ErrorStatus =@@ERROR	  
	ROLLBACK TRANSACTION
  END
	ELSE
    BEGIN	
      SET @NewEmployeeCode='0'
      SET @ErrorStatus =@@ERROR		      
      COMMIT TRANSACTION
    END
  
  END
GO