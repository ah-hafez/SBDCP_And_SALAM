GO
/****** Object:  DatabaseRole [aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess]    Script Date: 1/24/2019 8:34:07 AM ******/
CREATE ROLE [aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess]
GO
/****** Object:  Schema [aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess]    Script Date: 1/24/2019 8:34:07 AM ******/
CREATE SCHEMA [aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess]
GO
/****** Object:  Schema [Reports]    Script Date: 1/24/2019 8:34:07 AM ******/
CREATE SCHEMA [Reports]
GO
/****** Object:  UserDefinedFunction [Reports].[GetPermissionWeightById]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Reports].[GetPermissionWeightById](@PermissionId int)

RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @weight int

SET @weight =(SELECT P.Weight
FROM 
Permissions AS P
WHERE
   P.Id =@PermissionId
);

	-- Return the result of the function
	RETURN @weight
END



GO
/****** Object:  UserDefinedFunction [Reports].[TransactionsCompletedCount]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Reports].[TransactionsCompletedCount](@OrgUnitId int,@UserId int = null,@TransactionTypeId int = null,@CurrentUserId int,@FromDateTime varchar(50),@ToDateTime varchar(50))

RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Count int;
	DECLARE @Weight int = null;


set @Weight =(Select MAX(P.Weight)

from  
Permissions AS P
INNER JOIN UserPermissions UP ON P.Id = UP.PermissionId
INNER JOIN GroupPermissions GP ON P.Id = GP.Permission_Id

where UP.UserProfileId = @CurrentUserId
AND GP.Group_Id = 1030 );


SET @Count =(SELECT COUNT(T.Id)
FROM 
Transactions AS T 


	INNER JOIN Lookups L ON T.StatusId = L.Id

	INNER JOIN TransactionAssignmentHistories TA ON T.Id = TA.TransactionId
	INNER JOIN UserProfiles UP ON TA.FromUserId = UP.Id
	INNER JOIN OrgUnits OU ON TA.FromEntityId = OU.Id


WHERE
     TA.FromEntityId = @OrgUnitId 

	 AND  (TA.FromUserId  =IsNull(@UserId,TA.FromUserId) or @UserId is null)
	 AND  (T.TransactionTypeId  =@TransactionTypeId or @TransactionTypeId is null)

	--AND TA.Date >= @FromDateTime And TA.Date <= @ToDateTime
	AND TA.Date between IsNull(Convert(datetime,@FromDateTime, 103),TA.Date) AND IsNull(Convert(datetime,@ToDateTime , 103),TA.Date)

	AND (Reports.GetPermissionWeightById(T.ConfidentialityId) <= @Weight And @Weight is not null )

);
	-- Return the result of the function
	RETURN @Count
END



GO
/****** Object:  UserDefinedFunction [Reports].[TransactionsCountByStatus]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [Reports].[TransactionsCountByStatus](@StatusId int,@OrgUnitId int,@UserId int,@FromDateTime DateTime,@ToDateTime DateTime)

RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Count int

	-- Add the T-SQL statements to compute the return value here
SET @Count =(SELECT COUNT(T.Id)
FROM 
Transactions AS T 


	INNER JOIN Lookups L ON T.StatusId = L.Id

	INNER JOIN TransactionAssignments TA ON T.Id = TA.TransactionId
	INNER JOIN UserProfiles UP ON TA.ToUserId = UP.Id
	INNER JOIN OrgUnits OU ON TA.ToEntityId = OU.Id


WHERE
     TA.ToEntityId = @OrgUnitId AND TA.ToUserId = @UserId

	AND TA.Date >= @FromDateTime And TA.Date <= @ToDateTime

	AND T.StatusId = @StatusId

);

	-- Return the result of the function
	RETURN @Count

END


GO
/****** Object:  UserDefinedFunction [Reports].[TransactionsInProgressCount]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Reports].[TransactionsInProgressCount](@OrgUnitId int,@UserId int=null,@TransactionTypeId int = null,@CurrentUserId int,@FromDateTime varchar(50),@ToDateTime varchar(50))

RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Count int;
	DECLARE @Weight int = null;


set @Weight =(Select MAX(P.Weight)

from  
Permissions AS P
INNER JOIN UserPermissions UP ON P.Id = UP.PermissionId
INNER JOIN GroupPermissions GP ON P.Id = GP.Permission_Id

where UP.UserProfileId = @CurrentUserId
AND GP.Group_Id = 1030 );

	
SET @Count =(SELECT COUNT(T.Id)
FROM 
Transactions AS T 


	INNER JOIN Lookups L ON T.StatusId = L.Id

	INNER JOIN TransactionAssignments TA ON T.Id = TA.TransactionId
	INNER JOIN UserProfiles UP ON TA.ToUserId = UP.Id
	INNER JOIN OrgUnits OU ON TA.ToEntityId = OU.Id


WHERE
     TA.ToEntityId = @OrgUnitId 

	 AND  (TA.ToUserId  =IsNull(@UserId,TA.ToUserId ) or @UserId is null)
	 AND  (T.TransactionTypeId  =@TransactionTypeId or @TransactionTypeId is null)

	--AND TA.Date >= @FromDateTime And TA.Date <= @ToDateTime
	AND TA.Date between IsNull(Convert(datetime,@FromDateTime, 103),TA.Date) AND IsNull(Convert(datetime,@ToDateTime , 103),TA.Date)

	AND IsNull(T.RemindDate,GETDATE())  >= GETDATE()

	AND DATEDIFF(DAY,  DATEADD(day, -UP.TransactionProcessingPeriod, GETDATE()), TA.Date) >= 0

	AND T.StatusId = 1479

	AND (Reports.GetPermissionWeightById(T.ConfidentialityId) <= @Weight And @Weight is not null )
);

	-- Return the result of the function
	RETURN @Count
END



GO
/****** Object:  UserDefinedFunction [Reports].[TransactionsLateCount]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Reports].[TransactionsLateCount](@OrgUnitId int,@UserId int=null,@TransactionTypeId int = null,@CurrentUserId int,@FromDateTime varchar(50),@ToDateTime varchar(50))

RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Count int;
	DECLARE @Weight int = null;

set @Weight =(Select MAX(P.Weight)

from  
Permissions AS P
INNER JOIN UserPermissions UP ON P.Id = UP.PermissionId
INNER JOIN GroupPermissions GP ON P.Id = GP.Permission_Id

where UP.UserProfileId = @CurrentUserId
AND GP.Group_Id = 1030 );

SET @Count =(SELECT COUNT(T.Id)
FROM 
Transactions AS T 


	INNER JOIN TransactionAssignments TA ON T.Id = TA.TransactionId
	INNER JOIN UserProfiles UP ON TA.ToUserId = UP.Id
	INNER JOIN OrgUnits OU ON TA.ToEntityId = OU.Id


WHERE
     TA.ToEntityId = @OrgUnitId 

	 AND  (TA.ToUserId  = IsNull(@UserId,TA.ToUserId) or @UserId is null)
	 AND  (T.TransactionTypeId  =@TransactionTypeId or @TransactionTypeId is null)

	--AND TA.Date >= @FromDateTime And TA.Date <= @ToDateTime
	AND (TA.Date between IsNull(Convert(datetime,@FromDateTime, 103),TA.Date) AND IsNull(Convert(datetime,@ToDateTime , 103),TA.Date)

	OR T.RemindDate < GETDATE())

	AND DATEDIFF(DAY,  DATEADD(day, -UP.TransactionProcessingPeriod, GETDATE()), TA.Date) < 0

	AND (Reports.GetPermissionWeightById(T.ConfidentialityId) <= @Weight And @Weight is not null )

);

	-- Return the result of the function
	RETURN @Count
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchDocumentNumber]

 @DocumentNumber		nvarchar(50), 
 @OrgUnitId				int,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @Year					int,
 @TotalCount			bigint output

AS

BEGIN

--SELECT @OrgUnitId = -1

--GET Culture ID From Cultures table
DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName
-- SELECT @V_YEAR = TEXT FROM [dbo].[LookupLocalizations] WHERE Lookup_Id = @Year AND Culture_Id = 1

-- IF(@V_YEAR IS NULL)  SET @V_YEAR = (select right(convert(nvarchar(10), getdate(), 131),4))

SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID


CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
		(@DocumentNumber = '' OR TR.DOCUMENTNUMBER = @DocumentNumber) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		AND (@OrgUnitId = -1 OR TED.[EntityId] = @OrgUnitId)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 254)

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
		(@DocumentNumber = '' OR TR.DOCUMENTNUMBER = @DocumentNumber) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		AND (@OrgUnitId = -1 OR TED.[EntityId] = @OrgUnitId)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 254)

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		LL_SourceType.Text As TransactionType,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.weight as weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId,
		ST_SourceTypes.Color_Id as ColorCode
	FROM 
		Transactions TR WITH(NOLOCK)
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		LEFT JOIN TRANSACTIONTYPES ST_SourceTypes WITH(NOLOCK) ON ST_SourceTypes.Id = TR.TRANSACTIONTYPEID
		LEFT JOIN Localizations LL_SourceType WITH(NOLOCK) ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureID = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		
		
END
END

GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCachePollingStoredProcedure]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCachePollingStoredProcedure] AS
         SELECT tableName, changeId FROM dbo.AspNet_SqlCacheTablesForChangeNotification
         RETURN 0



GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheQueryRegisteredTablesStoredProcedure]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheQueryRegisteredTablesStoredProcedure] 
         AS
         SELECT tableName FROM dbo.AspNet_SqlCacheTablesForChangeNotification   


GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheRegisterTableStoredProcedure]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheRegisterTableStoredProcedure] 
             @tableName NVARCHAR(450) 
         AS
         BEGIN

         DECLARE @triggerName AS NVARCHAR(3000) 
         DECLARE @fullTriggerName AS NVARCHAR(3000)
         DECLARE @canonTableName NVARCHAR(3000) 
         DECLARE @quotedTableName NVARCHAR(3000) 

         /* Create the trigger name */ 
         SET @triggerName = REPLACE(@tableName, '[', '__o__') 
         SET @triggerName = REPLACE(@triggerName, ']', '__c__') 
         SET @triggerName = @triggerName + '_AspNet_SqlCacheNotification_Trigger' 
         SET @fullTriggerName = 'dbo.[' + @triggerName + ']' 

         /* Create the cannonicalized table name for trigger creation */ 
         /* Do not touch it if the name contains other delimiters */ 
         IF (CHARINDEX('.', @tableName) <> 0 OR 
             CHARINDEX('[', @tableName) <> 0 OR 
             CHARINDEX(']', @tableName) <> 0) 
             SET @canonTableName = @tableName 
         ELSE 
             SET @canonTableName = '[' + @tableName + ']' 

         /* First make sure the table exists */ 
         IF (SELECT OBJECT_ID(@tableName, 'U')) IS NULL 
         BEGIN 
             RAISERROR ('00000001', 16, 1) 
             RETURN 
         END 

         BEGIN TRAN
         /* Insert the value into the notification table */ 
         IF NOT EXISTS (SELECT tableName FROM dbo.AspNet_SqlCacheTablesForChangeNotification WITH (NOLOCK) WHERE tableName = @tableName) 
             IF NOT EXISTS (SELECT tableName FROM dbo.AspNet_SqlCacheTablesForChangeNotification WITH (TABLOCKX) WHERE tableName = @tableName) 
                 INSERT  dbo.AspNet_SqlCacheTablesForChangeNotification 
                 VALUES (@tableName, GETDATE(), 0)

         /* Create the trigger */ 
         SET @quotedTableName = QUOTENAME(@tableName, '''') 
         IF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = 'TR') 
             IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = 'TR') 
                 EXEC('CREATE TRIGGER ' + @fullTriggerName + ' ON ' + @canonTableName +'
                       FOR INSERT, UPDATE, DELETE AS BEGIN
                       SET NOCOUNT ON
                       EXEC dbo.AspNet_SqlCacheUpdateChangeIdStoredProcedure N' + @quotedTableName + '
                       END
                       ')
         COMMIT TRAN
         END
   

GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheUnRegisterTableStoredProcedure]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheUnRegisterTableStoredProcedure] 
             @tableName NVARCHAR(450) 
         AS
         BEGIN

         BEGIN TRAN
         DECLARE @triggerName AS NVARCHAR(3000) 
         DECLARE @fullTriggerName AS NVARCHAR(3000)
         SET @triggerName = REPLACE(@tableName, '[', '__o__') 
         SET @triggerName = REPLACE(@triggerName, ']', '__c__') 
         SET @triggerName = @triggerName + '_AspNet_SqlCacheNotification_Trigger' 
         SET @fullTriggerName = 'dbo.[' + @triggerName + ']' 

         /* Remove the table-row from the notification table */ 
         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = 'AspNet_SqlCacheTablesForChangeNotification' AND type = 'U') 
             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = 'AspNet_SqlCacheTablesForChangeNotification' AND type = 'U') 
             DELETE FROM dbo.AspNet_SqlCacheTablesForChangeNotification WHERE tableName = @tableName 

         /* Remove the trigger */ 
         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = 'TR') 
             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = 'TR') 
             EXEC('DROP TRIGGER ' + @fullTriggerName) 

         COMMIT TRAN
         END
 
 

GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheUpdateChangeIdStoredProcedure]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheUpdateChangeIdStoredProcedure] 
             @tableName NVARCHAR(450) 
         AS

         BEGIN 
             UPDATE dbo.AspNet_SqlCacheTablesForChangeNotification WITH (ROWLOCK) SET changeId = changeId + 1 
             WHERE tableName = @tableName
         END
 
 
GO
/****** Object:  StoredProcedure [Reports].[EmployeeAchievementsDetailedSP]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: 
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ==============================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	25-09-2016	OsamaS				re-design the SP to enhance the performance
  ==============================================================================================
*/	
CREATE PROCEDURE [Reports].[EmployeeAchievementsDetailedSP]
    @OrgUnitId int,
    @UserId int,
	@FromDateTime varchar(50),
	@ToDateTime varchar(50),
	@CultureName varchar(4)
AS

BEGIN

	DECLARE @CultureId int

	SELECT DISTINCT  
		@CultureId =C.Id
	FROM  
		Cultures AS C
	WHERE 
		C.ShortName = @CultureName

	CREATE TABLE #tem (Number int, TypeName nvarchar(50),Subject nvarchar(max),DateH varchar(50),StatusNumber int)

	INSERT INTO #tem (Number, TypeName, Subject, DateH, StatusNumber)
	SELECT 
		T.Number As Number , 
		LL.Text AS TypeName , 
		T.Subject AS Subject, 
		T.DateH AS DateH, 
		1 As StatusNumber 
	FROM 
		Transactions AS T 
		INNER JOIN Lookups LP ON T.TransactionTypeId = LP.Id
		INNER JOIN LookupLocalizations LL ON LP.Id = LL.Lookup_Id
		AND LL.Culture_Id = @CultureId
		INNER JOIN TransactionAssignmentHistories TA ON T.Id = TA.TransactionId
	WHERE
		TA.FromEntityId = @OrgUnitId 
		AND  TA.FromUserId  =IsNull(@UserId,TA.FromUserId)
		AND TA.Date between IsNull(Convert(datetime,@FromDateTime, 103),TA.Date) 
		AND IsNull(Convert(datetime,@ToDateTime , 103),TA.Date)

	UNION

	SELECT 
		T.Number As Number , 
		LL.Text AS TypeName , 
		T.Subject AS Subject, 
		T.DateH AS DateH, 
		2 As StatusNumber 
	FROM 
		Transactions AS T 
		INNER JOIN Lookups L ON T.StatusId = L.Id
		INNER JOIN Lookups LP ON T.TransactionTypeId = LP.Id
		INNER JOIN LookupLocalizations LL ON LP.Id = LL.Lookup_Id
		AND LL.Culture_Id = @CultureId
		INNER JOIN TransactionAssignments TA ON T.Id = TA.TransactionId
		INNER JOIN UserProfiles UP ON TA.ToUserId = UP.Id
		INNER JOIN OrgUnits OU ON TA.ToEntityId = OU.Id
	WHERE
		TA.ToEntityId = @OrgUnitId 
		AND  TA.ToUserId  =IsNull(@UserId,TA.ToUserId )
		AND TA.Date between IsNull(Convert(datetime,@FromDateTime, 103),TA.Date) 
		AND IsNull(Convert(datetime,@ToDateTime , 103),TA.Date)
		AND IsNull(T.RemindDate,GETDATE())  >= GETDATE()
		AND DATEDIFF(DAY,  DATEADD(day, -UP.TransactionProcessingPeriod, GETDATE()), TA.Date) >= 0
		AND T.StatusId = 1479

	UNION

	SELECT 
		T.Number As Number , 
		LL.Text AS TypeName , 
		T.Subject AS Subject, 
		T.DateH AS DateH, 
		3 As StatusNumber
	FROM 
		Transactions AS T 
		INNER JOIN Lookups LP ON T.TransactionTypeId = LP.Id
		INNER JOIN LookupLocalizations LL ON LP.Id = LL.Lookup_Id
		AND LL.Culture_Id = @CultureId
		INNER JOIN TransactionAssignments TA ON T.Id = TA.TransactionId
		INNER JOIN UserProfiles UP ON TA.ToUserId = UP.Id
		INNER JOIN OrgUnits OU ON TA.ToEntityId = OU.Id
	WHERE
		TA.ToEntityId = @OrgUnitId 
		AND  TA.ToUserId  =IsNull(@UserId,TA.ToUserId )
		AND TA.Date between IsNull(Convert(datetime,@FromDateTime, 103),TA.Date) 
		AND IsNull(Convert(datetime,@ToDateTime , 103),TA.Date)
		AND T.RemindDate < GETDATE()
		AND DATEDIFF(DAY,  DATEADD(day, -UP.TransactionProcessingPeriod, GETDATE()), TA.Date) < 0

	SELECT 
		t.DateH as DateH, 
		t.Number as Number, 
		t.StatusNumber as StatusNumber, 
		t.Subject as Subject,
		t.TypeName as TypeName
	FROM  
		#tem As t

END



GO
/****** Object:  StoredProcedure [Reports].[EmployeeAchievementsSP]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: 
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ==============================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	25-09-2016	OsamaS				re-design the SP to enhance the performance
  ==============================================================================================
*/	
CREATE PROCEDURE [Reports].[EmployeeAchievementsSP]	
    @OrgUnitId int,
    @UserId int,
	@FromDateTime varchar(50),
	@ToDateTime varchar(50)
AS

BEGIN

	DECLARE @t TABLE(InProgressCount int, LateCount int,CompletedCount int)
	INSERT @t VALUES
					(
						dbo.TransactionsInProgressCount(@OrgUnitId,@UserId,null,@FromDateTime,@ToDateTime),
						dbo.TransactionsLateCount(@OrgUnitId,@UserId,null,@FromDateTime,@ToDateTime),
						dbo.TransactionsCompletedCount(@OrgUnitId,@UserId,null,@FromDateTime,@ToDateTime)
					)
	SELECT 
		InProgressCount As InProgress, 
		LateCount As Late, 
		CompletedCount As Completed
	FROM @t
	
END



GO
/****** Object:  StoredProcedure [Reports].[OrgUnitAchievementsByTransactionTypeSP]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: 
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ==============================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	25-09-2016	OsamaS				re-design the SP to enhance the performance
  ==============================================================================================
*/	
CREATE PROCEDURE [Reports].[OrgUnitAchievementsByTransactionTypeSP]	
    @OrgUnitId int,
	@FromDateTime varchar(50),
	@ToDateTime varchar(50),
	@UserId int
AS

BEGIN

	DECLARE @Weight int = null;

	SELECT
		@Weight =  MAX(P.Weight)
	FROM  
		Permissions AS P
		INNER JOIN UserPermissions UP ON P.Id = UP.PermissionId
		INNER JOIN GroupPermissions GP ON P.Id = GP.Permission_Id
	WHERE 
		UP.UserProfileId = @UserId
		AND GP.Group_Id = 1030

	SELECT 
		Count(IT.Id) 
		AS Inbound, 
		Count(OET.Id) AS ExternalOutbound, 
		Count(OIT.Id) AS InternalOutbound
	FROM 
		Transactions T
		LEFT Outer JOIN Transactions OET on OET.Id = T.Id AND OET.TransactionTypeId = 15
		LEFT Outer JOIN Transactions IT on IT.Id = T.Id AND IT.TransactionTypeId = 14 
		LEFT Outer JOIN Transactions OIT on OIT.Id = T.Id AND OIT.TransactionTypeId = 16
		INNER JOIN Permissions AS TP ON TP.ID = T.ConfidentialityId
	Where 
		T.OrgUnitId = @OrgUnitId 
		AND T.Date between IsNull(Convert(datetime,@FromDateTime, 103),T.Date) 
		AND IsNull(Convert(datetime,@ToDateTime , 103),T.Date)
		AND TP.Weight <= @Weight 
		AND @Weight IS NOT NULL

END

GO
/****** Object:  StoredProcedure [Reports].[OrgUnitAchievementsSP]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: 
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ==============================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	25-09-2016	OsamaS				re-design the SP to enhance the performance
  ==============================================================================================
*/	
CREATE PROCEDURE [Reports].[OrgUnitAchievementsSP]
    @OrgUnitId int,
	@FromDateTime varchar(50),
	@ToDateTime varchar(50),
	@UserId int
AS

BEGIN

	DECLARE @t table(InProgressCount int, LateCount int,CompletedCount int)

	INSERT @t VALUES
					(
						Reports.TransactionsInProgressCount(@OrgUnitId,null,null,@UserId,@FromDateTime,@ToDateTime),
						Reports.TransactionsLateCount(@OrgUnitId,null,null,@UserId,@FromDateTime,@ToDateTime),
						Reports.TransactionsCompletedCount(@OrgUnitId,null,null,@UserId,@FromDateTime,@ToDateTime)
					)

	SELECT 
		InProgressCount As InProgress, 
		LateCount As Late, 
		CompletedCount As Completed
	FROM 
		@t

END



GO
/****** Object:  StoredProcedure [Reports].[StatisticalSP]    Script Date: 1/24/2019 8:34:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: 
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ==============================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	25-09-2016	OsamaS				re-design the SP to enhance the performance
  ==============================================================================================
*/	
CREATE PROCEDURE [Reports].[StatisticalSP]
@OrgUnitId int

AS
BEGIN

	SELECT 
		Count(IT.Id) AS Inbound, 
		Count(OET.Id) AS ExternalOutbound, 
		Count(OIT.Id) AS InternalOutbound
	FROM 
		Transactions T
		LEFT Outer JOIN Transactions OET on OET.Id = T.Id AND OET.TransactionTypeId = 15
		LEFT Outer JOIN Transactions IT on IT.Id = T.Id AND IT.TransactionTypeId = 14 
		LEFT Outer JOIN Transactions OIT on OIT.Id = T.Id AND OIT.TransactionTypeId = 16
	WHERE 
		T.OrgUnitId = @OrgUnitId

END


GO
/****** Object:  StoredProcedure [dbo].[Search_bySubject]    Script Date: 6/16/2019 8:41:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Search_bySubject]

 @Subject			nvarchar(4000),
 @HasFullPrivilege	bit,
 @OrgUnitId			int,
 @UserId			int,
 @TransactionCategoryId	int,
 @TransactionTypeId int, 
 @PageIndex			int, 
 @PageSize			int,
 @Ascending			bit, 
 @CultureName		nvarchar(50), 
 @OrderBy			nvarchar(50),
 @Year				int,
 @TotalCount		int output

WITH RECOMPILE

AS

SET NOCOUNT ON

BEGIN

	IF(@Subject IS NULL) SET @Subject=''

	--GET Culture ID From Cultures table
	DECLARE @V_CultureID int ,@V_YEAR INT

	DECLARE @V_FirstIndex int,@V_LastIndex  int 
	Set @V_FirstIndex = @PageIndex * @PageSize+1
	SET @V_LastIndex  = @PageIndex * @PageSize + @PageSize

	SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName
	/*----
	declare @V_YEAR nvarchar(50)
	declare @Year int = 1
	SELECT Lookup_Id, Culture_Id FROM [dbo].[LookupLocalizations] WHERE Lookup_Id = 1 AND Culture_Id = 1
	select @V_YEAR
	----*/
	--IF(@V_YEAR IS NULL)  SET @V_YEAR = (select right(convert(nvarchar(10), getdate(), 131),4))

	SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID

	set @Subject = REPLACE(@Subject,N' ',N'*')

	CREATE TABLE #InScopeTr(TransID int)
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
		(@Year = -1 or YearH = @V_YEAR)
		AND (@TransactionCategoryId =-1 OR TR.TRANSACTIONCATEGORYID =@TransactionCategoryId)
		AND (CONTAINS(Subject,@Subject))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
		(@Year = -1 or YearH = @V_YEAR)
		AND (@TransactionCategoryId =-1 OR TR.TRANSACTIONCATEGORYID =@TransactionCategoryId)
		AND (CONTAINS(Subject,@Subject))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
	ORDER BY TR.ID DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.TransactionTypeId,
		--LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.Weight as Weight,
		CAST(0 AS BIT) AS IsArchived,
		LL_TransType.text as TransactionType
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TRANSACTIONCATEGORYID AND LL_TransType.Culture_Id = @V_CultureID --
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
	ORDER BY
		TR.Id DESC
	
END


GO
/****** Object:  StoredProcedure [dbo].[SearchEntity]    Script Date: 7/5/2019 1:17:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: SearchEntity SP
 	Role		: 
	Auth		: <Author,Hanaa Nawasreh>
	Date		: 

	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	27-03-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
CREATE PROCEDURE [dbo].[SearchEntity]
 @ExternalParty			BIGINT, 
 @OrgUnitId				int,
 @TransactionTypeId		int, 
 @DateFrom				datetime,
 @DateTo				datetime,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @TotalCount			int output

AS

BEGIN

--GET Culture ID From Cultures table
DECLARE @V_CultureID int 
SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName

CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
	   (@TransactionTypeId =-1 OR TR.TRANSACTIONCATEGORYID =@TransactionTypeId)
		AND (@ExternalParty= -1 OR TR.ExternalPartyId = @ExternalParty) 
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
		(@TransactionTypeId =-1 OR TR.TRANSACTIONCATEGORYID =@TransactionTypeId)
		AND (@ExternalParty= -1 OR TR.ExternalPartyId = @ExternalParty) 
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.ExternalPartyId as ExternalParty,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		-- LL_SourceType.Text As Type,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.weight as weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId
		--ST_SourceTypes.Color_Id as ColorCode
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		-- LEFT JOIN SourceTypes ST_SourceTypes WITH(NOLOCK) ON ST_SourceTypes.Id = TR.SourceTypeId
		-- LEFT JOIN Localizations LL_SourceType WITH(NOLOCK) ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureID = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
END


END

GO
/****** Object:  StoredProcedure [dbo].[SearchCreator]    Script Date: 7/5/2019 1:21:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: SearchCreator SP
 	Role		: 
	Auth		: <Author,Hanaa Nawasreh>
	Date		: 

	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	27-03-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
CREATE PROCEDURE [dbo].[SearchCreator]
 @Creator				BIGINT, 
 @HasFullPrivilege		bit,
 @OrgUnitId				int,
 @UserId				int,
 @TransactionCategoryId	int, 
 @DateFrom				datetime,
 @DateTo				datetime,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @TotalCount			int output

AS

BEGIN

--GET Culture ID From Cultures table
DECLARE @V_CultureID int 
SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName

CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
	   (@TransactionCategoryId =-1 OR TR.TRANSACTIONCATEGORYID =@TransactionCategoryId)
		AND (@Creator= -1 OR TR.UserId = @Creator) 
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
		(@TransactionCategoryId =-1 OR TR.TRANSACTIONCATEGORYID =@TransactionCategoryId)
		AND (@Creator= -1 OR TR.UserId = @Creator)  
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.UserId as UsrId,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		--LL_SourceType.Text As Type,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.weight as weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		--LEFT JOIN SourceTypes ST_SourceTypes WITH(NOLOCK) ON ST_SourceTypes.Id = TR.SourceTypeId
		--LEFT JOIN Localizations LL_SourceType WITH(NOLOCK) ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureID = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
END


END
GO
/****** Object:  StoredProcedure [dbo].[SearchInbound]    Script Date: 7/7/2019 12:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: SearchInbound SP
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	27-03-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
ALTER PROCEDURE [dbo].[SearchInbound]

 @Number				BIGINT, 
 @OrgUnitId				int,
 @TransactionTypeId		int, 
 @SourceTypeId			int,
 @DateFrom				datetime,
 @DateTo				datetime,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @Year					int,
 @TotalCount			bigint output

AS

BEGIN

--SELECT @OrgUnitId = -1

--GET Culture ID From Cultures table
DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName
-- SELECT @V_YEAR = TEXT FROM [dbo].[LookupLocalizations] WHERE Lookup_Id = @Year AND Culture_Id = 1

-- IF(@V_YEAR IS NULL)  SET @V_YEAR = (select right(convert(nvarchar(10), getdate(), 131),4))

	SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID


CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
	   (@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		--AND (@SourceTypeId =-1 OR TR.SourceTypeId=@SourceTypeId)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 254)

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
		(@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.NUMBER = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		--AND (@SourceTypeId =-1 OR TR.SourceTypeId=@SourceTypeId)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 254)

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		LL_SourceType.Text As TransactionType,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.weight as weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId,
		ST_SourceTypes.Color_Id as ColorCode
	FROM 
		Transactions TR WITH(NOLOCK)
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		LEFT JOIN TRANSACTIONTYPES ST_SourceTypes WITH(NOLOCK) ON ST_SourceTypes.Id = TR.TRANSACTIONTYPEID
		LEFT JOIN Localizations LL_SourceType WITH(NOLOCK) ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureID = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		order by tr.id desc
		
END



END

GO
/****** Object:  StoredProcedure [dbo].[SearchOutboundDraft]    Script Date: 7/7/2019 12:03:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: SearchOutboundDraft SP
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	27-03-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
CREATE PROCEDURE [dbo].[SearchOutboundDraft]
 @Number				BIGINT, 
 @HasFullPrivilege		bit,
 @OrgUnitId				int,
 @UserId				int,
 @TransactionTypeId		int, 
 @DateFrom				datetime,
 @DateTo				datetime,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @Year					int,
 @TotalCount			int output
AS

BEGIN

--GET Culture ID From Cultures table
DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName
-- SELECT @V_YEAR = TEXT FROM [dbo].[LookupLocalizations] WHERE Lookup_Id = @Year AND Culture_Id = 1

-- IF(@V_YEAR IS NULL)  SET @V_YEAR = (select right(convert(nvarchar(10), getdate(), 131),4))
SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID

CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
	   (@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND TR.TRANSACTIONCATEGORYID = 257

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	WHERE 
		(@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND TR.TRANSACTIONCATEGORYID = 257

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.Weight as Weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
END

END

GO
/****** Object:  StoredProcedure [dbo].[SearchOutboundExternal]    Script Date: 6/16/2019 8:38:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: SearchOutboundExternal SP
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	27-03-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
CREATE PROCEDURE [dbo].[SearchOutboundExternal]
 @Number				BIGINT, 
 @OrgUnitId				int,
 @TransactionTypeId		int, 
 @SourceTypeId			int,
 @DateFrom				datetime,
 @DateTo				datetime,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @Year					int,
 @TotalCount			int output

AS

BEGIN

--GET Culture ID From Cultures table
DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName
-- SELECT @V_YEAR = TEXT FROM [dbo].[LookupLocalizations] WHERE Lookup_Id = @Year AND Culture_Id = 1

-- IF(@V_YEAR IS NULL)  SET @V_YEAR = (select right(convert(nvarchar(10), getdate(), 131),4))
	SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID

CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
	   (@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		--AND (@SourceTypeId =-1 OR TR.SourceTypeId=@SourceTypeId)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 255)

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
	LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId]

	WHERE 
		(@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		--AND (@SourceTypeId =-1 OR TR.SourceTypeId=@SourceTypeId)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
	    AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 255)

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.Weight as Weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
		order by tr.id desc
END

END

GO
/****** Object:  StoredProcedure [dbo].[SearchOutboundInternal]    Script Date: 6/13/2019 3:28:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: SearchOutboundInternal SP
 	Role		: 
	Auth		: 
	Date		: 
	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	27-03-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
CREATE PROCEDURE [dbo].[SearchOutboundInternal]
 @Number				BIGINT, 
 @OrgUnitId				int,
 @TransactionTypeId		int, 
 @SourceTypeId			int,
 @DateFrom				datetime,
 @DateTo				datetime,
 @PageIndex				int, 
 @PageSize				int,
 @Ascending				bit, 
 @CultureName			nvarchar(50), 
 @OrderBy				nvarchar(50),
 @Year					int,
 @TotalCount			int output

AS

BEGIN

SELECT @OrgUnitId = -1

--GET Culture ID From Cultures table
DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName
SELECT @V_YEAR = TEXT FROM [dbo].[LookupLocalizations] WHERE Lookup_Id = @Year AND Culture_Id = 1

-- IF(@V_YEAR IS NULL)  SET @V_YEAR = (select right(convert(nvarchar(10), getdate(), 131),4))
SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID

CREATE TABLE #InScopeTr(TransID int)

BEGIN
	SELECT 
		@TotalCount = count(TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
	   (@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		--AND (@SourceTypeId =-1 OR TR.SourceTypeId=@SourceTypeId)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 256)

	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
		(@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@Year = -1 or TR.YearH= @V_YEAR)
		--AND (@SourceTypeId =-1 OR TR.SourceTypeId=@SourceTypeId)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		AND (tr.TRANSACTIONCATEGORYID = 256)

	ORDER BY id DESC
	OFFSET @PageIndex * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT  
		ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,	
		TR.Id,
		TR.Number as Number,
		TR.TransactionTypeId,
		LL_TransType.Text As TransactionTypeName,
		TR.Date,
		TR.DateH,
		LOC_PR.Text As PriorityName,
		LL_Perm.Text as ConfidentialityName,
		TR.SourceTypeId,
		LOC_ExternalParty.Text as PartyName,
		LOC_OrgUnit.Text as OrgUnitName,
		TR.Subject,
		LL_Status.Text as StatusName,
		P_Permission.Weight as Weight,
		CAST(0 AS BIT) AS IsArchived,
		TA.ToUserId,
		TR.StatusId
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
		order by tr.id desc
END

END

GO
/****** Object:  StoredProcedure [dbo].[MobileSearch]    Script Date: 16/9/2019 12:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*	
	Desc		: MobileSearch SP
 	Role		: 
	Auth		: 
	Date		: 

	Calling Examples: 
  
  Change History:
  ============================================================================================================
    Date		     Author		           Description
    ----------	--------------	    ----------------------------------
	16-09-2019	OsamaS				Modify the SP to read from deactive DB in case the year was befor 1438
  ============================================================================================================
*/	
CREATE PROCEDURE [dbo].[MobileSearch]
 @Number				BIGINT, 
 @OrgUnitId				int,
 @TransactionTypeId		int, 
 @Subject				nvarchar(50),
 @TransCategory			int,
 @CultureName			nvarchar(2)

AS

BEGIN

--GET Culture ID From Cultures table
DECLARE @V_CultureID int

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@CultureName

CREATE TABLE #InScopeTr(TransID int)

BEGIN
	INSERT INTO #InScopeTr
	SELECT TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 

	WHERE 
		(@TransCategory =-1 OR TR.TransactionTypeId =@TransCategory)
		AND (@Number = -1 OR TR.Number = @Number) 
		AND (@TransactionTypeId =-1 OR TR.TransactionTypeId=@TransactionTypeId)
		AND (CONTAINS(Subject,@Subject))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
	ORDER BY id DESC

	SELECT  
		TR.Id AS TransID,
		TR.Number AS TransNo,
		TR.Subject AS TransTitle,
		TR.DateH AS TransDate,
		LOC_FROM_OrgUnit.Text AS TransFrom,
		TR.TransactionTypeId AS TransCategory,
		'' AS FileSize,
		LL_SourceType + ' - ' + CASE WHEN TA.FromEntityId = TA.ToEntityId AND TA.FromUserId = TA.ToUserId THEN LOC_FROM_OrgUnit.Text ELSE LOC_TO_OrgUnit.Text END AS TransSourceRow,
		TR.Number + ' - ' + LOC_Creating_OrgUnit.Text AS TransNumberRow,
		LOC_FROM_OrgUnit.Text AS EntityName
	FROM 
		Transactions TR WITH(NOLOCK)
		LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN #InScopeTr TT ON TT.TransID = TR.ID
		LEFT JOIN SourceTypes ST_SourceTypes WITH(NOLOCK) ON ST_SourceTypes.Id = TR.SourceTypeId
		LEFT JOIN Localizations LL_SourceType WITH(NOLOCK) ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureID = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit WITH(NOLOCK) ON OU_OrgUnit.Id = @OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit WITH(NOLOCK) ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
		LEFT JOIN OrgUnits OU_FROM_UNIT WITH(NOLOCK) ON OU_FROM_UNIT.Id = TA.FromEntityId
		LEFT JOIN Localizations LOC_FROM_OrgUnit WITH(NOLOCK) ON LOC_FROM_OrgUnit.LocalizationIdentifier_Id = OU_FROM_UNIT.LocalizationIdentifier_Id AND LOC_FROM_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_TO_UNIT WITH(NOLOCK) ON OU_TO_UNIT.Id = TA.ToEntityId
		LEFT JOIN Localizations LOC_TO_OrgUnit WITH(NOLOCK) ON LOC_TO_OrgUnit.LocalizationIdentifier_Id = OU_TO_UNIT.LocalizationIdentifier_Id AND LOC_TO_OrgUnit.CultureId = @V_CultureID
		LEFT JOIN OrgUnits OU_Creating_OrgUnit WITH(NOLOCK) ON OU_Creating_OrgUnit.Id = TR.OrgUnitId
		LEFT JOIN Localizations LOC_Creating_OrgUnit WITH(NOLOCK) ON LOC_Creating_OrgUnit.LocalizationIdentifier_Id = OU_Creating_OrgUnit.LocalizationIdentifier_Id AND LOC_Creating_OrgUnit.CultureId = @V_CultureID
END


END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Ahed Abo-Ghazal>
-- Create date: <Create Date,,09/04/2019>
-- Description:	<Description,,Insert Copies that assigned to entity>
-- =============================================
CREATE FUNCTION [dbo].[KeyValuePairs](@inputStr VARCHAR(MAX),@separator CHAR(1),@keyValueSeperator CHAR(1)) 
RETURNS @OutTable TABLE 
	(KeyName VARCHAR(MAX), KeyValue VARCHAR(MAX))
AS
BEGIN
	-- @separator = ','
	-- @keyValueSeperator = ':'

	DECLARE @separator_position INT , @keyValueSeperatorPosition INT
	DECLARE @match VARCHAR(MAX) 
	
	SET @inputStr = @inputStr + @separator
	
	WHILE PATINDEX('%' + @separator + '%' , @inputStr) <> 0 
	 BEGIN
	  SELECT @separator_position =  PATINDEX('%' + @separator + '%' , @inputStr)
	  SELECT @match = LEFT(@inputStr, @separator_position - 1)
	  IF @match <> '' 
		  BEGIN
            SELECT @keyValueSeperatorPosition = PATINDEX('%' + @keyValueSeperator + '%' , @match)
            IF @keyValueSeperatorPosition <> -1 
              BEGIN
        		INSERT @OutTable
				 VALUES (LEFT(@match,@keyValueSeperatorPosition -1),
				 RIGHT(@match,LEN(@match) - @keyValueSeperatorPosition))
              END
		   END		
 	  SELECT @inputStr = STUFF(@inputStr, 1, @separator_position, '')
	END

	RETURN
END



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Ahed Abo-Ghazal>
-- Create date: <Create Date,,09/04/2019>
-- Description:	<Description,,Insert Copies that assigned to entity>
-- =============================================
CREATE FUNCTION [dbo].[ConvertDelimitedListIntoTable] (
     @list NVARCHAR(MAX) ,@delimiter CHAR(1) )
RETURNS @table TABLE ( 
     item VARCHAR(255) NOT NULL )
AS 
    BEGIN
        DECLARE @pos INT ,@nextpos INT ,@valuelen INT

        SELECT  @pos = 0 ,@nextpos = 1

        WHILE @nextpos > 0 
            BEGIN
                SELECT  @nextpos = CHARINDEX(@delimiter,@list,@pos + 1)
                SELECT  @valuelen = CASE WHEN @nextpos > 0 THEN @nextpos
                                         ELSE LEN(@list) + 1
                                    END - @pos - 1
                INSERT  @table ( item )
                VALUES  ( CONVERT(INT,SUBSTRING(@list,@pos + 1,@valuelen)) )
                SELECT  @pos = @nextpos

            END

        DELETE  FROM @table
        WHERE   item = ''

        RETURN 
    END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Ahed Abo-Ghazal>
-- Create date: <Create Date,,09/04/2019>
-- Description:	<Description,,Insert Copies that assigned to entity>
-- =============================================
CREATE PROCEDURE [dbo].[InsertCopies]
	-- Add the parameters for the stored procedure here
	@EntitiesIds nvarchar(max) , 
	@TransactionId int ,
	@Date datetime , 
	@DateH nvarchar(max) ,
	@Viewed bit ,
	@IsSent int = NULL,
	@CreatedOn datetime,
	@CreatedBy int = NULL,
	@ModefiedOn datetime = NULL,
	@ModefiedBy int = NULL 
AS
BEGIN

	INSERT INTO [dbo].[TransactionCopies] (UserId,EntityId,TransactionId,[Date],DateH,Viewed,ActionId,IsSent,CreatedOn,CreatedBy,ModefiedOn,ModefiedBy)
	SELECT 
	[UserProfile_Id],
	[OrgUnit_Id],
	@TransactionId as TransactionId,
	@Date as [Date],
	@DateH as DateH,
	@Viewed as Viewed,
	temp.KeyValue as ActionId,
	@IsSent as IsSent,
	@CreatedOn as CreatedOn,
	@CreatedBy as CreatedBy,
	@ModefiedOn as ModefiedOn,
	@ModefiedBy as ModefiedBy
	FROM [dbo].[UserProfileOrgUnits]
	INNER JOIN (select * from KeyValuePairs(@EntitiesIds , ',' , ':')) AS temp ON  [dbo].[UserProfileOrgUnits].[OrgUnit_Id] = temp.KeyName
END

GO

GO
/****** Object:  StoredProcedure [dbo].[UpdateAllOrgUnitChild]    Script Date: 5/5/2019 8:25:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: MohammadH
-- Create date: 02/05/2019
-- Description:	Update All organization Unit Children
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAllOrgUnitChild]
	@ParentId int,
	@Number nvarchar(50)
AS
BEGIN
	  UPDATE [dbo].[OrgUnits]
	  SET BarCode = CAST(Number AS nvarchar) +'/'+ @Number
	  WHERE IsActive= 1 and IsDeleted = 0 and ParentId = @ParentId 
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateAllOrgUnitChild]    Script Date: 5/5/2019 8:25:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[DashboardHeaderGet]	

	@FromDate						DATETIME,
	@ToDate							DATETIME,
	@EntitID						INT, 
	@UserID							INT,
	@level							INT ,
	@DraftOutbound                  INT ,
	@InternalOutbound               INT ,
	@Inbound                        INT ,
	@ExternalOutbound               INT 

AS

BEGIN 

	DECLARE
	@OutboundCount					INT,
	@OutboundDraftCountCreated		INT,
	@OutboundDraftCountAssigned		INT,
	@InboundCountCreated			INT,
	@InboundCountAssigned			INT,
	@InternalOutboundCountCreated	INT,
	@InternalOutboundCountAssigned	INT,
	@DelayedCount					INT

IF @level = 1
	BEGIN

		--عدد معاملات الصادر الخارجي
		select 
			@OutboundCount = count(*) 
		from 
			Transactions 
		where 
		      TRANSACTIONCATEGORYID = @ExternalOutbound 
			--TransactionTypeId = 15 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المنشئة
		select 
			@OutboundDraftCountCreated = count(*) 
		from 
			Transactions 
		where 
		 TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المحالة
		select 
			@OutboundDraftCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToUserId = @UserID
		where 
		TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.[CreatedBy] <> @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المنشئة
		select 
			@InboundCountCreated = count(*) 
		from 
			Transactions 
		where 
		TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المحالة
		select 
			@InboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToUserId = @UserID
		where 
		TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.[CreatedBy] <> @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المنشئة
		select 
			@InternalOutboundCountCreated = count(*) 
		from 
			Transactions 
		where 
		TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المحالة
		select 
			@InternalOutboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToUserId = @UserID
		where 
		TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16 
			AND (Transactions.[CreatedBy] <> @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد المعاملات المتأخرة 
		select 
			@DelayedCount = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
		where 
			TransactionTypeId <> 15 
			and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
			AND ([TransactionAssignments].ToUserId = @UserID)

	END

	IF @level = 2
	BEGIN

		--عدد معاملات الصادر الخارجي
		select 
			@OutboundCount = count(*) 
		from 
			Transactions 
		where 
			  TRANSACTIONCATEGORYID = @ExternalOutbound 
			--TransactionTypeId = 15 
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المنشئة
		select 
			@OutboundDraftCountCreated = count(*) 
		from 
			Transactions 
		where 
			 TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المحالة
		select 
			@OutboundDraftCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToEntityId = @EntitID
		where 
			 TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.OrgUnitId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المنشئة
		select 
			@InboundCountCreated = count(*) 
		from 
			Transactions 
		where 
		TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المحالة
		select 
			@InboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToEntityId = @EntitID
		where 
		TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.OrgUnitId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المنشئة
		select 
			@InternalOutboundCountCreated = count(*) 
		from 
			Transactions 
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المحالة
		select 
			@InternalOutboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToEntityId = @EntitID
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16
			AND (Transactions.OrgUnitId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد المعاملات المتأخرة 
		select 
			@DelayedCount = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToEntityId = @EntitID
		where 
			TransactionTypeId <> 15 
			and ([RemindDate] < GETDATE() OR DATEADD(day, 15, [TransactionAssignments].Date) < GETDATE())

	END

	IF @level = 3
	BEGIN
		;WITH cte AS 
		 (
		  SELECT a.Id, a.parentId, a.name
		  FROM OrgUnits_VW a
		  WHERE Id = @EntitID
		  UNION ALL
		  SELECT a.Id, a.parentid, a.Name
		  FROM OrgUnits_VW a JOIN cte c ON a.parentId = c.id
		  )

		SELECT ID INTO #T FROM cte

		--عدد معاملات الصادر الخارجي
		select 
			@OutboundCount = count(*) 
		from 
			Transactions 
		where 
			 TRANSACTIONCATEGORYID = @ExternalOutbound 
			--TransactionTypeId = 15 
			AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المنشئة
		select 
			@OutboundDraftCountCreated = count(*) 
		from 
			Transactions 
		where 
			 TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المحالة
		select 
			@OutboundDraftCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			 TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.OrgUnitId <> @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId

		--عدد معاملات الوارد الخارجي المنشئة
		select 
			@InboundCountCreated = count(*) 
		from 
			Transactions 
		where 
			TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المحالة
		select 
			@InboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
				TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.OrgUnitId <> @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);

		--عدد معاملات المعاملة الداخلية المنشئة
		select 
			@InternalOutboundCountCreated = count(*) 
		from 
			Transactions 
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16
			AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المحالة
		select 
			@InternalOutboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16
			AND (Transactions.OrgUnitId <> @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);

		--عدد المعاملات المتأخرة 
		select 
			@DelayedCount = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TransactionTypeId <> 15 
			and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
			AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))

	END

	IF @level = 4
	BEGIN
		;WITH cte AS 
		 (
		  SELECT a.Id, a.parentId, a.name
		  FROM OrgUnits_VW a
		  WHERE Id = @EntitID
		  UNION ALL
		  SELECT a.Id, a.parentid, a.Name
		  FROM OrgUnits_VW a JOIN cte c ON a.parentId = c.id
		  )

		SELECT ID INTO #TT FROM cte

		--عدد معاملات الصادر الخارجي
		select 
			@OutboundCount = count(*) 
		from 
			Transactions 
		where 
		 TRANSACTIONCATEGORYID = @ExternalOutbound 
			--TransactionTypeId = 15 
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المنشئة
		select 
			@OutboundDraftCountCreated = count(*) 
		from 
			Transactions 
		where 
			TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المحالة
		select 
			@OutboundDraftCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @DraftOutbound
			--TransactionTypeId = 17 
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <>@EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);

		--عدد معاملات الوارد الخارجي المنشئة
		select 
			@InboundCountCreated = count(*) 
		from 
			Transactions 
		where 
			TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14  
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المحالة
		select 
			@InboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @Inbound
			--TransactionTypeId = 14 
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <>@EntitID )
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);

		--عدد معاملات المعاملة الداخلية المنشئة
		select 
			@InternalOutboundCountCreated = count(*) 
		from 
			Transactions 
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16
			AND (Transactions.OrgUnitId = @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المحالة
		select 
			@InternalOutboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound
			--TransactionTypeId = 16
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);

		--عدد المعاملات المتأخرة 
		select 
			@DelayedCount = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			 TRANSACTIONCATEGORYID = @ExternalOutbound 
			and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
			AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #TT))

	END


SELECT 	
	ISNULL(@OutboundCount,0)					OutboundCount,
	ISNULL(@OutboundDraftCountCreated,0)		OutboundDraftCountCreated,
	ISNULL(@OutboundDraftCountAssigned,0)		OutboundDraftCountAssigned,
	ISNULL(@InboundCountCreated,0)				InboundCountCreated,
	ISNULL(@InboundCountAssigned,0)				InboundCountAssigned,
	ISNULL(@InternalOutboundCountCreated,0)		InternalOutboundCountCreated,
	ISNULL(@InternalOutboundCountAssigned,0)	InternalOutboundCountAssigned,
	ISNULL(@DelayedCount,0)						DelayedCount

END;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 Create PROCEDURE [dbo].[DashboardDetailsGet]	

	@FromDate						DATETIME,
	@ToDate							DATETIME,
	@EntitID						INT, 
	@UserID							INT,
	@level							INT,
	@CountrID						INT,
	@CultureName					VARCHAR(10),
	@PageIndex						int, 
	@PageSize						int,
	@DraftOutbound                  INT ,
	@InternalOutbound               INT ,
	@Inbound                        INT ,
	@ExternalOutbound               INT ,
	@TotalCount						int output

AS

BEGIN 

	DECLARE @V_CultureID int 
	DECLARE @V_FirstIndex int,@V_LastIndex  int 
	Set @V_FirstIndex = @PageIndex * @PageSize + 1
	SET @V_LastIndex  = @PageIndex * @PageSize + @PageSize

	SELECT @V_CultureID = Id
	FROM [dbo].[Cultures]
	WHERE ShortName=@CultureName



CREATE TABLE #TEMP (TransID INT)

IF @level = 1
	BEGIN

		IF @CountrID = 1
		BEGIN
			--عدد معاملات الصادر الخارجي
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			  TRANSACTIONCATEGORYID = @ExternalOutbound 
				--TRANSACTIONCATEGORYID = @ExternalOutbound 
				AND (Transactions.[CreatedBy] = @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END
		
		IF @CountrID = 2
		BEGIN
			--عدد معاملات مسودة الخطاب المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			 TRANSACTIONCATEGORYID = @DraftOutbound
				-- TRANSACTIONCATEGORYID = @DraftOutbound 
				AND (Transactions.[CreatedBy] = @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 3
		BEGIN				
			--عدد معاملات مسودة الخطاب المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToUserId = @UserID
			where 
			    TRANSACTIONCATEGORYID = @DraftOutbound
				-- TRANSACTIONCATEGORYID = @DraftOutbound 
				AND (Transactions.[CreatedBy] <> @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 4
		BEGIN
			--عدد معاملات الوارد الخارجي المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			TRANSACTIONCATEGORYID = @Inbound
				--TRANSACTIONCATEGORYID = @Inbound 
				AND (Transactions.[CreatedBy] = @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 5
		BEGIN
			--عدد معاملات الوارد الخارجي المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToUserId = @UserID
			where 
			TRANSACTIONCATEGORYID = @Inbound
				--TRANSACTIONCATEGORYID = @Inbound 
				AND (Transactions.[CreatedBy] <> @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 6
		BEGIN
			--عدد معاملات المعاملة الداخلية المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			TRANSACTIONCATEGORYID = @InternalOutbound
				--TRANSACTIONCATEGORYID = @InternalOutbound
				AND (Transactions.[CreatedBy] = @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 7
		BEGIN
			--عدد معاملات المعاملة الداخلية المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToUserId = @UserID
			where 
			TRANSACTIONCATEGORYID = @InternalOutbound
				--TRANSACTIONCATEGORYID = @InternalOutbound
				AND (Transactions.[CreatedBy] <> @UserID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 8
		BEGIN
			--عدد المعاملات المتأخرة 
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
				INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
			where 
				TransactionTypeId <> @ExternalOutbound 
				and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
				AND ([TransactionAssignments].ToUserId = @UserID)
		END
	END

	IF @level = 2
	BEGIN

		IF @CountrID = 1
		BEGIN
			--عدد معاملات الصادر الخارجي
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			  TRANSACTIONCATEGORYID = @ExternalOutbound 
				--TRANSACTIONCATEGORYID = @ExternalOutbound 
				AND (Transactions.OrgUnitId = @EntitID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 2
		BEGIN
			--عدد معاملات مسودة الخطاب المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			 TRANSACTIONCATEGORYID = @DraftOutbound
				-- TRANSACTIONCATEGORYID = @DraftOutbound 
				AND (Transactions.OrgUnitId = @EntitID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 3
		BEGIN
			--عدد معاملات مسودة الخطاب المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToEntityId <>@EntitID
			where 

				TRANSACTIONCATEGORYID = @DraftOutbound
			-- TRANSACTIONCATEGORYID = @DraftOutbound 
			AND (Transactions.OrgUnitId = @EntitID)
			
		--	AND Transactions.Date BETWEEN @FromDate AND @ToDate
			--AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);

		END

		IF @CountrID = 4
		BEGIN
			--عدد معاملات الوارد الخارجي المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @Inbound 
				AND (Transactions.OrgUnitId = @EntitID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate

		END

		IF @CountrID = 5
		BEGIN
		--عدد معاملات الوارد الخارجي المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToEntityId = @EntitID
			where 
				TRANSACTIONCATEGORYID = @Inbound
			--TRANSACTIONCATEGORYID = @Inbound 
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <>@EntitID )
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);
END

		IF @CountrID = 6
		BEGIN
			--عدد معاملات المعاملة الداخلية المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @InternalOutbound
				AND (Transactions.OrgUnitId = @EntitID)
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 7
		BEGIN
			--عدد معاملات المعاملة الداخلية المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToEntityId = @EntitID
			where 
				TRANSACTIONCATEGORYID = @InternalOutbound
			--TRANSACTIONCATEGORYID = @InternalOutbound
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);
	END

		IF @CountrID = 8
		BEGIN
			--عدد المعاملات المتأخرة 
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
				AND TransactionAssignmentHistories.ToEntityId = @EntitID
			where 
				TransactionTypeId <> @ExternalOutbound 
				and ([RemindDate] < GETDATE() OR DATEADD(day, 15, [TransactionAssignments].Date) < GETDATE())
		END

	END

	IF @level = 3
	BEGIN
		;WITH cte AS 
		 (
		  SELECT a.Id, a.parentId, a.name
		  FROM OrgUnits_VW a
		  WHERE Id = @EntitID
		  UNION ALL
		  SELECT a.Id, a.parentid, a.Name
		  FROM OrgUnits_VW a JOIN cte c ON a.parentId = c.id
		  )

		SELECT ID INTO #T FROM cte

		IF @CountrID = 1
		BEGIN
			--عدد معاملات الصادر الخارجي
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @ExternalOutbound 
				AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 2
		BEGIN
			--عدد معاملات مسودة الخطاب المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				 TRANSACTIONCATEGORYID = @DraftOutbound 
				AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 3
		BEGIN
			--عدد معاملات مسودة الخطاب المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
				 TRANSACTIONCATEGORYID = @DraftOutbound 
				AND (Transactions.OrgUnitId <> @EntitID)
				AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
				AND TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId
		END

		IF @CountrID = 4
		BEGIN
			--عدد معاملات الوارد الخارجي المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @Inbound 
				AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 5
		BEGIN
			--عدد معاملات الوارد الخارجي المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
				TRANSACTIONCATEGORYID = @Inbound 
				AND (Transactions.OrgUnitId <> @EntitID)
				AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
				AND TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId
		END

		IF @CountrID = 6
		BEGIN
			--عدد معاملات المعاملة الداخلية المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @InternalOutbound
				AND (Transactions.OrgUnitId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 7
		BEGIN
			--عدد معاملات المعاملة الداخلية المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
				TRANSACTIONCATEGORYID = @InternalOutbound
				AND (Transactions.OrgUnitId <> @EntitID)
				AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
				AND TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId
		END

		IF @CountrID = 8
		BEGIN
			--عدد المعاملات المتأخرة 
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
				INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
				TransactionTypeId <> @ExternalOutbound 
				and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
				AND (TransactionAssignmentHistories.ToEntityId IN (SELECT ID FROM #T))
		END

	END

	IF @level = 4
	BEGIN
		IF @CountrID = 1
		BEGIN
			--عدد معاملات الصادر الخارجي
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				  TRANSACTIONCATEGORYID = @ExternalOutbound 
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END

		IF @CountrID = 2
		BEGIN
			--عدد معاملات مسودة الخطاب المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @DraftOutbound
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END
			IF @CountrID = 3
		BEGIN
			--عدد معاملات مسودة الخطاب المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
				TRANSACTIONCATEGORYID = @DraftOutbound
			-- TRANSACTIONCATEGORYID = @DraftOutbound 
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <>@EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);
          END
		IF @CountrID = 4
		BEGIN
			--عدد معاملات الوارد الخارجي المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
				TRANSACTIONCATEGORYID = @Inbound
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END
		IF @CountrID = 5
		BEGIN
			--عدد معاملات الوارد الخارجي المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
			TRANSACTIONCATEGORYID = @Inbound
			--TRANSACTIONCATEGORYID = @Inbound 
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <>@EntitID )
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);
	END
		IF @CountrID = 6
		BEGIN
			--عدد معاملات المعاملة الداخلية المنشئة
			INSERT INTO #TEMP
			select 
				Id
			from 
				Transactions 
			where 
			TRANSACTIONCATEGORYID = @InternalOutbound
				AND Transactions.Date BETWEEN @FromDate AND @ToDate
		END
			IF @CountrID = 7
		BEGIN
			--عدد معاملات المعاملة الداخلية المحالة
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			where 
				TRANSACTIONCATEGORYID = @InternalOutbound
			--TRANSACTIONCATEGORYID = @InternalOutbound
			AND (Transactions.OrgUnitId = @EntitID)
			AND (TransactionAssignmentHistories.ToEntityId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
			AND ("TransactionAssignmentHistories"."FromUserId" != "TransactionAssignmentHistories"."ToUserId" or "TransactionAssignmentHistories"."ToUserId" is null);
	END
		IF @CountrID = 8
		BEGIN
			--عدد المعاملات المتأخرة 
			INSERT INTO #TEMP
			select DISTINCT
				Transactions.Id
			from 
				Transactions 
				INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
				INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
			where 
				TransactionTypeId <> @ExternalOutbound 
				and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
		END

	END
	

	SELECT 	
		Transactions.ID,
		Transactions.Number,
		Transactions.Date,
		Transactions.DateH,
		Transactions.LetterTypeId,
		L1.Text AS LetterType,
		Transactions.PriorityId,
		L2.Text AS Priority,
		Transactions.ConfidentialityId,
		L4.Text AS Confidentiality,
		Transactions.SourceTypeId,
		L3.Text AS SourceType,
		Transactions.Subject,
		Transactions.CreatedOn,
		[UserProfiles_VW].Name as Creator

	FROM
		Transactions
		INNER JOIN #TEMP TE ON TE.TransID = Transactions.ID
		LEFT JOIN [dbo].[LetterTypes] ON Transactions.LetterTypeId = [LetterTypes].Id
		LEFT JOIN [dbo].[Localizations] L1 ON L1.LocalizationIdentifier_Id = [LetterTypes].LocalizationIdentifier_Id
		AND L1.CultureId = @V_CultureID
		LEFT JOIN [dbo].[Priorities] ON Transactions.PriorityId = [Priorities].Id
		LEFT JOIN [dbo].[Localizations] L2 ON L2.LocalizationIdentifier_Id = [Priorities].LocalizationIdentifier_Id
		AND l2.CultureId = @V_CultureID
		LEFT JOIN [dbo].[TRANSACTIONTYPES] ON Transactions.TRANSACTIONTYPEID = [TRANSACTIONTYPES].Id
		LEFT JOIN [dbo].[Localizations] L3 ON L3.LocalizationIdentifier_Id = [TRANSACTIONTYPES].LocalizationIdentifier_Id
		AND L3.CultureId = @V_CultureID
		LEFT JOIN [dbo].[Permissions] ON Transactions.ConfidentialityId = [Permissions].Id
		LEFT JOIN [dbo].[LookupLocalizations] L4 ON L4.Lookup_Id = [Permissions].Name_Id
		AND L4.Culture_Id = @V_CultureID
		LEFT JOIN [dbo].[UserProfiles_VW] ON Transactions.CreatedBy = [UserProfiles_VW].id
	ORDER BY Transactions.id DESC
		OFFSET @PageIndex * @PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY


	  set @TotalCount  = (SELECT COUNT(1)	FROM #TEMP)
	  SELECT @TotalCount as N'@TotalCount'

END;






GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	MohammadH
-- Create date: 24/06/2019
-- Description:	Admin Move User
-- =============================================
CREATE PROCEDURE [dbo].[AdminMoveUser]

	 @UserProfileId			INT,
	 @OrgUnitId				INT,
	 @NewOrgUnitId			INT,
	 @LoggedInUser			INT

AS
SET NOCOUNT ON
BEGIN
		
	UPDATE TransactionAssignments
	SET TrayId = 7,
		ToUserId = NULL,
		ModefiedBy = @LoggedInUser,
		ModefiedOn = GETDATE()
	WHERE
		TrayId = 1
		AND ToUserId = @UserProfileId
		AND ToEntityId = @OrgUnitId

	UPDATE TransactionAssignments
	SET ToUserId = NULL,
		ModefiedBy = @LoggedInUser,
		ModefiedOn = GETDATE()
	WHERE
		TrayId = 6
		AND ToUserId = @UserProfileId
		AND ToEntityId = @OrgUnitId

	UPDATE TransactionCopies
	SET [UserId] = NULL,
		ModefiedBy = @LoggedInUser,
		ModefiedOn = GETDATE()
	WHERE
		[UserId] = @UserProfileId
		AND [EntityId] = @OrgUnitId
	
	UPDATE [dbo].[UserProfileOrgUnits]
	SET [OrgUnit_Id] = @NewOrgUnitId
	WHERE
		[UserProfile_Id] = @UserProfileId
END;

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	MohammadM
-- Create date: 29/06/2019
-- Description:	Admin Move Transaction By Id
-- =============================================
CREATE PROCEDURE [dbo].[AdminMoveTransactionsByID]

	@TransID		INT, 
	@ToUserID		INT, 
	@ToEntityID		INT,
	@LoggedInUser	INT
 
AS

SET NOCOUNT ON

BEGIN

	UPDATE TransactionAssignments
	SET ToEntityId	= @ToEntityID,
		ToUserId	= @ToUserID,
		TrayId		= CASE WHEN TrayId = 1 AND @ToUserID IS NULL THEN 7 ELSE TrayId END,
		ModefiedBy	= @LoggedInUser,
		ModefiedOn	= GETDATE()
	WHERE
		TransactionId = @TransID

	INSERT INTO TransactionAssignmentHistories
	SELECT
		TransactionAssignmentHistories.TrayId,
		TransactionAssignmentHistories.FromUserId,
		@ToUserID,
		TransactionAssignmentHistories.TransactionId,
		TransactionAssignmentHistories.ActionId,
		TransactionAssignmentHistories.FromEntityId,
		@ToEntityID,
		'تم نقلها بواسطة مدير النظام',
		GETDATE(),
		FORMAT(GETDATE(),'dd/MM/yyyy','ar'),
		NULL ,
		@LoggedInUser,
		GETDATE(),
		 USERPROFILES.ID,
		NULL,
		NULL
	FROM TransactionAssignmentHistories
	 INNER JOIN [dbo].[USERPROFILES] ON USERPROFILES.ID = TransactionAssignmentHistories.USERDELEGATIONID
	WHERE TransactionId = @TransID

	INSERT INTO TransactionEntityDetails ([TransactionId], [EntityId], [CreatedOn], [CreatedBy])
	SELECT 
		TransactionAssignments.TransactionId,
		TransactionAssignments.ToEntityId,
		GETDATE(),
		@LoggedInUser
	FROM
		TransactionAssignments 
		LEFT JOIN TransactionEntityDetails ON TransactionAssignments.TransactionId = TransactionEntityDetails.TransactionId
		AND TransactionAssignments.ToEntityId = TransactionEntityDetails.EntityId
	WHERE
		TransactionAssignments.TransactionId = @TransID
		AND TransactionEntityDetails.Id IS NULL

END;

Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	MohammadM
-- Create date: 29/06/2019
-- Description:	Admin Move Transactions
-- =============================================
CREATE PROCEDURE [dbo].[AdminMoveTransactions]

	@ToUserID		INT, 
	@ToEntityID		INT,
	@FromUserID		INT, 
	@FromEntityID	INT,
	@LoggedInUser	INT
AS

SET NOCOUNT ON

BEGIN

	UPDATE TransactionAssignments
	SET ToEntityId	= @ToEntityID,
		ToUserId	= @ToUserID,
		TrayId		= CASE WHEN TrayId = 1 AND @ToUserID IS NULL THEN 7 ELSE TrayId END,
		ModefiedBy	= @LoggedInUser,
		ModefiedOn	= GETDATE()
	WHERE
		ToEntityId = @FromEntityID
		AND (ToUserId = @FromUserID OR @FromUserID IS NULL)

	INSERT INTO TRANSACTIONASSIGNMENTHISTORIES
	SELECT
		TransactionAssignmentHistories.TrayId,
		TransactionAssignmentHistories.FromUserId,
		@ToUserID,
		TransactionAssignmentHistories.TransactionId,
		TransactionAssignmentHistories.ActionId,
		TransactionAssignmentHistories.FromEntityId,
		@ToEntityID,
		'تم نقلها بواسطة مدير النظام',
		GETDATE(),
		FORMAT(GETDATE(),'dd/MM/yyyy','ar'),
		NULL ,
		@LoggedInUser,
		GETDATE(),
		 USERPROFILES.ID,
		NULL,
		NULL
	FROM TransactionAssignmentHistories
  INNER JOIN [dbo].[USERPROFILES] ON USERPROFILES.ID = TransactionAssignmentHistories.USERDELEGATIONID
				
	WHERE ToEntityId = @FromEntityID AND (ToUserId = @FromUserID OR @FromUserID IS NULL)

	INSERT INTO TransactionEntityDetails ([TransactionId], [EntityId], [CreatedOn], [CreatedBy])
	SELECT 
		TransactionAssignments.TransactionId,
		TransactionAssignments.ToEntityId,
		GETDATE(),
		@LoggedInUser
	FROM
		TransactionAssignments 
		LEFT JOIN TransactionEntityDetails ON TransactionAssignments.TransactionId = TransactionEntityDetails.TransactionId
		AND TransactionAssignments.ToEntityId = TransactionEntityDetails.EntityId
	WHERE
		ToEntityId = @ToEntityID
		AND TransactionEntityDetails.Id IS NULL

END;

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	MohammadM
-- Create date: 29/06/2019
-- Description:	Admin Move Entity
-- =============================================
CREATE PROCEDURE [dbo].[AdminMoveEntity]

 @OrgUnitId			INT,
 @NewParentID		INT,
 @LoggedInUser		INT

AS

SET NOCOUNT ON

BEGIN

	UPDATE OrgUnits
	SET ParentId = @NewParentID,
		ModefiedBy = @LoggedInUser,
		ModefiedOn = GETDATE()
	WHERE
		ID = @OrgUnitId 	

END;

GO
/****** Object:  StoredProcedure [dbo].[ReportSearch]    Script Date: 7/10/2019 9:03:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportSearch]

	@DateFrom	            DateTime,
	@DateTo                 DateTime,
	@TransactionTypeId	    INT ,
	@TransactionNumber      INT   = NULL,
	@TransactioDescription  NVARCHAR(20)   = NULL,

	-------المشتركة 
	@ExplanationEditorType       INT = NULL, ------ ExplanationEditorType from Explanation table 
	@IsAppointment               BIT,
	------- if @IsAppointment  =1
	@AppointmentDate             DateTime = NULL,
	@ConfidentialityId	         INT  = NULL,
	@PriorityId	                 INT  = NULL,
	@SubjectClassificationId     INT  = NULL, --------[TransactionSubjectClassifications]
	@Remarks                     NVARCHAR(20)  = NULL,   
	@DeliveryMethodId            INT  = NULL,
	
	-------------بيانات أصحاب العلاقة 
	@FullName	           NVARCHAR(120)  = NULL ,
	@CivilID	           NVARCHAR(10)  = NULL ,
	@MobileNumber	       NVARCHAR(20)  = NULL  ,
		-------------------For InBound
	@IsForIndividual      Bit,--Inbound Destination  Type
	--------------FOR IsForIndividual =0
	@InboundDateH	        NVARCHAR(256)  = NULL ,---Inbound Date
	@ExternalPartyId	    INT  = NULL ,---Inbound  Destination
	@DocumentNumber         NVARCHAR(Max)  = NULL,--Inbound_Doc_No
	@OutBoundDate           NVARCHAR(256)  = NULL,----Outbound  Date
	------------Assignment transactions
	@FromOrgUnitId	       INT  = NULL ,
	@FromUserId	           INT  = NULL ,
	@ToOrgUnitId	       INT  = NULL ,
	@ToUserId              INT  = NULL ,
	--@Out_Columns        NVARCHAR(514),
	@CultureName           NVARCHAR(50)  = NULL ,

	---Pagenation
	 @PageIndex			INT , 
     @PageSize			INT,
	 @TotalCount        INT OUTPUT 

	
WITH RECOMPILE

AS

SET NOCOUNT ON

BEGIN

	--GET Culture ID From Cultures table
	DECLARE @V_CultureID int 
	SELECT @V_CultureID = Id
	FROM [dbo].[Cultures]
	WHERE ShortName=@CultureName

	--@DateTo
	IF (@DateTo IS NOT NULL)
	SET @DateTo = @DateTo +'23:59:00'
	
	-----

	DECLARE @InboundTrTypeID    INT
	DECLARE @OutBoundTRTypeID   INT
	DECLARE @InternalOutboundTRTypeID  INT
	DECLARE @OutBoundDraftTRTypeID   INT

	SELECT @InboundTrTypeID=L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Inbound' AND L."CategoryId"=10;

	 SELECT @OutBoundTRTypeID =L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Outbound' AND L."CategoryId"=10;

	 SELECT @InternalOutboundTRTypeID =L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Internal Outbound' AND L."CategoryId"=10;

	 	 SELECT @OutBoundDraftTRTypeID =L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Outbound Draft' AND L."CategoryId"=10;

	---------Dynamic Statement
	DECLARE @SQLStatement       NVARCHAR(MAX)
	DECLARE @SELECT_Statemet    NVARCHAR(MAX)
	DECLARE @Count_Statemet     NVARCHAR(4000)
	DECLARE @FROM_Statemet     NVARCHAR(4000)
	DECLARE @WHERE_Statement    NVARCHAR(4000)
	DECLARE @parameterDefinition NVARCHAR(4000)


	
------Basic Parameters

SET @parameterDefinition=N' 
	@P_TransactionNumber   INT ,
	@P_TransactioDescription   NVARCHAR(20),
	@P_ExplanationEditorType   INT , 
	@P_IsAppointment               BIT,
	@P_AppointmentDate             DateTime,
	@P_ConfidentialityId	         INT ,
	@P_PriorityId	                 INT ,
	@P_SubjectClassificationId     INT , 
	@P_Remarks                     NVARCHAR(20),   
	@P_DeliveryMethodId            INT ,
	@P_FullName	        nvarchar(120) ,
	@P_CivilID	        nvarchar(10) ,
	@P_MobileNumber	    nvarchar(20)  ,
	@P_IsForIndividual   Bit,
	@P_InboundDateH	    NVARCHAR(256) ,
	@P_ExternalPartyId	    INT ,
	@P_DocumentNumber   NVARCHAR(Max), 
	@P_OutBoundDate    NVARCHAR(256),
	@P_FromOrgUnitId   INT ,
	@P_FromUserId	   INT ,
	@P_ToOrgUnitId	   INT ,
	@P_ToUserId        INT ,
	@P_CultureID       INT ,
	@P_PageIndex       INT  , 
	@P_PageSize        INT ,
	@P_TotalCount      INT OUTPUT '

	CREATE TABLE #InScopeTr(TransID int)

	INSERT INTO #InScopeTr 
	SELECT Id From TRANSACTIONS TR
	WHERE (@TransactionTypeId =-1 OR TR.TransactionTypeId =@TransactionTypeId)
	AND (TR.Date between @DateFrom AND @DateTo)
		
	SET @Count_Statemet = N'SELECT @P_TotalCount= Count(*) '
	SET @SELECT_Statemet = N'SELECT ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,
	TR.Id TransactionId,
	TR.TransactionTypeId,
	llOC_TransactionType.Text TransactionTypeText,
	TR.Date,
    TR.Number,
    TR.Subject TransactioDescription, 
    Explan.ExplanationEditorType,
    TR.ConfidentialityId,
    LLOC_Perm.text As ConfidentialityText,
    TR.PriorityId,
    LOC_PR.Text As PriorityText,
    TRSubClass.SubjectClassificationId    ,
    LOC_SubClass.Text SubjectClassificationText,
    TR.Remarks,
    TR.DeliveryMethodId,
    llOC_Delivery.Text DeliveryMethodText,
    TR.Subject,
    Names.FirstName,
	Names.CivilID,
	Names.MobileNumber,
    TR.ExternalPartyId,
    Loc_External.Text ExternalPartyText,
    TR.InboundDateH, 
    TR.DocumentNumber,
    TR.Createdon OutBoundDate,
    TRAssign.FromEntityId ,
    Loc_FromEntityId.Text FromEntityText,
    TRAssign.FromUserId ,
    Loc_FromUserId.Text FromUserText,
    TRAssign.ToEntityId,
    Loc_ToEntityId.Text ToEntityText,
    TRAssign.ToUserId,
    Loc_ToUserId.Text ToUserText,
	TR.RemindDate '
	
	SET @FROM_Statemet  =N'FROM     
		Transactions TR INNER JOIN  #InScopeTr ON TR.Id=#InScopeTr.TransId
		lEFT OUTER JOIN  TransactionNames   ON TR.Id =TransactionNames.TransactionId
		lEFT OUTER JOIN Names Names  ON TransactionNames.NameId=Names.Id	
		lEFT OUTER JOIN Permissions Perm ON Perm.Id = TR.ConfidentialityId
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm ON LLOC_Perm.Lookup_Id = Perm.Name_Id 
		AND LLOC_Perm.Culture_Id = @P_CultureID	
		lEFT OUTER JOIN Priorities PR ON PR.Id = TR.PriorityId	
		lEFT OUTER JOIN Localizations LOC_PR ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id 
		    AND LOC_PR.CultureId =@P_CultureID	
		lEFT OUTER JOIN LookupLocalizations llOC_Delivery ON TR.DeliveryMethodId=llOC_Delivery.Lookup_Id 
		    AND llOC_Delivery.Culture_Id =@P_CultureID	
		lEFT OUTER JOIN LookupLocalizations llOC_TransactionType ON TR.TransactionTypeId = llOC_TransactionType.Lookup_Id 
		AND llOC_TransactionType.Culture_Id =@P_CultureID	
		LEFT OUTER JOIN Explanations Explan  ON TR.Id= Explan.TransactionId
		LEFT OUTER JOIN TransactionSubjectClassificati TRSubClass ON TR.Id= TRSubClass.TransactionId
		LEFT OUTER JOIN SubjectClassifications SubClass  ON SubClass.Id=TRSubClass.SubjectClassificationId
		LEFT OUTER JOIN   Localizations LOC_SubClass  ON  SubClass.LocalizationIdentifier_Id=LOC_SubClass.LocalizationIdentifier_Id
		   AND LOC_SubClass.CultureId =@P_CultureID
		LEFT OUTER JOIN TransactionAssignments TRAssign ON TR.Id= TRAssign.TransactionId
	    LEFT OUTER JOIN OrgUnits OrgUnits_ToEntity ON TRAssign.ToEntityId=OrgUnits_ToEntity.Id
	    LEFT OUTER JOIN Localizations Loc_ToEntityId 
	       ON  OrgUnits_ToEntity.LocalizationIdentifier_Id=Loc_ToEntityId.LocalizationIdentifier_Id
	       AND Loc_ToEntityId.CultureId =@P_CultureID
	    LEFT OUTER JOIN OrgUnits OrgUnits_FromEntity ON TRAssign.FromEntityId=OrgUnits_FromEntity.Id
	    LEFT OUTER JOIN Localizations Loc_FromEntityId 
	      ON  OrgUnits_FromEntity.LocalizationIdentifier_Id=Loc_FromEntityId.LocalizationIdentifier_Id
		  AND Loc_FromEntityId.CultureId =@P_CultureID
	    LEFT OUTER JOIN	 UserProfiles  UserProfiles_ToUserId ON  TRAssign.ToUserId=UserProfiles_ToUserId.Id
	    LEFT OUTER JOIN Localizations Loc_ToUserId 
	         ON UserProfiles_ToUserId.LocalizationIdentifier_Id=Loc_ToUserId.LocalizationIdentifier_Id
	       AND Loc_ToUserId.CultureId =@P_CultureID
	   LEFT OUTER JOIN	 UserProfiles  UserProfiles_FromUserId ON  TRAssign.ToUserId=UserProfiles_FromUserId.Id
	   LEFT OUTER JOIN Localizations Loc_FromUserId 
	      ON UserProfiles_FromUserId.LocalizationIdentifier_Id=Loc_FromUserId.LocalizationIdentifier_Id
		  And Loc_FromUserId.CultureId =@P_CultureID		 
	  LEFT OUTER JOIN ExternalParties ON TR.ExternalPartyId =ExternalParties.Id
		LEFT OUTER JOIN Localizations Loc_External ON ExternalParties.Name_Id=
		Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=@P_CultureID 

		'
	SET  @WHERE_Statement	 =  'Where ( @P_TransactionNumber is null OR TR.Number = @P_TransactionNumber) 
	            AND (@P_ExplanationEditorType is null or Explan.ExplanationEditorType=@P_ExplanationEditorType)
	       		AND (@P_ConfidentialityId  is null OR TR.ConfidentialityId =@P_ConfidentialityId)
				AND (@P_PriorityId  is null  OR TR.PriorityId = @P_PriorityId)
				AND (@P_SubjectClassificationId is null or TRSubClass.SubjectClassificationId=@P_SubjectClassificationId)
			    AND (@P_DeliveryMethodId is null OR TR.DeliveryMethodId=@P_DeliveryMethodId)
				AND (@P_FullName is null  OR Names.FirstName =@P_FullName)
		        AND (@P_CivilID is null  OR Names.CivilID =@P_CivilID)
				AND (@P_MobileNumber is null  OR Names.MobileNumber =@P_MobileNumber)
				
	            AND (@P_FromOrgUnitId is null or  TRAssign.FromEntityId =@P_FromOrgUnitId)
	            AND (@P_FromUserId	is null or  TRAssign.FromUserId=@P_FromUserId )
	            AND (@P_ToOrgUnitId	is null or  TRAssign.ToEntityId=@P_ToOrgUnitId)
	            AND (@P_ToUserId is null or  TRAssign.ToUserId = @P_ToUserId) 
		 '
		IF (@TransactioDescription IS NOT NULL)
	SET  @WHERE_Statement = @WHERE_Statement +
	'  AND  CONTAINS(TR.Subject,@P_TransactioDescription)'

	IF 	(@Remarks IS NOT NULL)
	SET  @WHERE_Statement = @WHERE_Statement + 
	' AND	 CONTAINS(TR.Remarks,@P_Remarks)'

	--AND @IsAppointment ,@AppointmentDate
IF(@IsAppointment IS NOT NULL)
BEGIN
If (@IsAppointment =1)
BEGIN
	SET  @WHERE_Statement = @WHERE_Statement + 
	'AND (TR.RemindDate IS NOT NULL)
	 AND (@P_AppointmentDate IS NULL OR TR.RemindDate = @P_AppointmentDate ) '
	END
	ELSE
	BEGIN 
	SET  @WHERE_Statement = @WHERE_Statement + 
	'AND (TR.RemindDate IS NULL)'
	END 
		END
	----INBOUND
	IF (@TransactionTypeId=@InboundTrTypeID  or @TransactionTypeId=@InternalOutboundTRTypeID  )
	BEGIN 

	--SET @FROM_Statemet = @FROM_Statemet  + N'  LEFT OUTER JOIN ExternalParties ON TR.ExternalPartyId =ExternalParties.Id
	--	LEFT OUTER JOIN Localizations Loc_External ON ExternalParties.Name_Id=
	--	Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=@P_CultureID  '

		SET  @WHERE_Statement = @WHERE_Statement + ' AND	TR.IsForIndividual =@P_IsForIndividual '
			
			IF (@IsForIndividual =0)
				BEGIN
		SET  @WHERE_Statement = @WHERE_Statement + 
	' AND  (@P_InboundDateH IS NULL OR TR.InboundDateH=@P_InboundDateH	 )    
	  AND (@P_ExternalPartyId IS NULL OR TR.ExternalPartyId=@P_ExternalPartyId )	
	  AND (@P_DocumentNumber IS NULL OR TR.DocumentNumber=@P_DocumentNumber)'
				END
	END
		--- OUTBOUND
		IF (@TransactionTypeId=@OutBoundTRTypeID    or @TransactionTypeId=@OutBoundDraftTRTypeID   )
		BEGIN

		--	SET @FROM_Statemet = @FROM_Statemet  + N'  LEFT OUTER JOIN ExternalParties ON TR.ExternalPartyId =ExternalParties.Id
		--LEFT OUTER JOIN Localizations Loc_External ON ExternalParties.Name_Id=
		--Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=@P_CultureID  '

			SET  @WHERE_Statement = @WHERE_Statement 
			+ '	AND (@P_ExternalPartyId IS NULL OR TR.ExternalPartyId=@P_ExternalPartyId ) 
				AND ( @P_OutBoundDate IS NULL OR TR.Createdon= @P_OutBoundDate )'
	     END -- END IF 





		 -------For Return Total Count
		
SET @SQLStatement=@Count_Statemet + @FROM_Statemet + @WHERE_Statement
EXECUTE sp_executesql @SQLStatement, @parameterDefinition, 
    @P_TransactionNumber          = @TransactionNumber,
	@P_TransactioDescription      = @TransactioDescription,
	@P_ExplanationEditorType      = @ExplanationEditorType, 
	@P_IsAppointment              = @IsAppointment,
	@P_AppointmentDate            = @AppointmentDate,
	@P_ConfidentialityId	      = @ConfidentialityId,
	@P_PriorityId	              = @PriorityId,
	@P_SubjectClassificationId    = @SubjectClassificationId, 
	@P_Remarks                    = @Remarks,   
	@P_DeliveryMethodId           = @DeliveryMethodId,
	@P_FullName	         = @FullName,
	@P_CivilID	         = @CivilID,
	@P_MobileNumber	     = @MobileNumber,
	@P_IsForIndividual   = @IsForIndividual,
	@P_InboundDateH	     = @InboundDateH,
	@P_ExternalPartyId    =  @ExternalPartyId,
	@P_DocumentNumber     = @DocumentNumber, 
	@P_OutBoundDate       = @OutBoundDate,
	@P_FromOrgUnitId      = @FromOrgUnitId,
	@P_FromUserId	      = @FromUserId,
	@P_ToOrgUnitId	      = @ToOrgUnitId,
	@P_ToUserId           = @ToUserId,
	@P_CultureID          = @V_CultureID ,
	@P_PageIndex          = @PageIndex  , 
	@P_PageSize           = @PageSize,
	@P_TotalCount   = @TotalCount OUTPUT

		 -------Pagination

		SET @WHERE_Statement = @WHERE_Statement  +' ORDER BY TR.ID DESC
	OFFSET @P_PageIndex * @P_PageSize ROWS
	FETCH NEXT @P_PageSize ROWS ONLY  '

	-------For Result SET
SET @SQLStatement=@SELECT_Statemet + @FROM_Statemet + @WHERE_Statement

EXECUTE sp_executesql @SQLStatement, @parameterDefinition, 
    @P_TransactionNumber          = @TransactionNumber,
	@P_TransactioDescription      = @TransactioDescription,
	@P_ExplanationEditorType      = @ExplanationEditorType, 
	@P_IsAppointment              = @IsAppointment,
	@P_AppointmentDate            = @AppointmentDate,
	@P_ConfidentialityId	      = @ConfidentialityId,
	@P_PriorityId	              = @PriorityId,
	@P_SubjectClassificationId    = @SubjectClassificationId, 
	@P_Remarks                    = @Remarks,   
	@P_DeliveryMethodId           = @DeliveryMethodId,
	@P_FullName	         = @FullName,
	@P_CivilID	         = @CivilID,
	@P_MobileNumber	     = @MobileNumber,
	@P_IsForIndividual   = @IsForIndividual,
	@P_InboundDateH	     = @InboundDateH,
	@P_ExternalPartyId   =  @ExternalPartyId,
	@P_DocumentNumber     = @DocumentNumber, 
	@P_OutBoundDate       = @OutBoundDate,
	@P_FromOrgUnitId      = @FromOrgUnitId,
	@P_FromUserId	      = @FromUserId,
	@P_ToOrgUnitId	      = @ToOrgUnitId,
	@P_ToUserId           = @ToUserId,
	@P_CultureID          = @V_CultureID ,
	@P_PageIndex          = @PageIndex  , 
	@P_PageSize           = @PageSize ,
	@P_TotalCount   = @TotalCount OUTPUT;


	END 

	GO
/****** Object:  StoredProcedure [dbo].[ReportSearch]    Script Date: 7/16/2019 12:57:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER  PROCEDURE [dbo].[ReportSearch]

	@DateFrom	            DateTime,
	@DateTo                 DateTime,
	@TransactionCategoryId	    INT ,
	@TransactionNumber      INT   = NULL,
	@TransactioDescription  NVARCHAR(20)   = NULL,

	-------المشتركة 
	--@ExplanationEditorType       INT = NULL, ------ ExplanationEditorType from Explanation table 
	@IsAppointment               BIT,
	------- if @IsAppointment  =1
	@AppointmentDate             DateTime = NULL,
	@ConfidentialityId	         INT  = NULL,
	@PriorityId	                 INT  = NULL,
	@LetterTypeId     INT  = NULL, --------[TransactionSubjectClassifications]
	@Remarks                     NVARCHAR(20)  = NULL,   
	@DeliveryMethodId            INT  = NULL,
	
	-------------بيانات أصحاب العلاقة 
	@FullName	           NVARCHAR(120)  = NULL ,
	@CivilID	           NVARCHAR(10)  = NULL ,
	@MobileNumber	       NVARCHAR(20)  = NULL  ,
		-------------------For InBound
	@IsForIndividual      Bit,--Inbound Destination  Type
	--------------FOR IsForIndividual =0
	@InboundDateH	        NVARCHAR(256)  = NULL ,---Inbound Date
	@ExternalPartyId	    INT  = NULL ,---Inbound  Destination
	@DocumentNumber         NVARCHAR(Max)  = NULL,--Inbound_Doc_No
	@OutBoundDate           NVARCHAR(256)  = NULL,----Outbound  Date
	------------Assignment transactions
	@FromOrgUnitId	       INT  = NULL ,
	@FromUserId	           INT  = NULL ,
	@ToOrgUnitId	       INT  = NULL ,
	@ToUserId              INT  = NULL ,
	--@Out_Columns        NVARCHAR(514),
	@CultureName           NVARCHAR(50)  = NULL ,

	---Pagenation
	 @PageIndex			INT , 
     @PageSize			INT,
	 @TotalCount        INT OUTPUT 

	
WITH RECOMPILE

AS

SET NOCOUNT ON

BEGIN

	--GET Culture ID From Cultures table
	DECLARE @V_CultureID int 
	SELECT @V_CultureID = Id
	FROM [dbo].[Cultures]
	WHERE ShortName=@CultureName

	--@DateTo
	IF (@DateTo IS NOT NULL)
	SET @DateTo = @DateTo +'23:59:00'
	
	
	
	-----

	DECLARE @InboundTrTypeID    INT
	DECLARE @OutBoundTRTypeID   INT
	DECLARE @InternalOutboundTRTypeID  INT
	DECLARE @OutBoundDraftTRTypeID   INT

	SELECT @InboundTrTypeID=L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Inbound' AND L."CategoryId"=10;

	 SELECT @OutBoundTRTypeID =L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Outbound' AND L."CategoryId"=10;

	 SELECT @InternalOutboundTRTypeID =L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Internal Outbound' AND L."CategoryId"=10

	 	 SELECT @OutBoundDraftTRTypeID =L.Id FROM 
	 "LookupLocalizations" LLOC INNER JOIN "Lookups" L ON LLOC."Lookup_Id" =L."Id"
	 where "Text" ='Outbound Draft' AND L."CategoryId"=10;

	---------Dynamic Statement
	DECLARE @SQLStatement       NVARCHAR(MAX)
	DECLARE @SELECT_Statemet    NVARCHAR(MAX)
	DECLARE @Count_Statemet     NVARCHAR(4000)
	DECLARE @FROM_Statemet      NVARCHAR(MAX)
	DECLARE @WHERE_Statement    NVARCHAR(MAX)
	DECLARE @parameterDefinition NVARCHAR(4000)


	
------Basic Parameters

SET @parameterDefinition=N' 
	@P_TransactionNumber       INT ,
	@P_TransactioDescription   NVARCHAR(20),
	--@P_ExplanationEditorType   INT , 
	@P_IsAppointment           BIT,
	@P_AppointmentDate         DateTime,
	@P_ConfidentialityId	   INT ,
	@P_PriorityId	           INT ,
	@P_LetterTypeId     INT , 
	@P_Remarks                     NVARCHAR(20),   
	@P_DeliveryMethodId            INT ,
	@P_FullName	        NVARCHAR(120) ,
	@P_CivilID	        NVARCHAR(10) ,
	@P_MobileNumber	    NVARCHAR(20)  ,
	@P_IsForIndividual   Bit,
	@P_InboundDateH	    NVARCHAR(256) ,
	@P_ExternalPartyId	    INT ,
	@P_DocumentNumber   NVARCHAR(Max), 
	@P_OutBoundDate    NVARCHAR(256),
	@P_FromOrgUnitId   INT ,
	@P_FromUserId	   INT ,
	@P_ToOrgUnitId	   INT ,
	@P_ToUserId        INT ,
	@P_CultureID       INT ,
	@P_PageIndex       INT  , 
	@P_PageSize        INT ,
	@P_TotalCount      INT OUTPUT '

	CREATE TABLE #InScopeTr(TransID int)

	INSERT INTO #InScopeTr 
	SELECT Id From TRANSACTIONS TR
	WHERE (@TransactionCategoryId =-1 OR TR.TransactionCategoryId =@TransactionCategoryId)
	AND (TR.Date between @DateFrom AND @DateTo)
		
--	Select * from #InScopeTr

	SET @Count_Statemet = N' SELECT @P_TotalCount= Count(*) '
	SET @SELECT_Statemet = N' SELECT ROW_NUMBER() OVER(ORDER BY TR.ID asc) AS RowNumber,
	 TR.TransactionCategoryId,
	 TR.ID TransactionId ,
	llOC_TransactionType.Text TransactionTypeText,
	TR.OrgUnitId,
	Loc_CreatorEntityId.Text OrgUnitText,
	TR.Date,
    TR.Number,
    TR.Subject TransactioDescription, 
    --Explan.ExplanationEditorType,
    TR.ConfidentialityId,
    LLOC_Perm.text As ConfidentialityText,
    TR.PriorityId,
    LOC_PR.Text As PriorityText,
     TR.Remarks,
    TR.DeliveryMethodId,
    llOC_Delivery.Text DeliveryMethodText,
    TR.Subject,
    Names.FirstName,
	Names.CivilID,
	Names.MobileNumber,
    TR.ExternalPartyId,
    Loc_External.Text ExternalPartyText,
    TR.InboundDateH, 
    TR.DocumentNumber,
    TR.Createdon OutBoundDate,
    TRAssign.FromEntityId ,
    Loc_FromEntityId.Text FromEntityText,
    TRAssign.FromUserId ,
    Loc_FromUserId.Text FromUserText,
    TRAssign.ToEntityId,
    Loc_ToEntityId.Text ToEntityText,
    TRAssign.ToUserId,
    Loc_ToUserId.Text ToUserText,
	TR.RemindDate ,
	TR.LetterTypeId,
	LOC_LT.Text LetterTypeText ,
	LOC_ST.Text SourceTypeText ,
	TR.OutboundDraftId
 '
	
	SET @FROM_Statemet  =N' FROM      
		Transactions TR INNER JOIN  #InScopeTr ON TR.Id=#InScopeTr.TransId
		lEFT OUTER JOIN  TransactionNames   ON TR.Id =TransactionNames.TransactionId
		lEFT OUTER JOIN Names Names  ON TransactionNames.NameId=Names.Id	
		lEFT OUTER JOIN Permissions Perm ON Perm.Id = TR.ConfidentialityId
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm ON LLOC_Perm.Lookup_Id = Perm.Name_Id 
		AND LLOC_Perm.Culture_Id = @P_CultureID	
		lEFT OUTER JOIN Priorities PR ON PR.Id = TR.PriorityId	
		lEFT OUTER JOIN Localizations LOC_PR ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id 
		    AND LOC_PR.CultureId =@P_CultureID	
		lEFT OUTER JOIN LookupLocalizations llOC_Delivery ON TR.DeliveryMethodId=llOC_Delivery.Lookup_Id 
		    AND llOC_Delivery.Culture_Id =@P_CultureID	
		lEFT OUTER JOIN LookupLocalizations llOC_TransactionType ON TR.TransactionCategoryId = llOC_TransactionType.Lookup_Id 
		AND llOC_TransactionType.Culture_Id =@P_CultureID	
		--LEFT OUTER JOIN Explanations Explan  ON TR.Id= Explan.TransactionId
		
		LEFT OUTER JOIN LetterTypes LT ON  LT.Id= TR.LetterTypeId
        LEFT OUTER JOIN   Localizations LOC_LT  ON  LT.LocalizationIdentifier_Id=LOC_LT.LocalizationIdentifier_Id
		   AND LOC_LT.CultureId =@P_CultureID

	  -- LEFT OUTER JOIN SourceTypes ST  ON TR.LetterTypeId = ST.Id
LEFT OUTER JOIN   Localizations LOC_ST  ON  LT.LocalizationIdentifier_Id=LOC_ST.LocalizationIdentifier_Id
		   AND LOC_ST.CultureId =@P_CultureID

	     LEFT OUTER JOIN OrgUnits Creator_OrgUnits ON TR.OrgUnitId=Creator_OrgUnits.Id
	    LEFT OUTER JOIN Localizations Loc_CreatorEntityId 
	       ON  Creator_OrgUnits.LocalizationIdentifier_Id=Loc_CreatorEntityId.LocalizationIdentifier_Id
	       AND Loc_CreatorEntityId.CultureId =@P_CultureID

		LEFT OUTER JOIN TransactionAssignments TRAssign ON TR.Id= TRAssign.TransactionId
	    LEFT OUTER JOIN OrgUnits OrgUnits_ToEntity ON TRAssign.ToEntityId=OrgUnits_ToEntity.Id
	    LEFT OUTER JOIN Localizations Loc_ToEntityId 
	       ON  OrgUnits_ToEntity.LocalizationIdentifier_Id=Loc_ToEntityId.LocalizationIdentifier_Id
	       AND Loc_ToEntityId.CultureId =@P_CultureID
	    LEFT OUTER JOIN OrgUnits OrgUnits_FromEntity ON TRAssign.FromEntityId=OrgUnits_FromEntity.Id
	    LEFT OUTER JOIN Localizations Loc_FromEntityId 
	      ON  OrgUnits_FromEntity.LocalizationIdentifier_Id=Loc_FromEntityId.LocalizationIdentifier_Id
		  AND Loc_FromEntityId.CultureId =@P_CultureID
	    LEFT OUTER JOIN	 UserProfiles  UserProfiles_ToUserId ON  TRAssign.ToUserId=UserProfiles_ToUserId.Id
	    LEFT OUTER JOIN Localizations Loc_ToUserId 
	         ON UserProfiles_ToUserId.LocalizationIdentifier_Id=Loc_ToUserId.LocalizationIdentifier_Id
	       AND Loc_ToUserId.CultureId =@P_CultureID

	   LEFT OUTER JOIN	 UserProfiles  UserProfiles_FromUserId ON  TRAssign.ToUserId=UserProfiles_FromUserId.Id
	   LEFT OUTER JOIN Localizations Loc_FromUserId 
	      ON UserProfiles_FromUserId.LocalizationIdentifier_Id=Loc_FromUserId.LocalizationIdentifier_Id
		  And Loc_FromUserId.CultureId =@P_CultureID		 
	  LEFT OUTER JOIN ExternalParties ON TR.ExternalPartyId =ExternalParties.Id
		LEFT OUTER JOIN Localizations Loc_External ON ExternalParties.Name_Id=
		Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=@P_CultureID 

		'
	SET  @WHERE_Statement	 =  ' Where ( @P_TransactionNumber is null OR TR.Number = @P_TransactionNumber) 
	            --AND (@P_ExplanationEditorType is null or Explan.ExplanationEditorType=@P_ExplanationEditorType)
	       		AND (@P_ConfidentialityId  is null OR TR.ConfidentialityId =@P_ConfidentialityId)
				AND (@P_PriorityId  is null  OR TR.PriorityId = @P_PriorityId)
				AND (@P_LetterTypeId is null or TR.LetterTypeId=@P_LetterTypeId)
			    AND (@P_DeliveryMethodId is null OR TR.DeliveryMethodId=@P_DeliveryMethodId)
				AND (@P_FullName is null  OR Names.FirstName =@P_FullName)
		        AND (@P_CivilID is null  OR Names.CivilID =@P_CivilID)
				AND (@P_MobileNumber is null  OR Names.MobileNumber =@P_MobileNumber)
				
	            AND (@P_FromOrgUnitId is null or  TRAssign.FromEntityId =@P_FromOrgUnitId)
	            AND (@P_FromUserId	is null or  TRAssign.FromUserId=@P_FromUserId )
	            AND (@P_ToOrgUnitId	is null or  TRAssign.ToEntityId=@P_ToOrgUnitId)
	            AND (@P_ToUserId is null or  TRAssign.ToUserId = @P_ToUserId) 
	 	 '
		IF (@TransactioDescription IS NOT NULL)
	SET  @WHERE_Statement = @WHERE_Statement +
	'  AND  CONTAINS(TR.Subject,@P_TransactioDescription)  '

	IF 	(@Remarks IS NOT NULL)
	SET  @WHERE_Statement = @WHERE_Statement + 
	' AND	 CONTAINS(TR.Remarks,@P_Remarks) '

	--AND @IsAppointment ,@AppointmentDate
IF(@IsAppointment IS NOT NULL)
BEGIN
If (@IsAppointment =1)
BEGIN
	SET  @WHERE_Statement = @WHERE_Statement + 
	'AND (TR.RemindDate IS NOT NULL)
	 AND (@P_AppointmentDate IS NULL OR TR.RemindDate = @P_AppointmentDate )  '
	END
	ELSE
	BEGIN 
	SET  @WHERE_Statement = @WHERE_Statement + 
	'AND (TR.RemindDate IS NULL) '
	END 
		END
	----INBOUND
	IF (@TransactionCategoryId=@InboundTrTypeID  or @TransactionCategoryId=@InternalOutboundTRTypeID  )
	BEGIN 
		SET  @WHERE_Statement = @WHERE_Statement + '  AND	(@P_IsForIndividual is null or TR.IsForIndividual = @P_IsForIndividual )'
			IF (@IsForIndividual =0)
				BEGIN
		SET  @WHERE_Statement = @WHERE_Statement + 
	' AND  (@P_InboundDateH IS NULL OR TR.InboundDateH=@P_InboundDateH	 )    
	  AND (@P_ExternalPartyId IS NULL OR TR.ExternalPartyId=@P_ExternalPartyId )	
	  AND (@P_DocumentNumber IS NULL OR TR.DocumentNumber=@P_DocumentNumber) '
				END
	END
		--- OUTBOUND
		IF (@TransactionCategoryId=@OutBoundTRTypeID    or @TransactionCategoryId=@OutBoundDraftTRTypeID   )
		BEGIN

		--	SET @FROM_Statemet = @FROM_Statemet  + N'  LEFT OUTER JOIN ExternalParties ON TR.ExternalPartyId =ExternalParties.Id
		--LEFT OUTER JOIN Localizations Loc_External ON ExternalParties.Name_Id=
		--Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=@P_CultureID  '

			SET  @WHERE_Statement = @WHERE_Statement 
			+ '	AND (@P_ExternalPartyId IS NULL OR TR.ExternalPartyId=@P_ExternalPartyId ) 
				AND ( @P_OutBoundDate IS NULL OR TR.Createdon= @P_OutBoundDate ) '
	     END -- END IF 
		 		 -------For Return Total Count
		
SET @SQLStatement=@Count_Statemet + @FROM_Statemet + @WHERE_Statement
EXECUTE sp_executesql @SQLStatement, @parameterDefinition, 
    @P_TransactionNumber          = @TransactionNumber,
	@P_TransactioDescription      = @TransactioDescription,
	--@P_ExplanationEditorType      = @ExplanationEditorType, 
	@P_IsAppointment              = @IsAppointment,
	@P_AppointmentDate            = @AppointmentDate,
	@P_ConfidentialityId	      = @ConfidentialityId,
	@P_PriorityId	              = @PriorityId,
	@P_LetterTypeId    = @LetterTypeId, 
	@P_Remarks                    = @Remarks,   
	@P_DeliveryMethodId           = @DeliveryMethodId,
	@P_FullName	         = @FullName,
	@P_CivilID	         = @CivilID,
	@P_MobileNumber	     = @MobileNumber,
	@P_IsForIndividual   = @IsForIndividual,
	@P_InboundDateH	     = @InboundDateH,
	@P_ExternalPartyId    =  @ExternalPartyId,
	@P_DocumentNumber     = @DocumentNumber, 
	@P_OutBoundDate       = @OutBoundDate,
	@P_FromOrgUnitId      = @FromOrgUnitId,
	@P_FromUserId	      = @FromUserId,
	@P_ToOrgUnitId	      = @ToOrgUnitId,
	@P_ToUserId           = @ToUserId,
	@P_CultureID          = @V_CultureID ,
	@P_PageIndex          = @PageIndex  , 
	@P_PageSize           = @PageSize,
	@P_TotalCount   = @TotalCount OUTPUT

		 -------Pagination

		SET @WHERE_Statement = @WHERE_Statement  +'  ORDER BY TR.ID DESC 
	OFFSET @P_PageIndex * @P_PageSize ROWS 
	FETCH NEXT @P_PageSize ROWS ONLY  '

	-------For Result SET
	
--	print @SQLStatement
SET @SQLStatement=@SELECT_Statemet + @FROM_Statemet + @WHERE_Statement
print @SQLStatement
EXECUTE sp_executesql @SQLStatement, @parameterDefinition, 
    @P_TransactionNumber          = @TransactionNumber,
	@P_TransactioDescription      = @TransactioDescription,
	--@P_ExplanationEditorType      = @ExplanationEditorType, 
	@P_IsAppointment              = @IsAppointment,
	@P_AppointmentDate            = @AppointmentDate,
	@P_ConfidentialityId	      = @ConfidentialityId,
	@P_PriorityId	              = @PriorityId,
	@P_LetterTypeId    = @LetterTypeId, 
	@P_Remarks                    = @Remarks,   
	@P_DeliveryMethodId           = @DeliveryMethodId,
	@P_FullName	         = @FullName,
	@P_CivilID	         = @CivilID,
	@P_MobileNumber	     = @MobileNumber,
	@P_IsForIndividual   = @IsForIndividual,
	@P_InboundDateH	     = @InboundDateH,
	@P_ExternalPartyId   =  @ExternalPartyId,
	@P_DocumentNumber     = @DocumentNumber, 
	@P_OutBoundDate       = @OutBoundDate,
	@P_FromOrgUnitId      = @FromOrgUnitId,
	@P_FromUserId	      = @FromUserId,
	@P_ToOrgUnitId	      = @ToOrgUnitId,
	@P_ToUserId           = @ToUserId,
	@P_CultureID          = @V_CultureID ,
	@P_PageIndex          = @PageIndex  , 
	@P_PageSize           = @PageSize ,
	@P_TotalCount   = @TotalCount OUTPUT;


	END 
GO
/****** Object:  StoredProcedure [dbo].[ReportStatistical]    Script Date: 7/18/2019 8:28:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

USE [CPPA]
GO
/****** Object:  StoredProcedure [dbo].[ReportStatistical]    Script Date: 11/28/2021 12:02:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[ReportStatistical]	

	@ReportType						INT,
	@FromDate						DATETIME,
	@ToDate							DATETIME,
	@EntitID						INT, 
	@UserID							INT,
	@level							INT,

	@LetterTypeId					INT,
	@AppointmentDate				DATETIME,
	@ConfidentialityId				INT,
	@PriorityId						INT,
	@TransactionTypeId					INT,
	@Remarks						NVARCHAR(MAX),
	@DeliveryMethodId				INT,
	@PageIndex						int, 
	@PageSize						int,
	@DraftOutbound                  INT ,
	@InternalOutbound               INT ,
	@Inbound                        INT ,
	@ExternalOutbound               INT ,
	@TotalCount						int output


AS

BEGIN 

	Create TABLE #DATA (
							OrgUnitsID INT,
							UserProfilesID INT,
							OutboundCount INT,
							OutboundDraftCountCreated INT,
							OutboundDraftCountAssigned INT,
							InboundCountCreated INT,
							InboundCountAssigned INT,
							InternalOutboundCountCreated INT,
							InternalOutboundCountAssigned INT,
							DelayedCount INT
						)

	DECLARE
	@OutboundCount					INT,
	@OutboundDraftCountCreated		INT,
	@OutboundDraftCountAssigned		INT,
	@InboundCountCreated			INT,
	@InboundCountAssigned			INT,
	@InternalOutboundCountCreated	INT,
	@InternalOutboundCountAssigned	INT,
	@DelayedCount					INT

IF @level = 1
	BEGIN

		--عدد معاملات الصادر الخارجي
		select 
			@OutboundCount = count(*) 
		from 
			Transactions 
			
		where 
			TRANSACTIONCATEGORYID = @ExternalOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المنشئة
		select 
			@OutboundDraftCountCreated = count(*) 
		from 
			Transactions
			
		where 
			TRANSACTIONCATEGORYID = @DraftOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات مسودة الخطاب المحالة
		select 
			@OutboundDraftCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToUserId = @UserID
		where 
			TRANSACTIONCATEGORYID = @DraftOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] <> @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المنشئة
		select 
			@InboundCountCreated = count(*) 
		from 
			Transactions 
			
		where 
			TRANSACTIONCATEGORYID = @Inbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات الوارد الخارجي المحالة
		select 
			@InboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToUserId = @UserID
		where 
			TRANSACTIONCATEGORYID = @Inbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] <> @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المنشئة
		select 
			@InternalOutboundCountCreated = count(*) 
		from 
			Transactions 
			
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] = @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد معاملات المعاملة الداخلية المحالة
		select 
			@InternalOutboundCountAssigned = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			AND TransactionAssignmentHistories.ToUserId = @UserID
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND (Transactions.[CreatedBy] <> @UserID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate

		--عدد المعاملات المتأخرة 
		select 
			@DelayedCount = count(DISTINCT Transactions.ID) 
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TransactionTypeId <>  @Inbound
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (Transactions.DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
			and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
			AND ([TransactionAssignments].ToUserId = @UserID)
			AND Transactions.StatusId = 1479


			INSERT INTO #DATA
			VALUES(@EntitID,@UserID,@OutboundCount,@OutboundDraftCountCreated,@OutboundDraftCountAssigned,@InboundCountCreated,@InboundCountAssigned,@InternalOutboundCountCreated,@InternalOutboundCountAssigned,@DelayedCount)

	END

	IF @level = 2
	BEGIN

		INSERT INTO #DATA(OrgUnitsID,UserProfilesID)
		SELECT
			[OrgUnit_Id],
			[UserProfile_Id]
		FROM
			[dbo].[UserProfileOrgUnits]
			INNER JOIN [dbo].[UserProfiles] ON [UserProfileOrgUnits].UserProfile_Id = [UserProfiles].Id AND [UserProfiles].IsActive = 1
			INNER JOIN [dbo].[OrgUnits] ON [UserProfileOrgUnits].OrgUnit_Id = [OrgUnits].Id AND [OrgUnits].IsActive = 1
		WHERE
			[OrgUnit_Id] = @EntitID

		--عدد معاملات الصادر الخارجي
		UPDATE DA
		SET OutboundCount = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							OrgUnitId,
							Transactions.CreatedBy,
							COUNT(1) CO
						FROM
							Transactions
							
						where 
							TRANSACTIONCATEGORYID = @ExternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
							AND (Transactions.OrgUnitId = @EntitID)
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							OrgUnitId,Transactions.CreatedBy ) T ON DA.OrgUnitsID = T.OrgUnitId AND DA.UserProfilesID = T.CreatedBy



		--عدد معاملات مسودة الخطاب المنشئة
		UPDATE DA
		SET OutboundDraftCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							OrgUnitId,
							Transactions.CreatedBy,
							COUNT(1) CO
						from 
							Transactions 
							
						where 
							TRANSACTIONCATEGORYID = @DraftOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
							AND (Transactions.OrgUnitId = @EntitID)
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							OrgUnitId,Transactions.CreatedBy ) T ON DA.OrgUnitsID = T.OrgUnitId AND DA.UserProfilesID = T.CreatedBy


		--عدد معاملات مسودة الخطاب المحالة
		UPDATE DA
		SET OutboundDraftCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							[FromEntityId],
							TAH.CreatedBy,
							COUNT(1) CO
						from 
							Transactions 
							
							INNER JOIN [dbo].[TransactionAssignmentHistories] TAH ON TAH.TransactionId = Transactions.Id
							AND TAH.[FromEntityId] = @EntitID
						where 
							TRANSACTIONCATEGORYID = @DraftOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
							AND (Transactions.OrgUnitId <> @EntitID)
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							[FromEntityId],TAH.CreatedBy ) T ON DA.OrgUnitsID = T.[FromEntityId] AND DA.UserProfilesID = T.CreatedBy


		--عدد معاملات الوارد الخارجي المنشئة
		UPDATE DA
		SET InboundCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							OrgUnitId,
							Transactions.CreatedBy,
							COUNT(1) CO
						from 
							Transactions 
							
						where 
							TRANSACTIONCATEGORYID = @Inbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
							AND (Transactions.OrgUnitId = @EntitID)
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							OrgUnitId,Transactions.CreatedBy ) T ON DA.OrgUnitsID = T.OrgUnitId AND DA.UserProfilesID = T.CreatedBy

		--عدد معاملات الوارد الخارجي المحالة
		UPDATE DA
		SET InboundCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							[FromEntityId],
							TAH.CreatedBy,
							COUNT(1) CO
						from 
							Transactions 
							
							INNER JOIN [dbo].[TransactionAssignmentHistories] TAH ON TAH.TransactionId = Transactions.Id
							AND TAH.[FromEntityId] = @EntitID
						where 
						TRANSACTIONCATEGORYID = @Inbound 
						AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
						AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
						AND (PriorityId = @PriorityId OR @PriorityId = -1)
						AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
						AND (Remarks = @Remarks OR @Remarks IS NULL)
						AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
						AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
						AND (Transactions.OrgUnitId <> @EntitID)
						AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							[FromEntityId],TAH.CreatedBy ) T ON DA.OrgUnitsID = T.[FromEntityId] AND DA.UserProfilesID = T.CreatedBy


		--عدد معاملات المعاملة الداخلية المنشئة
		UPDATE DA
		SET InternalOutboundCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							OrgUnitId,
							Transactions.CreatedBy,
							COUNT(1) CO
						from 
							Transactions 
							
						where 
							TRANSACTIONCATEGORYID = @InternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
							AND (Transactions.OrgUnitId = @EntitID)
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							OrgUnitId,Transactions.CreatedBy ) T ON DA.OrgUnitsID = T.OrgUnitId AND DA.UserProfilesID = T.CreatedBy


		--عدد معاملات المعاملة الداخلية المحالة
		UPDATE DA
		SET InternalOutboundCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT
							[FromEntityId],
							TAH.CreatedBy,
							COUNT(1) CO
						from 
							Transactions 
							
							INNER JOIN [dbo].[TransactionAssignmentHistories] TAH ON TAH.TransactionId = Transactions.Id
							AND TAH.[FromEntityId] = @EntitID
						where 
							TRANSACTIONCATEGORYID = @InternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							AND (TransactionTypeId = @TransactionTypeId OR @TransactionTypeId = -1)
							AND (Transactions.OrgUnitId <> @EntitID)
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							[FromEntityId],TAH.CreatedBy ) T ON DA.OrgUnitsID = T.[FromEntityId] AND DA.UserProfilesID = T.CreatedBy

		--عدد المعاملات المتأخرة 
		UPDATE DA
		SET DelayedCount = CO
		FROM
			#DATA DA
			INNER JOIN (SELECT 
			[TransactionAssignments].ToEntityId,
			[TransactionAssignments].ToUserId,
			COUNT(1) CO
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			AND [TransactionAssignments].ToEntityId = @EntitID
		where 
			TransactionTypeId <> @Inbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (Transactions.DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			and ([RemindDate] < GETDATE() OR DATEADD(day, 15, [TransactionAssignments].Date) < GETDATE())
			AND Transactions.StatusId = 1479
		GROUP BY
			[TransactionAssignments].ToEntityId,
			[TransactionAssignments].ToUserId) T ON DA.OrgUnitsID = T.ToEntityId AND DA.UserProfilesID = T.ToUserId
	END

	IF @level = 3
	BEGIN
		;WITH cte AS 
		 (
		  SELECT a.Id, a.parentId, a.name
		  FROM OrgUnits_VW a
		  WHERE Id = @EntitID
		  UNION ALL
		  SELECT a.Id, a.parentid, a.Name
		  FROM OrgUnits_VW a JOIN cte c ON a.parentId = c.id
		  )

		INSERT INTO #DATA(OrgUnitsID,UserProfilesID)
		SELECT 
			OrgUnit_Id,
			UserProfile_Id
		FROM
			[dbo].[UserProfileOrgUnits]
			INNER JOIN [dbo].[UserProfiles] ON [UserProfileOrgUnits].UserProfile_Id = [UserProfiles].Id AND [UserProfiles].IsActive = 1
			INNER JOIN [dbo].[OrgUnits] ON [UserProfileOrgUnits].OrgUnit_Id = [OrgUnits].Id AND [OrgUnits].IsActive = 1
			INNER JOIN CTE ON [UserProfileOrgUnits].OrgUnit_Id = CTE.id

		--عدد معاملات الصادر الخارجي
		UPDATE DA
			SET OutboundCount = CO
		FROM
			#DATA DA
			INNER JOIN (
						SELECT
							DA.OrgUnitsID,
							DA.UserProfilesID,
							COUNT(1) CO
						from 
							Transactions 			
							INNER JOIN #DATA DA ON Transactions.OrgUnitId = Da.OrgUnitsID AND Transactions.CreatedBy = DA.UserProfilesID		
						where 
							TRANSACTIONCATEGORYID = @ExternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							DA.OrgUnitsID,
							DA.UserProfilesID) T ON T.OrgUnitsID = Da.OrgUnitsID AND T.UserProfilesID = DA.UserProfilesID

			
		--عدد معاملات مسودة الخطاب المنشئة
		UPDATE DA
			SET OutboundDraftCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (
						SELECT
							DA.OrgUnitsID,
							DA.UserProfilesID,
							COUNT(1) CO
						from 
							Transactions 
							
							INNER JOIN #DATA DA ON Transactions.OrgUnitId = Da.OrgUnitsID AND Transactions.CreatedBy = DA.UserProfilesID		
						where 
							TRANSACTIONCATEGORYID = @DraftOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
						
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							DA.OrgUnitsID,
							DA.UserProfilesID) T ON T.OrgUnitsID = Da.OrgUnitsID AND T.UserProfilesID = DA.UserProfilesID


		--عدد معاملات مسودة الخطاب المحالة
		UPDATE DA
			SET OutboundDraftCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			FromEntityId,
			FromUserId,
			count(1) CO
		from 
			Transactions 		
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			INNER JOIN #DATA DA ON TransactionAssignmentHistories.FromEntityId = Da.OrgUnitsID AND TransactionAssignmentHistories.FromUserId = DA.UserProfilesID
		where 
			TRANSACTIONCATEGORYID = @DraftOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			
			AND (Transactions.OrgUnitId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
		GROUP BY
			FromEntityId,
			FromUserId ) T ON T.FromEntityId = Da.OrgUnitsID AND T.FromUserId = DA.UserProfilesID

		--عدد معاملات الوارد الخارجي المنشئة
		UPDATE DA
			SET InboundCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (
						SELECT
							DA.OrgUnitsID,
							DA.UserProfilesID,
							COUNT(1) CO
						from 
							Transactions 
							
							INNER JOIN #DATA DA ON Transactions.OrgUnitId = Da.OrgUnitsID AND Transactions.CreatedBy = DA.UserProfilesID		
						where 
							TRANSACTIONCATEGORYID = @Inbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							DA.OrgUnitsID,
							DA.UserProfilesID) T ON T.OrgUnitsID = Da.OrgUnitsID AND T.UserProfilesID = DA.UserProfilesID

		--عدد معاملات الوارد الخارجي المحالة

		UPDATE DA
			SET InboundCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			FromEntityId,
			FromUserId,
			count(1) CO
		from 
			Transactions 		
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			INNER JOIN #DATA DA ON TransactionAssignmentHistories.FromEntityId = Da.OrgUnitsID AND TransactionAssignmentHistories.FromUserId = DA.UserProfilesID
		where 
			TRANSACTIONCATEGORYID = @Inbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			
			AND (Transactions.OrgUnitId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
		GROUP BY
			FromEntityId,
			FromUserId ) T ON T.FromEntityId = Da.OrgUnitsID AND T.FromUserId = DA.UserProfilesID


		--عدد معاملات المعاملة الداخلية المنشئة
		UPDATE DA
			SET InternalOutboundCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (
						SELECT
							DA.OrgUnitsID,
							DA.UserProfilesID,
							COUNT(1) CO
						from 
							Transactions 
							
							INNER JOIN #DATA DA ON Transactions.OrgUnitId = Da.OrgUnitsID AND Transactions.CreatedBy = DA.UserProfilesID		
						where 
							TRANSACTIONCATEGORYID = @InternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							DA.OrgUnitsID,
							DA.UserProfilesID) T ON T.OrgUnitsID = Da.OrgUnitsID AND T.UserProfilesID = DA.UserProfilesID


		--عدد معاملات المعاملة الداخلية المحالة
		UPDATE DA
			SET InternalOutboundCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			FromEntityId,
			FromUserId,
			count(1) CO
		from 
			Transactions 		
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
			INNER JOIN #DATA DA ON TransactionAssignmentHistories.FromEntityId = Da.OrgUnitsID AND TransactionAssignmentHistories.FromUserId = DA.UserProfilesID
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			
			AND (Transactions.OrgUnitId <> @EntitID)
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
		GROUP BY
			FromEntityId,
			FromUserId ) T ON T.FromEntityId = Da.OrgUnitsID AND T.FromUserId = DA.UserProfilesID


		--عدد المعاملات المتأخرة 
		UPDATE DA
			SET DelayedCount = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			[TransactionAssignments].ToEntityId,
			[TransactionAssignments].ToUserId,
			COUNT(1) CO
		from 
			Transactions 		
			INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
			INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id
			INNER JOIN #DATA DA ON [TransactionAssignments].ToEntityId = Da.OrgUnitsID AND [TransactionAssignments].ToUserId = DA.UserProfilesID
		where 
			 TRANSACTIONCATEGORYID = @ExternalOutbound  
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (Transactions.DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			
			and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
			AND Transactions.StatusId = 1479
			GROUP BY
			[TransactionAssignments].ToEntityId,
			[TransactionAssignments].ToUserId) T ON T.ToEntityId = Da.OrgUnitsID AND T.ToUserId = DA.UserProfilesID


	END

	IF @level = 4
	BEGIN

		INSERT INTO #DATA(OrgUnitsID,UserProfilesID)
		SELECT 
			OrgUnit_Id,
			UserProfile_Id
		FROM
			[dbo].[UserProfileOrgUnits]
			INNER JOIN [dbo].[UserProfiles] ON [UserProfileOrgUnits].UserProfile_Id = [UserProfiles].Id AND [UserProfiles].IsActive = 1
			INNER JOIN [dbo].[OrgUnits] ON [UserProfileOrgUnits].OrgUnit_Id = [OrgUnits].Id AND [OrgUnits].IsActive = 1

		--عدد معاملات الصادر الخارجي
		UPDATE DA
			SET OutboundCount = CO
		FROM
			#DATA DA
			INNER JOIN (
						select 
							Transactions.OrgUnitId,
							Transactions.CreatedBy,
							count(*) CO
						from 
							Transactions 	
						where 
							TRANSACTIONCATEGORYID = @ExternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							 
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							Transactions.OrgUnitId,
							Transactions.CreatedBy ) T ON T.OrgUnitId = Da.OrgUnitsID AND T.CreatedBy = DA.UserProfilesID

		--عدد معاملات مسودة الخطاب المنشئة
		UPDATE DA
			SET OutboundDraftCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (
						select 
							Transactions.OrgUnitId,
							Transactions.CreatedBy,
							count(*) CO
						from 
							Transactions 					
						where 
							TRANSACTIONCATEGORYID = @DraftOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							 
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							Transactions.OrgUnitId,
							Transactions.CreatedBy ) T ON T.OrgUnitId = Da.OrgUnitsID AND T.CreatedBy = DA.UserProfilesID

		--عدد معاملات مسودة الخطاب المحالة
		UPDATE DA
			SET OutboundDraftCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			FromEntityId,
			FromUserId,
			count(1) CO
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @DraftOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
		GROUP BY
			FromEntityId,
			FromUserId ) T ON T.FromEntityId <> Da.OrgUnitsID AND T.FromUserId <> DA.UserProfilesID

		--عدد معاملات الوارد الخارجي المنشئة
		UPDATE DA
			SET InboundCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (
						select 
							Transactions.OrgUnitId,
							Transactions.CreatedBy,
							count(*) CO
						from 
							Transactions 						
						where 
							TRANSACTIONCATEGORYID = @Inbound
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							 
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							Transactions.OrgUnitId,
							Transactions.CreatedBy ) T ON T.OrgUnitId = Da.OrgUnitsID AND T.CreatedBy = DA.UserProfilesID

		--عدد معاملات الوارد الخارجي المحالة
		UPDATE DA
			SET InboundCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			FromEntityId,
			FromUserId,
			count(1) CO
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @Inbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
		GROUP BY
			FromEntityId,
			FromUserId ) T ON T.FromEntityId <> Da.OrgUnitsID AND T.FromUserId <> DA.UserProfilesID

		--عدد معاملات المعاملة الداخلية المنشئة
		UPDATE DA
			SET InternalOutboundCountCreated = CO
		FROM
			#DATA DA
			INNER JOIN (
						select 
							Transactions.OrgUnitId,
							Transactions.CreatedBy,
							count(*) CO
						from 
							Transactions 
						where 
							TRANSACTIONCATEGORYID = @InternalOutbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							 
							AND Transactions.Date BETWEEN @FromDate AND @ToDate
						GROUP BY
							Transactions.OrgUnitId,
							Transactions.CreatedBy ) T ON T.OrgUnitId = Da.OrgUnitsID AND T.CreatedBy = DA.UserProfilesID


		--عدد معاملات المعاملة الداخلية المحالة
		UPDATE DA
			SET InternalOutboundCountAssigned = CO
		FROM
			#DATA DA
			INNER JOIN (
		SELECT	
			FromEntityId,
			FromUserId,
			count(1) CO
		from 
			Transactions 
			
			INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
		where 
			TRANSACTIONCATEGORYID = @InternalOutbound 
			AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
			AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
			AND (PriorityId = @PriorityId OR @PriorityId = -1)
			AND (DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
			AND (Remarks = @Remarks OR @Remarks IS NULL)
			AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
			 
			AND Transactions.Date BETWEEN @FromDate AND @ToDate
		GROUP BY
			FromEntityId,
			FromUserId ) T ON T.FromEntityId <> Da.OrgUnitsID AND T.FromUserId <> DA.UserProfilesID

		--عدد المعاملات المتأخرة 
		UPDATE DA
			SET DelayedCount = CO
		FROM
			#DATA DA
			INNER JOIN (
						SELECT DISTINCT
							[TransactionAssignments].ToEntityId,
							[TransactionAssignments].ToUserId,
							COUNT(1) CO
						from 
							Transactions 					
							INNER JOIN [dbo].[TransactionAssignments] ON Transactions.ID = [TransactionAssignments].TransactionId
							INNER JOIN UserProfiles ON [TransactionAssignments].ToUserId = UserProfiles.Id		
		                	INNER JOIN [dbo].[TransactionAssignmentHistories] ON TransactionAssignmentHistories.TransactionId = Transactions.Id
						where 
							TRANSACTIONCATEGORYID <> @Inbound 
							AND (LetterTypeId = @LetterTypeId OR @LetterTypeId = -1)
							AND (ConfidentialityId = @ConfidentialityId OR @ConfidentialityId = -1)
							AND (PriorityId = @PriorityId OR @PriorityId = -1)
							AND (Transactions.DeliveryMethodId = @DeliveryMethodId OR @DeliveryMethodId = -1)
							AND (Remarks = @Remarks OR @Remarks IS NULL)
							AND (RemindDate = @AppointmentDate OR @AppointmentDate IS NULL)
							 
							and ([RemindDate] < GETDATE() OR DATEADD(day, UserProfiles.TransactionProcessingPeriod, [TransactionAssignments].Date) < GETDATE())
							
						GROUP BY
							[TransactionAssignments].ToEntityId,
							[TransactionAssignments].ToUserId ) T ON T.ToEntityId = Da.OrgUnitsID AND T.ToUserId = DA.UserProfilesID
	END


	IF @ReportType = 1
	BEGIN
		select DISTINCT
			OrgUnitsID ,
			[OrgUnits_VW].Name OrgUnitName,
			NULL AS UserProfilesID ,
			'' AS UserProfileName,
			ISNULL(OutboundCount,0) OutboundCount,
			ISNULL(OutboundDraftCountCreated,0) OutboundDraftCountCreated,
			ISNULL(OutboundDraftCountAssigned,0) OutboundDraftCountAssigned,
			ISNULL(InboundCountCreated,0) InboundCountCreated,
			ISNULL(InboundCountAssigned,0) InboundCountAssigned,
			ISNULL(InternalOutboundCountCreated,0) InternalOutboundCountCreated,
			ISNULL(InternalOutboundCountAssigned,0) InternalOutboundCountAssigned,
			ISNULL(DelayedCount,0) DelayedCount
		from(
			SELECT DISTINCT
				OrgUnitsID ,
				SUM(OutboundCount) OutboundCount ,
				SUM(OutboundDraftCountCreated) OutboundDraftCountCreated,
				SUM(OutboundDraftCountAssigned) OutboundDraftCountAssigned,
				SUM(InboundCountCreated) InboundCountCreated,
				SUM(InboundCountAssigned) InboundCountAssigned,
				SUM(InternalOutboundCountCreated) InternalOutboundCountCreated,
				SUM(InternalOutboundCountAssigned) InternalOutboundCountAssigned,
				SUM(DelayedCount) DelayedCount
			from 
				#DATA DA
			GROUP BY
				DA.OrgUnitsID
		) t
		INNER JOIN [dbo].[OrgUnits_VW] ON T.OrgUnitsID = [OrgUnits_VW].id

		SELECT DISTINCT
			@TotalCount = count(1)
		FROM 
			#DATA DA
			INNER JOIN [dbo].[OrgUnits_VW] ON DA.OrgUnitsID = [OrgUnits_VW].id
			INNER JOIN [dbo].[UserProfiles_VW] ON DA.UserProfilesID = [UserProfiles_VW].id
	END
	ELSE
	BEGIN
		SELECT DISTINCT
			OrgUnitsID ,
			[OrgUnits_VW].Name OrgUnitName,
			UserProfilesID ,
			[UserProfiles_VW].Name UserProfileName,
			ISNULL(OutboundCount,0) OutboundCount,
			ISNULL(OutboundDraftCountCreated,0) OutboundDraftCountCreated,
			ISNULL(OutboundDraftCountAssigned,0) OutboundDraftCountAssigned,
			ISNULL(InboundCountCreated,0) InboundCountCreated,
			ISNULL(InboundCountAssigned,0) InboundCountAssigned,
			ISNULL(InternalOutboundCountCreated,0) InternalOutboundCountCreated,
			ISNULL(InternalOutboundCountAssigned,0) InternalOutboundCountAssigned,
			ISNULL(DelayedCount,0) DelayedCount 
		from 
			#DATA DA
			INNER JOIN [dbo].[OrgUnits_VW] ON DA.OrgUnitsID = [OrgUnits_VW].id
			INNER JOIN [dbo].[UserProfiles_VW] ON DA.UserProfilesID = [UserProfiles_VW].id
		ORDER BY 
			OrgUnitsID
			OFFSET @PageIndex * @PageSize ROWS
			FETCH NEXT @PageSize ROWS ONLY

		SELECT DISTINCT
			@TotalCount = count(1)
		FROM 
			#DATA DA
			INNER JOIN [dbo].[OrgUnits_VW] ON DA.OrgUnitsID = [OrgUnits_VW].id
			INNER JOIN [dbo].[UserProfiles_VW] ON DA.UserProfilesID = [UserProfiles_VW].id

	END
END;

---------------------Views----------------------------------------------

---------------------Views----------------------------------------------
GO
/***** Object: View [dbo].[ExternalParties_VW] Script Date: 7/11/2019 10:35:34 AM *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ExternalParties_VW] AS
SELECT 
ExternalParties.id,
ExternalParties.[Number],
Localizations.[Text] Name,
ExternalParties.ParentId,
t1.Text ParentName
FROM 
ExternalParties
INNER JOIN Localizations ON ExternalParties.[Name_Id] = Localizations.LocalizationIdentifier_Id
AND Localizations.CultureId = 1
LEFT JOIN ExternalParties T2 ON T2.Id = ExternalParties.ParentId
LEFT JOIN Localizations T1 ON T2.[Name_Id] = T1.LocalizationIdentifier_Id
AND T1.CultureId = 1
GO
/***** Object: View [dbo].[OrgUnits_VW] Script Date: 7/11/2019 10:35:35 AM *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[OrgUnits_VW] AS
SELECT 
OrgUnits.id,
OrgUnits.[Number],
Localizations.[Text] Name,
OrgUnits.ParentId,
t1.Text ParentName,
OrgUnits.IsActive
FROM 
OrgUnits
INNER JOIN Localizations ON OrgUnits.LocalizationIdentifier_Id = Localizations.LocalizationIdentifier_Id
AND Localizations.CultureId = 1
LEFT JOIN OrgUnits T2 ON T2.Id = OrgUnits.ParentId
LEFT JOIN Localizations T1 ON T2.LocalizationIdentifier_Id = T1.LocalizationIdentifier_Id
AND T1.CultureId = 1
GO
/***** Object: View [dbo].[UserProfiles_VW] Script Date: 7/11/2019 10:35:35 AM *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[UserProfiles_VW] AS
SELECT 
UserProfiles.id,
UserProfiles.[UserName],
AspNetUsers.UserName AspNetUsersUserName,
Localizations.[Text] Name,
UserProfiles.[IsActive],
Localizations.CultureId,
UserProfiles.Email,
OrgUnits.Id ENTITY_ID,
T2.Text ENTITY_NAME

FROM 
UserProfiles
LEFT JOIN [dbo].[AspNetUsers] ON UserProfiles.IdentityId = [AspNetUsers].Id
INNER JOIN Localizations ON UserProfiles.LocalizationIdentifier_Id = Localizations.LocalizationIdentifier_Id
AND Localizations.CultureId = 1
LEFT JOIN UserProfileOrgUnits ON UserProfileOrgUnits.UserProfile_Id = UserProfiles.Id
LEFT JOIN OrgUnits ON UserProfileOrgUnits.OrgUnit_Id = OrgUnits.Id
LEFT JOIN Localizations T2 ON T2.LocalizationIdentifier_Id = OrgUnits.LocalizationIdentifier_Id
AND T2.CultureId = 1
GO
