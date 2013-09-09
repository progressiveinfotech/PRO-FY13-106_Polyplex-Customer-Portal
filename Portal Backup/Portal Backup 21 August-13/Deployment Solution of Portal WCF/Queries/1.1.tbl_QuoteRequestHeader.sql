

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tbl_QuoteRequestHeader_OrderDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tbl_QuoteRequestHeader] DROP CONSTRAINT [DF_tbl_QuoteRequestHeader_OrderDate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tbl_QuoteRequestHeader_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tbl_QuoteRequestHeader] DROP CONSTRAINT [DF_tbl_QuoteRequestHeader_CreatedOn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tbl_QuoteRequestHeader_ModifiedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tbl_QuoteRequestHeader] DROP CONSTRAINT [DF_tbl_QuoteRequestHeader_ModifiedOn]
END

GO



/****** Object:  Table [dbo].[tbl_QuoteRequestHeader]    Script Date: 08/21/2013 21:07:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_QuoteRequestHeader]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_QuoteRequestHeader]
GO



/****** Object:  Table [dbo].[tbl_QuoteRequestHeader]    Script Date: 08/21/2013 21:07:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_QuoteRequestHeader](
	[AutoId] [bigint] IDENTITY(1,1) NOT NULL,
	[Series] [numeric](18, 0) NULL,
	[QuoteRequestNo] [nvarchar](50) NOT NULL,
	[PINo] [nvarchar](50) NULL,
	[OrderConfNo] [nvarchar](50) NULL,
	[CustomerGroup] [numeric](18, 0) NOT NULL,
	[CustomerAccNo] [numeric](18, 0) NOT NULL,
	[WareHouseId] [numeric](18, 0) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[OrderStatus] [nvarchar](1) NULL,
	[Remarks] [nvarchar](50) NULL,
	[ActiveStatus] [bit] NULL,
	[CreatedBy] [numeric](18, 0) NOT NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [numeric](18, 0) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_tbl_QuoteRequestHeader] PRIMARY KEY CLUSTERED 
(
	[AutoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_QuoteRequestHeader] ADD  CONSTRAINT [DF_tbl_QuoteRequestHeader_OrderDate]  DEFAULT (getdate()) FOR [OrderDate]
GO

ALTER TABLE [dbo].[tbl_QuoteRequestHeader] ADD  CONSTRAINT [DF_tbl_QuoteRequestHeader_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[tbl_QuoteRequestHeader] ADD  CONSTRAINT [DF_tbl_QuoteRequestHeader_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO


