

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblWarehouseMapping_ActiveStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblWarehouseMapping] DROP CONSTRAINT [DF_tblWarehouseMapping_ActiveStatus]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblWarehouseMapping_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblWarehouseMapping] DROP CONSTRAINT [DF_tblWarehouseMapping_CreatedOn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblWarehouseMapping_ModifiedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblWarehouseMapping] DROP CONSTRAINT [DF_tblWarehouseMapping_ModifiedOn]
END

GO



/****** Object:  Table [dbo].[tblWarehouseMapping]    Script Date: 08/21/2013 20:15:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblWarehouseMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tblWarehouseMapping]
GO



/****** Object:  Table [dbo].[tblWarehouseMapping]    Script Date: 08/21/2013 20:15:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblWarehouseMapping](
	[AutoId] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerId] [numeric](18, 0) NOT NULL,
	[PlantId] [numeric](18, 0) NOT NULL,
	[WareHouseId] [numeric](18, 0) NOT NULL,
	[ActiveStatus] [bit] NOT NULL,
	[CreatedBy] [numeric](18, 0) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [numeric](18, 0) NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tblWarehouseMapping] PRIMARY KEY CLUSTERED 
(
	[AutoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblWarehouseMapping] ADD  CONSTRAINT [DF_tblWarehouseMapping_ActiveStatus]  DEFAULT ((1)) FOR [ActiveStatus]
GO

ALTER TABLE [dbo].[tblWarehouseMapping] ADD  CONSTRAINT [DF_tblWarehouseMapping_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[tblWarehouseMapping] ADD  CONSTRAINT [DF_tblWarehouseMapping_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO


