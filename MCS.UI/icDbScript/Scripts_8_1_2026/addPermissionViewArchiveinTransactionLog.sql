-- 1️⃣ Insert into Lookups (auto-generates Id)
INSERT INTO [Lookups] 
([CategoryId], [IsActive], [Sort], [EnumReference], [CreatedOn], [CreatedBy], [ModefiedOn], [ModefiedBy]) 
VALUES 
(23, 1, 24, 27, GETDATE(), 1, NULL, NULL);

-- Capture the generated Lookup Id
DECLARE @LookupId INT = SCOPE_IDENTITY();

-- 2️⃣ Insert localized names for Arabic and English
INSERT INTO [LookupLocalizations] 
([Text], [CreatedOn], [CreatedBy], [ModefiedOn], [ModefiedBy], [Culture_Id], [Lookup_Id]) 
VALUES 
(N'عرض سجل الأرشيف في سجل المعاملة', GETDATE(), 1, NULL, NULL, 1, @LookupId),
(N'View the archive record in the transaction log', GETDATE(), 1, NULL, NULL, 2, @LookupId);

-- 3️⃣ Insert into Permissions (auto-generates Id)
INSERT INTO [Permissions] 
([Code], [IsUserDefined], [Weight], [CreatedOn], [CreatedBy], [ModefiedOn], [ModefiedBy], [Name_Id]) 
VALUES 
(N'IC.ViewArchiveInTransactionLog', 0, NULL, GETDATE(), 1, NULL, NULL, @LookupId);

-- Capture the generated Permission Id
DECLARE @PermissionId INT = SCOPE_IDENTITY();

-- 4️⃣ Insert into GroupPermissions
INSERT INTO [GroupPermissions] ([Group_Id], [Permission_Id]) 
VALUES (1093, @PermissionId);