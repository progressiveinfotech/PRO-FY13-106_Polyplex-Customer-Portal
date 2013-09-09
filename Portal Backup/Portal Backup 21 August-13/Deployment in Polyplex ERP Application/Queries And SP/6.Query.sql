--select * from Com_UserType_Mst
Insert into Com_UserType_Mst(Type,Description,ActiveStatus,CreatedBy,CreatedOn )
Values('General','General',1,1,GETDATE ())

Insert into Com_UserType_Mst(Type,Description,ActiveStatus,CreatedBy,CreatedOn )
Values('C_Owner','C_Owner',1,1,GETDATE ())

Insert into Com_UserType_Mst(Type,Description,ActiveStatus,CreatedBy,CreatedOn )
Values('AR_Owner','AR_Owner',1,1,GETDATE ())

Insert into Com_UserType_Mst(Type,Description,ActiveStatus,CreatedBy,CreatedOn )
Values('Customer','Customer',1,1,GETDATE ())

update Com_User_Mst set UserTypeId =(select UserTypeId from Com_UserType_Mst where Type ='General')
GO

INSERT INTO Com_Group_Mst (Group_Name,Description ,Active ) VALUES ('Customer','Customer',1)
INSERT INTO Com_Group_Mst (Group_Name,Description ,Active ) VALUES ('C_Owner','C_Owner',1)
INSERT INTO Com_Group_Mst (Group_Name,Description ,Active ) VALUES ('AR_Owner','AR_Owner',1)
GO