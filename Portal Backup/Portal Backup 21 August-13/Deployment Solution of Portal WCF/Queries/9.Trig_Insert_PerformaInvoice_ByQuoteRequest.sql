

/****** Object:  Trigger [Trig_Insert_PerformaInvoice_ByQuoteRequest]    Script Date: 08/21/2013 21:14:11 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Trig_Insert_PerformaInvoice_ByQuoteRequest]'))
DROP TRIGGER [dbo].[Trig_Insert_PerformaInvoice_ByQuoteRequest]
GO



/****** Object:  Trigger [dbo].[Trig_Insert_PerformaInvoice_ByQuoteRequest]    Script Date: 08/21/2013 21:14:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Trigger [dbo].[Trig_Insert_PerformaInvoice_ByQuoteRequest] ON [dbo].[tbl_QuoteRequestHeader]
For Update
AS
     Begin Transaction
     Begin  --------------Declare Variables  ----------------------------------------------------------
     
     Declare @QuoteRequestNo varchar(50)
	 Declare @QuoteRequestAutoId numeric(18,0)
     Declare @Customer as numeric(18,0)
     Declare @OrderStatus nvarchar(1)
     
     DECLARE @ProformaInvoiceSeries numeric(18, 0)
     DECLARE @PINo as  nvarchar(50)
     DECLARE @Moduleformid as nvarchar(50)
     DECLARE @Output nvarchar(50)
     DECLARE @PITypeName nvarchar(50) 
     Declare @OrganisationId numeric(18,0)
	 Declare @FStartYear nvarchar(20)
	 Declare @FEndYear nvarchar(20)
	 Declare @Year nvarchar(20)
	 Declare @SalesOrganization nvarchar(20)
	 Declare @SalesType nvarchar(20)
	 Declare @SalesArea numeric(18,0)
	 Declare @DistributionChannelCode nvarchar(20)
	 Declare @Consignee as numeric(18,0)
	 Declare @RecCount as Numeric(18,0)
	 Declare @DeliveryTo as numeric(18,0)
	 Declare @FinalDestination as numeric(18,0)
	 Declare @DeliveryTolerance as numeric(18,0)
     Declare @TermsOfDelivery as numeric(18,0)
     Declare @TermsofDeliveryCity as nvarchar(100)
     Declare @Logistics as  numeric(18,0)
     Declare @Currency as numeric(18,0)
	 Declare @PickingLocation numeric(18,0)
	 Declare @TransitDays numeric(18,0)=0
	 Declare @CommittedETD as datetime
     Declare @CommittedETA as datetime
     Declare @PaymentMode as numeric(18,0)  
     Declare @PackingStandard as numeric(18,0)  
     Declare @PaymentTerms as numeric(18,0)
     Declare @CreditDays as numeric(18,0)
     Declare @ProfitCenter as nvarchar(20)
     Declare @LengthTolerence as numeric(18,2)
     Declare @CreatedBy as numeric(18,0)
     Declare @UOM as  Varchar(10)
     Declare @SourceLeadtime numeric(18,0)
     Declare @DeliveryLeadtime numeric(18,0)
     Declare @DeliveryDescription numeric(18,0)
     
     
     Declare @PerformaInvoiceID as  numeric(18,0)  
     Declare @Mil numeric(18,2)  
     Declare @UnitPrice numeric(18,2)  
     Declare @FilmTypeId numeric(18,0)  
     Declare @Thickness numeric(18,0)
     
	 End
 
          
 	 Select @QuoteRequestAutoId=HeaderTable.AutoId from inserted HeaderTable
	 Select @Customer=HeaderTable.CustomerAccNo from inserted HeaderTable
	 Select @CreatedBy=HeaderTable.CreatedBy from inserted HeaderTable
	 Select @QuoteRequestNo=HeaderTable.QuoteRequestNo from inserted HeaderTable
	 Select @OrderStatus=HeaderTable.OrderStatus from inserted HeaderTable
	 
	 Set @RecCount=isnull((Select COUNT(*) from Sal_Glb_PerformaInvoiceHeader_Tran where QuoteRequestId=@QuoteRequestAutoId),0)
	  
	 
	 if @OrderStatus='N' or @OrderStatus='P' or @RecCount>0 ----N means Not approved and P means Pending
	 Commit Transaction
	 Return	  
		 
     Begin  ------------- Get Organization and FinancialYear ------------------------------------------
     
     Set @OrganisationId=(Select top 1 AutoId from Com_SalesOrgnization_Mst)
	 Set @SalesOrganization=(Select distinct top 1 SalesOrgDesc from Com_SalesOrgnization_Mst Where ActiveStatus ='1' and AutoId =@OrganisationId)
	 Set @FStartYear=(Select FinancialStartYear from Com_SalesOrgnization_Mst Where ActiveStatus ='1' and AutoId =@OrganisationId)
	 Set @FEndYear=(Select FinancialEndYear from Com_SalesOrgnization_Mst Where ActiveStatus ='1' and AutoId =@OrganisationId)
	 Set @Year= SUBSTRING(@FStartYear,3,4) + '-' + SUBSTRING(@FEndYear,3,4)
	
	 ----------------------------------------------------------------------------------------------------
	 End
     
     Begin  --------------Get ModuleForm Id,Sales Type,Sales Area,Distribution Channel,Consignee-------
     
     Set @Moduleformid=(select ModuleFormID from Com_Module_Mst where FormURL='Sales/ProformaInvoice.aspx')
     SET @ProformaInvoiceSeries = (select  MAX(ProformaInvoiceSeries) from [Sal_Glb_PerformaInvoiceHeader_Tran] where [Year]=@Year)+1
     set @PITypeName='Sample'
     Set @SalesType=(Select top 1 SalesTypeId from Com_SalesType_Mst where SalesTypeCode='S' and ActiveStatus=1)
     Set @SalesArea=(SELECT distinct top 1 SalesArea from Sal_Glb_Customer_Mst_Sales as S inner join Com_SalesArea_Mst as C on S.SalesArea = C.AutoId  
                     Where CustomerId =1 and C.ActiveStatus ='1')
     Set @DistributionChannelCode= (Select distinct top 1 DCName from view_salesArea where autoid=@SalesArea)
     Set @Consignee= (Case When (SELECT Count(ConsigneeToID) from Com_ConsigneeTo_Mst where CustomerID =@Customer and ActiveStatus ='1')=0
                      then 0 else (SELECT top 1 ConsigneeToID from Com_ConsigneeTo_Mst where CustomerID =@Customer and ActiveStatus ='1') end)
     End
   
     Begin  --------------Get DeliveryTo,Final Destination---------------------------------------------
     
     Set @RecCount = (select Count(DeliveryToID) from Com_DeliveryTo_Mst where ActiveStatus=1 and CustomerID=@Customer)
     if @RecCount > 0
     begin
     Set @DeliveryTo = (Select top 1 DeliveryToID from Com_DeliveryTo_Mst where ActiveStatus=1 and CustomerID=@Customer)
     end
     else 
     begin
     Set @DeliveryTo=0
     End
     
  
    
    set @RecCount=(SELECT COUNT(*) FROM Sal_Glb_FinalDestination_Mst as F 
                   where FinalDestinationID in(select FinalDestinationID from Com_DeliveryTo_Mst where CustomerID=@Customer))
                   
    if @RecCount>0
    begin
    Set @FinalDestination = (SELECT top 1 FinalDestinationID FROM Sal_Glb_FinalDestination_Mst as F 
                             where FinalDestinationID in(select FinalDestinationID from Com_DeliveryTo_Mst where CustomerID=@Customer))
    end
    else
    begin
    Set @RecCount= (SELECT COUNT(*) FROM Sal_Glb_FinalDestination_Mst as F inner join Sal_Glb_Customer_Mst_Sales as S on
                     F.FinalDestinationID =S.FinalDestination and S.CustomerId =@Customer where S.ActiveStatus ='1')
                  
    if @RecCount>0
    begin
    Set @FinalDestination = (SELECT top 1 FinalDestinationID FROM Sal_Glb_FinalDestination_Mst as F inner join Sal_Glb_Customer_Mst_Sales 
                             as S on F.FinalDestinationID =S.FinalDestination and S.CustomerId =@Customer where S.ActiveStatus ='1')               
    end
    else
    begin
    Set @FinalDestination = (SELECT top 1 FinalDestinationID FROM Sal_Glb_FinalDestination_Mst where Active='1')
    end
    end
      
    End
    
     Begin  --------------Get Delivery Tolerance,Terms Of Delivery, Delivery City----------------------
     
     if((SELECT distinct top 1 DeliveryTolerance FROM [Sal_Glb_Customer_Mst_Sales] as C inner join Sal_Glb_Customer_Mst as S 
                 on C.CustomerId = S.CustomerID Where S.CustomerId =@Customer and SalesArea =@SalesArea))=''    
     Set @DeliveryTolerance=0
     else
     Set @DeliveryTolerance=(SELECT distinct top 1 DeliveryTolerance FROM [Sal_Glb_Customer_Mst_Sales] as C inner join Sal_Glb_Customer_Mst as S 
                 on C.CustomerId = S.CustomerID Where S.CustomerId =@Customer and SalesArea =@SalesArea)

     Set @TermsOfDelivery = (SELECT  distinct top 1 TermsOfDeliveryId FROM Com_TermsOfDelivery_Mst as T 
                             inner join Sal_Glb_Customer_Mst_Sales as S on T.TermsOfDeliveryId=CAST(S.TermsOfDelivery as int)
                             and S.CustomerId =@Customer where S.ActiveStatus ='1')
      
     Set @TermsofDeliveryCity= (SELECT distinct top 1 TermsOfDeliveryDesc FROM [Sal_Glb_Customer_Mst_Sales] as C inner join Sal_Glb_Customer_Mst as S 
                 on C.CustomerId = S.CustomerID Where S.CustomerId =@Customer and SalesArea =@SalesArea)
     
     End
     
     Begin  --------------Get Logistics,Currency,PickingLocation,CommitedETD,ETA,Payment Mode----------
     Set @Logistics= (SELECT top 1 LogisticID FROM Sal_Glb_Logistic_Mst WHERE ActiveStatus ='1')
     
     Set @Currency= (SELECT top 1 CurrencyID from Com_Currency_Mst as C inner join Sal_Glb_Customer_Mst_Sales as S 
                     on C.CurrencyID=S.Currency and S.CustomerId =@Customer where S.ActiveStatus ='1')
                     
     Set @PickingLocation = (SELECT top 1 autoid From Prod_StorageLocation_Mst where FGS=1 and Status=1)                
     
     --Set @RecCount = (Select COUNT(*)  from Com_Route_Mst A Inner Join Sal_Glb_FinalDestination_Mst B
     --                       On A.FinalDestinationID = B.FinalDestinationID Inner Join Prod_StorageLocation_Mst C On A.StorageLocationID = C.autoid 
     --                       Where A.FinalDestinationID =@FinalDestination and A.StorageLocationID =@PickingLocation)
    
     Set @TransitDays =  isnull((Select distinct top 1  IsNull(B.TransitDays,0) As TransitDays from Com_Route_Mst A Inner Join Sal_Glb_FinalDestination_Mst B
                            On A.FinalDestinationID = B.FinalDestinationID Inner Join Prod_StorageLocation_Mst C On A.StorageLocationID = C.autoid 
                            Where A.FinalDestinationID =@FinalDestination and A.StorageLocationID =@PickingLocation),0)
                         
  
     Set @SourceLeadtime = isnull((Select distinct top 1 IsNull(C.SourceLeadTime,0) As SourceLeadTime from Com_Route_Mst A Inner Join Sal_Glb_FinalDestination_Mst B
                            On A.FinalDestinationID = B.FinalDestinationID Inner Join Prod_StorageLocation_Mst C On A.StorageLocationID = C.autoid 
                            Where A.FinalDestinationID =@FinalDestination and A.StorageLocationID =@PickingLocation),0)
     
     Set @CommittedETD =  (Select DATEADD(Day,(@SourceLeadtime),GETDATE()))
   
     Set @CommittedETA =   (Select DATEADD(Day,(@SourceLeadtime+@TransitDays),GETDATE()))
      
     Set @PaymentMode = isnull((SELECT distinct top 1 PayModeID from Com_Paymode_Mst as C inner join Sal_Glb_Customer_Mst_Sales as S on C.PayModeID=CAST(S.PaymentMode as int)
                        and S.CustomerId =@Customer),0)
     End
     
     Begin  ------------Get Packing Standard,Payment terms,Credit Days,Profit Center-------------------
     
     Set @PackingStandard= isnull((SELECT Distinct Top 1 Packing FROM Sal_Glb_Customer_Mst_Sales as C inner join Sal_Glb_Customer_Mst as S on C.CustomerId = S.CustomerID
                         where S.CustomerId =@Customer and SalesArea =@SalesArea),0)
     
     Set @PaymentTerms= isnull((SELECT distinct Top 1 PaymentsTermsID from Sal_Glb_PaymentTerms_Mst as P inner join 
                        Sal_Glb_Customer_Mst_Sales as S on P.PaymentsTermsID = S.PaymentTerms and S.CustomerId =@Customer where S.ActiveStatus ='1' ),0)
   
     Set @CreditDays =(Select creditdays from Sal_Glb_PaymentTerms_Mst where PaymentsTermsID=@PaymentTerms)
  
     Set @ProfitCenter= (Select PCName from view_salesArea where autoid=@SalesArea)
    
     Set @LengthTolerence= Case When (SELECT distinct top 1 LengthTolerance FROM [Sal_Glb_Customer_Mst_Sales] as C inner join Sal_Glb_Customer_Mst as S 
                             On C.CustomerId = S.CustomerID Where S.CustomerId =1 and SalesArea =1) ='' then 0 else
                             (SELECT distinct top 1 LengthTolerance FROM [Sal_Glb_Customer_Mst_Sales] as C inner join Sal_Glb_Customer_Mst as S 
                             On C.CustomerId = S.CustomerID Where S.CustomerId =1 and SalesArea =1) end
           
     Set @UOM=(Case When (select ISNULL(UOM,'') as UOM from Sal_Glb_Customer_Mst where CustomerID=221) = '' 
                Then (Select top 1 AutoId from Proc_UOM_Master where UOM_Type = 'QT') Else
                (select ISNULL(UOM,'') as UOM from Sal_Glb_Customer_Mst where CustomerID=221)
                 End )
     End
    
     Begin --------------Get SourceLeadTime, DeliveryLeadTime, DeliveryDescription-------------------- 
     
     Set @DeliveryLeadtime= (@SourceLeadtime + @TransitDays)
     Set @DeliveryDescription= isnull((Select RouteID  from Com_Route_Mst where FinalDestinationID=@FinalDestination and StorageLocationID=@PickingLocation),0)
                                  
     End
     
     IF(@ProformaInvoiceSeries IS NOT NULL)
     BEGIN
		  exec Sp_GetNumberSeries_Withoutput_ForCriteria @Moduleformid,@Year,@PITypeName,'0',@Output output
          set @PINo= 'PI'+@Year+@Output	
          set @Output=@Output 
          
     END
     ELSE
	 BEGIN
		if @PITypeName='Domestic'
		begin
		set @Output='11000000'
		set @PINo= 'PI'+@Year+'11000000'
		end
	    if @PITypeName='Export'
	    begin
	    set @PINo= 'PI'+@Year+'12000000'
		set @Output='12000000'
		end
	    if @PITypeName='Sample'
	    begin
	  	set @PINo= 'PI'+@Year+'13000000'
		set @Output='13000000'
		end
		if @PITypeName='Tolling'
		begin
	    set @PINo= 'PI'+@Year+'14000000'
		set @Output='14000000'
		end
		if @PITypeName='Consignment'
		begin
	    set @PINo= 'PI'+@Year+'15000000'
		set @Output='15000000'
		end
		set @ProformaInvoiceSeries=1
		END
     
     INSERT INTO [Sal_Glb_PerformaInvoiceHeader_Tran]
       ([ProformaInvoiceSeries]
       ,[PINo]
       ,[Year]
       ,[PIDate]
       ,[SalesOrganization]
       ,SalesType
       ,[SalesArea]
       ,[DistributionChannelCode]
       ,[Customer]
       ,[Consignee]
       ,[DeliveryTo]
       ,[CustomerOrderNo]
       ,[FinalDestination]
       ,[DeliveryTolerance]
       ,[TermsOfDelivery]
       ,[TermsofDeliveryCity]
       ,[Logistics]
       ,[Currency]
       ,CommittedETD
       ,CommittedETA
       ,[PaymentMode]
       ,[NoOfShipment]
       ,[PackingStandard]
       ,[PaymentTerms]
       ,[CreditDays]
       ,[FilmCategory]
       ,[UnitOfSales]           
       ,[ProfitCenter]           
       ,[TypeofUnit]
       ,[PreInspection]
       ,[SpecialInstruction]
       ,[PortofShipment]
       ,[PortofDischarge]
       ,[LengthTolerence]
       ,[Header]           
       ,[ActiveStatus]
       ,[CreatedBy]
       ,[CreatedOn]           
       ,[StatusCode]
       ,[UOM]
       ,CommisionPercentage
       ,AgentCode
       ,PickUpLocationID
       ,SourceLeadTimeDays
       ,DeliveryLeadTimeDays
       ,RouteID
       ,QuoteRequestId
       )
 VALUES
       (@ProformaInvoiceSeries
       ,@PINo
       ,@Year
       ,GETDATE()
       ,@SalesOrganization
       ,@SalesType
       ,@SalesArea
       ,@DistributionChannelCode
       ,@Customer
       ,@Consignee
       ,@DeliveryTo
       ,''
       ,@FinalDestination
       ,@DeliveryTolerance
       ,@TermsOfDelivery
       ,@TermsofDeliveryCity
       ,@Logistics
       ,@Currency
       ,@CommittedETD
       ,@CommittedETA
       ,@PaymentMode
       ,0
       ,@PackingStandard
       ,@PaymentTerms
       ,@CreditDays
       ,0
       ,0
       ,@ProfitCenter
       ,'FPS'
       ,0
       ,''
       ,''
       ,''
       ,@LengthTolerence
       ,''
       ,1
       ,@CreatedBy
       ,GETDATE()           
       ,'Open'
       ,@UOM
       ,0.00
       ,''
       ,@PickingLocation
       ,@SourceLeadtime
       ,@DeliveryLeadtime
       ,@DeliveryDescription
       ,@QuoteRequestAutoId
    )	     
     
     Set @PerformaInvoiceID = (Select IDENT_CURRENT('Sal_Glb_PerformaInvoiceHeader_Tran'))
   
     Update Com_Series_Master set [status]=CAST(@Output as numeric(18,0)) Where YEAR=@Year and formid=@Moduleformid and 
                criteriaId=(select AutoID from Com_Series_Category Where CategoryDescription=@PITypeName)
     
     IF @@ERROR <> 0 
	 BEGIN	
	 ROLLBACK TRANSACTION
	 END

	 --Set @FilmTypeId=isnull((select FilmTypeID from Com_FilmType_Mst where FilmTypeCode in(Select [Type] from tbl_QuoteRequestDetail
	 --                Where QuoteRequestId=@QuoteRequestAutoId)),0)
	                
  --   Set @Thickness= isnull((Select top 1 Thickness from tbl_QuoteRequestDetail Where QuoteRequestId=@QuoteRequestAutoId),0)
       
	 --Set @UnitPrice = isnull((Select (Select dbo.Fun_GetConversion(UOMID,101,Price)) As PricInLBS
  --                       from Sal_OrgAreaCustThickType_Mapping_Tran A Inner Join Com_Thickness_Mst B On a.ThicknessId = b.ThicknessId
  --                      Where SalesAreaId = @SalesArea and FilmTypeId = @FilmTypeId and A.ActiveStatus = 1 
  --                      And GETDATE() between StartDate AND DateAdd(day,1,EndDate) And 
  --                      CurrencyId=@Currency And CustomerId=@Customer and cast(B.Thickness As Numeric(8,2)) = @Thickness ),0)
	 
	 INSERT INTO [Sal_Glb_PerformaInvoiceLineItem_Tran]
           ([PerformaInvoiceID]
           ,[LineNo]
           ,[FilmType]
           ,[Thickness]
           ,[Core]
           ,[WidthInMM]
           ,[WidthInInch]
           ,[LengthInMtr]
           ,[LengthInFt]
           ,[UnitPrice]
           ,[NoOfRolls]
           ,[ReqQuantityInKG]
           ,[ReqQuatityinLBS]
           ,[Application]
           ,[COFMin]
           ,[COFMax]
           ,[ODMin]
           ,[ODTarget]
           ,[ODMax]
           ,[ShipmentNo]          
           ,[NoofRollsInPallet]
           ,[CustomerArticalNo]
           ,[CustomerPartNo]
           ,[ActiveStatus]
           ,[OtherQuantity]
           ,[CreatedBy]
           ,[CreatedOn]           
           ,[StatusCode]
           ,SKU_No
           ,Gauge
           ,Mil)
     Select  @PerformaInvoiceID
            ,ObjReqDetail.[LineNo]
            ,isnull((select FilmTypeID from Com_FilmType_Mst where FilmTypeCode=ObjReqDetail.[Type]),0)
            ,ObjReqDetail.Thickness
            ,ObjReqDetail.Core
            ,ObjReqDetail.WidthInMM
            ,ObjReqDetail.WidthInInch
            ,ObjReqDetail.LengthInMtr
            ,ObjReqDetail.LengthInFt
            ,isnull((Select (Select dbo.Fun_GetConversion(UOMID,101,Price)) As PricInLBS
                         from Sal_OrgAreaCustThickType_Mapping_Tran A Inner Join Com_Thickness_Mst B On a.ThicknessId = b.ThicknessId
                        Where SalesAreaId = @SalesArea and FilmTypeId = (isnull((select FilmTypeID from Com_FilmType_Mst 
                        where FilmTypeCode=ObjReqDetail.[Type]),0)) 
                        and A.ActiveStatus = 1 
                        And GETDATE() between StartDate AND DateAdd(day,1,EndDate) And 
                        CurrencyId=@Currency And CustomerId=@Customer and cast(B.Thickness As Numeric(8,2)) = ObjReqDetail.Thickness ),0)
            ,ObjReqDetail.NoOfRolls
            ,ObjReqDetail.QtyInKg
            ,ObjReqDetail.QtyInLbs
            ,'1'
            ,0.00
            ,0.00
            ,0.00
            ,0.00
            ,0.00
            ,''          
            ,1
            ,''
            ,''
            ,1
            ,0.00
            ,@CreatedBy
            ,GETDATE()           
            ,'Open'
            ,''
            ,ObjReqDetail.Gauge
            ,isnull((ObjReqDetail.Gauge/100),0) from tbl_QuoteRequestDetail ObjReqDetail Where ObjReqDetail.QuoteRequestId=@QuoteRequestAutoId 
     IF @@ERROR <> 0 
     BEGIN	
	 ROLLBACK TRANSACTION
	 END
	 UPDATE tbl_QuoteRequestHeader Set PINo=@PINo where AutoId=@QuoteRequestAutoId
	 IF @@ERROR <> 0 
	 BEGIN	
	 ROLLBACK TRANSACTION
	 END	
     Else
	 BEGIN
	 COMMIT TRANSACTION
	 END

GO


