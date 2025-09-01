
CREATE TABLE [dbo].[IC_SUBJECT](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ITEM_CODE] [nvarchar](1000) NULL,
	[ITEM_DISPLAY] [nvarchar](1000) NULL,
	[ITEM_DESCRIPTION_AR] [nvarchar](1000) NULL,
	[PARENT_ID] [int] NULL,
	[ACTIVE] [bit] NOT NULL,
	[CREATEDON] [datetime] NOT NULL,
	[CREATEDBY] [int] NULL,
	[MODEFIEDON] [datetime] NULL,
	[MODEFIEDBY] [int] NULL,
	[NUMBER] [nvarchar](1000) NULL,
 CONSTRAINT [PK_dbo.IC_SUBJECTS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--------------------------------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[IC_SUBJECTS_TRANSACTIONS]    Script Date: 4/30/2023 12:50:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IC_SUBJECTS_TRANSACTION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TRANSACTIONID] [int] NOT NULL,
	[IC_SUBJECTID] [int] NOT NULL,
	[CREATEDON] [datetime] NULL,
	[CREATEDBY] [int] NULL,
	[MODEFIEDON] [datetime] NULL,
	[MODEFIEDBY] [int] NULL,
 CONSTRAINT [PK_dbo.IC_SUBJECTS_TRANSACTIONS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[IC_SUBJECTS_TRANSACTION]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IC_SUBJECTS_TRANSACTION_dbo.TRANSACTIONS_TRANSACTIONID] FOREIGN KEY([TRANSACTIONID])
REFERENCES [dbo].[TRANSACTIONS] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IC_SUBJECTS_TRANSACTION] CHECK CONSTRAINT [FK_dbo.IC_SUBJECTS_TRANSACTION_dbo.TRANSACTIONS_TRANSACTIONID]
GO



ALTER TABLE [dbo].[IC_SUBJECTS_TRANSACTION]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IC_SUBJECTS_TRANSACTIONS_dbo.IC_SUBJECT_ID] FOREIGN KEY([IC_SUBJECTID])
REFERENCES [dbo].[IC_SUBJECT] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IC_SUBJECTS_TRANSACTION] CHECK CONSTRAINT [FK_dbo.IC_SUBJECTS_TRANSACTIONS_dbo.IC_SUBJECT_ID]
GO



--------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[IC_SUBJECT]
           ([ITEM_CODE]
           ,[ITEM_DISPLAY]
           ,[ITEM_DESCRIPTION_AR]
           ,[PARENT_ID]
           ,[ACTIVE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY])
     VALUES
           (1
           ,N'دليل الارشيف'
           ,N'دليل الارشيف'
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,NULL
           ,NULL)
GO


Create   FUNCTION [dbo].[IsTransactionInIC](@TransId int) RETURNS int
AS
BEGIN

    DECLARE @IsInIc bit =0
      select @IsInIc=1  from [dbo].[IC_SUBJECTS_TRANSACTION] where [TRANSACTIONID]=@TransId     
    RETURN @IsInIc
END

GO


-------------------------------------------------------------------------------------------------------------

create  PROCEDURE [dbo].[SearchIC]
 
@TransNumber				nvarchar(50), 
@TransType      int , 
@Year     int , 
@OrgUnitId				int,
@culutre   nvarchar(30) ,
@userId   int




AS

BEGIN

declare @MianTransId int ; 

declare @type int=255 ; 
declare @subType int=-1 ; 

declare @TempId int ; 

declare @UserMaxWeight int ; 

DECLARE @MinLinkedNumbers int ;

DECLARE @MaxLinkedNumbers int ;

DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@culutre

SELECT @V_YEAR = TEXT FROM LOOKUPLOCALIZATIONS WHERE LOOKUP_ID = @Year AND CULTURE_ID = @V_CultureID



 select @UserMaxWeight=max(P_Permission.WEIGHT) from USERGROUPS ug 
        WITH(NOLOCK)
        LEFT JOIN [dbo].[GROUPPERMISSIONS] GPermissions  WITH(NOLOCK) ON GPermissions.GROUP_ID = ug.GROUPID
        LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = GPermissions.PERMISSION_ID
    where [USERID]=@userId

	if @UserMaxWeight is null 
	   begin 
	   set @UserMaxWeight=0
	   end 

	   if @TransType= 1
	      begin 
		  set  @type=254 ; 

		  set @subType=256
	    end 

  select   distinct TR.Id, TR.[Date] , TR.DateH  , TR.Number 
		            ,CASE  WHEN @UserMaxWeight >= P_Permission.Weight THEN  TR.[Subject] ELSE '****'  end as  [Subject]  
				     , TR.[CONFIDENTIALITYID] 
					 --,  28 as CONFIDENTIALITYID 
					, TR.[PRIORITYID] ,TR.[STATUSID]   
                    ,TR.[TRANSACTIONCATEGORYID]
                    , LOC_ExternalParty.Text as PartyName
		            , LOC_OrgUnit.Text as OrgUnitName
		            , LL_Status.Text as StatusName 
		            , LL_TransType.Text As TransactionTypeName
		            , LL_Perm.Text as ConfidentialityName
		            , LOC_PR.Text As PriorityName 
		            ,TA.ToUserId
		            ,TA.ToEntityId 
		            --,TR.CONFIDENTIALITYID
		            ,TR.RemindDate
		            ,TR.RemindDateH
		             ,[dbo].[GetMainDocInfoId] (TR.Id) as MainDocId
					 --,CASE  WHEN @UserMaxWeight >= P_Permission.Weight THEN  1 ELSE 0  end as  HasPermission
					 ,P_Permission.Weight as Weight
					 ,[dbo].[IsTransactionInIC] (TR.Id)  as IsInIc
					 ,[dbo].[GetIcName] (TR.Id)  as IcName
					

          from Transactions TR WITH(NOLOCK)
               LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
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
			   INNER JOIN BARCODES br on br.REFERENCEID = TR.Id
		where br.VALUE=@TransNumber and TR.ISDELETED=0 and tr.YEARH=@Year and (Tr.TRANSACTIONCATEGORYID=@type or Tr.TRANSACTIONCATEGORYID=@subType) 
		--and Tr.STATUSID=391
        order by tr.id 
	
