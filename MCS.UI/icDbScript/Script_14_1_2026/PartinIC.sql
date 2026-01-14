
ALTER TABLE [SBDCP].[dbo].[IC_SUBJECTS_TRANSACTION]
ADD [Part] NVARCHAR(255)
GO

ALTER    PROCEDURE [dbo].[SearchICByTransactionID]
 
@TransactionID	int, 
@culutre   nvarchar(30) ,
@userId   int




AS

BEGIN

declare @MianTransId int ; 


declare @subType int=-1 ; 

declare @TempId int ; 

declare @UserMaxWeight int ; 

DECLARE @MinLinkedNumbers int ;

DECLARE @MaxLinkedNumbers int ;

DECLARE @V_CultureID int ,@V_YEAR INT

SELECT @V_CultureID = Id FROM [dbo].[Cultures] WHERE ShortName=@culutre




 select @UserMaxWeight=max(P_Permission.WEIGHT) from USERGROUPS ug 
        WITH(NOLOCK)
        LEFT JOIN [dbo].[GROUPPERMISSIONS] GPermissions  WITH(NOLOCK) ON GPermissions.GROUP_ID = ug.GROUPID
        LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = GPermissions.PERMISSION_ID
    where [USERID]=@userId

	if @UserMaxWeight is null 
	   begin 
	   set @UserMaxWeight=0
	   end 

	   

  select   distinct TR.Id, TR.[Date] , TR.DateH  , TR.Number 
		            ,CASE  WHEN @UserMaxWeight >= P_Permission.Weight THEN  TR.[Subject] ELSE '****'  end as  [Subject]  
				     , TR.[CONFIDENTIALITYID] 
					 --,  28 as CONFIDENTIALITYID 
					, TR.[PRIORITYID] ,TR.[STATUSID]   
                    ,TR.[TRANSACTIONCATEGORYID]
                    , LOC_ExternalParty.Text as PartyName
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
					 ,[dbo].[GetIcName] (TR.Id)  as IcName,
					 u.Name as ModifiedUser
					 ,[dbo].[GetIcId] (TR.Id)  as IcId,
					 ST.Description,
					 ST.Part,
					 ST.Number as OrderFileNumber,
					 dbo.GetFullClassificationNameByTransactionId(TR.Id) as FullClassificationName
					 
					

          from Transactions TR WITH(NOLOCK)
               LEFT JOIN Permissions P_Permission WITH(NOLOCK) ON P_Permission.Id = TR.ConfidentialityId
		       LEFT JOIN LookupLocalizations LL_Perm WITH(NOLOCK) ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = @V_CultureID
		       LEFT JOIN Priorities PR WITH(NOLOCK) ON PR.Id = TR.PriorityId
		       LEFT JOIN Localizations LOC_PR WITH(NOLOCK) ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =@V_CultureID
		       LEFT JOIN LookupLocalizations LL_TransType WITH(NOLOCK) ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = @V_CultureID
		       LEFT JOIN ExternalParties EP_ExternalParty WITH(NOLOCK) ON EP_ExternalParty.Id = TR.ExternalPartyId
		       LEFT JOIN Localizations LOC_ExternalParty WITH(NOLOCK) ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = @V_CultureID
		       LEFT JOIN LookupLocalizations LL_Status WITH(NOLOCK) ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = @V_CultureID
			   
		    left join IC_SUBJECTS_TRANSACTION ST on ST.TRANSACTIONID=TR.ID 
			left join IC_SUBJECT ICS on ICS.ID=ST.IC_SUBJECTID
			 LEFT JOIN UserProfiles_VW u WITH(NOLOCK) on ST.CREATEDBY =u.id
			INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
			   INNER JOIN BARCODES br on br.REFERENCEID = TR.Id
		where TR.ID=@TransactionID 
		 and TR.ISDELETED=0 
		
		--and Tr.STATUSID=391
        order by tr.id 
	
END
GO

----------------------------------------------------------------------------------------------------------------

ALTER  PROCEDURE [dbo].[SearchIC]
 
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
					 ,[dbo].[GetIcName] (TR.Id)  as IcName,
					 u.Name as ModifiedUser
					 ,[dbo].[GetIcId] (TR.Id)  as IcId,
					 ST.Description,
					 ST.Part,
					 ST.Number as OrderFileNumber
					 
					

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
			   
		    left join IC_SUBJECTS_TRANSACTION ST on ST.TRANSACTIONID=TR.ID 
			 LEFT JOIN UserProfiles_VW u WITH(NOLOCK) on ST.CREATEDBY =u.id
			INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
			   INNER JOIN BARCODES br on br.REFERENCEID = TR.Id
		where br.VALUE=@TransNumber 
		 and TR.ISDELETED=0 
		 and tr.YEARH=@Year and (Tr.TRANSACTIONCATEGORYID=@type or Tr.TRANSACTIONCATEGORYID=@subType) 
		--and Tr.STATUSID=391
        order by tr.id 
	
END
GO

----------------------------------------------------------------------------------------------------------------
