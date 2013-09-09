

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tbl_QuoteRequestDetail_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tbl_QuoteRequestDetail] DROP CONSTRAINT [DF_tbl_QuoteRequestDetail_CreatedOn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tbl_QuoteRequestDetail_ModifiedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tbl_QuoteRequestDetail] DROP CONSTRAINT [DF_tbl_QuoteRequestDetail_ModifiedOn]
END

GO



/****** Object:  Table [dbo].[tbl_QuoteRequestDetail]    Script Date: 08/21/2013 21:08:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_QuoteRequestDetail]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_QuoteRequestDetail]
GO



/****** Object:  Table [dbo].[tbl_QuoteRequestDetail]    Script Date: 08/21/2013 21:08:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_QuoteRequestDetail](
	[AutoId] [bigint] IDENTITY(1,1) NOT NULL,
	[QuoteRequestId] [bigint] NOT NULL,
	[LineNo] [numeric](18, 0) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Thickness] [numeric](18, 2) NOT NULL,
	[Gauge] [numeric](18, 2) NOT NULL,
	[WidthInMM] [numeric](18, 2) NOT NULL,
	[WidthInInch] [numeric](18, 3) NOT NULL,
	[LengthInMtr] [numeric](18, 2) NOT NULL,
	[LengthInFt] [numeric](18, 3) NOT NULL,
	[QtyInKg] [numeric](18, 2) NOT NULL,
	[QtyInLbs] [numeric](18, 2) NOT NULL,
	[NoOfRolls] [numeric](18, 0) NOT NULL,
	[WarehouseId] [numeric](18, 0) NOT NULL,
	[ActiveStatus] [bit] NOT NULL,
	[CreatedBy] [numeric](18, 0) NOT NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [numeric](18, 0) NULL,
	[ModifiedOn] [datetime] NULL,
	[Core] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_QuoteRequestDetail] PRIMARY KEY CLUSTERED 
(
	[AutoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_QuoteRequestDetail] ADD  CONSTRAINT [DF_tbl_QuoteRequestDetail_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[tbl_QuoteRequestDetail] ADD  CONSTRAINT [DF_tbl_QuoteRequestDetail_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO


