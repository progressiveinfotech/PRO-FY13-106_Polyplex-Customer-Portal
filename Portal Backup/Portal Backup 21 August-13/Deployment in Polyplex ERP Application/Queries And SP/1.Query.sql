
INSERT [dbo].[Com_Module_Mst] ([ModuleName], [SubModuleName], [FormName], [FormURL], [AccessLevel], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [ActiveStatus], [LocationID]) VALUES
 (N'Admin', N'General', N'ADG01 - Customer User Mapping', N'Commonforms/CustomerUserMapping.aspx', N'Local', CAST(1 AS Numeric(18, 0)), NULL, NULL, NULL, 1, NULL)
INSERT [dbo].[Com_Module_Mst] ([ModuleName], [SubModuleName], [FormName], [FormURL], [AccessLevel], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [ActiveStatus], [LocationID]) VALUES
 (N'Admin', N'General', N'ADG02 - Warehouse Mapping', N'Commonforms/WarehouseMapping.aspx', N'Local', CAST(1 AS Numeric(18, 0)), NULL, NULL, NULL, 1, NULL)
GO
 
 
 INSERT [dbo].[Com_Group_Module_Mapping] ( [GroupId], [ModuleFormID], [Read], [Write], [Modify], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
 VALUES ( CAST(1 AS Numeric(18, 0)), (select ModuleFormID from Com_Module_Mst where FormName ='ADG01 - Customer User Mapping'), 1, 1, 1, NULL, NULL, NULL, NULL)

INSERT [dbo].[Com_Group_Module_Mapping] ( [GroupId], [ModuleFormID], [Read], [Write], [Modify], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES
 ( CAST(1 AS Numeric(18, 0)), (select ModuleFormID from Com_Module_Mst where FormName ='ADG02 - Warehouse Mapping'), 1, 1, 1, NULL, NULL, NULL, NULL)
Go