END


----------------------------------------------------------------------------------------------------------------

GO 

create  FUNCTION [dbo].[GetIcName](@TransId int) RETURNS nvarchar(1000)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @IcName nvarchar(1000)=N'----------'
    DECLARE @IcId int
	
	select @IcId=[IC_SUBJECTID] from [dbo].[IC_SUBJECTS_TRANSACTION] where [TRANSACTIONID]=@TransId

    select @IcName=ITEM_DESCRIPTION_AR  from [dbo].[IC_SUBJECT] where [ID]=@IcId   


    RETURN @IcName
END

GO 


-------------------------------------------------------------------------
CREATE   PROCEDURE [dbo].[DeleteIC]
 
@IcId   int
AS

BEGIN


DECLARE @NumberOfTransaction int=0

CREATE TABLE #Ids([RowNumber] [int] IDENTITY(1,1) NOT NULL, Id INT)

	  ;WITH n(id) AS 
   (SELECT ID
    FROM [dbo].[IC_SUBJECT]
    WHERE ID  = @IcId  
        UNION ALL
    SELECT nplus1.ID
    FROM [dbo].[IC_SUBJECT] as nplus1, n
    WHERE n.id = nplus1.PARENT_ID )
	insert into #Ids
SELECT  distinct id FROM n 


