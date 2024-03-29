IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Update_In_Com_Login_Mst_ForPassword]') AND type in (N'P', N'PC'))
DROP PROC SP_Update_In_Com_Login_Mst_ForPassword
GO

/****** Object:  StoredProcedure [dbo].[SP_Update_In_Com_Login_Mst_ForPassword]    Script Date: 08/21/2013 20:47:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_Update_In_Com_Login_Mst_ForPassword]    

	 @UserID as numeric(18,0)
	,@Password as nvarchar(50)	
	,@ModifiedBy AS numeric(18,0)
	,@ErrorStatus as nvarchar(100) OUTPUT   
AS

BEGIN TRANSACTION

	SET NOCOUNT ON;
   
   UPDATE [Com_Login_Mst]
   SET [Password] = @Password      
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedOn] = GETDATE ()
 WHERE [UserID] = @UserID
 
 IF @@ERROR <> 0 
  BEGIN	
    SET @ErrorStatus =@@ERROR	  
	ROLLBACK TRANSACTION
  END
	ELSE
    BEGIN	
      SET @ErrorStatus =@@ERROR		      
      COMMIT TRANSACTION
    END
GO