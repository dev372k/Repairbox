
-- Priority
INSERT INTO [dbo].[RepairPriorities] ([Name], [ProcessCharges], [CreatedAt], [IsActive], [IsDeleted]) 
	VALUES ('PANA LA 3 ZILE' ,10.0 ,'2023-05-14 00:00:00.000' ,1 ,0)

INSERT INTO [dbo].[RepairPriorities] ([Name], [ProcessCharges], [CreatedAt], [IsActive], [IsDeleted]) 
	VALUES ('Medium' ,10.0 ,'2023-05-14 00:00:00.000' ,1 ,0)

INSERT INTO [dbo].[RepairPriorities] ([Name], [ProcessCharges], [CreatedAt], [IsActive], [IsDeleted]) 
	VALUES ('High' ,10.0 ,'2023-05-14 00:00:00.000' ,1 ,0)

INSERT INTO [dbo].[RepairPriorities] ([Name], [ProcessCharges], [CreatedAt], [IsActive], [IsDeleted]) 
	VALUES ('Urgent' ,10.0 ,'2023-05-14 00:00:00.000' ,1 ,0)

INSERT INTO [dbo].[RepairPriorities] ([Name], [ProcessCharges], [CreatedAt], [IsActive], [IsDeleted]) 
	VALUES ('Rush (Next Day)' ,20.0 ,'2023-05-14 00:00:00.000' ,1 ,0)


-- Status
INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Ok','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Done','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Ok','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Collected','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Spark','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Work Started','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('InProgress','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Delivery','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Resolved','2023-05-14',1, 0) 

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Pending','2023-05-14',1, 0)

INSERT INTO [dbo].[RepairStatuses]([Name],[CreatedAt],[IsActive],[IsDeleted]) 
VALUES ('Open','2023-05-14',1, 0)

