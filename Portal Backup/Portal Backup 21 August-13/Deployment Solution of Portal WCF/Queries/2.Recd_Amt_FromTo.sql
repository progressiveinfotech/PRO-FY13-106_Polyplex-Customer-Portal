

/****** Object:  UserDefinedFunction [dbo].[Recd_Amt_FromTo]    Script Date: 08/21/2013 20:41:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Recd_Amt_FromTo]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Recd_Amt_FromTo]
GO



/****** Object:  UserDefinedFunction [dbo].[Recd_Amt_FromTo]    Script Date: 08/21/2013 20:41:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Lalit Joshi
-- Create date: 07-31-2013
-- Description:	Receivables Ageing
-- =============================================
Create FUNCTION [dbo].[Recd_Amt_FromTo] 
(
	-- Add the parameters for the function here
	@Ino as VarChar(25),
	@FD as SmallDateTime,
	@TD as SmallDateTime
)
RETURNS Decimal(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result Decimal(18,2)
	Set @Result = 0


	Declare @Xno as VarChar(25)
	Declare @Xamt as Decimal(18,2)
	Declare @Xdt as SmallDateTime
	-- Add the T-SQL statements to compute the return value here

	DECLARE test CURSOR FOR Select VrNo as Vno, (AmtAdj + EPDsc + MiscDsc + CrAdj + BnkChrg) as TAdj, VrDt
	From TblDebtAdj where InvoiceNo = @Ino

	OPEN test

	FETCH NEXT FROM test INTO @Xno, @Xamt, @Xdt

	WHILE @@FETCH_STATUS = 0
		begin
			--Set @Xdt = (Select top 1 VrDt From TblGLTrn Where (Vtype + Vnm) = @Xno)
 			if @Xdt between @FD and @TD
				Begin
					Set @Result = @Result + @Xamt
				End
		   -- Get the next Payment Receipt For the given Invoice
			FETCH NEXT FROM test INTO @Xno, @Xamt, @Xdt
		end
	CLOSE test
	DEALLOCATE test

	-- Return the result of the function
	RETURN @Result
END



GO


