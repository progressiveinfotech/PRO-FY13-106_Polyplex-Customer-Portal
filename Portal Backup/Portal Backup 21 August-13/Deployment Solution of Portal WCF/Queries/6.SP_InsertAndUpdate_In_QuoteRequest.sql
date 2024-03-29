
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertAndUpdate_In_QuoteRequest]') AND type in (N'P', N'PC'))
DROP PROC SP_InsertAndUpdate_In_QuoteRequest
GO

/****** Object:  StoredProcedure [dbo].[SP_InsertAndUpdate_In_QuoteRequest]    Script Date: 08/21/2013 20:52:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_InsertAndUpdate_In_QuoteRequest]
            
			@AutoId as numeric(18,0),
		    @CustomerGroup  as  numeric(18,0),
			@CustomerAccNo  as  numeric(18,0),
			@WareHouseId  as  numeric(18,0),
		    @OrderStatus  as nvarchar(1),
			@Remarks  as nvarchar(50),
			@CreatedBy  as  numeric(18,0),
			@ModifiedBy  as  numeric(18,0),			
			@NewQuoteNo as nvarchar(30) OUTPUT ,
			@ErrorStatus as nvarchar(50) OUTPUT,
			@dtLineitems AS Proc_QuoteRequest_LineItems_Trans readonly   
AS
BEGIN TRANSACTION
	
	SET NOCOUNT ON;

    IF(@AutoId =0)
    BEGIn
         
         DECLARE @Series numeric(18, 0)
	     DECLARE @QuoteNo as  nvarchar(50)
	     
	     SET @Series = (select  MAX([Series]) from [tbl_QuoteRequestHeader] where [CustomerGroup]=@CustomerGroup)+1
	     IF(@Series IS NOT NULL)
			 BEGIN
			   SET @QuoteNo= 'QR'+cast(REPLACE(STR(@Series, 5), SPACE(1), '0') as varchar(50))
			 END
	     ELSE
			 BEGIN
				set @QuoteNo= 'QR'+'00001'
				set @Series=1
			 END
	     
	     INSERT INTO [tbl_QuoteRequestHeader]
           ([Series]
           ,[QuoteRequestNo]
           ,[CustomerGroup]
           ,[CustomerAccNo]
           ,[WareHouseId]
           ,[OrderStatus]
           ,[Remarks]
           ,[ActiveStatus]
           ,[CreatedBy]
           ,[CreatedOn]
          
           )
     VALUES
           (@Series
           ,@QuoteNo
           ,@CustomerGroup
           ,@CustomerAccNo
           ,@WareHouseId
           ,@OrderStatus
           ,@Remarks
           ,1
           ,@CreatedBy
           ,GETDATE()
           )  
        Set @AutoId = (Select IDENT_CURRENT('tbl_QuoteRequestHeader'))
        IF @@ERROR <> 0 
		BEGIN	
		  SET @ErrorStatus =@@ERROR
		  SET @NewQuoteNo =''
		  ROLLBACK TRANSACTION
		 END
		 
		 	DECLARE  				
				@Type nvarchar(50),
				@Thickness numeric(18, 2),
				@Gauge numeric(18, 2),
				@WidthInMM numeric(18, 2),
				@WidthInInch numeric(18, 3),
				@LengthInMtr numeric(18, 2),
				@LengthInFt numeric(18, 3),
				@QtyInKg numeric(18, 2),
				@QtyInLbs numeric(18, 2),
				@NoOfRolls numeric(18, 0),
				@Core numeric(18, 2),
				@LineNo numeric(18,0)
				
				Set @LineNo=10
				
		  	DECLARE My_cursor CURSOR FAST_FORWARD FOR SELECT 
				    Type,
					Thickness,
					Gauge,
					WidthInMM,
					WidthInInch,
					LengthInMtr,
					LengthInFt,
					QtyInKg,
					QtyInLbs,
					NoOfRolls,Core
					from @dtLineitems
		
		    OPEN My_cursor
		 
		  	FETCH NEXT FROM My_cursor INTO 
				@Type,
				@Thickness,
				@Gauge,
				@WidthInMM,
				@WidthInInch,
				@LengthInMtr,
				@LengthInFt,
				@QtyInKg,
				@QtyInLbs,
				@NoOfRolls,
				@Core
								  
			WHILE @@FETCH_STATUS=0
			
			Begin
		    INSERT INTO [tbl_QuoteRequestDetail]
           ([QuoteRequestId]
           ,[Type]
           ,[Thickness]
           ,[Gauge]
           ,[WidthInMM]
           ,[WidthInInch]
           ,[LengthInMtr]
           ,[LengthInFt]
           ,[QtyInKg]
           ,[QtyInLbs]
           ,[NoOfRolls]
           ,[WarehouseId]
           ,[ActiveStatus]
           ,[CreatedBy]
           ,[CreatedOn]
           ,Core
           ,[LineNo]
           )
     VALUES
           (@AutoId
           ,@Type
           ,@Thickness
           ,@Gauge
           ,@WidthInMM
           ,@WidthInInch
           ,@LengthInMtr
           ,@LengthInFt
           ,@QtyInKg
           ,@QtyInLbs
           ,@NoOfRolls
           ,@WarehouseId
           ,1
           ,@CreatedBy
           ,GETDATE()
           ,@Core
           ,@LineNo
           ) 
		    Set @LineNo=@LineNo+10
		    IF @@ERROR <> 0 
		BEGIN	
		  SET @ErrorStatus =@@ERROR
		  SET @NewQuoteNo =''
		  ROLLBACK TRANSACTION
		 END
		 
			FETCH NEXT FROM My_cursor INTO 
				@Type,
				@Thickness,
				@Gauge,
				@WidthInMM,
				@WidthInInch,
				@LengthInMtr,
				@LengthInFt,
				@QtyInKg,
				@QtyInLbs,
				@NoOfRolls,
				@Core
				  
			 IF @@ERROR <> 0 
		BEGIN	
		  SET @ErrorStatus =@@ERROR
		  SET @NewQuoteNo =''
		  ROLLBACK TRANSACTION
		 END		
			
		end
		 Close My_cursor
         Deallocate My_cursor				    
	 
	end
	ELSE IF(@AutoId >0)
	begin
	
	UPDATE [tbl_QuoteRequestHeader]
    SET 
      [OrderStatus] = @OrderStatus
      ,[Remarks] = @Remarks
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedOn] = GETDATE()  WHERE AutoId=@AutoId
	
	IF @@ERROR <> 0 
		BEGIN	
		  SET @ErrorStatus =@@ERROR
		  SET @NewQuoteNo =''
		  ROLLBACK TRANSACTION
		 END
	end
	
	IF @@ERROR <> 0 
			BEGIN	
			SET @ErrorStatus =@@ERROR
			SET @NewQuoteNo =''	  
			ROLLBACK TRANSACTION
			END
			Else
			Begin
			SET @ErrorStatus = @@ERROR
			if(@AutoId =0)
			begin
			SET @NewQuoteNo =	@QuoteNo
			end
			else if(@AutoId > 0)
			begin
			SET @NewQuoteNo =	'0'
			end
			Commit Transaction  
			End
    
   GO
		
			
				  
		