select @NumberOfTransaction=count(*) from [dbo].[IC_SUBJECTS_TRANSACTION] where [IC_SUBJECTID] in ( select Id from #Ids) 


   if @NumberOfTransaction>0 
       begin 
         select 0 
		 drop table #Ids 

        return 
    end 


	delete from [dbo].[IC_SUBJECT] where [ID] in(select Id from #Ids) 

	select 1

 drop table #Ids 

	

END


-----------------------------------------------------------------------------------------------------

Go 


CREATE NONCLUSTERED INDEX [NonClusteredIndex-20230502-131806] ON [dbo].[IC_SUBJECT]
(
	[ITEM_CODE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


----------------------------------------------------------------------------

CREATE NONCLUSTERED INDEX [NonClusteredIndex-20230502-131825] ON [dbo].[IC_SUBJECT]
(
	[NUMBER] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

--------------------------------------------------------------------------

CREATE NONCLUSTERED INDEX [NonClusteredIndex-20230502-131839] ON [dbo].[IC_SUBJECT]
(
	[PARENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


----------------------------------------------------------------------------------------------


CREATE NONCLUSTERED INDEX [NonClusteredIndex-20230502-132128] ON [dbo].[IC_SUBJECTS_TRANSACTION]
(
	[TRANSACTIONID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

---------------------------------------------------------------------------------------------

CREATE NONCLUSTERED INDEX [NonClusteredIndex-20230502-132143] ON [dbo].[IC_SUBJECTS_TRANSACTION]
(
	[IC_SUBJECTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-------------------------------------------------------------------------------------

Create or ALTER  FUNCTION [dbo].[GetMainDocInfoId](@TransId int) RETURNS int
AS
BEGIN
    -- Declare the return variable here
    DECLARE @mainDocId int     select @mainDocId=[ID]  from [dbo].[DOCUMENTINFO] where [TRANSACTIONID]=@TransId     if @mainDocId IS NULL     begin 
      set @mainDocId=0
    end     -- Return the result of the function
    RETURN @mainDocId
END

GO 


---------------------------------------------------------------------------------------------------------

declare  @LookId as int =0; 
declare  @LookPermissionId as int =0; 
declare  @PermissionId as int =0; 
declare  @GroupId as int =0; 




   INSERT INTO [dbo].[LOOKUPS]
           ([CATEGORYID]
           ,[ISACTIVE]
           ,[SORT]
           ,[ENUMREFERENCE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY])
     VALUES
           (23
           ,1
           ,18
           ,22
           ,GEtDate()
           ,1
           ,NULL
           ,NULL)

          
	set @LookId=  SCOPE_IDENTITY();
-------------------------------------------------

	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'الارشيف'
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,1
           ,@LookId)

	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'Archiving Module'
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,2
           ,@LookId)

	INSERT INTO [dbo].[PERMISSIONS]
           ([CODE]
           ,[ISUSERDEFINED]
           ,[WEIGHT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[NAME_ID])
     VALUES
           ('IC'
           ,0
           ,NULL
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,@LookId)

    set @PermissionId=SCOPE_IDENTITY();


    INSERT INTO [dbo].[GROUPS]
           ([ISUSERDEFINED]
           ,[ISACTIVE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[GROUPNAME_ID])
     VALUES
           (0
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,0
           ,@LookId)

     set @GroupId=SCOPE_IDENTITY();

     insert into [dbo].[GROUPPERMISSIONS] values(@GroupId,@PermissionId) 

    -------------*************ADD IC ***************-----------------------

	 INSERT INTO [dbo].[LOOKUPS]
           ([CATEGORYID]
           ,[ISACTIVE]
           ,[SORT]
           ,[ENUMREFERENCE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY])
     VALUES
           (23
           ,1
           ,19
           ,23
           ,GEtDate()
           ,1
           ,NULL
           ,NULL)

          
	set @LookId=  SCOPE_IDENTITY();


	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'اضافة ارشيف'
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,1
           ,@LookId)

	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'Add Archiving '
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,2
           ,@LookId)

	INSERT INTO [dbo].[PERMISSIONS]
           ([CODE]
           ,[ISUSERDEFINED]
           ,[WEIGHT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[NAME_ID])
     VALUES
           ('IC.Add'
           ,0
           ,NULL
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,@LookId)

     set @PermissionId=SCOPE_IDENTITY();


   insert into [dbo].[GROUPPERMISSIONS] values(@GroupId,@PermissionId)

   -------------*************Delet IC ***************-----------------------

   INSERT INTO [dbo].[LOOKUPS]
           ([CATEGORYID]
           ,[ISACTIVE]
           ,[SORT]
           ,[ENUMREFERENCE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY])
     VALUES
           (23
           ,1
           ,20
           ,24
           ,GEtDate()
           ,1
           ,NULL
           ,NULL)

          
	set @LookId=  SCOPE_IDENTITY();


	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'حذف ارشيف'
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,1
           ,@LookId)

	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'Delete Archiving '
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,2
           ,@LookId)

	INSERT INTO [dbo].[PERMISSIONS]
           ([CODE]
           ,[ISUSERDEFINED]
           ,[WEIGHT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[NAME_ID])
     VALUES
           ('IC.Delete'
           ,0
           ,NULL
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,@LookId)

     set @PermissionId=SCOPE_IDENTITY();


   insert into [dbo].[GROUPPERMISSIONS] values(@GroupId,@PermissionId)


    -------------*************Update IC ***************-----------------------

   INSERT INTO [dbo].[LOOKUPS]
           ([CATEGORYID]
           ,[ISACTIVE]
           ,[SORT]
           ,[ENUMREFERENCE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY])
     VALUES
           (23
           ,1
           ,21
           ,25
           ,GEtDate()
           ,1
           ,NULL
           ,NULL)

          
	set @LookId=  SCOPE_IDENTITY();


	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'تعديل ارشيف'
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,1
           ,@LookId)

	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'Update Archiving '
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,2
           ,@LookId)

	INSERT INTO [dbo].[PERMISSIONS]
           ([CODE]
           ,[ISUSERDEFINED]
           ,[WEIGHT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[NAME_ID])
     VALUES
           ('IC.Update'
           ,0
           ,NULL
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,@LookId)

     set @PermissionId=SCOPE_IDENTITY();


   insert into [dbo].[GROUPPERMISSIONS] values(@GroupId,@PermissionId)




   -------------*************Classification IC ***************-----------------------

   INSERT INTO [dbo].[LOOKUPS]
           ([CATEGORYID]
           ,[ISACTIVE]
           ,[SORT]
           ,[ENUMREFERENCE]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY])
     VALUES
           (23
           ,1
           ,22
           ,26
           ,GEtDate()
           ,1
           ,NULL
           ,NULL)

          
	set @LookId=  SCOPE_IDENTITY();


	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N' تصنيف الارشيف'
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,1
           ,@LookId)

	INSERT INTO [dbo].[LOOKUPLOCALIZATIONS]
           ([TEXT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[CULTURE_ID]
           ,[LOOKUP_ID])
     VALUES
           (N'Classification Archiving '
           ,GEtDate()
           ,1
           ,NULL
           ,NULL
           ,2
           ,@LookId)

	INSERT INTO [dbo].[PERMISSIONS]
           ([CODE]
           ,[ISUSERDEFINED]
           ,[WEIGHT]
           ,[CREATEDON]
           ,[CREATEDBY]
           ,[MODEFIEDON]
           ,[MODEFIEDBY]
           ,[NAME_ID])
     VALUES
           ('IC.Classification'
           ,0
           ,NULL
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,@LookId)

     set @PermissionId=SCOPE_IDENTITY();


   insert into [dbo].[GROUPPERMISSIONS] values(@GroupId,@PermissionId)




GO

GOc



