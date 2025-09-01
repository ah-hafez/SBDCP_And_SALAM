/****** Object:  StoredProcedure [dbo].[SearchEntity]    Script Date: 10/31/2024 3:00:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 ALTER PROCEDURE [dbo].[SearchEntity]
 @ExternalParty			BIGINT, 
 @DocumentNumber         nvarchar(200),
 @Number                int,
 @OrgUnitId				int,
 @UserId                int,
@TransactionCategoryId	         int,
@ConfidentialityId               int,
@LetterTypeId                    int,
@StatusId                        int,
@PriorityId                      int,
@FromPartyId                     int,
@SignedByDepartmentId            int,
@SignedById                      int, 
@DirectedToUserId                nvarchar(200),
@DestinationPartyId              int,
@CreatedDepartmentId             int,
@DirectedToId                    int, 
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
		@TotalCount = count(distinct TR.Id)
	FROM 
		dbo.Transactions TR WITH(NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
	    LEFT JOIN  TRANSACTIONASSIGNMENTHISTORIES TAH WITH(NOLOCK) ON TR.ID = TAH.TRANSACTIONID
	WHERE  
	   (@ExternalParty= -1 OR TR.ExternalPartyId = @ExternalParty)
	    AND (@Number= -1 OR TR.NUMBER = @Number)
		AND (@DocumentNumber = '-1' OR TR.DOCUMENTNUMBER = @DocumentNumber)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		--AND (@TransactionCategoryId = -1 OR TR.TRANSACTIONCATEGORYID =@TransactionCategoryId)
		AND (@ConfidentialityId = -1 OR TR.CONFIDENTIALITYID = @ConfidentialityId)
		AND (@LetterTypeId = -1 OR TR.LETTERTYPEID = @LetterTypeId) 
		AND (@StatusId = -1 OR TR.STATUSID = @StatusId) 
		AND (@PriorityId = -1 OR TR.PRIORITYID = @PriorityId) 
		AND (@FromPartyId = -1 OR  TAH.FROMENTITYID = @FromPartyId)
		AND (@CreatedDepartmentId = -1 OR TED.CREATEDBY = @CreatedDepartmentId)
		AND (@SignedById = -1 OR TR.SIGNEDBYUSERID= @SignedById)
		AND (@SignedById = -1 OR TR.SIGNEDBYUSERID= @SignedById)
		AND (@DestinationPartyId = -1 OR TR.ENTITYID = @DestinationPartyId)
	INSERT INTO #InScopeTr
	SELECT distinct  TR.ID
	FROM 
		transactions TR WITH (NOLOCK)
		LEFT JOIN TransactionEntityDetails TED WITH(NOLOCK) ON TR.Id = TED.[TransactionId] 
		LEFT JOIN  TRANSACTIONASSIGNMENTHISTORIES TAH WITH(NOLOCK) ON TR.ID = TAH.TRANSACTIONID
	WHERE  
	    (@ExternalParty= -1 OR TR.ExternalPartyId = @ExternalParty) 
		AND (@Number= -1 OR TR.NUMBER = @Number)
		AND (@DocumentNumber = '-1' OR TR.DOCUMENTNUMBER = @DocumentNumber)
		AND (TR.Date between ISNULL(@DateFrom,TR.Date) AND ISNULL(@DateTo,TR.Date))
		AND (TED.[EntityId] = @OrgUnitId OR @OrgUnitId = -1)
		AND (TR.StatusId <> 1624 )
		--AND (@TransactionCategoryId = -1 OR TR.TRANSACTIONCATEGORYID =@TransactionCategoryId)
		AND (@ConfidentialityId = -1 OR TR.CONFIDENTIALITYID = @ConfidentialityId)
		AND (@LetterTypeId = -1 OR TR.LETTERTYPEID = @LetterTypeId) 
		AND (@StatusId = -1 OR TR.STATUSID = @StatusId) 
		AND (@PriorityId = -1 OR TR.PRIORITYID = @PriorityId) 
		AND (@FromPartyId = -1 OR  TAH.FROMENTITYID = @FromPartyId)
		AND (@CreatedDepartmentId = -1 OR TED.CREATEDBY = @CreatedDepartmentId)
		AND (@SignedById = -1 OR TR.SIGNEDBYUSERID= @SignedById)
		AND (@SignedById = -1 OR TR.SIGNEDBYUSERID= @SignedById)
		AND (@DestinationPartyId = -1 OR TR.ENTITYID = @DestinationPartyId)

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
		TR.StatusId,
		TR.CONFIDENTIALITYID,
		TR.RemindDate,
		TR.RemindDateH,
		TR.Encrypted
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

update BARCODEDESIGNS
 
set HTML = N'<!doctype html>
<html lang="en">
 
<head>
<style>
        .barocdediv {
            padding-left: 2px;
            font-family: Kanun AR+LT;
            font-size: 14px;
        }
</style>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
</head>
 
<body style="background:url({15}) ;background-size: 250px 180px; no-repeat center; margin-top:40px">
<div dir="rtl"
        style="padding: 15px 1px; width: 227px; height: 50px; text-align: right; font-weight: bold; font-size:14px;">
<div style="display:flex;padding-right:20px">
<div class="barocdediv"> {4} {5} </div>
</div>
<div style="display:flex; padding-right:20px">
<div class="barocdediv"> {8} {9} </div>
</div>
<div class="barocdediv">{6} </div>
<div style="display:flex;padding-right:20px">
<div class="barocdediv">{11} {12} </div>
</div>
<div style="display:flex;padding-right:20px">
<div class="barocdediv">{13} {14} </div>
</div>
 
    </div>
<div style="padding-top: 20px;">
<div style="padding-right:40px;;">
<div style="display:inline-block; vertical-align:middle;"><img src="{1}" alt=""
                    style="height: 16px; max-width: 150px;margin-bottom: -7px; margin-left:30px;"></div>
<div style="display:inline-block; vertical-align:middle;"></div>
</div>
</div>
</body>
 
</html>'
 
where TYPEID in (437,438,439) and IsElectronic = 0