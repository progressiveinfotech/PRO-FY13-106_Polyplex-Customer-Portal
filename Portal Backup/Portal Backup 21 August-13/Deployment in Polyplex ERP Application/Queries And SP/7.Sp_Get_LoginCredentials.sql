IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Get_LoginCredentials]') AND type in (N'P', N'PC'))
DROP PROC Sp_Get_LoginCredentials
GO

/****** Object:  StoredProcedure [dbo].[Sp_Get_LoginCredentials]    Script Date: 08/21/2013 20:21:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sp_Get_LoginCredentials] 
@LoginId nvarchar(50),
@Password nvarchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;
    SELECT U.UserID,U.UserName,U.LocationID, U.UserTypeId,POReleaseGroup FROM Com_Login_Mst AS L INNER JOIN Com_User_Mst AS U ON L.UserID = U.UserID 
    Inner join Com_UserType_Mst Utype On U.UserTypeId=Utype.UserTypeId WHERE L.LoginID =@LoginId  AND L.Password =@Password AND U.ActiveStatus =1 
    and Utype.Type!='Customer'
   END
   
   GO