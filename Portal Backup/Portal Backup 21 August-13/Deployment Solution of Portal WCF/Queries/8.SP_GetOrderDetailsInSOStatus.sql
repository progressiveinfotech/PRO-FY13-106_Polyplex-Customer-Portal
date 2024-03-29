
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetOrderDetailsInSOStatus]') AND type in (N'P', N'PC'))
DROP PROC SP_GetOrderDetailsInSOStatus
GO

/****** Object:  StoredProcedure [dbo].[SP_GetOrderDetailsInSOStatus]    Script Date: 08/21/2013 20:55:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetOrderDetailsInSOStatus]    

     @SONo nvarchar(50),
     @LineNo int,
	 @Type nvarchar(50)
 
AS

BEGIN TRANSACTION

	SET NOCOUNT ON;
   
   BEGIN
                
        if(@Type ='OrderQty')
        BEGIN
        
        Select f.FilmTypeCode, a.Gauge, a.Thickness,a.SOCore,a.SOWidthInMM,a.SOWidthInInch,a.SOLengthInMtr,a.SOLengthInFt,a.SOReqQuantityInKG,
        a.SOReqQuantityInLBS,a.SONoOfRolls       
            from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on a.SalesOrderId =b.SalesOrderId 
            inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
            where a.ActiveStatus = 1 and b.ActiveStatus = 1 and b.SONo =@SONo and a.[LineNo]=@LineNo
        
        END
        else if(@Type ='ProducedQty')
        BEGIN
        
        Select f.FilmTypeCode, a.Gauge, a.Thickness,a.SOCore,a.SOWidthInMM,a.SOWidthInInch,a.SOLengthInMtr,a.SOLengthInFt,a.SOReqQuantityInKG,
        a.SOReqQuantityInLBS,a.SONoOfRolls       
            from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on a.SalesOrderId =b.SalesOrderId 
            inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
            where a.ActiveStatus = 1 and b.ActiveStatus = 1 and b.SONo =@SONo and a.[LineNo]=@LineNo
        
        END
        else if(@Type ='PlannedDispatchQty')
        BEGIN
        
        Select distinct f.FilmTypeCode, a.Gauge, a.Thickness,a.SOCore,a.SOWidthInMM,a.SOWidthInInch,a.SOLengthInMtr,a.SOLengthInFt,a.SOReqQuantityInKG,
        a.SOReqQuantityInLBS,a.SONoOfRolls ,(select RRNumber from Sal_RollReservation_Tran where AutoID =C.RollReservationID) as PickListNo    
            from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on a.SalesOrderId =b.SalesOrderId 
            inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
            inner join Prod_RollSlitting_Details as C on b.SONo = C.SalesOrdNo
            where a.ActiveStatus = 1 and b.ActiveStatus = 1
            and (C.ISS_PAC is not null or C.ISS_PAC <>0) and C.RollReservationID > 0
             and b.SONo =@SONo and a.[LineNo]=@LineNo
        
        END
        else if(@Type ='DispatchedQty')
        BEGIN
        
        Select distinct f.FilmTypeCode, a.Gauge, a.Thickness,a.SOCore,a.SOWidthInMM,a.SOWidthInInch,a.SOLengthInMtr,a.SOLengthInFt,a.SOReqQuantityInKG,
        a.SOReqQuantityInLBS,a.SONoOfRolls ,(select InvNo from Sal_RollReservation_Tran where AutoID =C.RollReservationID) as PackingListNo,
        (select replace(CONVERT(VARCHAR(11),ISS_DATE ,101),' ','/')  from Sal_RollReservation_Tran where AutoID =C.RollReservationID) as PackingListDate    
            from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on a.SalesOrderId =b.SalesOrderId 
            inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
            inner join Prod_RollSlitting_Details as C on b.SONo = C.SalesOrdNo
            where a.ActiveStatus = 1 and b.ActiveStatus = 1
            and (C.ISS_PAC is not null or C.ISS_PAC <>0) and C.RollReservationID > 0
             and b.SONo =@SONo and a.[LineNo]=@LineNo
        
        END
        else if(@Type ='InvoicedQty')
        BEGIN
        
        Select distinct f.FilmTypeCode, a.Gauge, a.Thickness,a.SOCore,a.SOWidthInMM,a.SOWidthInInch,a.SOLengthInMtr,a.SOLengthInFt,a.SOReqQuantityInKG,
        a.SOReqQuantityInLBS,a.SONoOfRolls ,        
        (select InvNo from Sal_RollReservation_Tran where AutoID =C.RollReservationID) as InvoiceNo,
        (select replace(CONVERT(VARCHAR(11),InvoiceDate ,101),' ','/') from Sal_Glb_Invoice_Tran where InvoiceNo =(select InvNo 
        from Sal_RollReservation_Tran where AutoID =C.RollReservationID)) as InvoiceDate
        ,a.SOUnitPrice,ROUND(CAST(( a.SOUnitPrice * a.SOReqQuantityInKG) as decimal(18,2)),2) as ExtendedValue  
            from Sal_Glb_OrderLineItem a inner join Sal_Glb_OrderInformations b on a.SalesOrderId =b.SalesOrderId 
            inner join Com_FilmType_Mst f on a.filmtype = f.filmtypeid
            inner join Prod_RollSlitting_Details as C on b.SONo = C.SalesOrdNo
            where a.ActiveStatus = 1 and b.ActiveStatus = 1
            and (C.ISS_PAC is not null or C.ISS_PAC <>0) and C.RollReservationID > 0
             and b.SONo =@SONo and a.[LineNo]=@LineNo
        
        END
   END    
 
GO