

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblCustomerUserMapping_ActiveStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblCustomerUserMapping] DROP CONSTRAINT [DF_tblCustomerUserMapping_ActiveStatus]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblCustomerUserMapping_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblCustomerUserMapping] DROP CONSTRAINT [DF_tblCustomerUserMapping_CreatedOn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblCustomerUserMapping_ModifiedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblCustomerUserMapping] DROP CONSTRAINT [DF_tblCustomerUserMapping_ModifiedOn]
END

GO



/****** Object:  Table [dbo].[tblCustomerUserMapping]    Script Date: 08/21/2013 20:14:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblCustomerUserMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tblCustomerUserMapping]
GO



/****** Object:  Table [dbo].[tblCustomerUserMapping]    Script Date: 08/21/2013 20:14:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCustomerUserMapping](
	[AutoId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [numeric](18, 0) NOT NULL,
	[CustomerId] [numeric](18, 0) NOT NULL,
	[CustomerGroupId] [numeric](18, 0) NOT NULL,
	[ActiveStatus] [bit] NOT NULL,
	[CreatedBy] [numeric](18, 0) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [numeric](18, 0) NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tblCustomerUserMapping] PRIMARY KEY CLUSTERED 
(
	[AutoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblCustomerUserMapping] ADD  CONSTRAINT [DF_tblCustomerUserMapping_ActiveStatus]  DEFAULT ((1)) FOR [ActiveStatus]
GO

ALTER TABLE [dbo].[tblCustomerUserMapping] ADD  CONSTRAINT [DF_tblCustomerUserMapping_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[tblCustomerUserMapping] ADD  CONSTRAINT [DF_tblCustomerUserMapping_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO


