Create GLOBAL TEMPORARY TABLE InScopeTr(TRANSID     NUMBER) ON COMMIT PRESERVE ROWS;
/
CREATE GLOBAL TEMPORARY TABLE TT_REPORT_DATA 
   (OrgUnitsID NUMBER, 
	UserProfilesID NUMBER, 
	OutboundCount NUMBER, 
	OutboundDraftCountCreated NUMBER, 
	OutboundDraftCountAssigned NUMBER, 
	InboundCountCreated NUMBER, 
	InboundCountAssigned NUMBER, 
	InternalOutboundCountCreated NUMBER, 
	InternalOutboundCountAssigned NUMBER, 
	DelayedCount NUMBER
   ) ON COMMIT PRESERVE ROWS ;
  /
  CREATE GLOBAL TEMPORARY TABLE TEMP_COUNTERS_ByPERIOD 
   (
    id Number NOT NULL,
    From_Date TImeStamp , To_Date TImeStamp,MY_TRAN_COUNT Number(10,1) , DELAYED_COUNT Number(10,1)
    ,WITH_APPOITMENT_COUNT Number(10,1) ,Trans_Copies_COUNT Number(10,1)
   ) ON COMMIT PRESERVE ROWS;
 /
CREATE GLOBAL TEMPORARY TABLE TT_TEMP 
   (	TRANSID NUMBER
   ) ON COMMIT PRESERVE ROWS ;
  
  /
CREATE OR REPLACE FORCE EDITIONABLE VIEW OrgUnits_VW (Id, "NUMBER", Name, ParentId, ParentName, IsActive, Counter_Id) AS 
SELECT        
OrgUnits.Id, 
OrgUnits."NUMBER", 
Localizations."TEXT"  Name, 
OrgUnits.ParentId, 
T1."TEXT"  ParentName, 
OrgUnits.IsActive, 
OrgUnits.Counter_Id
FROM            
OrgUnits 
INNER JOIN Localizations ON OrgUnits.LocalizationIdentifier_Id = Localizations.LocalizationIdentifier_Id 
AND Localizations.CultureId = 1 
LEFT OUTER JOIN OrgUnits T2 ON T2.Id = OrgUnits.ParentId 
LEFT OUTER JOIN Localizations T1 ON T2.LocalizationIdentifier_Id = T1.LocalizationIdentifier_Id 
AND T1.CultureId = 1;

CREATE OR REPLACE FORCE EDITIONABLE VIEW UserProfiles_VW (Id, UserName, AspNetUsersUserName, Name, IsActive, CultureId, Email, ENTITY_ID, ENTITY_NAME) AS 
SELECT        
UserProfiles.Id,
UserProfiles.UserName,
AspNetUsers.UserName AspNetUsersUserName,
Localizations."TEXT" Name,
UserProfiles.IsActive,
Localizations.CultureId,
UserProfiles.Email,
OrgUnits.Id ENTITY_ID,
T2."TEXT" ENTITY_NAME
FROM            
UserProfiles
LEFT JOIN AspNetUsers ON UserProfiles.IdentityId = AspNetUsers.Id
INNER JOIN Localizations ON UserProfiles.LocalizationIdentifier_Id = Localizations.LocalizationIdentifier_Id
AND Localizations.CultureId = 1
LEFT JOIN UserProfileOrgUnits ON UserProfileOrgUnits.UserProfile_Id = UserProfiles.Id
LEFT JOIN OrgUnits ON UserProfileOrgUnits.OrgUnit_Id = OrgUnits.Id
LEFT JOIN Localizations T2 ON T2.LocalizationIdentifier_Id = OrgUnits.LocalizationIdentifier_Id
AND T2.CultureId = 1;
/
---------------------------DASHBOARD_HEADER_GET---------------------
create or replace PROCEDURE "DASHBOARD_HEADER_GET" 
        (

        P_FROM_DATE IN DATE DEFAULT NULL,
        P_TO_DATE   IN DATE DEFAULT NULL,
        P_ENTITY_ID IN NUMBER DEFAULT NULL,
        P_USER_ID   IN NUMBER DEFAULT NULL,
        P_LEVEL     IN NUMBER DEFAULT NULL,
        p_Status               	Number,
        p_Inbound             	Number,
        p_Outbound            	Number,
        p_Draft                 Number,
        p_Internal              Number,
		CV_1        OUT SYS_REFCURSOR

        )

AS

v_OutboundCount                NUMBER(10, 0);
v_OutboundDraftCountCreated    NUMBER(10, 0);
v_OutboundDraftCountAssigned   NUMBER(10, 0);
v_InboundCountCreated          NUMBER(10, 0);
v_InboundCountAssigned         NUMBER(10, 0);
v_InternalOutboundCountCreated NUMBER(10, 0);
v_InternalOutboundCountAssigne NUMBER(10, 0);
v_DelayedCount                 NUMBER(10, 0);

BEGIN

IF P_LEVEL = 1 THEN
BEGIN

--??? ??????? ?????? ???????
SELECT
COUNT(DISTINCT Transactions.Id) INTO v_OutboundCount
FROM
Transactions
inner join lookups on lookups.id = StatusId
WHERE
TransactionCategoryId = p_Outbound
AND(Transactions.CreatedBy = P_USER_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(Transactions.CreatedBy = P_USER_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id AND TransactionAssignmentHistories.ToUserId = P_USER_ID AND transactionassignmenthistories.toentityid = p_entity_id
WHERE 
TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(Transactions.CreatedBy = P_USER_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountAssigned
FROM Transactions
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
inner join lookups on lookups.id = StatusId
AND TransactionAssignmentHistories.ToUserId = P_USER_ID
AND transactionassignmenthistories.toentityid = p_entity_id
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(Transactions.CreatedBy = P_USER_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountAssigne
FROM Transactions
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
inner join lookups on lookups.id = StatusId
AND TransactionAssignmentHistories.ToUserId = P_USER_ID
AND transactionassignmenthistories.toentityid = p_entity_id
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);

--??? ????????? ????????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_DelayedCount
FROM Transactions
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
AND(TransactionAssignments.ToUserId = P_USER_ID)
AND lookups.enumreference not in (2,12);

END;
END IF;

IF P_LEVEL = 2 THEN
BEGIN
--??? ??????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundCount
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Outbound
AND(OrgUnitId = P_ENTITY_ID)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(OrgUnitId = P_ENTITY_ID)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToEntityId = P_ENTITY_ID
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(OrgUnitId = P_ENTITY_ID)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToEntityId = P_ENTITY_ID
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(OrgUnitId = P_ENTITY_ID)
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountAssigne
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToEntityId = P_ENTITY_ID
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);

--??? ????????? ????????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_DelayedCount
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
--INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
--AND TransactionAssignmentHistories.ToEntityId = P_ENTITY_ID
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
AND(transactionassignments.toentityid = P_ENTITY_ID)
AND lookups.enumreference not in (2,12);
END;
END IF;

IF P_LEVEL = 3 THEN
BEGIN

--??? ??????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundCount
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Outbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);
--AND TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId;

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);
--AND (TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId or TransactionAssignmentHistories.ToUserId is null);

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountAssigne
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);
--AND (TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId or TransactionAssignmentHistories.ToUserId is null);

--??? ????????? ????????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_DelayedCount
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
--INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE
OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
--AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND(transactionassignments.toentityid IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND lookups.enumreference not in (2,12);
END;
END IF;

IF P_LEVEL = 4 THEN
BEGIN

--??? ??????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundCount
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Outbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ????? ?????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);
--AND TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId;

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ?????? ??????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);
--AND (TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId or TransactionAssignmentHistories.ToUserId is null);

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_FROM_DATE AND P_TO_DATE;

--??? ??????? ???????? ???????? ???????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountAssigne
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_FROM_DATE AND P_TO_DATE
AND lookups.enumreference not in (2,12);
--AND (TransactionAssignmentHistories.FromUserId != TransactionAssignmentHistories.ToUserId or TransactionAssignmentHistories.ToUserId is null);

--??? ????????? ????????
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_DelayedCount
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
--INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
--AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND(transactionassignments.toentityid IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
AND lookups.enumreference not in (2,12);

END;
END IF;

OPEN CV_1 FOR
SELECT
NVL(v_OutboundCount, 0) "OutboundCount",
    NVL(v_OutboundDraftCountCreated, 0) "OutboundDraftCountCreated",
        NVL(v_OutboundDraftCountAssigned, 0) "OutboundDraftCountAssigned",
            NVL(v_InboundCountCreated, 0) "InboundCountCreated",

                NVL(v_InboundCountAssigned, 0) "InboundCountAssigned",
                    NVL(v_InternalOutboundCountCreated, 0) "InternalOutboundCountCreated",
                        NVL(v_InternalOutboundCountAssigne, 0)   "InternalOutboundCountAssigned",
                            NVL(v_DelayedCount, 0)                    "DelayedCount"
FROM
DUAL;

END;

/

--------- DASHBOARD_DETAILS_GET -------------
create or replace PROCEDURE "DASHBOARD_DETAILS_GET" 
(
  P_From_Date    DATE,
  P_To_Date      DATE,
  P_Entity_ID     NUMBER,
  P_User_ID      NUMBER,
  P_level       NUMBER,
  P_CountrID    NUMBER,
  P_CultureName NVARCHAR2 DEFAULT NULL,
  P_PageIndex   NUMBER,
  P_PageSize    NUMBER,
  P_Inbound    	Number,
  P_Outbound   	Number,
  P_Draft       Number,
  P_Internal    Number,
  P_TotalCount  OUT Number,
  P_cur         OUT SYS_REFCURSOR
)

IS
    V_CultureID  NUMBER(10,0);
    V_FirstIndex NUMBER(10,0);
    V_LastIndex  NUMBER(10,0);

BEGIN
    V_FirstIndex := P_PageIndex * P_PageSize + 1 ;
    V_LastIndex  := P_PageIndex * P_PageSize + P_PageSize ;

DELETE FROM TT_TEMP;

SELECT Id INTO V_CultureID FROM Cultures WHERE ShortName = P_CultureName;


    IF p_level = 1 THEN
      BEGIN
        IF p_CountrID = 1 THEN
          BEGIN
            --??? ??????? ?????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
               FROM
Transactions
inner join lookups on lookups.id = StatusId
WHERE
TransactionCategoryId = p_Outbound
AND(Transactions.CreatedBy = P_User_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 2 THEN
          BEGIN
            --??? ??????? ????? ?????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(Transactions.CreatedBy = P_User_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 3 THEN
          BEGIN
            --??? ??????? ????? ?????? ???????
            INSERT INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToUserId = P_User_ID
AND transactionassignmenthistories.toentityid = p_entity_id
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
        IF p_CountrID = 4 THEN
          BEGIN
            --??? ??????? ?????? ??????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(Transactions.CreatedBy = P_User_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 5 THEN
          BEGIN
            --??? ??????? ?????? ??????? ???????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
inner join lookups on lookups.id = StatusId
AND TransactionAssignmentHistories.ToUserId = P_User_ID
AND transactionassignmenthistories.toentityid = p_entity_id
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
        IF p_CountrID = 6 THEN
          BEGIN
            --??? ??????? ???????? ???????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(Transactions.CreatedBy = P_User_ID)
AND(transactions.orgunitid = p_entity_id)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 7 THEN
          BEGIN
            --??? ??????? ???????? ???????? ???????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
inner join lookups on lookups.id = StatusId
AND TransactionAssignmentHistories.ToUserId = P_User_ID
AND transactionassignmenthistories.toentityid = p_entity_id
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
        IF p_CountrID = 8 THEN
          BEGIN
            --??? ????????? ????????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
AND(TransactionAssignments.ToUserId = P_User_ID)
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
      END;
    END IF;
    IF p_level = 2 THEN
      BEGIN
        IF p_CountrID = 1 THEN
          BEGIN
            --??? ??????? ?????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Outbound
AND(OrgUnitId = P_Entity_ID)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 2 THEN
          BEGIN
            --??? ??????? ????? ?????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(OrgUnitId = P_Entity_ID)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 3 THEN
          BEGIN
            --??? ??????? ????? ?????? ???????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToEntityId = P_Entity_ID
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
        IF p_CountrID = 4 THEN
          BEGIN
            --??? ??????? ?????? ??????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(OrgUnitId = P_Entity_ID)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 5 THEN
          BEGIN
            --??? ??????? ?????? ??????? ???????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToEntityId = P_Entity_ID
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
        IF p_CountrID = 6 THEN
          BEGIN
            --??? ??????? ???????? ???????? ???????
            INSERT
            INTO TT_TEMP
              (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(OrgUnitId = P_Entity_ID)
AND "DATE" BETWEEN P_From_Date AND P_To_Date
              );
          END;
        END IF;
        IF p_CountrID = 7 THEN
          BEGIN
            --??? ??????? ???????? ???????? ???????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
AND TransactionAssignmentHistories.ToEntityId = P_Entity_ID
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
        IF p_CountrID = 8 THEN
          BEGIN
            --??? ????????? ????????
            INSERT
            INTO TT_TEMP
              ( SELECT DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
--INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
--AND TransactionAssignmentHistories.ToEntityId = P_Entity_ID
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
AND(transactionassignments.toentityid = P_Entity_ID)
AND lookups.enumreference not in (2,12)
              );
          END;
        END IF;
      END;
    END IF;
    IF p_level = 3 THEN
    BEGIN
      IF p_CountrID = 1 THEN
        BEGIN
          --??? ??????? ?????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Outbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;
      END IF;
      IF p_CountrID = 2 THEN
        BEGIN
          --??? ??????? ????? ?????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;

      END IF;
      IF p_CountrID = 3 THEN
        BEGIN
          --??? ??????? ????? ?????? ???????
          INSERT
          INTO TT_TEMP
            (
              SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
            );
        END;

      END IF;
      IF p_CountrID = 4 THEN
        BEGIN
          --??? ??????? ?????? ??????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;
      END IF;


       IF p_CountrID = 5 THEN
        BEGIN
          --??? ??????? ?????? ??????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
            );
        END;
      END IF;


      IF p_CountrID = 6 THEN
        BEGIN
          --??? ??????? ???????? ???????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
 FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;
      END IF;

      IF p_CountrID = 7 THEN
        BEGIN
          --??? ??????? ???????? ???????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
            );
        END;
      END IF;

      IF p_CountrID = 8 THEN
        BEGIN
          --??? ????????? ????????
          INSERT
          INTO TT_TEMP
            ( SELECT DISTINCT Transactions.Id
 FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
--INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE
OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
--AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND(transactionassignments.toentityid IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND lookups.enumreference not in (2,12)
            );
        END;
      END IF;
    END;
  END IF;
    IF p_level = 4 THEN
    BEGIN
      IF p_CountrID = 1 THEN
        BEGIN
          --??? ??????? ?????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Outbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;
      END IF;
      IF p_CountrID = 2 THEN
        BEGIN
          --??? ??????? ????? ?????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;

      END IF;
      IF p_CountrID = 3 THEN
        BEGIN
          --??? ??????? ????? ?????? ???????
          INSERT
          INTO TT_TEMP
            (
              SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
            );
        END;

      END IF;
      IF p_CountrID = 4 THEN
        BEGIN
          --??? ??????? ?????? ??????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;
      END IF;


       IF p_CountrID = 5 THEN
        BEGIN
          --??? ??????? ?????? ??????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
            );
        END;
      END IF;


      IF p_CountrID = 6 THEN
        BEGIN
          --??? ??????? ???????? ???????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND "DATE" BETWEEN P_From_Date AND P_To_Date
            );
        END;
      END IF;

      IF p_CountrID = 7 THEN
        BEGIN
          --??? ??????? ???????? ???????? ???????
          INSERT
          INTO TT_TEMP
            (SELECT  DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND TransactionAssignmentHistories."DATE" BETWEEN P_From_Date AND P_To_Date
AND lookups.enumreference not in (2,12)
            );
        END;
      END IF;

      IF p_CountrID = 8 THEN
        BEGIN
          --??? ????????? ????????
          INSERT
          INTO TT_TEMP
            ( SELECT DISTINCT Transactions.Id
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
--INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND STATUSID <> (select Id from Lookups where  categoryid=30 and enumreference=4)
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
--AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND(transactionassignments.toentityid IN(SELECT Id FROM OrgUnits START WITH Id = P_Entity_ID CONNECT BY ParentId = PRIOR Id))
AND lookups.enumreference not in (2,12)
            );
        END;
      END IF;
    END;
  END IF;

  OPEN p_cur FOR
  SELECT 
    Transactions.Id ,
    Transactions."NUMBER" ,
    Transactions."DATE" ,
    Transactions.DateH ,
    Transactions.LetterTypeId ,
    L1.Text LetterType ,
    Transactions.PriorityId ,
    L2.Text Priority ,
    Transactions.ConfidentialityId ,
    L4.Text Confidentiality ,
    Transactions.TransactionTypeId ,
    L3.Text TransactionType ,
    Transactions.Subject ,
    Transactions.CreatedOn ,
    TT.Text Creator
  FROM 
    Transactions
  JOIN TT_TEMP TE
  ON TE.TRANSID = Transactions.Id
  LEFT JOIN LetterTypes
  ON Transactions.LetterTypeId = LetterTypes.Id
  LEFT JOIN Localizations L1
  ON L1.LocalizationIdentifier_Id = LetterTypes.LocalizationIdentifier_Id
  AND L1.CultureId                = V_CultureID
  LEFT JOIN Priorities
  ON Transactions.PriorityId = Priorities.Id
  LEFT JOIN Localizations L2
  ON L2.LocalizationIdentifier_Id = Priorities.LocalizationIdentifier_Id
  AND l2.CultureId                = V_CultureID
  LEFT JOIN TransactionTypes
  ON Transactions.TransactionTypeId = TransactionTypes.Id
  LEFT JOIN Localizations L3
  ON L3.LocalizationIdentifier_Id = TransactionTypes.LocalizationIdentifier_Id
  AND L3.CultureId                = V_CultureID
  LEFT JOIN Permissions
  ON Transactions.ConfidentialityId = Permissions.Id
  LEFT JOIN LookupLocalizations L4
  ON L4.Lookup_Id   = Permissions.Name_Id
  AND L4.Culture_Id = V_CultureID
  LEFT JOIN UserProfiles ON Transactions.CreatedBy = UserProfiles.Id
  LEFT JOIN Localizations TT ON UserProfiles.LocalizationIdentifier_Id = TT.LocalizationIdentifier_Id
  AND TT.CultureId = V_CultureID

  ORDER BY
  Transactions.Id DESC
  OFFSET p_PageIndex * p_PageSize ROWS
  FETCH NEXT p_PageSize ROWS ONLY;


  SELECT COUNT(1)
  INTO p_TotalCount
  FROM TT_TEMP ;

  DELETE FROM TT_TEMP;

END;
/
-------------------------Search_InBound-----------------------
Create or replace PROCEDURE SEARCH_INBOUND(

p_Number				Number,
p_HasFullPrivilege      Number,
p_OrgUnitId				Number,
p_UserId				Number,
p_TransactionCategoryId	Number,
p_TransactionTypeId     Number,
p_DateFrom				Date,
p_DateTo				Date,
p_PageIndex				Number,
p_PageSize				Number,
p_Ascending				number,
p_CultureName			nvarchar2,
p_OrderBy				nvarchar2,
p_Year					Number,
p_Status                Number,
p_TotalCount			OUT Number,
p_cur                   OUT SYS_REFCURSOR)
IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;



BEGIN

SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;
END IF;

SELECT TO_NUMBER("TEXT") INTO v_Year FROM LookupLocalizations  where Lookup_Id = p_Year
AND Culture_Id = v_CultureID;

IF(v_Year IS NULL) THEN
v_Year:= to_char(sysdate, 'yyyy', 'nls_calendar=''arabic hijrah''');
END IF;

INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId
WHERE

TR.YearH = v_Year
AND(TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND(p_TransactionTypeId = -1 OR TR.TransactionTypeId = p_TransactionTypeId)
AND(p_Number = -1 OR TR."NUMBER" = p_Number)
AND(p_OrgUnitId = -1 OR TED.EntityId = p_OrgUnitId )
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);
--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM InScopeTr ;


open p_cur for 
SELECT  ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
TR.Id,
    TR."NUMBER" as "NUMBER",
        TR.TransactionCategoryId,
            LL_TransType."TEXT" As TransactionCategoryName,
                TR."DATE",
                    TR.DateH,
                        LOC_PR."TEXT" As PriorityName,
                            LL_Perm."TEXT" as ConfidentialityName,
                                TR.TransactionTypeId,
                                    LL_SourceType."TEXT" As TransactionType,
									   TA.ToUserId,
									   TA.ToEntityId,
                                        LOC_ExternalParty."TEXT" as PartyName,
                                            LOC_OrgUnit."TEXT" as OrgUnitName,
                                                TR.Subject,
                                                    LL_Status."TEXT" as StatusName,
													TR.StatusId as StatusId,
                                                        P_Permission."WEIGHT" as Weight,
															CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
INNER JOIN INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id


ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END;

/
------------------------- Search_OutboundExternal-----------------------
Create or replace PROCEDURE SEARCH_OUTBOUND_EXTERNAL(

    p_Number				Number,
	p_HasFullPrivilege      Number,
    p_OrgUnitId				Number,
    p_UserId				Number,
    p_TransactionCategoryId		Number,
    p_TransactionTypeId			Number,
    p_DateFrom				Date,
    p_DateTo				Date,
    p_PageIndex				Number,
    p_PageSize				Number,
    p_Ascending				number,
    p_CultureName			nvarchar2,
    p_OrderBy				nvarchar2,
    p_Year					Number,
    p_Status                Number,
    p_TotalCount			OUT Number,
    p_cur                   OUT SYS_REFCURSOR)

IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;


BEGIN

SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;

END IF;

SELECT TO_NUMBER(Text) INTO v_Year FROM LookupLocalizations  where Lookup_Id = p_Year
AND Culture_Id = v_CultureID;

IF(v_Year IS NULL) THEN
v_Year:= to_char(sysdate, 'yyyy', 'nls_calendar=''arabic hijrah''');
END IF;


INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId 
WHERE
TR.YearH = v_Year
AND(TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND(p_TransactionTypeId = -1 OR TR.TransactionTypeId = p_TransactionTypeId)
AND(p_Number = -1 OR TR."NUMBER" = p_Number)
AND(TED.EntityId = p_OrgUnitId or p_OrgUnitId = -1)
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);

--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM INSCOPETR ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
        TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,                          
										 TA.ToUserId,
										 TA.ToEntityId,
										   TR.StatusId,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
INNER JOIN INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END;

/
------------------------- Search_OutboundInternal-----------------------
Create or replace PROCEDURE SEARCH_OUTBOUND_INTERNAL(

    p_Number				Number,	
	p_HasFullPrivilege      Number,
    p_OrgUnitId				Number,
    p_UserId				Number,
    p_TransactionCategoryId		Number,
    p_TransactionTypeId			Number,
    p_DateFrom				Date,
    p_DateTo				Date,
    p_PageIndex				Number,
    p_PageSize				Number,
    p_Ascending				number,
    p_CultureName			nvarchar2,
    p_OrderBy				nvarchar2,
    p_Year					Number,
    p_Status                Number,
    p_TotalCount			OUT Number,
    p_cur                   OUT SYS_REFCURSOR)

IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;


BEGIN

--WITH RECOMPILE
--ALTER PROCEDURE my_procedure COMPILE;
--When a procedure is compiled for the first time or recompiled, the procedure's query plan is optimized for the current state of the database and its objects.

SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;
DBMS_OUTPUT.put_line(V_CultureID);

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;

END IF;

SELECT TO_NUMBER(Text) INTO v_Year FROM LookupLocalizations  where Lookup_Id = p_Year
AND Culture_Id = v_CultureID;

IF(v_Year IS NULL) THEN
v_Year:= to_char(sysdate, 'yyyy', 'nls_calendar=''arabic hijrah''');
END IF;

INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId 
WHERE
TR.YearH = v_Year
AND(TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND(p_TransactionTypeId = -1 OR TR.TransactionTypeId = p_TransactionTypeId)
AND(p_Number = -1 OR TR."NUMBER" = p_Number)
AND(TED.EntityId = p_OrgUnitId or p_OrgUnitId = -1)
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);

--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM INSCOPETR ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
        TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
										   TA.ToUserId,
										   TA.ToEntityId,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
INNER JOIN INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END;

/
------------------------- Search_OutboundDraft-----------------------
Create or replace PROCEDURE SEARCH_OUTBOUND_DRAFT(

    p_Number				Number,
	p_HasFullPrivilege      Number,
    p_OrgUnitId				Number,
    p_UserId				Number,
    p_TransactionCategoryId		Number,
    p_TransactionTypeId			Number,
    p_DateFrom				Date,
    p_DateTo				Date,
    p_PageIndex				Number,
    p_PageSize				Number,
    p_Ascending				number,
    p_CultureName			nvarchar2,
    p_OrderBy				nvarchar2,
    p_Year					Number,
    p_Status                Number,
    p_TotalCount			OUT Number,
    p_cur                   OUT SYS_REFCURSOR)

IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;


BEGIN

SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;
--DBMS_OUTPUT.put_line (v_CultureID);

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;


END IF;

SELECT TO_NUMBER(Text) INTO v_Year FROM LookupLocalizations  where Lookup_Id = p_Year
AND Culture_Id = v_CultureID;

IF(v_Year IS NULL) THEN
v_Year:= to_char(sysdate, 'yyyy', 'nls_calendar=''arabic hijrah''');

END IF;


INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId 
WHERE
TR.YearH = v_Year
AND(TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND(p_TransactionTypeId = -1 OR TR.TransactionTypeId = p_TransactionTypeId)
AND(p_Number = -1 OR TR."NUMBER" = p_Number)
AND(p_OrgUnitId = -1 OR TED.EntityId = p_OrgUnitId  )
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);

--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM INSCOPETR ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
        TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
									      TA.ToUserId,
										  TA.ToEntityId,
										  TR.IsDeleted,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR 
INNER JOIN  INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END;
/
------------------------- Search_bySubject-----------------------
Create or replace PROCEDURE SEARCH_SUBJECT(

        p_Subject			    nvarchar2,
		p_HasFullPrivilege      Number,
        p_OrgUnitId			    number,
        p_UserId				Number,
        p_TransactionCategoryId     number,
        p_PageIndex			    number,
        p_PageSize			    number,
        p_Ascending			    number,
        p_CultureName		    nvarchar2,
        p_OrderBy			    nvarchar2,
        p_Year				    number,
        p_Status                Number,
        p_TotalCount		    OUT number,
        p_cur                   OUT SYS_REFCURSOR)

IS
--WITH RECOMPILE
v_CultureID    number;
v_DateTo       TimeStamp;
v_Subject      nvarchar2(256);
v_Year         number;
v_TotalCount   number;

BEGIN


SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;
DBMS_OUTPUT.put_line(v_CultureID);


SELECT TO_NUMBER(Text) INTO v_Year FROM LookupLocalizations  where Lookup_Id = p_Year AND Culture_Id = v_CultureID;

IF(v_Year IS NULL) THEN
v_Year:= to_char(sysdate, 'yyyy', 'nls_calendar=''arabic hijrah''');
DBMS_OUTPUT.put_line(v_Year);
END IF;

--IF p_Subject <> '' AND p_Subject IS NOT--An empty string is treated as a null value in Oracle
IF(p_Subject IS NOT NULL)  THEN
v_Subject:= REPLACE(p_Subject, ' ', ' ');

END IF;


INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId 
WHERE
YearH = v_Year
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND( v_Subject IS NULL OR   INSTR(Lower(TR.Subject),Lower( v_Subject)) > 0)
AND(p_OrgUnitId = -1  OR TED.EntityId = p_OrgUnitId  )
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);

--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM INSCOPETR ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
       TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
									      TA.ToUserId,
										  TA.ToEntityId,
										   TR.StatusId,
										     TR.IsDeleted,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
                                                                0 AS IsArchived,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
INNER JOIN InScopeTr TT ON TT.TRANSID = TR.Id
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY  ;

DELETE FROM INSCOPETR ;

END;

/
------------------------- Search_byBarcode-----------------------
create or replace PROCEDURE SEARCH_BARCODE(

        p_Barcode			    nvarchar2, 
        p_PageIndex			    number,
        p_PageSize			    number,
        p_Ascending			    number,
        p_CultureName		    nvarchar2,
        p_OrderBy			    nvarchar2, 
        p_TotalCount		    OUT number,
        p_cur                   OUT SYS_REFCURSOR)

IS
--WITH RECOMPILE
v_CultureID    number; 
v_TotalCount   number;

BEGIN


SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;
DBMS_OUTPUT.put_line(v_CultureID);


INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN Barcodes B
ON TR.Id = B.ReferenceId 
WHERE
B.Value = p_Barcode;


--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM INSCOPETR ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
       TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
									      TA.ToUserId,
										  TA.ToEntityId,
										   TR.StatusId,
										     TR.IsDeleted,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
                                                                0 AS IsArchived

FROM
Transactions  TR
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
INNER JOIN InScopeTr TT ON TT.TRANSID = TR.Id
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY  ;

DELETE FROM INSCOPETR ;

END;
/
------------------------- Search_Entity-----------------------
 Create or replace PROCEDURE SEARCH_ENTITY(

        p_ExternalParty				Number,
		p_HasFullPrivilege      Number,
        p_OrgUnitId				Number,
        p_UserId				Number,
        p_TransactionCategoryId		Number,
        p_DateFrom				Date,
        p_DateTo				Date,
        p_PageIndex				Number,
        p_PageSize				Number,
        p_Ascending				number,
        p_CultureName			nvarchar2,
        p_OrderBy				nvarchar2,
        p_Status                Number,
        p_TotalCount			OUT Number,
        p_cur                   OUT SYS_REFCURSOR)

IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;


BEGIN

SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;

END IF;

INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR--WITH(NOLOCK)
LEFT JOIN TransactionEntityDetails TED--WITH(NOLOCK)
ON TR.Id = TED.TransactionId
WHERE
    (TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND(p_ExternalParty = -1 OR TR.ExternalPartyId = p_ExternalParty)
AND(p_OrgUnitId = -1 OR TED.EntityId = p_OrgUnitId )
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);


SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM InScopeTr ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
        TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
									     TA.ToUserId,
										 TA.ToEntityId,
										   TR.StatusId,
                                            TR.IsDeleted,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
INNER JOIN INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OU_OrgUnit  ON OU_OrgUnit.Id = p_OrgUnitId
LEFT JOIN Localizations LOC_OrgUnit ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END;

/
-------------------------MobileSearch-------------------------
create or replace PROCEDURE MOBILE_SEARCH 
(
 p_Number				NUMBER, 
 p_OrgUnitId			NUMBER,
 p_TransactionTypeId	NUMBER, 
 p_Subject				NVARCHAR2,
 p_TransCategory		NUMBER,
 p_CultureName			NVARCHAR2,
 p_cur                  OUT SYS_REFCURSOR
 )

IS
v_CultureID number;

BEGIN

SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId 
	WHERE 
		(p_TransCategory =-1 OR TR.TransactionCategoryId =p_TransCategory)
		AND (p_Number = -1 OR TR."NUMBER" = p_Number) 
		AND (p_TransactionTypeId =-1 OR TR.TransactionTypeId=p_TransactionTypeId)
		AND( p_Subject IS NULL OR   INSTR(Lower(TR.Subject),Lower( p_Subject)) > 0)
		AND (TED.EntityId = p_OrgUnitId OR p_OrgUnitId = -1)
		AND (TR.StatusId <> 1624 );
    
    
    open p_cur for 
	SELECT
		TR.Id AS TransID,
		TR."NUMBER" AS TransNo,
		TR.Subject AS TransTitle,
		TR.DateH AS TransDate,
		LOC_FROM_OrgUnit."TEXT" AS TransFrom,
		TR.TransactionCategoryId AS TransCategory,
		'' AS FileSize,
		LL_SourceType."TEXT" || ' - ' || CASE WHEN TA.FromEntityId = TA.ToEntityId AND TA.FromUserId = TA.ToUserId THEN LOC_FROM_OrgUnit."TEXT" ELSE LOC_TO_OrgUnit."TEXT" END AS TransSourceRow,
		TR."NUMBER" || ' - ' || LOC_Creating_OrgUnit."TEXT" AS TransNumberRow,
		LOC_FROM_OrgUnit."TEXT" AS EntityName,
		P_Permission.Code AS PrivilegeName
	FROM 
		Transactions TR
		LEFT JOIN Permissions P_Permission ON P_Permission.Id = TR.ConfidentialityId
		INNER JOIN InScopeTr TT ON TT.TRANSID = TR.Id
		LEFT JOIN TransactionTypes ST_SourceTypes ON ST_SourceTypes.Id = TR.TransactionTypeId
		LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = v_CultureID
		LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = v_CultureID
		LEFT JOIN Priorities PR ON PR.Id = TR.PriorityId
		LEFT JOIN Localizations LOC_PR ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId =v_CultureID
		LEFT JOIN LookupLocalizations LL_TransType ON LL_TransType.Lookup_Id = TR.TransactionTypeId AND LL_TransType.Culture_Id = v_CultureID
		LEFT JOIN ExternalParties EP_ExternalParty ON EP_ExternalParty.Id = TR.ExternalPartyId
		LEFT JOIN Localizations LOC_ExternalParty ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = v_CultureID
		LEFT JOIN OrgUnits OU_OrgUnit ON OU_OrgUnit.Id = p_OrgUnitId
		LEFT JOIN Localizations LOC_OrgUnit ON LOC_OrgUnit.LocalizationIdentifier_Id = OU_OrgUnit.LocalizationIdentifier_Id AND LOC_OrgUnit.CultureId = v_CultureID
		LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = v_CultureID
		INNER JOIN TransactionAssignments TA on TA.TransactionId = TR.Id
		LEFT JOIN OrgUnits OU_FROM_UNIT ON OU_FROM_UNIT.Id = TA.FromEntityId
		LEFT JOIN Localizations LOC_FROM_OrgUnit ON LOC_FROM_OrgUnit.LocalizationIdentifier_Id = OU_FROM_UNIT.LocalizationIdentifier_Id AND LOC_FROM_OrgUnit.CultureId = v_CultureID
		LEFT JOIN OrgUnits OU_TO_UNIT ON OU_TO_UNIT.Id = TA.ToEntityId
		LEFT JOIN Localizations LOC_TO_OrgUnit ON LOC_TO_OrgUnit.LocalizationIdentifier_Id = OU_TO_UNIT.LocalizationIdentifier_Id AND LOC_TO_OrgUnit.CultureId = v_CultureID
		LEFT JOIN OrgUnits OU_Creating_OrgUnit ON OU_Creating_OrgUnit.Id = TR.OrgUnitId
		LEFT JOIN Localizations LOC_Creating_OrgUnit ON LOC_Creating_OrgUnit.LocalizationIdentifier_Id = OU_Creating_OrgUnit.LocalizationIdentifier_Id AND LOC_Creating_OrgUnit.CultureId = v_CultureID
ORDER BY
TR.Id DESC;

DELETE FROM INSCOPETR ;
END;
/
------------------------- Search_Creator-----------------------
Create or replace PROCEDURE SEARCH_CREATOR(

        p_Creator				Number,
		p_HasFullPrivilege      Number,
        p_OrgUnitId				Number,
		p_UserId				Number,
        p_TransactionCategoryId		Number,
        p_DateFrom				Date,
        p_DateTo				Date,
        p_PageIndex				Number,
        p_PageSize				Number,
        p_Ascending				number,
        p_CultureName			nvarchar2,
        p_OrderBy				nvarchar2,
        p_Status                Number,
        p_TotalCount			OUT Number,
        p_cur                   OUT SYS_REFCURSOR)

IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;


BEGIN

SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;
DBMS_OUTPUT.put_line(v_DateTo);
--DBMS_OUTPUT.put_line(sysdate);
END IF;

INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId
WHERE
    (TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
AND(p_Creator = -1 OR TR.UserId = p_Creator)
AND(TED.EntityId = p_OrgUnitId or p_OrgUnitId = -1)
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);

SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM InScopeTr ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
        TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
						                 TA.ToUserId,
										 TA.ToEntityId,
										   TR.StatusId,
                                            TR.IsDeleted,
                                             LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
INNER JOIN INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END; 
/ 


------------------------- Search_ASSIGNTRANSACTION-----------------------
Create or replace PROCEDURE SEARCH_ASSIGNTRANSACTION(

        p_FromEntity           Number,
		p_HasFullPrivilege      Number,
        p_EntityId				Number,
        p_OrgUnitId				Number,
        p_UserId				Number,
        p_TransactionCategoryId		Number,
        p_DateFrom				Date,
        p_DateTo				Date,
        p_PageIndex				Number,
        p_PageSize				Number,
        p_Ascending				number,
        p_CultureName			nvarchar2,
        p_OrderBy				nvarchar2,
        p_Status                Number,
        p_TotalCount			OUT Number,
        p_cur                   OUT SYS_REFCURSOR)

IS
v_CultureID number;
v_Year      number;
v_DateTo  TimeStamp;
v_TotalCount   number;
v_ToEntity    number;
v_FromEntity number;


BEGIN

SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;

--@DateTo
IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= p_DateTo;
DBMS_OUTPUT.put_line(v_DateTo);
--DBMS_OUTPUT.put_line(sysdate);
END IF;

IF p_FromEntity = 1 THEN
   v_ToEntity  := -1;
   v_FromEntity := p_EntityId;

ELSE
   v_ToEntity  := p_EntityId;
   v_FromEntity := -1;

END IF;

INSERT INTO InScopeTr
SELECT DISTINCT TR.Id
FROM
Transactions TR
INNER JOIN transactionassignmenthistories TAH
ON TR.Id = TAH.TransactionId
WHERE
    (TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
AND(p_TransactionCategoryId = -1 OR TR.TransactionCategoryId = p_TransactionCategoryId)
--AND(p_Creator = -1 OR TR.UserId = p_Creator)
AND(TAH.TOENTITYID <>TAH.FROMENTITYID AND (TAH.TOENTITYID = v_ToEntity or TAH.FROMENTITYID=v_FromEntity))
AND(TR.StatusId <> p_Status )
AND 
(
       EXISTS(
            select 
                1
            from 
                TransactionAssignmentHistories TAH 
                left Join 
                USERDELEGATIONS UD on TAH.USERDELEGATIONID =UD.ID 
                left join
                userpreferences UP on ud.userpreferenceid = UP.ID
            where 
                (TAH.fromuserid =p_UserId or TAH.touserid=p_UserId) 
                and TAH.TransactionId = TR.ID
                AND (
                        TAH.USERDELEGATIONID is null 
                        OR 
                        (up.userprofileid = p_UserId)
                        OR
                        (UD.userprofileid = p_UserId and ((UD.SHOWTRANSACTION=1)OR (ud.statusid = (select id from lookups where categoryid=48 and enumreference=2)))))        
            )
OR
    (p_HasFullPrivilege = 1)
);
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM InScopeTr ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
        TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
						                 TA.ToUserId,
										 TA.ToEntityId,
										   TR.StatusId,
                                            TR.IsDeleted,
                                             LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
																CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
INNER JOIN INSCOPETR TT ON TT.TRANSID = TR.Id
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

DELETE FROM INSCOPETR ;

END;


/



-----------------SEARCH_DOCUMENT_NUMBER -------------------
create or replace PROCEDURE SEARCH_DOCUMENT_NUMBER(

        p_DocumentNumber        nvarchar2,
        p_OrgUnitId			    number, 
        p_PageIndex			    number,
        p_PageSize			    number,
        p_Ascending			    number,
        p_CultureName		    nvarchar2,
        p_OrderBy			    nvarchar2,
        p_Year				    number,
        p_Status                Number,
        p_TotalCount		    OUT number,
        p_cur                   OUT SYS_REFCURSOR)

IS
--WITH RECOMPILE
v_CultureID    number; 
v_Year         number;
v_TotalCount   number;

BEGIN


SELECT Id INTO V_CultureID
FROM Cultures  where ShortName = p_CultureName;
DBMS_OUTPUT.put_line(v_CultureID);


SELECT TO_NUMBER(Text) INTO v_Year FROM LookupLocalizations  where Lookup_Id = p_Year AND Culture_Id = v_CultureID;

IF(v_Year IS NULL) THEN
v_Year:= to_char(sysdate, 'yyyy', 'nls_calendar=''arabic hijrah''');
DBMS_OUTPUT.put_line(v_Year);
END IF;

INSERT INTO InScopeTr
SELECT TR.Id
FROM
Transactions TR
LEFT JOIN TransactionEntityDetails TED
ON TR.Id = TED.TransactionId 
WHERE
YearH = v_Year
AND( TR.DocumentNumber = p_DocumentNumber) 
AND(p_OrgUnitId = -1  OR TED.EntityId = p_OrgUnitId  )
AND(TR.StatusId <> p_Status );


--Return Total Count
SELECT 	COUNT(TRANSID) INTO p_TotalCount
FROM INSCOPETR ;

open p_cur for 
SELECT  
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id,
       TR."NUMBER" as "NUMBER",
            TR.TransactionCategoryId,
                LL_TransType."TEXT" As TransactionCategoryName,
                    TR."DATE",
                        TR.DateH,
                            LOC_PR."TEXT" As PriorityName,
                                LL_Perm."TEXT" as ConfidentialityName,
                                    TR.TransactionTypeId,
                                        LL_SourceType."TEXT" As TransactionType,
									      TA.ToUserId,
										  TA.ToEntityId,
										   TR.StatusId,
										     TR.IsDeleted,
                                            LOC_ExternalParty."TEXT" as PartyName,
                                                LOC_OrgUnit."TEXT" as OrgUnitName,
                                                    TR.Subject,
                                                        LL_Status."TEXT" as StatusName,
														TR.StatusId as StatusId,
                                                            P_Permission.Weight as Weight,
                                                                0 AS IsArchived,
																	CASE WHEN EXISTS (SELECT 1 FROM TransactionLinks WHERE TransactionId = TR.Id OR ToTransactionId = TR.Id) THEN 1 ELSE 0 END AS HasLinks

FROM
Transactions  TR
LEFT JOIN Permissions P_Permission  ON P_Permission.Id = TR.ConfidentialityId
INNER JOIN InScopeTr TT ON TT.TRANSID = TR.Id
LEFT JOIN TransactionTypes ST_SourceTypes  ON ST_SourceTypes.Id = TR.TransactionTypeId
LEFT JOIN Localizations LL_SourceType ON LL_SourceType.LocalizationIdentifier_Id = ST_SourceTypes.LocalizationIdentifier_Id AND LL_SourceType.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Perm ON LL_Perm.Lookup_Id = P_Permission.Name_Id AND LL_Perm.Culture_Id = V_CultureID
LEFT JOIN Priorities PR  ON PR.Id = TR.PriorityId
LEFT JOIN Localizations LOC_PR  ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id AND LOC_PR.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_TransType  ON LL_TransType.Lookup_Id = TR.TransactionCategoryId AND LL_TransType.Culture_Id = V_CultureID
LEFT JOIN ExternalParties EP_ExternalParty  ON EP_ExternalParty.Id = TR.ExternalPartyId
LEFT JOIN Localizations LOC_ExternalParty  ON LOC_ExternalParty.LocalizationIdentifier_Id = EP_ExternalParty.Name_Id AND LOC_ExternalParty.CultureId = V_CultureID
LEFT JOIN OrgUnits OrgUnits  ON  TR.OrgUnitId =OrgUnits.Id
LEFT JOIN Localizations LOC_OrgUnit ON  OrgUnits.LocalizationIdentifier_Id = LOC_OrgUnit.LocalizationIdentifier_Id
AND LOC_OrgUnit.CultureId = V_CultureID
LEFT JOIN LookupLocalizations LL_Status ON LL_Status.Lookup_Id = TR.StatusId AND LL_Status.Culture_Id = V_CultureID
INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
ORDER BY
TR.Id DESC
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY  ;

DELETE FROM INSCOPETR ;

END;
/
-----------------------REPORT_TRANSACTIONSSEARCH------------------
create or replace PROCEDURE REPORT_TRANSACTIONSSEARCH
(
    p_DateFrom					"DATE" ,
	p_DateTo                   "DATE" ,
	p_TransactionCategoryId	   "NUMBER" ,
	p_TransactionNumber        "NUMBER" ,
	p_TransactioDescription    NVARCHAR2 ,
	-------المشتركة 
	p_TransactionTypeId    "NUMBER" , 
	p_IsAppointment            NUMBER,
	------- if p_IsAppointment  =1
	p_AppointmentDate          Date,
	p_ConfidentialityId	       "NUMBER" ,
	p_PriorityId	           "NUMBER" ,
	p_LetterTypeId 			   "NUMBER" , 
	p_Remarks                  NVARCHAR2 ,   
	p_DeliveryMethodId         "NUMBER" ,
	p_TransactionStatusId         "NUMBER" ,
	 -------------بيانات أصحاب العلاقة 
	p_FullName	               NVARCHAR2 ,
	p_CivilID	               NVARCHAR2 ,
	p_MobileNumber	           NVARCHAR2  ,

	------For InBound
	p_IsForIndividual          NUMBER,
---FOR IsForIndividual =0
	p_InboundDateH	           NVARCHAR2 ,   --Inbound Date
	p_ExternalPartyId	       "NUMBER" ,      --Inbound  Destination
	p_DocumentNumber           NVARCHAR2,    --Inbound_Doc_No
	p_OutBoundDate             NVARCHAR2,    --Outbound  Date
---Assignment transactions
	p_FromOrgUnitId	          "NUMBER" ,
	p_FromUserId	          "NUMBER" ,
	p_ToOrgUnitId	          "NUMBER" ,
	p_ToUserId                "NUMBER" ,
	p_CultureName             NVARCHAR2 , 
     --LEVEL
    P_ENTITY_ID              "NUMBER" DEFAULT NULL,
    P_USER_ID                "NUMBER" DEFAULT NULL,
    P_LEVEL                  "NUMBER" DEFAULT NULL,
    --Pagenation
    p_PageIndex				Number,
    p_PageSize	           	Number,
    p_TotalCount			OUT Number,
    p_Cur                   OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
v_DateTo               TIMESTAMP;
v_Subject              NVARCHAR2(20);

 BEGIN

SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

IF(p_DateTo IS NOT NULL) THEN
v_DateTo:= INTERVAL '1' DAY + INTERVAL '-1' Second + p_DateTo;
END IF;

IF(p_TransactioDescription IS NOT NULL)  THEN
v_Subject:= REPLACE(p_TransactioDescription, ' ', '*');
END IF;

INSERT INTO InScopeTr
SELECT distinct TR.Id 
FROM
Transactions TR
WHERE (p_TransactionCategoryId =-1 OR TR.TransactionCategoryId =p_TransactionCategoryId)
AND(TR."DATE" between NVL(p_DateFrom, TR."DATE") AND NVL(v_DateTo, TR."DATE"))
ORDER BY
TR.Id;

OPEN p_Cur FOR  
SELECT  distinct
	ROW_NUMBER() OVER(ORDER BY TR.Id asc) AS RowNumber,
    TR.Id TransactionId,
    TR.TransactionTypeId ,
	TR.TransactionCategoryId,
    llOC_TransactionCategory."TEXT" TransactionCategoryText,
	TR."DATE",
    TR."NUMBER",
	Loc_CreatorEntityId."TEXT" OrgUnitText,
    TR.Subject TransactioDescription, 
    LLOC_Perm."TEXT" ConfidentialityText,
    LOC_PR."TEXT"  PriorityText,
    TR.Remarks,
    llOC_Delivery."TEXT" DeliveryMethodText,
    TR.Subject ,
    Names.FirstName,
    Names.CivilID,
    Names.MobileNumber,
    Loc_External."TEXT" ExternalPartyText,
    TR.InboundDateH, 
    TR.DocumentNumber ,
    TR.CreatedOn OutBoundDate,
    Loc_FromEntityId."TEXT" FromEntityText,
    Loc_FromUserId."TEXT" FromUserText,
    Loc_ToEntityId."TEXT" ToEntityText,
    Loc_ToUserId."TEXT" ToUserText,
	UserProfiles_ToUserId.Id ToUserId,
    TR.RemindDate ,
	LOC_LT."TEXT" LetterTypeText,
    LOC_ST."TEXT" TransactionTypeText,
	TR.OutboundDraftId 

	FROM     
		Transactions TR INNER JOIN  INSCOPETR TT ON TT.TRANSID = TR.Id
		lEFT OUTER JOIN  TransactionNames TRName   ON TR.Id =TRName.TransactionId
		lEFT OUTER JOIN Names Names  ON TRName.NameId=Names.Id	
		lEFT OUTER JOIN Permissions Perm ON Perm.Id = TR.ConfidentialityId
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm ON LLOC_Perm.Lookup_Id = Perm.Name_Id	
             AND LLOC_Perm.Culture_Id = v_CultureID	
		lEFT OUTER JOIN Priorities PR ON PR.Id = TR.PriorityId	
		LEFT OUTER JOIN  Localizations LOC_PR ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id
           AND LOC_PR.CultureId = v_CultureID	
		lEFT OUTER JOIN LookupLocalizations llOC_Delivery ON llOC_Delivery.Lookup_Id = TR.DeliveryMethodId
		    AND llOC_Delivery.Culture_Id =v_CultureID	
        lEFT OUTER JOIN LookupLocalizations llOC_TransactionCategory ON TR.TransactionCategoryId = llOC_TransactionCategory.Lookup_Id
	    	AND llOC_TransactionCategory.Culture_Id =v_CultureID
         LEFT OUTER JOIN LetterTypes LT ON  LT.Id= TR.LetterTypeId
         LEFT OUTER JOIN   Localizations LOC_LT  ON  LT.LocalizationIdentifier_Id=LOC_LT.LocalizationIdentifier_Id
		 AND LOC_LT.CultureId =v_CultureID
        LEFT OUTER JOIN TransactionTypes ST  ON TR.TransactionTypeId = ST.Id
        LEFT OUTER JOIN   Localizations LOC_ST  ON  ST.LocalizationIdentifier_Id =LOC_ST.LocalizationIdentifier_Id
		 AND LOC_ST.CultureId =v_CultureID

               LEFT OUTER JOIN OrgUnits Creator_OrgUnits ON TR.OrgUnitId = Creator_OrgUnits.Id
	    LEFT OUTER JOIN Localizations Loc_CreatorEntityId 
	       ON  Creator_OrgUnits.LocalizationIdentifier_Id = Loc_CreatorEntityId.LocalizationIdentifier_Id
	       AND Loc_CreatorEntityId.CultureId =v_CultureID

		LEFT OUTER JOIN TransactionAssignments TRAssign ON TR.Id = TRAssign.TransactionId
	    LEFT OUTER JOIN OrgUnits OrgUnits_ToEntity ON TRAssign.ToEntityId=OrgUnits_ToEntity.Id
	    LEFT OUTER JOIN  Localizations Loc_ToEntityId 
	       ON  OrgUnits_ToEntity.LocalizationIdentifier_Id=Loc_ToEntityId.LocalizationIdentifier_Id   AND Loc_ToEntityId.CultureId = v_CultureID
	   LEFT OUTER JOIN OrgUnits OrgUnits_FromEntity ON TRAssign.FromEntityId=OrgUnits_FromEntity.Id
	   LEFT OUTER JOIN  Localizations Loc_FromEntityId  ON  OrgUnits_FromEntity.LocalizationIdentifier_Id=Loc_FromEntityId.LocalizationIdentifier_Id 		  AND Loc_FromEntityId.CultureId = v_CultureID
	   LEFT OUTER JOIN	 UserProfiles  UserProfiles_ToUserId ON  TRAssign.ToUserId=UserProfiles_ToUserId.Id
	   LEFT OUTER JOIN  Localizations Loc_ToUserId  ON UserProfiles_ToUserId.LocalizationIdentifier_Id=Loc_ToUserId.LocalizationIdentifier_Id     AND Loc_ToUserId.CultureId = v_CultureID
	   LEFT OUTER JOIN	 UserProfiles  UserProfiles_FromUserId ON  TRAssign.ToUserId=UserProfiles_FromUserId.Id
	   LEFT OUTER JOIN  Localizations Loc_FromUserId  ON UserProfiles_FromUserId.LocalizationIdentifier_Id=Loc_FromUserId.LocalizationIdentifier_Id	  AND Loc_FromUserId.CultureId = v_CultureID
	   LEFT OUTER JOIN ExternalParties EXTPart ON EXTPart.Id= TR.ExternalPartyId 
	   LEFT OUTER JOIN Localizations Loc_External ON EXTPart.Name_Id =	Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=v_CultureID


		Where     ( p_TransactionNumber IS NULL OR TR."NUMBER" = p_TransactionNumber) 
	            AND (p_TransactionTypeId IS NULL OR TR.TransactionTypeId =p_TransactionTypeId)
	       		AND (p_ConfidentialityId  IS NULL OR TR.ConfidentialityId =p_ConfidentialityId)
				AND (p_PriorityId  IS NULL OR TR.PriorityId = p_PriorityId)
				AND (p_LetterTypeId IS NULL OR TR.LetterTypeId =p_LetterTypeId)
			    AND (p_DeliveryMethodId IS NULL OR TR.DeliveryMethodId=p_DeliveryMethodId)
				AND (p_TransactionStatusId IS NULL OR TR.STATUSID=p_TransactionStatusId)
				AND (p_FullName IS NULL OR Names.FirstName =p_FullName)
		        AND (p_CivilID IS NULL OR Names.CivilID =p_CivilID)
				AND (p_MobileNumber IS NULL OR Names.MobileNumber =p_MobileNumber)

	            AND (p_FromOrgUnitId IS NULL OR  TRAssign.FromEntityId =p_FromOrgUnitId)
	            AND (p_FromUserId	IS NULL OR  TRAssign.FromUserId=p_FromUserId )
	            AND (p_ToOrgUnitId	IS NULL OR  TRAssign.ToEntityId =p_ToOrgUnitId)
	          AND (p_ToUserId IS NULL OR  TRAssign.ToUserId = p_ToUserId) 


  AND (p_IsAppointment IS NULL OR 
 (p_IsAppointment  = 1 AND TR.RemindDate is not null )  OR
 ( p_IsAppointment   =  0   AND TR.RemindDate is  null  ))

  AND   (p_AppointmentDate IS NULL OR TR.RemindDate = p_AppointmentDate ) 
  AND	(p_IsForIndividual IS NULL OR TR.IsForIndividual =p_IsForIndividual )
  AND   (p_InboundDateH IS NULL OR TR.InboundDateH=p_InboundDateH	 )    
  AND   (p_ExternalPartyId IS NULL OR TR.ExternalPartyId =p_ExternalPartyId )	
  AND   (p_DocumentNumber IS NULL OR  TR.DocumentNumber =p_DocumentNumber)
  AND   ( p_OutBoundDate IS NULL OR TR.CreatedOn = p_OutBoundDate )
  AND   (p_TransactioDescription IS NULL OR    INSTR(Lower(TR.Subject),Lower( v_Subject)) > 0)
  AND   (p_Remarks IS NULL OR   INSTR(Lower(TR.Remarks), Lower(p_Remarks) )  > 0)

      AND (     (P_LEVEL   =1 AND   TRAssign.ToUserId =p_USER_ID )  
            OR  (P_LEVEL   =2 AND   TRAssign.ToEntityId =P_ENTITY_ID ) 
            OR  (P_LEVEL   =3 AND   TRAssign.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id ))
            OR  (P_LEVEL   =4 AND   TRAssign.ToEntityId IN(SELECT Id FROM OrgUnits ))

            )

     ORDER BY
TR.Id 
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

-------------TOTAL COUNT
SELECT 	  COUNT(TR.Id) INTO p_TotalCount
	FROM     
		Transactions TR INNER JOIN  INSCOPETR TT ON TT.TRANSID = TR.Id
		lEFT OUTER JOIN  TransactionNames TRName   ON TR.Id =TRName.TransactionId
		lEFT OUTER JOIN Names Names  ON TRName.NameId=Names.Id	
		lEFT OUTER JOIN Permissions Perm ON Perm.Id = TR.ConfidentialityId
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm ON LLOC_Perm.Lookup_Id = Perm.Name_Id	  AND LLOC_Perm.Culture_Id = v_CultureID	
		lEFT OUTER JOIN Priorities PR ON PR.Id = TR.PriorityId	
		LEFT OUTER JOIN  Localizations LOC_PR ON LOC_PR.LocalizationIdentifier_Id = PR.LocalizationIdentifier_Id     AND LOC_PR.CultureId = v_CultureID	
		lEFT OUTER JOIN LookupLocalizations llOC_Delivery ON llOC_Delivery.Lookup_Id = TR.DeliveryMethodId    AND llOC_Delivery.Culture_Id =v_CultureID	
        lEFT OUTER JOIN LookupLocalizations llOC_TransactionCategory ON TR.TransactionCategoryId = llOC_TransactionCategory.Lookup_Id    	AND llOC_TransactionCategory.Culture_Id =v_CultureID
		LEFT OUTER JOIN LetterTypes LT ON  LT.Id= TR.LetterTypeId
        LEFT OUTER JOIN   Localizations LOC_LT  ON  LT.LocalizationIdentifier_Id=LOC_LT.LocalizationIdentifier_Id	 AND LOC_LT.CultureId =v_CultureID
        LEFT OUTER JOIN TransactionTypes ST  ON TR.TransactionTypeId = ST.Id
        LEFT OUTER JOIN   Localizations LOC_ST  ON  ST.LocalizationIdentifier_Id =LOC_ST.LocalizationIdentifier_Id  AND LOC_ST.CultureId =v_CultureID
        LEFT OUTER JOIN OrgUnits Creator_OrgUnits ON TR.OrgUnitId = Creator_OrgUnits.Id
	    LEFT OUTER JOIN Localizations Loc_CreatorEntityId 
	       ON  Creator_OrgUnits.LocalizationIdentifier_Id = Loc_CreatorEntityId.LocalizationIdentifier_Id
	       AND Loc_CreatorEntityId.CultureId =v_CultureID
        LEFT OUTER JOIN TransactionAssignments TRAssign ON TR.Id = TRAssign.TransactionId
	    LEFT OUTER JOIN OrgUnits OrgUnits_ToEntity ON TRAssign.ToEntityId=OrgUnits_ToEntity.Id
	    LEFT OUTER JOIN  Localizations Loc_ToEntityId       ON  OrgUnits_ToEntity.LocalizationIdentifier_Id=Loc_ToEntityId.LocalizationIdentifier_Id   AND Loc_ToEntityId.CultureId = v_CultureID
	    LEFT OUTER JOIN OrgUnits OrgUnits_FromEntity ON TRAssign.FromEntityId=OrgUnits_FromEntity.Id
	    LEFT OUTER JOIN  Localizations Loc_FromEntityId  ON  OrgUnits_FromEntity.LocalizationIdentifier_Id=Loc_FromEntityId.LocalizationIdentifier_Id 		  AND Loc_FromEntityId.CultureId = v_CultureID
	    LEFT OUTER JOIN	 UserProfiles  UserProfiles_ToUserId ON  TRAssign.ToUserId=UserProfiles_ToUserId.Id
	    LEFT OUTER JOIN  Localizations Loc_ToUserId  ON UserProfiles_ToUserId.LocalizationIdentifier_Id=Loc_ToUserId.LocalizationIdentifier_Id     AND Loc_ToUserId.CultureId = v_CultureID
	    LEFT OUTER JOIN	 UserProfiles  UserProfiles_FromUserId ON  TRAssign.ToUserId=UserProfiles_FromUserId.Id
	    LEFT OUTER JOIN  Localizations Loc_FromUserId  ON UserProfiles_FromUserId.LocalizationIdentifier_Id=Loc_FromUserId.LocalizationIdentifier_Id	  AND Loc_FromUserId.CultureId = v_CultureID
	    LEFT OUTER JOIN ExternalParties EXTPart ON EXTPart.Id= TR.ExternalPartyId 
	    LEFT OUTER JOIN Localizations Loc_External ON EXTPart.Name_Id =	Loc_External.LocalizationIdentifier_Id and Loc_External.CultureId=v_CultureID

		WHERE     ( p_TransactionNumber IS NULL OR TR."NUMBER" = p_TransactionNumber) 
   AND (p_TransactionTypeId IS NULL OR TR.TransactionTypeId=p_TransactionTypeId)
   AND (p_ConfidentialityId  IS NULL OR TR.ConfidentialityId =p_ConfidentialityId)
   AND (p_PriorityId  IS NULL OR TR.PriorityId = p_PriorityId)
   AND (p_LetterTypeId IS NULL OR TR.LetterTypeId =p_LetterTypeId)
   AND (p_DeliveryMethodId IS NULL OR TR.DeliveryMethodId=p_DeliveryMethodId)
   AND (p_TransactionStatusId IS NULL OR TR.STATUSID=p_TransactionStatusId)
   AND (p_FullName IS NULL OR Names.FirstName =p_FullName)
   AND (p_CivilID IS NULL OR Names.CivilID =p_CivilID)
   AND (p_MobileNumber IS NULL OR Names.MobileNumber =p_MobileNumber)
   AND (p_FromOrgUnitId IS NULL OR  TRAssign.FromEntityId =p_FromOrgUnitId)
   AND (p_FromUserId	IS NULL OR  TRAssign.FromUserId=p_FromUserId )
   AND (p_ToOrgUnitId	IS NULL OR  TRAssign.ToEntityId =p_ToOrgUnitId)
   AND (p_ToUserId IS NULL OR  TRAssign.ToUserId = p_ToUserId) 
   AND (p_IsAppointment IS NULL OR 
        (p_IsAppointment  = 1 AND TR.RemindDate is not null )  OR
        ( p_IsAppointment   =  0   AND TR.RemindDate is  null  ))
    AND (p_AppointmentDate IS NULL OR TR.RemindDate = p_AppointmentDate ) 
    AND	(  p_IsForIndividual IS NULL OR TR.IsForIndividual =p_IsForIndividual )
    AND  (p_InboundDateH IS NULL OR TR.InboundDateH=p_InboundDateH	 )    
    AND (p_ExternalPartyId IS NULL OR TR.ExternalPartyId =p_ExternalPartyId )	
    AND (p_DocumentNumber IS NULL OR  TR.DocumentNumber =p_DocumentNumber)
    AND ( p_OutBoundDate IS NULL OR TR.CreatedOn = p_OutBoundDate )
    AND  (p_TransactioDescription IS NULL OR    INSTR(Lower(TR.Subject),Lower( v_Subject)) > 0)
    AND (p_Remarks IS NULL OR   INSTR(Lower(TR.Remarks), Lower(p_Remarks) )  > 0)
       AND (     (P_LEVEL   =1 AND   TRAssign.ToUserId =P_USER_ID )  
            OR  (P_LEVEL   =2  AND   TRAssign.ToEntityId =P_ENTITY_ID ) 
            OR  (P_LEVEL   =3  AND   TRAssign.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id ))
            OR  (P_LEVEL   =4  AND   TRAssign.ToEntityId IN(SELECT Id FROM OrgUnits))
        );

        DELETE FROM INSCOPETR ;
END ;
/
-----------------REPORT_STATISTICALS-----------------------
create or replace PROCEDURE REPORT_STATISTICALS(
    P_ReportType  IN "NUMBER" DEFAULT NULL ,
    P_FromDate    IN "DATE" DEFAULT NULL ,
    P_ToDate  IN "DATE" DEFAULT NULL ,
    P_EntitID IN "NUMBER" DEFAULT NULL ,
    P_UserID  IN "NUMBER" DEFAULT NULL ,
    P_level   IN "NUMBER" DEFAULT NULL ,
    P_LetterTypeId  IN "NUMBER" DEFAULT NULL ,
    P_AppointmentDate   IN "DATE" DEFAULT NULL ,
    P_ConfidentialityId    IN "NUMBER" DEFAULT NULL ,
    P_PriorityId  IN "NUMBER" DEFAULT NULL ,
    P_TransactionTypeId IN "NUMBER" DEFAULT NULL ,
    P_DeliveryMethodId  IN "NUMBER" DEFAULT NULL ,
    P_Remarks               NVARCHAR2 ,
    p_PageIndex				Number,
    p_PageSize	           	Number,
    p_Status               	Number,
    p_Inbound             	Number,
    p_Outbound            	Number,
    p_Draft                 Number,
    p_Internal              Number,
	p_TotalCount			OUT Number,
    cv_1 OUT SYS_REFCURSOR )
AS
  v_OutboundCount   NUMBER(10,0);
  v_OutboundDraftCountCreated    NUMBER(10,0);
  v_OutboundDraftCountAssigned   NUMBER(10,0);
  v_InboundCountCreated   NUMBER(10,0);
  v_InboundCountAssigned  NUMBER(10,0);
  v_InternalOutboundCountCreated NUMBER(10,0);
  v_InternalOutboundCountAssigne NUMBER(10,0);
  v_DelayedCount    NUMBER(10,0);
BEGIN

  IF P_level = 1 THEN
    BEGIN
    --عدد معاملات الصادر الخارجي
SELECT
COUNT(DISTINCT Transactions.Id) INTO v_OutboundCount
FROM
Transactions
inner join lookups on lookups.id = StatusId
WHERE
TransactionCategoryId = p_Outbound
AND(Transactions.CreatedBy = P_UserID)
AND(transactions.orgunitid = P_EntitID)
AND "DATE" BETWEEN P_FromDate AND P_ToDate;

    --عدد معاملات مسودة الخطاب المنشئة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Draft
AND(Transactions.CreatedBy = P_UserID)
AND(transactions.orgunitid = P_EntitID)
AND "DATE" BETWEEN P_FromDate AND P_ToDate;

    --عدد معاملات مسودة الخطاب المحالة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_OutboundDraftCountAssigned
FROM Transactions
inner join lookups on lookups.id = StatusId
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id AND TransactionAssignmentHistories.ToUserId = P_UserID AND transactionassignmenthistories.toentityid = P_EntitID
WHERE 
TransactionCategoryId = P_Draft
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
AND lookups.enumreference not in (2,12);

    --عدد معاملات الوارد الخارجي المنشئة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Inbound
AND(Transactions.CreatedBy = P_UserID)
AND(transactions.orgunitid = P_EntitID)
AND "DATE" BETWEEN P_FromDate AND P_ToDate;

    --عدد معاملات الوارد الخارجي المحالة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InboundCountAssigned
FROM Transactions
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
inner join lookups on lookups.id = StatusId
AND TransactionAssignmentHistories.ToUserId = P_UserID
AND transactionassignmenthistories.toentityid = P_EntitID
WHERE TransactionCategoryId = P_Inbound
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
AND lookups.enumreference not in (2,12);

    --عدد معاملات المعاملة الداخلية المنشئة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountCreated
FROM Transactions
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId = P_Internal
AND(Transactions.CreatedBy = P_UserID)
AND(transactions.orgunitid = P_EntitID)
AND "DATE" BETWEEN P_FromDate AND P_ToDate;

    --عدد معاملات المعاملة الداخلية المحالة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_InternalOutboundCountAssigne
FROM Transactions
INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
inner join lookups on lookups.id = StatusId
AND TransactionAssignmentHistories.ToUserId = P_UserID
AND transactionassignmenthistories.toentityid = P_EntitID
WHERE TransactionCategoryId = P_Internal
AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
AND lookups.enumreference not in (2,12);

    --عدد المعاملات المتأخرة
SELECT COUNT(DISTINCT Transactions.Id)
INTO v_DelayedCount
FROM Transactions
INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
inner join lookups on lookups.id = StatusId
WHERE TransactionCategoryId <> P_Outbound
AND TransactionCategoryId <> p_Draft
AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
AND(TransactionAssignments.ToUserId = P_UserID)
AND lookups.enumreference not in (2,12);


    INSERT INTO TT_REPORT_DATA VALUES
      (
         P_EntitID,
         P_UserID,
         v_OutboundCount,
         v_OutboundDraftCountCreated,
         v_OutboundDraftCountAssigned,
         v_InboundCountCreated,
         v_InboundCountAssigned,
         v_InternalOutboundCountCreated,
         v_InternalOutboundCountAssigne,
         v_DelayedCount
      );
  END;
  END IF;

  IF P_level = 2 THEN
  BEGIN

    INSERT INTO TT_REPORT_DATA
      (
        OrgUnitsID,
        UserProfilesID
      )
    SELECT 
      OrgUnit_Id ,
      UserProfile_Id
    FROM 
      UserProfileOrgUnits
      JOIN UserProfiles ON UserProfileOrgUnits.UserProfile_Id = UserProfiles.Id
      AND UserProfiles.IsActive  = 1
      JOIN OrgUnits ON UserProfileOrgUnits.OrgUnit_Id = OrgUnits.Id
      AND OrgUnits.IsActive  = 1
    WHERE 
      OrgUnit_Id = P_EntitID
      AND (P_UserID = -1 or UserProfile_Id = P_UserID);

  --عدد معاملات الصادر الخارجي
  MERGE INTO TT_REPORT_DATA DA USING
  (
    SELECT DISTINCT
      DA.ROWID row_id,
      CO
    FROM 
      TT_REPORT_DATA ,
      TT_REPORT_DATA DA
    JOIN
   (
    SELECT 
      Transactions.OrgUnitId ,
      Transactions.CreatedBy ,
      COUNT(1) CO
    FROM 
      Transactions
    inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = P_Outbound
        AND(OrgUnitId = P_EntitID)
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY 
      Transactions.OrgUnitId,Transactions.CreatedBy
   ) T ON DA.OrgUnitsID = T.OrgUnitId
    AND DA.UserProfilesID  = T.CreatedBy
  )
  src ON ( DA.ROWID = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET OutboundCount = CO;


  --عدد معاملات مسودة الخطاب المنشئة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT Transactions.OrgUnitId ,
            Transactions.CreatedBy ,
            COUNT(1) CO
    FROM Transactions
    inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Draft
        AND(OrgUnitId = P_EntitID)
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY Transactions.OrgUnitId, Transactions.CreatedBy
    ) T ON DA.OrgUnitsID = T.OrgUnitId
  AND DA.UserProfilesID  = T.CreatedBy
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET OutboundDraftCountCreated = CO;
  --عدد معاملات مسودة الخطاب المحالة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT TransactionAssignmentHistories.ToEntityId ,
            TransactionAssignmentHistories.ToUserId ,
   COUNT(1) CO
    FROM Transactions
    inner join lookups on lookups.id = StatusId
    INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
                    AND TransactionAssignmentHistories.ToEntityId = P_EntitID
    WHERE TransactionCategoryId = p_Draft
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
    GROUP BY TransactionAssignmentHistories.ToEntityId,TransactionAssignmentHistories.ToUserId
    ) T ON Da.OrgUnitsID = T.ToEntityId
  AND DA.UserProfilesID  = T.ToUserId
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET OutboundDraftCountAssigned = CO;

  --عدد معاملات الوارد الخارجي المنشئة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT Transactions.OrgUnitId ,
   Transactions.CreatedBy ,
   COUNT(1) CO
    FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Inbound
        AND(OrgUnitId = P_EntitID)
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY Transactions.OrgUnitId,
   Transactions.CreatedBy
    ) T ON DA.OrgUnitsID = T.OrgUnitId
  AND DA.UserProfilesID  = T.CreatedBy
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET InboundCountCreated = CO;
  --عدد معاملات الوارد الخارجي المحالة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT TransactionAssignmentHistories.ToEntityId ,
            TransactionAssignmentHistories.ToUserId ,
   COUNT(1) CO
    FROM Transactions
            inner join lookups on lookups.id = StatusId
            INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
                    AND TransactionAssignmentHistories.ToEntityId = P_EntitID
    WHERE TransactionCategoryId = p_Inbound
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
    GROUP BY TransactionAssignmentHistories.ToEntityId, TransactionAssignmentHistories.ToUserId
    ) T ON Da.OrgUnitsID = T.ToEntityId
  AND DA.UserProfilesID  = T.ToUserId
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET InboundCountAssigned = CO;
  --عدد معاملات المعاملة الداخلية المنشئة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT Transactions.OrgUnitId ,
   Transactions.CreatedBy ,
   COUNT(1) CO
    FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Internal
        AND(OrgUnitId = P_EntitID)
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY Transactions.OrgUnitId,
   Transactions.CreatedBy
    ) T ON Da.OrgUnitsID = T.OrgUnitId
  AND DA.UserProfilesID  = T.CreatedBy
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET InternalOutboundCountCreated = CO;
  --عدد معاملات المعاملة الداخلية المحالة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT TransactionAssignmentHistories.ToEntityId ,
            TransactionAssignmentHistories.ToUserId ,
   COUNT(1) CO
    FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
            AND TransactionAssignmentHistories.ToEntityId = P_EntitID
    WHERE TransactionCategoryId = p_Internal
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
    GROUP BY TransactionAssignmentHistories.ToEntityId, TransactionAssignmentHistories.ToUserId
    ) T ON Da.OrgUnitsID = T.ToEntityId
  AND DA.UserProfilesID  = T.ToUserId
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET InternalOutboundCountAssigned = CO;
  --عدد المعاملات المتأخرة
  MERGE INTO TT_REPORT_DATA DA USING
  (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT TransactionAssignments.ToEntityId ,
   TransactionAssignments.ToUserId ,
   COUNT(1) CO
    FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
        --INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
        --AND TransactionAssignmentHistories.ToEntityId = P_ENTITY_ID
        INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
    WHERE TransactionCategoryId <> p_Outbound
        AND TransactionCategoryId <> p_Draft
        AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
        AND(transactionassignments.toentityid = P_EntitID)
        AND lookups.enumreference not in (2,12)
    GROUP BY TransactionAssignments.ToEntityId,
   TransactionAssignments.ToUserId
    ) T ON Da.OrgUnitsID = T.ToEntityId
  AND DA.UserProfilesID  = T.ToUserId
  ) src ON ( DA.ROWID    = src.row_id )
    WHEN MATCHED THEN
  UPDATE SET DelayedCount = CO;
    END;
  END IF;

IF P_level = 3 THEN
    BEGIN

    INSERT
    INTO TT_REPORT_DATA
  (
    OrgUnitsID,
    UserProfilesID
  )
  (SELECT OrgUnit_Id ,
   UserProfile_Id
    FROM UserProfileOrgUnits
    JOIN UserProfiles
    ON UserProfileOrgUnits.UserProfile_Id = UserProfiles.Id
    AND UserProfiles.IsActive  = 1
    JOIN OrgUnits
    ON UserProfileOrgUnits.OrgUnit_Id = OrgUnits.Id
    AND OrgUnits.IsActive  = 1
    AND(OrgUnits.Id IN( SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
    AND (P_UserID = -1 or UserProfileOrgUnits.UserProfile_Id = P_UserID)
  ) ;


 --   عدد معاملات الصادر الخارجي
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT Transactions.OrgUnitId ,
            Transactions.CreatedBy ,
            COUNT(1) CO
    FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Outbound
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY 
      Transactions.OrgUnitId,Transactions.CreatedBy
   ) T ON DA.OrgUnitsID = T.OrgUnitId
    AND DA.UserProfilesID  = T.CreatedBy
  )
  src ON ( DA.ROWID = src.row_id )
    WHEN MATCHED THEN
  UPDATE
  SET OutboundCount = CO;


    --عدد معاملات مسودة الخطاب المنشئة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
        TT_REPORT_DATA DA
    JOIN
  (SELECT Transactions.OrgUnitId ,
          Transactions.CreatedBy ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Draft
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY 
      Transactions.OrgUnitId,Transactions.CreatedBy
   ) T ON DA.OrgUnitsID = T.OrgUnitId
    AND DA.UserProfilesID  = T.CreatedBy
  )
  src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET OutboundDraftCountCreated = CO;



    --عدد معاملات مسودة الخطاب المحالة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignmentHistories.ToEntityId ,
        TransactionAssignmentHistories.ToUserId ,
    COUNT(1) CO
  FROM Transactions
    inner join lookups on lookups.id = StatusId
    INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId = p_Draft
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignmentHistories.ToEntityId,
    TransactionAssignmentHistories.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET OutboundDraftCountAssigned = CO;


    --عدد معاملات الوارد الخارجي المنشئة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT Transactions.OrgUnitId ,
          Transactions.CreatedBy ,
          COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Inbound
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY 
      Transactions.OrgUnitId,Transactions.CreatedBy
   ) T ON DA.OrgUnitsID = T.OrgUnitId
    AND DA.UserProfilesID  = T.CreatedBy
  )
  src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InboundCountCreated = CO;


    --عدد معاملات الوارد الخارجي المحالة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT  DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignmentHistories.ToEntityId ,
          TransactionAssignmentHistories.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId = p_Inbound
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignmentHistories.ToEntityId,
    TransactionAssignmentHistories.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId = DA.UserProfilesID
    ) src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InboundCountAssigned = CO;


    --عدد معاملات المعاملة الداخلية المنشئة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT Transactions.OrgUnitId ,
          Transactions.CreatedBy ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Internal
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY 
      Transactions.OrgUnitId,Transactions.CreatedBy
   ) T ON DA.OrgUnitsID = T.OrgUnitId
    AND DA.UserProfilesID  = T.CreatedBy
  )
  src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InternalOutboundCountCreated = CO;


    --عدد معاملات المعاملة الداخلية المحالة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignmentHistories.ToEntityId ,
    TransactionAssignmentHistories.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId = p_Internal
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignmentHistories.ToEntityId,
    TransactionAssignmentHistories.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InternalOutboundCountAssigned = CO; 
    --عدد المعاملات المتأخرة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignments.ToEntityId ,
            TransactionAssignments.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
        INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
        --INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId <> p_Outbound
        AND TransactionCategoryId <> p_Draft
        AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
        --AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
        AND(transactionassignments.toentityid IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignments.ToEntityId,
    TransactionAssignments.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID   = src.row_id )
  WHEN MATCHED THEN
    UPDATE SET DelayedCount = CO;
  END;
END IF;


IF P_level = 4 THEN
  BEGIN

   INSERT
    INTO TT_REPORT_DATA
  (
    OrgUnitsID,
    UserProfilesID
  )
  (SELECT OrgUnit_Id ,
   UserProfile_Id
    FROM UserProfileOrgUnits
    JOIN UserProfiles
    ON UserProfileOrgUnits.UserProfile_Id = UserProfiles.Id
    AND UserProfiles.IsActive  = 1
    JOIN OrgUnits
    ON UserProfileOrgUnits.OrgUnit_Id = OrgUnits.Id
    AND OrgUnits.IsActive  = 1
    AND(OrgUnits.Id IN( SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
    AND (P_UserID = -1 or UserProfileOrgUnits.UserProfile_Id = P_UserID)
  ) ;
    --عدد معاملات الصادر الخارجي
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
    CO
  FROM TT_REPORT_DATA ,
    TT_REPORT_DATA DA
  JOIN
    (SELECT Transactions.OrgUnitId ,
   Transactions.CreatedBy ,
   COUNT(*) CO
    FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Outbound
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
    GROUP BY Transactions.OrgUnitId, Transactions.CreatedBy
    ) T ON T.OrgUnitId = Da.OrgUnitsID
  AND T.CreatedBy  = DA.UserProfilesID
    )
    src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET OutboundCount = CO;
    --عدد معاملات مسودة الخطاب المنشئة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT Transactions.OrgUnitId ,
    Transactions.CreatedBy ,
    COUNT(*) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Draft
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
  GROUP BY Transactions.OrgUnitId,
    Transactions.CreatedBy
  ) T ON T.OrgUnitId = Da.OrgUnitsID
    AND T.CreatedBy  = DA.UserProfilesID
    ) src ON ( DA.ROWID  = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET OutboundDraftCountCreated = CO;
    --عدد معاملات مسودة الخطاب المحالة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignmentHistories.ToEntityId ,
    TransactionAssignmentHistories.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId = p_Draft
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignmentHistories.ToEntityId,
    TransactionAssignmentHistories.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET OutboundDraftCountAssigned = CO;
    --عدد معاملات الوارد الخارجي المنشئة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT Transactions.OrgUnitId ,
    Transactions.CreatedBy ,
    COUNT(*) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Inbound
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
  GROUP BY Transactions.OrgUnitId,
    Transactions.CreatedBy
  ) T ON T.OrgUnitId = Da.OrgUnitsID
    AND T.CreatedBy  = DA.UserProfilesID
    ) src ON ( DA.ROWID  = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InboundCountCreated = CO;
    --عدد معاملات الوارد الخارجي المحالة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignmentHistories.ToEntityId ,
    TransactionAssignmentHistories.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId = p_Inbound
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignmentHistories.ToEntityId,
    TransactionAssignmentHistories.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InboundCountAssigned = CO;
    --عدد معاملات المعاملة الداخلية المنشئة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT Transactions.OrgUnitId ,
    Transactions.CreatedBy ,
    COUNT(*) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
    WHERE TransactionCategoryId = p_Internal
        AND(OrgUnitId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND "DATE" BETWEEN P_FromDate AND P_ToDate
  GROUP BY Transactions.OrgUnitId,
    Transactions.CreatedBy
  ) T ON T.OrgUnitId = Da.OrgUnitsID
    AND T.CreatedBy  = DA.UserProfilesID
    ) src ON ( DA.ROWID  = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InternalOutboundCountCreated = CO;

    --عدد معاملات المعاملة الداخلية المحالة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignmentHistories.ToEntityId ,
    TransactionAssignmentHistories.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId = P_Internal
        AND (transactionassignmenthistories.touserid != transactionassignmenthistories.fromuserid OR transactionassignmenthistories.toentityid != transactionassignmenthistories.fromentityid)
        AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND TransactionAssignmentHistories."DATE" BETWEEN P_FromDate AND P_ToDate
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignmentHistories.ToEntityId,
    TransactionAssignmentHistories.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET InternalOutboundCountAssigned = CO;

    --عدد المعاملات المتأخرة
    MERGE INTO TT_REPORT_DATA DA USING
    (SELECT DISTINCT DA.ROWID row_id,
  CO
    FROM TT_REPORT_DATA ,
  TT_REPORT_DATA DA
    JOIN
  (SELECT TransactionAssignments.ToEntityId ,
    TransactionAssignments.ToUserId ,
    COUNT(1) CO
  FROM Transactions
        inner join lookups on lookups.id = StatusId
        INNER JOIN TransactionAssignments ON Transactions.Id = TransactionAssignments.TransactionId
        INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
        --INNER JOIN TransactionAssignmentHistories ON TransactionAssignmentHistories.TransactionId = Transactions.Id
    WHERE TransactionCategoryId <> p_Outbound
        AND TransactionCategoryId <> p_Draft
        AND(RemindDate < SYSDATE OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
        --AND(TransactionAssignmentHistories.ToEntityId IN(SELECT Id FROM OrgUnits START WITH Id = P_ENTITY_ID CONNECT BY ParentId = PRIOR Id))
        AND(transactionassignments.toentityid IN(SELECT Id FROM OrgUnits START WITH Id = P_EntitID CONNECT BY ParentId = PRIOR Id))
        AND lookups.enumreference not in (2,12)
  GROUP BY TransactionAssignments.ToEntityId,
    TransactionAssignments.ToUserId
  ) T ON T.ToEntityId = Da.OrgUnitsID
    AND T.ToUserId    = DA.UserProfilesID
    ) src ON ( DA.ROWID   = src.row_id )
  WHEN MATCHED THEN
    UPDATE
    SET DelayedCount = CO;
  END;
END IF;

IF P_ReportType = 1 THEN
  BEGIN
    OPEN cv_1 FOR SELECT OrgUnitsID ,
    OrgUnits_VW.Name OrgUnitName ,
    NULL AS UserProfilesID ,
    NULL AS UserProfileName ,
    NVL(OutboundCount, 0) OutboundCount ,
    NVL(OutboundDraftCountCreated, 0) OutboundDraftCountCreated ,
    NVL(OutboundDraftCountAssigned, 0) OutboundDraftCountAssigned ,
    NVL(InboundCountCreated, 0) InboundCountCreated ,
    NVL(InboundCountAssigned, 0) InboundCountAssigned ,
    NVL(InternalOutboundCountCreated, 0) InternalOutboundCountCreated ,
    NVL(InternalOutboundCountAssigned, 0) InternalOutboundCountAssigned ,
    NVL(DelayedCount, 0) DelayedCount FROM
    (SELECT OrgUnitsID ,
  SUM(OutboundCount) OutboundCount ,
  SUM(OutboundDraftCountCreated) OutboundDraftCountCreated ,
  SUM(OutboundDraftCountAssigned) OutboundDraftCountAssigned ,
  SUM(InboundCountCreated) InboundCountCreated ,
  SUM(InboundCountAssigned) InboundCountAssigned ,
  SUM(InternalOutboundCountCreated) InternalOutboundCountCreated ,
  SUM(InternalOutboundCountAssigned) InternalOutboundCountAssigned ,
  SUM(DelayedCount)  DelayedCount
    FROM TT_REPORT_DATA DA
    GROUP BY Da.OrgUnitsID
    ) t JOIN OrgUnits_VW ON T.OrgUnitsID = OrgUnits_VW.Id ;
  END;
ELSE
  BEGIN
    OPEN cv_1 FOR SELECT OrgUnitsID ,
    OrgUnits_VW.Name OrgUnitName ,
    UserProfilesID ,
    UserProfiles_VW.Name UserProfileName ,
    NVL(OutboundCount, 0) OutboundCount ,
    NVL(OutboundDraftCountCreated, 0) OutboundDraftCountCreated ,
    NVL(OutboundDraftCountAssigned, 0) OutboundDraftCountAssigned ,
    NVL(InboundCountCreated, 0) InboundCountCreated ,
    NVL(InboundCountAssigned, 0) InboundCountAssigned ,
    NVL(InternalOutboundCountCreated, 0) InternalOutboundCountCreated ,
    NVL(InternalOutboundCountAssigned, 0) InternalOutboundCountAssigned ,
    NVL(DelayedCount, 0) DelayedCount FROM TT_REPORT_DATA DA JOIN OrgUnits_VW ON 
    Da.OrgUnitsID = OrgUnits_VW.Id JOIN UserProfiles_VW ON DA.UserProfilesID = UserProfiles_VW.Id 
    AND DA.OrgUnitsID = UserProfiles_VW.ENTITY_ID
   ORDER BY
    OrgUnitsID 
    OFFSET p_PageIndex * p_PageSize ROWS
    FETCH NEXT p_PageSize ROWS ONLY;


  SELECT COUNT(1) INTO p_TotalCount  
  FROM TT_REPORT_DATA DA  ;

  END;
END IF;

DELETE FROM TT_REPORT_DATA;
END;

/
CREATE GLOBAL TEMPORARY TABLE AUDITPROPERTIES 
   (	PROPERTYNAME VARCHAR2(32 BYTE) NOT NULL ENABLE
   ) ON COMMIT PRESERVE ROWS ;
/
create or replace PROCEDURE GET_MAINDATA_AUDITDETAILS 
(
p_AuditId       NUMBER,
 p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
--INSERT INTO AuditProperties  VALUES('Date');
INSERT INTO AuditProperties  VALUES('DateH');
INSERT INTO AuditProperties  VALUES('Number');
INSERT INTO AuditProperties  VALUES('IsForIndividual');
--INSERT INTO AuditProperties  VALUES('PrintedDeliveryReport');
--INSERT INTO AuditProperties  VALUES('DocumentNumber');
INSERT INTO AuditProperties  VALUES('Subject');
INSERT INTO AuditProperties  VALUES('InboundDateH');
INSERT INTO AuditProperties  VALUES('Remarks');

 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId

UNION 
--- PriorityId
SELECT 
        AD.PropertyName,
        LOC_PR_OLD."TEXT"    PropertyOldValue,
        LOC_PR_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 
FROM AuditDetails AD lEFT OUTER JOIN Priorities PR_OLD ON PR_OLD.Id = AD.PropertyOldValue 
		LEFT OUTER JOIN  Localizations LOC_PR_OLD ON LOC_PR_OLD.LocalizationIdentifier_Id = PR_OLD.LocalizationIdentifier_Id
        AND LOC_PR_OLD.CultureId = v_CultureID	
lEFT OUTER JOIN Priorities PR_NEW ON PR_NEW.Id = AD.PropertyNewValue 
		LEFT OUTER JOIN  Localizations LOC_PR_NEW ON LOC_PR_NEW.LocalizationIdentifier_Id = PR_NEW.LocalizationIdentifier_Id
        AND LOC_PR_NEW.CultureId = v_CultureID	
WHERE  AD.PropertyName = 'PriorityId'
AND    AD.Audit_Id =p_AuditId

UNION 

-------------ConfidentialityId ------------

SELECT 
        AD.PropertyName,
        LLOC_Perm_OLD."TEXT"    PropertyOldValue,
        LLOC_Perm_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD lEFT OUTER JOIN Permissions Perm_OLD ON Perm_OLD.Id =  AD.PropertyOldValue 
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm_OLD ON LLOC_Perm_OLD.Lookup_Id = Perm_OLD.Name_Id	
           AND LLOC_Perm_OLD.Culture_Id = v_CultureID	 

lEFT OUTER JOIN Permissions Perm_NEW ON Perm_NEW.Id =  AD.PropertyNewValue 
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm_NEW ON LLOC_Perm_NEW.Lookup_Id = Perm_NEW.Name_Id	
           AND LLOC_Perm_NEW.Culture_Id = v_CultureID	

WHERE  AD.PropertyName = 'ConfidentialityId'
AND    AD.Audit_Id =p_AuditId

UNION 
--------ExternalPartyId
SELECT 
        AD.PropertyName,
        Loc_External_OLD."TEXT"    PropertyOldValue,
        Loc_External_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD LEFT OUTER JOIN ExternalParties EXTPart_OLD ON EXTPart_OLD.Id= AD.PropertyOldValue 
	   LEFT OUTER JOIN Localizations Loc_External_OLD ON EXTPart_OLD.Name_Id =	Loc_External_OLD.LocalizationIdentifier_Id 
     AND Loc_External_OLD.CultureId=v_CultureID

       LEFT OUTER JOIN ExternalParties EXTPart_NEW ON EXTPart_NEW.Id= AD.PropertyNewValue 
	   LEFT OUTER JOIN Localizations Loc_External_NEW ON EXTPart_NEW.Name_Id =	Loc_External_NEW.LocalizationIdentifier_Id 
      AND Loc_External_NEW.CultureId=v_CultureID

WHERE  AD.PropertyName = 'ExternalPartyId'
AND    AD.Audit_Id =p_AuditId

UNION

--------DeliveryMethodId

SELECT 
        AD.PropertyName,
        llOC_Delivery_OLD."TEXT"       PropertyOldValue,
        llOC_Delivery_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD

        lEFT OUTER JOIN LookupLocalizations llOC_Delivery_OLD ON llOC_Delivery_OLD.Lookup_Id =  AD.PropertyOldValue 
		AND llOC_Delivery_OLD.Culture_Id =v_CultureID	

          lEFT OUTER JOIN LookupLocalizations llOC_Delivery_NEW ON llOC_Delivery_NEW.Lookup_Id =  AD.PropertyOldValue 
		  AND llOC_Delivery_NEW.Culture_Id =v_CultureID	

WHERE  AD.PropertyName = 'DeliveryMethodId'
AND    AD.Audit_Id =p_AuditId

UNION 
-------UserId

SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE  AD.PropertyName = 'UserId'
AND    AD.Audit_Id =p_AuditId
UNION

---------ToUSERID--
SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE       AD.PropertyName = 'ToUserId'
     AND    AD.Audit_Id     =   p_AuditId

UNION
----SOURCE Type
SELECT 
        AD.PropertyName,
        LOC_SourceType_OLD."TEXT" PropertyOldValue,
        LOC_SourceType_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD  
LEFT JOIN TransactionTypes ST_OLD ON ST_OLD.Id = AD.PropertyOldValue 
LEFT JOIN Localizations LOC_SourceType_OLD ON LOC_SourceType_OLD.LocalizationIdentifier_Id = ST_OLD.LocalizationIdentifier_Id 
--AND LOC_SourceType_OLD.CultureId = V_CultureID

LEFT JOIN TransactionTypes ST_NEW  ON ST_NEW.Id = AD.PropertyNewValue 
LEFT JOIN Localizations LOC_SourceType_NEW ON LOC_SourceType_NEW.LocalizationIdentifier_Id = ST_NEW.LocalizationIdentifier_Id 
AND LOC_SourceType_NEW.CultureId = V_CultureID

WHERE  AD.PropertyName = 'SourceTypeId'
AND    AD.Audit_Id =p_AuditId


--------------LetterTypes

UNION

SELECT 
        AD.PropertyName,
        LOC_LT_OLD."TEXT"  PropertyOldValue,
        LOC_LT_NEW."TEXT"  PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD  
   LEFT OUTER JOIN LetterTypes LT_NEW ON  LT_NEW.Id= AD.PropertyNewValue
         LEFT OUTER JOIN   Localizations LOC_LT_NEW  ON  LT_NEW.LocalizationIdentifier_Id=LOC_LT_NEW.LocalizationIdentifier_Id
		 AND LOC_LT_NEW.CultureId =1

  LEFT OUTER JOIN LetterTypes LT_OLD ON  LT_OLD.Id= AD.PropertyOldValue
         LEFT OUTER JOIN   Localizations LOC_LT_OLD  ON  LT_OLD.LocalizationIdentifier_Id=LOC_LT_OLD.LocalizationIdentifier_Id
		 AND LOC_LT_OLD.CultureId =1
WHERE  AD.PropertyName = 'LetterTypeId'
AND    AD.Audit_Id =p_AuditId
--------------------




----Entity ID

UNION
SELECT 
        AD.PropertyName,
        Loc_Org_OLD."TEXT"    PropertyOldValue,
        Loc_Org_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD LEFT OUTER JOIN orgunits ORGPart_OLD ON ORGPart_OLD.Id= AD.PropertyOldValue 
	   LEFT OUTER JOIN Localizations Loc_Org_OLD ON ORGpart_old.localizationidentifier_id =	Loc_Org_OLD.LocalizationIdentifier_Id 
    AND Loc_Org_OLD.CultureId=1

       LEFT OUTER JOIN orgunits ORGPart_NEW ON ORGPart_NEW.Id= AD.PropertyNewValue 
	   LEFT OUTER JOIN Localizations Loc_Org_NEW ON ORGPart_NEW.localizationidentifier_id =	Loc_Org_NEW.LocalizationIdentifier_Id 
      AND Loc_Org_NEW.CultureId=1

WHERE  AD.PropertyName = 'EntityId'
AND    AD.Audit_Id =p_AuditId;

END GET_MAINDATA_AUDITDETAILS;


/
create or replace PROCEDURE GET_TransactionAssignment_AUDITDETAILS 
(
 p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
INSERT INTO AuditProperties  VALUES('Date');
INSERT INTO AuditProperties  VALUES('DateH');
INSERT INTO AuditProperties  VALUES('Viewed');
INSERT INTO AuditProperties  VALUES('IsPopulariazation');

 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId

UNION
-----TRayID
SELECT 
        AD.PropertyName,
        LLOC_T_OLD."TEXT"    PropertyOldValue,
        LLOC_T_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD LEFT OUTER JOIN Trays T_NEW  ON AD.PropertyNewValue =T_NEW.Id
lEFT OUTER JOIN LookupLocalizations LLOC_T_NEW ON  T_NEW.Name_Id = LLOC_T_NEW.Lookup_Id 
           AND LLOC_T_NEW.Culture_Id = v_CultureID

LEFT OUTER JOIN Trays T_OLD  ON AD.PropertyOldValue =T_OLD.Id
lEFT OUTER JOIN LookupLocalizations LLOC_T_OLD ON  T_OLD.Name_Id = LLOC_T_OLD.Lookup_Id 
           AND LLOC_T_OLD.Culture_Id = v_CultureID

WHERE  AD.PropertyName = 'TrayId'
AND    AD.Audit_Id =p_AuditId
-----------------------
UNION

-----FromUserId
SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE       AD.PropertyName = 'FromUserId'
     AND    AD.Audit_Id     =   p_AuditId

UNION
-----------ToUserId
SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE       AD.PropertyName = 'ToUserId'
     AND    AD.Audit_Id     =   p_AuditId

UNION

-----FromEntityId-----
SELECT 
        AD.PropertyName,
        Loc_FromEntity_OLD."TEXT"       PropertyOldValue,
        Loc_FromEntity_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD     

  LEFT OUTER JOIN OrgUnits FromEntity_NEW  ON AD.PropertyNewValue = FromEntity_NEW .Id
	    LEFT OUTER JOIN Localizations Loc_FromEntity_NEW 
	       ON  FromEntity_NEW .LocalizationIdentifier_Id = Loc_FromEntity_NEW .LocalizationIdentifier_Id
	       AND Loc_FromEntity_NEW.CultureId =v_CultureID

          LEFT OUTER JOIN OrgUnits FromEntity_OLD  ON AD.PropertyOldValue = FromEntity_OLD.Id
	      LEFT OUTER JOIN Localizations Loc_FromEntity_OLD
	      ON  FromEntity_OLD .LocalizationIdentifier_Id = Loc_FromEntity_OLD .LocalizationIdentifier_Id
	      AND Loc_FromEntity_OLD.CultureId =v_CultureID

WHERE       AD.PropertyName = 'FromEntityId'
     AND    AD.Audit_Id     =   p_AuditId
----------------
UNION

-----ToEntityId-----
SELECT 
        AD.PropertyName,
        Loc_ToEntity_OLD."TEXT"       PropertyOldValue,
        Loc_ToEntity_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD     

  LEFT OUTER JOIN OrgUnits ToEntity_NEW  ON AD.PropertyNewValue = ToEntity_NEW .Id
	    LEFT OUTER JOIN Localizations Loc_ToEntity_NEW 
	       ON  ToEntity_NEW .LocalizationIdentifier_Id = Loc_ToEntity_NEW .LocalizationIdentifier_Id
	       AND Loc_ToEntity_NEW.CultureId =v_CultureID

          LEFT OUTER JOIN OrgUnits ToEntity_OLD  ON AD.PropertyOldValue = ToEntity_OLD.Id
	      LEFT OUTER JOIN Localizations Loc_ToEntity_OLD
	      ON  ToEntity_OLD .LocalizationIdentifier_Id = Loc_ToEntity_OLD .LocalizationIdentifier_Id
	      AND Loc_ToEntity_OLD.CultureId =v_CultureID

WHERE       AD.PropertyName = 'ToEntityId'
     AND    AD.Audit_Id     =   p_AuditId

-----
UNION

-----DeliveryMethodId


SELECT 
        AD.PropertyName,
        llOC_Delivery_OLD."TEXT"       PropertyOldValue,
        llOC_Delivery_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD

        lEFT OUTER JOIN LookupLocalizations llOC_Delivery_OLD ON llOC_Delivery_OLD.Lookup_Id =  AD.PropertyOldValue 
		AND llOC_Delivery_OLD.Culture_Id =v_CultureID	

          lEFT OUTER JOIN LookupLocalizations llOC_Delivery_NEW ON llOC_Delivery_NEW.Lookup_Id =  AD.PropertyOldValue 
		  AND llOC_Delivery_NEW.Culture_Id =v_CultureID	

WHERE  AD.PropertyName = 'DeliveryMethodId'
AND    AD.Audit_Id =p_AuditId;

END GET_TRANSACTIONASSIGNMENT_AUDITDETAILS;
/
create or replace PROCEDURE GET_Name_AUDITDETAILS 
(
 p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
INSERT INTO AuditProperties  VALUES('CivilID');
INSERT INTO AuditProperties  VALUES('FirstName');
INSERT INTO AuditProperties  VALUES('MobileNumber');
INSERT INTO AuditProperties  VALUES('Gender');



 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId


UNION
-----NationalityId


SELECT 
        AD.PropertyName,
        LLOC_Nationality_OLD."TEXT"    PropertyOldValue,
        LLOC_Nationality_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD 
lEFT OUTER JOIN LookupLocalizations LLOC_Nationality_NEW ON LLOC_Nationality_NEW.Lookup_Id = AD.PropertyNewValue	
           AND LLOC_Nationality_NEW.Culture_Id = v_CultureID

lEFT OUTER JOIN LookupLocalizations LLOC_Nationality_OLD ON LLOC_Nationality_OLD.Lookup_Id = AD.PropertyOldValue	
           AND LLOC_Nationality_OLD.Culture_Id = v_CultureID

WHERE  AD.PropertyName = 'NationalityId'
AND    AD.Audit_Id =p_AuditId;
-----------------------
END GET_Name_AUDITDETAILS;
/
create or replace PROCEDURE GET_Attachment_AUDITDETAILS 
(
 p_AuditId       NUMBER,
 p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   
WHERE PropertyName='Count'
AND AD.Audit_Id =p_AuditId

UNION
-------TypeID

SELECT 
        AD.PropertyName,
        LOC_Attach_OLD."TEXT"    PropertyOldValue,
        LOC_Attach_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD 
lEFT OUTER JOIN AttachmentTypes Attach_OLD ON Attach_OLD.Id =  AD.PropertyOldValue 
		lEFT OUTER JOIN Localizations   LOC_Attach_OLD ON LOC_Attach_OLD.LocalizationIdentifier_Id = Attach_OLD.LocalizationIdentifier_Id 	
           AND LOC_Attach_OLD.CultureId = v_CultureID	 

lEFT OUTER JOIN AttachmentTypes Attach_NEW ON Attach_NEW.Id =  AD.PropertyNewValue 
		lEFT OUTER JOIN Localizations   LOC_Attach_NEW ON LOC_Attach_NEW.LocalizationIdentifier_Id = Attach_NEW.LocalizationIdentifier_Id 	
           AND LOC_Attach_NEW.CultureId = v_CultureID

WHERE  AD.PropertyName = 'TypeId'
AND    AD.Audit_Id =p_AuditId;
-----------------------



END GET_Attachment_AUDITDETAILS;
/
create or replace PROCEDURE GET_DocumentInfo_AUDITDETAILS 
(
p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
INSERT INTO AuditProperties  VALUES('Size');
INSERT INTO AuditProperties  VALUES('MimeType');

 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId;




END GET_DocumentInfo_AUDITDETAILS ;
/
create or replace PROCEDURE GET_Explanation_AUDITDETAILS 
(
 p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
--INSERT INTO AuditProperties  VALUES('Date');
INSERT INTO AuditProperties  VALUES('DateH');
INSERT INTO AuditProperties  VALUES('Description');


 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId

UNION
-------Permission

SELECT 
        AD.PropertyName,
        LLOC_Perm_OLD."TEXT"    PropertyOldValue,
        LLOC_Perm_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD lEFT OUTER JOIN Permissions Perm_OLD ON Perm_OLD.Id =  AD.PropertyOldValue 
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm_OLD ON LLOC_Perm_OLD.Lookup_Id = Perm_OLD.Name_Id	
           AND LLOC_Perm_OLD.Culture_Id = v_CultureID	 

lEFT OUTER JOIN Permissions Perm_NEW ON Perm_NEW.Id =  AD.PropertyNewValue 
		lEFT OUTER JOIN LookupLocalizations LLOC_Perm_NEW ON LLOC_Perm_NEW.Lookup_Id = Perm_NEW.Name_Id	
           AND LLOC_Perm_NEW.Culture_Id = v_CultureID	

WHERE  AD.PropertyName = 'PermissionId'
AND    AD.Audit_Id =p_AuditId
-----------------------
UNION
-------UserId

SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE  AD.PropertyName = 'FromUserId'
AND    AD.Audit_Id =p_AuditId;
--
--UNION
--
-------ExplanationEditorType
--
--SELECT 
--        AD.PropertyName,
--        AD.PropertyOldValue,
--        AD.PropertyNewValue,
--        AD.CreatedOn 
--
--FROM AuditDetails AD 
--WHERE  AD.PropertyName = 'ExplanationEditorType'
--AND    AD.Audit_Id =p_AuditId;
--------------------------

END GET_Explanation_AUDITDETAILS;
/
create or replace PROCEDURE GET_EC_AUDITDETAILS 
(
p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
--INSERT INTO AuditProperties  VALUES('Date');
INSERT INTO AuditProperties  VALUES('DateH');
INSERT INTO AuditProperties  VALUES('Viewed');


 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId

UNION
-----------Actions---
SELECT 
        AD.PropertyName,
        LOC_Action_OLD."TEXT"    PropertyOldValue,
        LOC_Action_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD lEFT OUTER JOIN Actions Action_OLD ON Action_OLD.Id =  AD.PropertyOldValue 
		lEFT OUTER JOIN Localizations LOC_Action_OLD ON LOC_Action_OLD.LocalizationIdentifier_Id = Action_OLD.LocalizationIdentifier_Id	
           AND LOC_Action_OLD.CultureId = v_CultureID	 

lEFT OUTER JOIN Actions Action_NEW ON Action_NEW.Id =  AD.PropertyNewValue 
		lEFT OUTER JOIN Localizations LOC_Action_NEW ON LOC_Action_NEW.LocalizationIdentifier_Id = Action_NEW.LocalizationIdentifier_Id	
           AND LOC_Action_NEW.CultureId = v_CultureID	

WHERE  AD.PropertyName = 'ActionId'
AND    AD.Audit_Id =p_AuditId
-----------------------
UNION
-------UserId

SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE  AD.PropertyName = 'UserId'
AND    AD.Audit_Id =p_AuditId

UNION

-----EntityID

SELECT 
        AD.PropertyName,
        Loc_Entity_OLD."TEXT"       PropertyOldValue,
        Loc_Entity_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD     

  LEFT OUTER JOIN OrgUnits Entity_NEW  ON AD.PropertyNewValue = Entity_NEW .Id
	    LEFT OUTER JOIN Localizations Loc_Entity_NEW 
	       ON  Entity_NEW .LocalizationIdentifier_Id = Loc_Entity_NEW .LocalizationIdentifier_Id
	       AND Loc_Entity_NEW.CultureId =v_CultureID

          LEFT OUTER JOIN OrgUnits Entity_OLD  ON AD.PropertyOldValue = Entity_OLD.Id
	      LEFT OUTER JOIN Localizations Loc_Entity_OLD
	      ON  Entity_OLD .LocalizationIdentifier_Id = Loc_Entity_OLD .LocalizationIdentifier_Id
	      AND Loc_Entity_OLD.CultureId =v_CultureID

WHERE       AD.PropertyName = 'EntityId'
     AND    AD.Audit_Id     =   p_AuditId;
--------------------------

END GET_EC_AUDITDETAILS ;
/
create or replace PROCEDURE GET_IC_AUDITDETAILS 
(
 p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
--INSERT INTO AuditProperties  VALUES('Date');
INSERT INTO AuditProperties  VALUES('DateH');
INSERT INTO AuditProperties  VALUES('Viewed');


 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId

UNION
-----------Actions---
SELECT 
        AD.PropertyName,
        LOC_Action_OLD."TEXT"    PropertyOldValue,
        LOC_Action_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD lEFT OUTER JOIN Actions Action_OLD ON Action_OLD.Id =  AD.PropertyOldValue 
		lEFT OUTER JOIN Localizations LOC_Action_OLD ON LOC_Action_OLD.LocalizationIdentifier_Id = Action_OLD.LocalizationIdentifier_Id	
           AND LOC_Action_OLD.CultureId = v_CultureID	 

lEFT OUTER JOIN Actions Action_NEW ON Action_NEW.Id =  AD.PropertyNewValue 
		lEFT OUTER JOIN Localizations LOC_Action_NEW ON LOC_Action_NEW.LocalizationIdentifier_Id = Action_NEW.LocalizationIdentifier_Id	
           AND LOC_Action_NEW.CultureId = v_CultureID	

WHERE  AD.PropertyName = 'ActionId'
AND    AD.Audit_Id =p_AuditId
-----------------------
UNION
-------UserId

SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE  AD.PropertyName = 'UserId'
AND    AD.Audit_Id =p_AuditId

UNION

-----EntityID

SELECT 
        AD.PropertyName,
        Loc_Entity_OLD."TEXT"       PropertyOldValue,
        Loc_Entity_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD     

  LEFT OUTER JOIN OrgUnits Entity_NEW  ON AD.PropertyNewValue = Entity_NEW .Id
	    LEFT OUTER JOIN Localizations Loc_Entity_NEW 
	       ON  Entity_NEW .LocalizationIdentifier_Id = Loc_Entity_NEW .LocalizationIdentifier_Id
	       AND Loc_Entity_NEW.CultureId =v_CultureID

          LEFT OUTER JOIN OrgUnits Entity_OLD  ON AD.PropertyOldValue = Entity_OLD.Id
	      LEFT OUTER JOIN Localizations Loc_Entity_OLD
	      ON  Entity_OLD .LocalizationIdentifier_Id = Loc_Entity_OLD .LocalizationIdentifier_Id
	      AND Loc_Entity_OLD.CultureId =v_CultureID

WHERE       AD.PropertyName = 'EntityId'
     AND    AD.Audit_Id     =   p_AuditId;
--------------------------

END GET_IC_AUDITDETAILS;
/
create or replace PROCEDURE GET_MAIN_AUDIT 
(
p_PrimaryKey    "NUMBER" ,
p_EntityName    NVARCHAR2,
p_CultureName   NVARCHAR2 ,
p_PropName      NVARCHAR2  ,
p_AuditType     "NUMBER"  ,
p_AuditDateFrom     DATE,
p_AuditDateTo     DATE,
p_UserId        "NUMBER" ,
p_PageIndex    NUMBER,
p_PageSize     NUMBER,
p_OrderBy      NVARCHAR2,
p_Ascending    NUMBER,
p_Cur           OUT SYS_REFCURSOR ,
p_TotalCount OUT NUMBER

) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for select DISTINCT
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy
 
 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy = User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Aud.Id 
 
WHERE Aud.EntityName=p_EntityName
AND (TRUNC(Aud."DATE") >=  p_AuditDateFrom AND TRUNC(Aud."DATE") <= p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Aud.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Aud.PrimaryKeyValue)=p_PrimaryKey

ORDER BY 
 CASE WHEN (p_OrderBy = 'Id') THEN Aud.Id END DESC,
 CASE WHEN (p_Ascending = 1 AND p_OrderBy = 'CreatedOn') THEN Aud.CreatedOn END ASC,
 CASE WHEN (p_Ascending = 0 AND p_OrderBy = 'CreatedOn') THEN Aud.CreatedOn END DESC

OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;



SELECT COUNT(DISTINCT Audt.Id) INTO p_TotalCount 
FROM Audits Audt  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Audt.CreatedBy = User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Audt.Id 
 
WHERE Audt.EntityName=p_EntityName
AND (TRUNC(Audt."DATE") >=  p_AuditDateFrom AND TRUNC(Audt."DATE") <= p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Audt.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Audt.PrimaryKeyValue)=p_PrimaryKey;

END GET_MAIN_AUDIT;
/
create or replace PROCEDURE GET_MAIN_AUDIT_Attachments 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_Cur             OUT SYS_REFCURSOR
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for     select
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID

 WHERE Aud.EntityName=p_EntityName
 --Ask hassan to convert it to number
AND   to_number(Aud.PrimaryKeyValue)IN 

 (
    SELECT Id FROM  Attachments WHERE TransactionId =  p_PrimaryKey
 );
 
END GET_MAIN_AUDIT_Attachments;
/
create or replace PROCEDURE GET_MAIN_AUDIT_COPIES 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_Cur             OUT SYS_REFCURSOR
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for     select
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID

 WHERE Aud.EntityName=p_EntityName
 --Ask hassan to convert it to number
AND   to_number(Aud.PrimaryKeyValue)IN 

 (
    SELECT Id FROM TransactionCopies WHERE TransactionId =  p_PrimaryKey
 );

END GET_MAIN_AUDIT_COPIES;
/
create or replace PROCEDURE GET_MAIN_AUDIT_EX_COPIES 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_Cur             OUT SYS_REFCURSOR
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for     select
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID

 WHERE Aud.EntityName=p_EntityName
 --Ask hassan to convert it to number
AND   to_number(Aud.PrimaryKeyValue)IN 

 (
    SELECT Id FROM TransactionExternalCopies WHERE TransactionId =  p_PrimaryKey
 );

END GET_MAIN_AUDIT_EX_COPIES;
/
create or replace PROCEDURE GET_MAIN_AUDIT_EXPLANATIONS 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_Cur             OUT SYS_REFCURSOR
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for     select
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID

 WHERE Aud.EntityName=p_EntityName
 --Ask hassan to convert it to number
AND   to_number(Aud.PrimaryKeyValue)IN 

 (
  SELECT Id FROM Explanations WHERE TransactionId =  p_PrimaryKey
 );

END GET_MAIN_AUDIT_EXPLANATIONS;
/
create or replace PROCEDURE GET_MAIN_AUDIT_LINKS 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_Cur             OUT SYS_REFCURSOR
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for     select
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID

 WHERE Aud.EntityName=p_EntityName
 --Ask hassan to convert it to number
AND   to_number(Aud.PrimaryKeyValue)IN 

 (
     SELECT Id FROM  TransactionLinks WHERE TransactionId =  p_PrimaryKey
 );

END GET_MAIN_AUDIT_LINKS;
/
create or replace PROCEDURE GET_MAIN_AUDIT_NAMES 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_Cur             OUT SYS_REFCURSOR
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for     select
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID

 WHERE Aud.EntityName=p_EntityName
 --Ask hassan to convert it to number
AND   to_number(Aud.PrimaryKeyValue)IN 

 (
   SELECT Id FROM TransactionNames WHERE TransactionId =  p_PrimaryKey
 );

END GET_MAIN_AUDIT_NAMES;
/
create or replace PROCEDURE GET_MAIN_AUDIT_BY_TRANS_ID 
(
p_PrimaryKey      IN "NUMBER" ,
p_EntityName      IN NVARCHAR2,
p_CultureName     IN NVARCHAR2 ,
p_PropName      NVARCHAR2  ,
p_AuditType     "NUMBER"  ,
p_AuditDateFrom     DATE,
p_AuditDateTo     DATE,
p_UserId        "NUMBER" ,
p_PageIndex    NUMBER,
p_PageSize     NUMBER,
p_OrderBy      NVARCHAR2,
p_Ascending    NUMBER,
p_Cur           OUT SYS_REFCURSOR ,
p_TotalCount OUT NUMBER
) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for select DISTINCT
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID 
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Aud.Id 
 
WHERE Aud.EntityName=p_EntityName
AND (TRUNC(Aud."DATE") >=  p_AuditDateFrom AND TRUNC(Aud."DATE") <= p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Aud.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Aud.TransactionId)=p_PrimaryKey
ORDER BY 
 CASE WHEN (p_OrderBy = 'Id') THEN Aud.Id END DESC,
 CASE WHEN (p_Ascending = 1 AND p_OrderBy = 'CreatedOn') THEN Aud.CreatedOn END ASC,
 CASE WHEN (p_Ascending = 0 AND p_OrderBy = 'CreatedOn') THEN Aud.CreatedOn END DESC
 
OFFSET p_PageIndex * p_PageSize ROWS
FETCH NEXT p_PageSize ROWS ONLY;

SELECT COUNT(DISTINCT Audt.Id) INTO p_TotalCount 
FROM Audits Audt  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Audt.CreatedBy = User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Audt.Id 
 
WHERE Audt.EntityName=p_EntityName
AND (TRUNC(Audt."DATE") >=  p_AuditDateFrom AND TRUNC(Audt."DATE") <= p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Audt.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Audt.PrimaryKeyValue)=p_PrimaryKey;


END GET_MAIN_AUDIT_BY_TRANS_ID;
/
create or replace PROCEDURE GET_TASK_AUDITDETAILS 
(
p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
INSERT INTO AuditProperties  VALUES('TaskDescription');
INSERT INTO AuditProperties  VALUES('DateH');



 OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId


UNION
-----ToUserId
SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE  AD.PropertyName = 'ToUserId'
AND    AD.Audit_Id =p_AuditId

UNION
-----ToOrgUnitId

SELECT 
        AD.PropertyName,
        Loc_Entity_OLD."TEXT"       PropertyOldValue,
        Loc_Entity_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD     

  LEFT OUTER JOIN OrgUnits Entity_NEW  ON AD.PropertyNewValue = Entity_NEW .Id
	    LEFT OUTER JOIN Localizations Loc_Entity_NEW 
	       ON  Entity_NEW .LocalizationIdentifier_Id = Loc_Entity_NEW .LocalizationIdentifier_Id
	       AND Loc_Entity_NEW.CultureId =v_CultureID

          LEFT OUTER JOIN OrgUnits Entity_OLD  ON AD.PropertyOldValue = Entity_OLD.Id
	      LEFT OUTER JOIN Localizations Loc_Entity_OLD
	      ON  Entity_OLD .LocalizationIdentifier_Id = Loc_Entity_OLD .LocalizationIdentifier_Id
	      AND Loc_Entity_OLD.CultureId =v_CultureID

WHERE       AD.PropertyName = 'ToOrgUnitId'
AND    AD.Audit_Id = p_AuditId

---------------------------

UNION
-----StatusId

SELECT 
        AD.PropertyName,
        LLOC_STATUS_OLD."TEXT"    PropertyOldValue,
        LLOC_STATUS_NEW."TEXT"    PropertyNewValue,
        AD.CreatedOn 

FROM AuditDetails AD 
lEFT OUTER JOIN LookupLocalizations LLOC_STATUS_NEW ON LLOC_STATUS_NEW.Lookup_Id = AD.PropertyNewValue	
           AND LLOC_STATUS_NEW.Culture_Id = v_CultureID

lEFT OUTER JOIN LookupLocalizations LLOC_STATUS_OLD ON LLOC_STATUS_OLD.Lookup_Id = AD.PropertyOldValue	
           AND LLOC_STATUS_OLD.Culture_Id = v_CultureID

WHERE  AD.PropertyName = 'StatusId'
AND    AD.Audit_Id = p_AuditId;

---------------------------
END GET_TASK_AUDITDETAILS;
/
create or replace PROCEDURE GET_FOLLOWUP_AUDITDETAILS 
(
p_AuditId       NUMBER,
  p_PropName      NVARCHAR2,
 p_CultureName   NVARCHAR2 ,
 p_Cur           OUT SYS_REFCURSOR
)

IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO v_CultureID
FROM Cultures  where ShortName = p_CultureName;

---Populate Temp Table with needed Propert that don't need Realtions
INSERT INTO AuditProperties  VALUES('DateToH');

OPEN p_Cur FOR
SELECT 
        AD.PropertyName,
        AD.PropertyOldValue,
        AD.PropertyNewValue,
        AD.CreatedOn      
FROM AuditDetails AD   INNER JOIN AUDITPROPERTIES  Properties
    ON AD.PropertyName=Properties.PROPERTYNAME
WHERE  AD.Audit_Id =p_AuditId

UNION
-------UserId
SELECT 
        AD.PropertyName,
        Loc_User_OLD."TEXT"       PropertyOldValue,
        Loc_User_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD             
           LEFT OUTER JOIN	 UserProfiles  User_NEW ON User_NEW.Id =AD.PropertyNewValue 
	       LEFT OUTER JOIN  Localizations  Loc_User_NEW  ON User_NEW.LocalizationIdentifier_Id=Loc_User_NEW.LocalizationIdentifier_Id   
         AND Loc_User_NEW.CultureId = v_CultureID
           LEFT OUTER JOIN	 UserProfiles  User_OLD ON User_OLD.Id =AD.PropertyOldValue 
	       LEFT OUTER JOIN  Localizations      Loc_User_OLD  ON User_OLD.LocalizationIdentifier_Id=Loc_User_OLD.LocalizationIdentifier_Id   
         AND Loc_User_OLD.CultureId = v_CultureID

WHERE  AD.PropertyName = 'UserId'
AND    AD.Audit_Id =p_AuditId

UNION

-----EntityID

SELECT 
        AD.PropertyName,
        Loc_Entity_OLD."TEXT"       PropertyOldValue,
        Loc_Entity_NEW."TEXT"       PropertyNewValue,
        AD.CreatedOn 
        FROM AuditDetails AD     

  LEFT OUTER JOIN OrgUnits Entity_NEW  ON AD.PropertyNewValue = Entity_NEW .Id
	    LEFT OUTER JOIN Localizations Loc_Entity_NEW 
	       ON  Entity_NEW .LocalizationIdentifier_Id = Loc_Entity_NEW .LocalizationIdentifier_Id
	       AND Loc_Entity_NEW.CultureId =v_CultureID

          LEFT OUTER JOIN OrgUnits Entity_OLD  ON AD.PropertyOldValue = Entity_OLD.Id
	      LEFT OUTER JOIN Localizations Loc_Entity_OLD
	      ON  Entity_OLD .LocalizationIdentifier_Id = Loc_Entity_OLD .LocalizationIdentifier_Id
	      AND Loc_Entity_OLD.CultureId =v_CultureID

WHERE       AD.PropertyName = 'EntityId'
     AND    AD.Audit_Id     =   p_AuditId;
--------------------------

END GET_FOLLOWUP_AUDITDETAILS;
/
--------------------ADMIN_MOVE_ENTITY---------------------
create or replace PROCEDURE ADMIN_MOVE_ENTITY
(
  p_OrgUnitId IN NUMBER,
  p_NewParentID IN NUMBER,
  p_LoggedInUser IN NUMBER
)
AS
BEGIN

   BEGIN
      UPDATE OrgUnits
         SET ParentId = p_NewParentID,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  Id = p_OrgUnitId;

   END;
END;
/
----------------ADMIN_MOVE_TRANSACTION_BYID-------------------
create or replace PROCEDURE ADMIN_MOVE_TRANSACTION_BYID
(
  p_TransID IN NUMBER,
  p_ToUserID IN NUMBER,
  p_ToEntityID IN NUMBER,
  p_LoggedInUser IN NUMBER,
  p_TrayMyTransactions 	 IN NUMBER,
  p_TrayOrgUnit 		 IN NUMBER
)
IS
DATEHJ varchar2(50);
BEGIN
 SELECT TO_CHAR(SYSDATE, 'dd/mm/rrrr HH:MI AM', 'nls_calendar=''arabic hijrah''') into DATEHJ FROM DUAL;
   BEGIN

   UPDATE transactions
         SET entityid = p_ToEntityID,
             ToUserId = p_ToUserID,             
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  id = p_TransID;

      UPDATE TransactionAssignments
         SET ToEntityId = p_ToEntityID,
             ToUserId = p_ToUserID,
             TrayId = CASE 
                           WHEN TrayId = p_TrayMyTransactions
                             AND p_ToUserID IS NULL THEN p_TrayOrgUnit
             ELSE TrayId
                END,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TransactionId = p_TransID;

      INSERT INTO TransactionAssignmentHistories(TrayId ,FromUserId, ToUserId ,TransactionId,ActionId , FromEntityId , 
       ToEntityId, Description, "DATE" , DateH ,CreatedOn, CreatedBy  )
        SELECT 
              --   SQ_TransactionAssig_1982977396.nextval,
                 TransactionAssignmentHistories.TrayId ,
                 TransactionAssignmentHistories.FromUserId ,
                 p_ToUserID ,
                 TransactionAssignmentHistories.TransactionId,
                 TransactionAssignmentHistories.ActionId,
                 TransactionAssignmentHistories.FromEntityId ,
                 p_ToEntityID ,
                 'تم نقلها بواسطة مدير النظام' ,
                 SYSDATE ,
                  DATEHJ,
                 SYSDATE ,
                 p_LoggedInUser
          FROM TransactionAssignmentHistories
           WHERE  TransactionId = p_TransID ;

      INSERT INTO TransactionEntityDetails
        ( TransactionId, EntityId, CreatedOn, CreatedBy )
        ( SELECT TransactionAssignments.TransactionId ,
                 TransactionAssignments.ToEntityId ,
                 SYSDATE ,
                 p_LoggedInUser 
          FROM TransactionAssignments 
                 LEFT JOIN TransactionEntityDetails    ON TransactionAssignments.TransactionId = TransactionEntityDetails.TransactionId
                 AND TransactionAssignments.ToEntityId = TransactionEntityDetails.EntityId
           WHERE  TransactionAssignments.TransactionId = p_TransID
                    AND TransactionEntityDetails.Id IS NULL );
   END;

END;
/
-------------ADMIN_MOVE_TRANSACTIONS-------------
create or replace PROCEDURE ADMIN_MOVE_TRANSACTIONS
(
  p_ToUserID IN NUMBER,
  p_ToEntityID IN NUMBER,
  p_FromUserID IN NUMBER,
  p_FromEntityID IN NUMBER,
  p_LoggedInUser IN NUMBER,
  p_TrayMyTransactions  IN NUMBER,
  p_TrayOrgUnit   IN NUMBER
)
IS
DATEHJ varchar2(50);
BEGIN
 SELECT TO_CHAR(SYSDATE, 'dd/mm/rrrr HH:MI AM', 'nls_calendar=''arabic hijrah''') into DATEHJ FROM DUAL;
   BEGIN
      UPDATE TransactionAssignments
         SET ToEntityId = p_ToEntityID,
             ToUserId = p_ToUserID,
             TrayId = CASE 
                           WHEN TrayId = p_TrayMyTransactions
                             AND p_ToUserID IS NULL THEN p_TrayOrgUnit
             ELSE TrayId   END,
            ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  ToEntityId = p_FromEntityID
        AND ( ToUserId = p_FromUserID  OR p_FromUserID IS NULL );

      INSERT INTO TransactionAssignmentHistories(TrayId ,FromUserId, ToUserId ,TransactionId,ActionId , FromEntityId , 
       ToEntityId, Description, "DATE" , DateH ,CreatedOn, CreatedBy  )
        ( SELECT 
              --   SQ_TransactionAssig_1982977396.nextval,
                 TransactionAssignmentHistories.TrayId ,
                 TransactionAssignmentHistories.FromUserId ,
                  p_ToUserID ,
                 TransactionAssignmentHistories.TransactionId ,
                 TransactionAssignmentHistories.ActionId,
                 TransactionAssignmentHistories.FromEntityId ,
                 p_ToEntityID ,
                 'تم نقلها بواسطة مدير النظام' ,
                 SYSDATE ,
                 DATEHJ ,
                 SYSDATE ,
                 p_LoggedInUser
          FROM TransactionAssignmentHistories 
           WHERE  ToEntityId = p_FromEntityID
                    AND ( ToUserId = p_FromUserID
                    OR p_FromUserID IS NULL ) );

      INSERT INTO TransactionEntityDetails ( TransactionId, EntityId, CreatedOn, CreatedBy )
        ( SELECT TransactionAssignments.TransactionId ,
                 TransactionAssignments.ToEntityId ,
                 SYSDATE ,
                 p_LoggedInUser 
          FROM TransactionAssignments 
                 LEFT JOIN TransactionEntityDetails    ON TransactionAssignments.TransactionId = TransactionEntityDetails.TransactionId
                 AND TransactionAssignments.ToEntityId = TransactionEntityDetails.EntityId
           WHERE  ToEntityId = p_ToEntityID
                    AND TransactionEntityDetails.Id IS NULL );
   END;

END;
/
-----------------ADMIN_MOVE_USER---------------
create or replace PROCEDURE ADMIN_MOVE_USER
(
  p_UserProfileId IN NUMBER,  
  p_OrgUnitId IN NUMBER,
  p_NewOrgUnitId IN NUMBER,
  p_LoggedInUser IN NUMBER,
  p_TrayOrgUnit		   IN NUMBER,
  p_TraySaved		   IN NUMBER,
  p_TrayMyTransactions IN NUMBER
)
AS
BEGIN
   BEGIN
      UPDATE TransactionAssignments
         SET TrayId = p_TrayOrgUnit,
             ToUserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TrayId = p_TrayMyTransactions
        AND ToUserId = p_UserProfileId
        AND ToEntityId = p_OrgUnitId;

      UPDATE TransactionAssignments
         SET ToUserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TrayId = p_TraySaved 
        AND ToUserId = p_UserProfileId
        AND ToEntityId = p_OrgUnitId;  

      UPDATE TransactionCopies
         SET UserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  UserId = p_UserProfileId
        AND EntityId = p_OrgUnitId;

      UPDATE UserProfileOrgUnits
         SET OrgUnit_Id = p_NewOrgUnitId
       WHERE  UserProfile_Id = p_UserProfileId
       AND OrgUnit_Id=p_OrgUnitId;

	   UPDATE UserProfiles
         SET MAINORGUNITID = p_NewOrgUnitId
       WHERE  ID = p_UserProfileId;


   END;
END;
/
create or replace PROCEDURE ADMIN_MOVE_USERS
(
  p_UserProfileIds IN NVARCHAR2,  
  p_OrgUnitId IN NUMBER,
  p_NewOrgUnitId IN NUMBER,
  p_LoggedInUser IN NUMBER,
  p_TrayOrgUnit		   IN NUMBER,
  p_TraySaved		   IN NUMBER,
  p_TrayMyTransactions IN NUMBER,
  p_TrayDraftOutbound IN NUMBER,
  p_IsExternal boolean
)
AS
v_OrgUnitId number;
v_NewOrgUnitId number;
BEGIN
    IF p_IsExternal THEN
        BEGIN
            SELECT Id Into v_OrgUnitId FROM OrgUnits where ExternalId = p_OrgUnitId;
            SELECT Id Into v_NewOrgUnitId FROM OrgUnits where ExternalId = p_NewOrgUnitId;
        END;
    ELSE
        BEGIN
            v_OrgUnitId := p_OrgUnitId;
            v_NewOrgUnitId := p_NewOrgUnitId;
        END;
    END IF;
   BEGIN
      UPDATE TransactionAssignments
         SET TrayId = p_TrayOrgUnit,
             ToUserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TrayId = p_TrayMyTransactions
        AND ToUserId In(select regexp_substr(p_UserProfileIds,'[^,]+', 1, level) s1 from dual
    connect by regexp_substr(p_UserProfileIds, '[^,]+', 1, level) is not null)
        AND ToEntityId = v_OrgUnitId;

        UPDATE TransactionAssignments
         SET TrayId = p_TrayOrgUnit,
             ToUserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TrayId = p_TrayDraftOutbound
        AND ToUserId In(select regexp_substr(p_UserProfileIds,'[^,]+', 1, level) s2 from dual
    connect by regexp_substr(p_UserProfileIds, '[^,]+', 1, level) is not null)
        AND ToEntityId = v_OrgUnitId;


    --  UPDATE TransactionAssignments
    --     SET ToUserId = NULL,
    --         ModefiedBy = p_LoggedInUser,
    --         ModefiedOn = SYSDATE
    --   WHERE  TrayId = p_TraySaved 
    --    AND ToUserId In (select regexp_substr(p_UserProfileIds,'[^,]+', 1, level) s2 from dual
    --connect by regexp_substr(p_UserProfileIds, '[^,]+', 1, level) is not null)
    --    AND ToEntityId = p_OrgUnitId;  

      UPDATE TransactionCopies
         SET UserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  UserId In (select regexp_substr(p_UserProfileIds,'[^,]+', 1, level) s3 from dual
    connect by regexp_substr(p_UserProfileIds, '[^,]+', 1, level) is not null)
        AND EntityId = v_OrgUnitId;

      UPDATE UserProfileOrgUnits
         SET OrgUnit_Id = v_NewOrgUnitId
       WHERE  UserProfile_Id In (select regexp_substr(p_UserProfileIds,'[^,]+', 1, level) s4 from dual
    connect by regexp_substr(p_UserProfileIds, '[^,]+', 1, level) is not null)
       AND OrgUnit_Id = v_OrgUnitId;

	      UPDATE UserProfiles
         SET MAINORGUNITID = v_NewOrgUnitId
       WHERE  ID In (select regexp_substr(p_UserProfileIds,'[^,]+', 1, level) s5 from dual
	   connect by regexp_substr(p_UserProfileIds, '[^,]+', 1, level) is not null);

   END;
END;
/
create or replace PROCEDURE  GET_MAIN_AUDIT_FOR_PRINT 
(

p_PrimaryKey    "NUMBER" ,
p_EntityName    NVARCHAR2,
p_CultureName   NVARCHAR2 ,
p_PropName      NVARCHAR2  ,
p_AuditType     "NUMBER"  ,
p_AuditDateFrom     DATE,
p_AuditDateTo     DATE,
p_UserId        "NUMBER" ,
p_PageIndex    NUMBER,
p_PageSize     NUMBER,
p_OrderBy      NVARCHAR2,
p_Ascending    NUMBER,
p_Cur           OUT SYS_REFCURSOR ,
p_TotalCount OUT NUMBER

) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for select DISTINCT
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy = User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Aud.Id 

WHERE Aud.EntityName=p_EntityName
AND (TRUNC(Aud."DATE") >=  p_AuditDateFrom AND TRUNC(Aud."DATE") <= p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Aud.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Aud.PrimaryKeyValue)=p_PrimaryKey;

SELECT COUNT(DISTINCT Audt.Id) INTO p_TotalCount 
FROM Audits Audt  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Audt.CreatedBy = User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Audt.Id 

WHERE Audt.EntityName=p_EntityName
AND ( TRUNC(Audt."DATE") >=  p_AuditDateFrom AND TRUNC(Audt."DATE") <= p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Audt.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Audt.PrimaryKeyValue)=p_PrimaryKey;

END GET_MAIN_AUDIT_FOR_PRINT;
/
  CREATE OR REPLACE  PROCEDURE GET_MA_FOR_PRINT_BY_TRANS_ID 
(

p_PrimaryKey    "NUMBER" ,
p_EntityName    NVARCHAR2,
p_CultureName   NVARCHAR2 ,
p_PropName      NVARCHAR2  ,
p_AuditType     "NUMBER"  ,
p_AuditDateFrom     DATE,
p_AuditDateTo     DATE,
p_UserId        "NUMBER" ,
p_PageIndex    NUMBER,
p_PageSize     NUMBER,
p_OrderBy      NVARCHAR2,
p_Ascending    NUMBER,
p_Cur           OUT SYS_REFCURSOR ,
p_TotalCount OUT NUMBER

) 
IS
v_CultureID            NUMBER;
BEGIN
SELECT Id INTO  v_CultureID
FROM  Cultures  where ShortName = p_CultureName;

 open p_Cur for select DISTINCT
 Aud.Id,
 Aud."DATE" ,
 Aud.OperationType,
 Loc_User_CreatedBy."TEXT"   CreatedBy

 FROM Audits Aud  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Aud.CreatedBy =User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID 
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Aud.Id 
 
WHERE Aud.EntityName=p_EntityName
AND ( Aud."DATE" BETWEEN  p_AuditDateFrom AND p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Aud.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Aud.TransactionId)=p_PrimaryKey;


SELECT COUNT(DISTINCT Audt.Id) INTO p_TotalCount 
FROM Audits Audt  LEFT OUTER JOIN  UserProfiles  User_CreatedBy ON Audt.CreatedBy = User_CreatedBy.Id 
 LEFT OUTER JOIN  Localizations    Loc_User_CreatedBy 
 ON User_CreatedBy.LocalizationIdentifier_Id = Loc_User_CreatedBy.LocalizationIdentifier_Id 
 AND Loc_User_CreatedBy.CultureId = v_CultureID
 LEFT JOIN AuditDetails AudDetails ON AudDetails.Audit_Id = Audt.Id 
 
WHERE Audt.EntityName=p_EntityName
AND ( Audt."DATE" BETWEEN  p_AuditDateFrom AND p_AuditDateTo)
AND (p_UserId = '-1' OR User_CreatedBy.Id  = p_UserId)
AND (p_AuditType = '-1' OR Audt.OperationType = p_AuditType )
AND (p_PropName = 'none' OR AudDetails.PropertyName = p_PropName)
AND   to_number(Audt.PrimaryKeyValue)=p_PrimaryKey;

END GET_MA_FOR_PRINT_BY_TRANS_ID;
/
------------- USER_MOBILE_DASHBOARD_ENTITIES_ACCOMPLESHMENTS ------
create or replace PROCEDURE USER_MOBILE_DASHBOARD_ENTITIES_ACCOMPLESHMENTS(
	p_ENTITY_ID		      "NUMBER" DEFAULT  3,
	p_PERIOD_COUNT        "NUMBER" DEFAULT  12,
	p_SELECTED_PERIOD	  "NUMBER" DEFAULT  2,--by Year 0, by Month 1, by weeks 2
    p_Status              Number,
	p_Inbound             Number,
	p_Internal		      Number,
	p_cur                 OUT SYS_REFCURSOR
    )
	
    
		
AS
v_First_Period     TimeStamp; 
v_Last_Period      TimeStamp; 
v_I                NUMBER(10, 0) :=1;
v_Count            NUMBER; 
v_DT_FROM_DATE   TimeStamp;
v_DT_TO_DATE     TimeStamp;

BEGIN
		-------------------------DashBoard by Years----------------------
IF p_SELECTED_PERIOD = 0  THEN
--Rerturn First day in this Year into v_Last_Period
  SELECT  TRUNC(sysdate,'YEAR') into v_Last_Period FROM Dual;

SELECT  add_months(v_Last_Period, -((p_PERIOD_COUNT-1) * 12)) into v_First_Period  FROM  DUAL;  
WHILE (v_First_Period <=v_Last_Period)
LOOP
 INSERT INTO  TEMP_COUNTERS_ByPERIOD (id,From_Date,To_Date)
   VALUES  (v_I, v_First_Period ,ADD_MONTHS(v_First_Period,12)-1);
             v_First_Period := add_months(v_First_Period, 12) ;      
             v_I :=v_I+1;
END LOOP;

END IF;
-----------------------DashBoard by Months----------------------
--ELS
IF  (p_SELECTED_PERIOD = 1) 	THEN

SELECT TRUNC(SYSDATE,'month')  into v_Last_Period FROM  DUAL  ;

 SELECT  add_months(v_Last_Period, -(p_PERIOD_COUNT-1) ) into v_First_Period
FROM  DUAL; 
  While (v_First_Period <=v_Last_Period)
 LOOP
 INSERT INTO  TEMP_COUNTERS_ByPERIOD(id,From_Date,To_Date)
 VALUES (v_I,  v_First_Period  , LAST_DAY(v_First_Period));

  v_First_Period   :=  add_months(v_First_Period,1);
     v_I :=v_I+1;
END LOOP;
END IF;
		-------------------------DashBoard by weeks----------------------
--	 ELS
    IF  (p_SELECTED_PERIOD = 2 )
	THEN
--First day of this week
SELECT   TRUNC(sysdate, 'iw') -1  INTO v_Last_Period  From dual ;
SELECT TRUNC(v_Last_Period) -((p_PERIOD_COUNT-1)*7)  INTO v_First_Period FROM dual;
 While (v_First_Period <=v_Last_Period)
  LOOP
   INSERT INTO  TEMP_COUNTERS_ByPERIOD(id,From_Date,To_Date)
VALUES (v_I, v_First_Period  ,(  TRUNC(v_First_Period) + 6 ));
 --DBMS_OUTPUT.PUT_LINE( v_First_Period );
v_First_Period :=  TRUNC(v_First_Period) + 7 ;
	--	SET  v_First_Period=  DateAdd (wk  ,1, v_First_Period)
	     v_I :=v_I+1;
    	 END LOOP;
	END IF ;
--Count of temp table
SELECT COUNT(1)  INTO v_Count FROM TEMP_COUNTERS_ByPERIOD; 
   v_I:=1;

While (v_I <=v_Count)
Loop
SELECT  From_Date, To_Date INTO v_DT_FROM_DATE ,v_DT_TO_DATE
FROM TEMP_COUNTERS_ByPERIOD 
WHERE id = v_I;
-----------------Entity Transactions 
---MY TRansaction
		UPDATE TEMP_COUNTERS_ByPERIOD
SET MY_TRAN_COUNT =  (  SELECT COUNT( TR.Id)
								FROM Transactions TR 
				             INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
                            	INNER JOIN UserProfiles ON TA.ToUserId = UserProfiles.Id
									WHERE	  (TransactionCategoryId = p_Inbound OR TransactionCategoryId = p_Internal)
                                   AND(TA.ToEntityId = p_ENTITY_ID)
                                    AND TR."DATE" BETWEEN v_DT_FROM_DATE AND v_DT_TO_DATE
                                    AND    TR.RemindDate is null 
                                    AND   (TR.StatusId <>p_Status)
                                     AND (TR.RemindDate>= sysdate OR TA."DATE" + UserProfiles.TransactionProcessingPeriod >= sysdate ) )     
                                          WHERE  id = v_I;                    

--------------------WITH_APPOITMEN--------

		UPDATE TEMP_COUNTERS_ByPERIOD
SET WITH_APPOITMENT_COUNT = 
                        (SELECT COUNT(TR.Id)
								FROM Transactions TR 
							--	INNER JOIN   TRAY_WITH_APPOITMENT_VW 
                             INNER JOIN TransactionAssignments   TA ON TA.Id=TR.Id 
								INNER JOIN UserProfiles ON TA.ToUserId = UserProfiles.Id
										WHERE	 (TransactionCategoryId = p_Inbound OR TransactionCategoryId = p_Internal)
                                   AND(TA.ToEntityId = p_ENTITY_ID)
                                    AND TR."DATE" BETWEEN v_DT_FROM_DATE AND v_DT_TO_DATE
                                    AND    TR.RemindDate is not null 
                                    AND   (TR.StatusId <>p_Status)
                                     AND (TR.RemindDate>= sysdate OR TA."DATE" + UserProfiles.TransactionProcessingPeriod >= sysdate ) ) 
	   WHERE  Id =v_I;


--------------- Delayed Transactions---------
	 		UPDATE TEMP_COUNTERS_ByPERIOD
SET DELAYED_COUNT =  (  SELECT COUNT(TR.Id)  FROM Transactions TR
INNER JOIN TransactionAssignments ON TR.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
WHERE  (TransactionCategoryId = p_Inbound OR TransactionCategoryId = p_Internal)
                                   AND(TransactionAssignments.ToEntityId = p_ENTITY_ID)
                                    AND TR."DATE" BETWEEN v_DT_FROM_DATE AND v_DT_TO_DATE 
                                    AND   (TR.StatusId <>p_Status)
                                     AND (TR.RemindDate < sysdate OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < sysdate ) ) 
       where Id =v_I;

   -------------------------------------- External Copies

	 	UPDATE TEMP_COUNTERS_ByPERIOD
SET Trans_Copies_COUNT =(SELECT  COUNT(TR.Id)
	FROM   Transactions TR 
INNER JOIN TransactionExternalCopies ExtCopies ON TR.Id = ExtCopies.TransactionId

	WHERE
	    (ExtCopies.EntityId = p_ENTITY_ID)
        AND TR."DATE" BETWEEN v_DT_FROM_DATE AND v_DT_TO_DATE )
	   where id =v_I;


--------Interanl 

	 	UPDATE TEMP_COUNTERS_ByPERIOD
SET Trans_Copies_COUNT =Trans_Copies_COUNT + (SELECT  COUNT(TR.Id)
	FROM   Transactions TR 
INNER JOIN TransactionCopies Copies ON TR.Id = Copies.TransactionId

	WHERE
	    (Copies.EntityId = p_ENTITY_ID)
        AND TR."DATE" BETWEEN v_DT_FROM_DATE AND v_DT_TO_DATE )
	   where id =v_I;

  v_I :=v_I +1;    
   END LOOP;
---------------------
------Result
OPEN p_cur FOR
SELECT id,
	From_date,TO_Date,
	NVL(MY_TRAN_COUNT,0) Transactions,
	NVL(DELAYED_COUNT,0) DELAYED,
	NVL(WITH_APPOITMENT_COUNT,0) WITH_APPOITMENT,
	NVL(Trans_Copies_COUNT,0) TRANS_PARTIES
FROM  TEMP_COUNTERS_ByPERIOD ;

    END USER_MOBILE_DASHBOARD_ENTITIES_ACCOMPLESHMENTS ;
/
--------USER_MOBILE_DASHBOARD_USER_ACCOMPLESHMENTS------
create or replace PROCEDURE USER_MOBILE_DASHBOARD_USER_ACCOMPLESHMENTS(
	p_ENTITY_ID		      "NUMBER" DEFAULT  -1,
    p_USER_ID             "NUMBER" DEFAULT  -1,
	p_Status               	Number,
    p_Inbound             	Number,
    p_Internal              Number,
    p_cur                 OUT SYS_REFCURSOR
    )
					
AS

BEGIN

INSERT INTO  TEMP_COUNTERS_ByPERIOD (MY_TRAN_COUNT,DELAYED_COUNT,WITH_APPOITMENT_COUNT,Trans_Copies_COUNT)
 values (0,0,0,0);

 ---MY TRansaction
		UPDATE TEMP_COUNTERS_ByPERIOD
SET MY_TRAN_COUNT =  (  SELECT COUNT( TR.Id)
								FROM Transactions TR    
                                INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
							    INNER JOIN UserProfiles ON TA.ToUserId = UserProfiles.Id 
									WHERE	 (TransactionCategoryId = p_Inbound OR TransactionCategoryId = p_Internal)
                                        AND(ToEntityId = p_ENTITY_ID) AND (TA.ToUserId =p_USER_ID)
                                        AND    (TR.StatusId NOT IN (p_Status))
                                        AND (TR.RemindDate >= sysdate  OR TA."DATE" + UserProfiles.TransactionProcessingPeriod >= SYSDATE) 
                                        AND (TR.RemindDate = null or TR.RemindDate is null));

--------------------WITH_APPOITMEN--------

		UPDATE TEMP_COUNTERS_ByPERIOD
SET WITH_APPOITMENT_COUNT = 
                        (SELECT COUNT(TR.Id)
								FROM Transactions TR 
						   INNER JOIN TransactionAssignments TA ON TA.TransactionId = TR.Id
                             INNER JOIN TransactionAssignments   TA ON TA.Id=TR.Id 
                              INNER JOIN UserProfiles ON TA.ToUserId = UserProfiles.Id

						     WHERE	 (TransactionCategoryId = p_Inbound OR TransactionCategoryId = p_Internal)
                                   AND(TA.ToEntityId = p_ENTITY_ID) AND (TA.ToUserId =p_USER_ID)
                                   AND (TR.StatusId NOT IN (p_Status)) 
                              AND (TR.RemindDate>= sysdate 
                              OR TA."DATE" + UserProfiles.TransactionProcessingPeriod>= SYSDATE)
                              AND (TR.RemindDate <> null or TR.RemindDate is not null));


--------------- Delayed Transactions---------
	 		UPDATE TEMP_COUNTERS_ByPERIOD
SET DELAYED_COUNT =  (  
SELECT COUNT(TR.Id)  
FROM Transactions TR
INNER JOIN TransactionAssignments ON TR.Id = TransactionAssignments.TransactionId
INNER JOIN UserProfiles ON TransactionAssignments.ToUserId = UserProfiles.Id
WHERE (TransactionCategoryId = p_Inbound OR TransactionCategoryId = p_Internal)
        AND(RemindDate < SYSDATE  OR TransactionAssignments."DATE" + UserProfiles.TransactionProcessingPeriod < SYSDATE)
        AND(TransactionAssignments.ToEntityId = p_ENTITY_ID) AND (TransactionAssignments.ToUserId =p_USER_ID)
        AND StatusId <> p_Status) ;

   -------------------------------------- Transaction Copiers

-------------------------------------- External Copies

	 	UPDATE TEMP_COUNTERS_ByPERIOD
SET Trans_Copies_COUNT =(SELECT  COUNT(TR.Id)
	FROM   Transactions TR 
INNER JOIN TransactionExternalCopies ExtCopies ON TR.Id = ExtCopies.TransactionId

	WHERE
	    (ExtCopies.UserId=p_USER_ID) AND (ExtCopies.EntityId = p_ENTITY_ID)
   );
--------Interanl 
	 	UPDATE TEMP_COUNTERS_ByPERIOD
SET Trans_Copies_COUNT =Trans_Copies_COUNT + (SELECT  COUNT(TR.Id)
	FROM   Transactions TR 
INNER JOIN TransactionCopies Copies ON TR.Id = Copies.TransactionId

	WHERE
	 (Copies.UserId=p_USER_ID)   AND(Copies.EntityId = p_ENTITY_ID)   );

------Result
OPEN p_cur FOR
SELECT id,

	NVL(MY_TRAN_COUNT,0) Transactions,
	NVL(DELAYED_COUNT,0) DELAYED,
	NVL(WITH_APPOITMENT_COUNT,0) WITH_APPOITMENT,
	NVL(Trans_Copies_COUNT,0) TRANS_PARTIES
FROM  TEMP_COUNTERS_ByPERIOD ;
END USER_MOBILE_DASHBOARD_USER_ACCOMPLESHMENTS;
/ 

create or replace PROCEDURE MERGE_DEPARTMENTS
(
  p_MergedEntityId IN NUMBER,
  p_BaseEntityId IN NUMBER,
  p_ManagerId IN NUMBER,
  p_UserId IN NUMBER
)
AS
BEGIN
  declare
   v_BaseEntityLineage NVARCHAR2(100);
   v_MergedEntityLineage NVARCHAR2(100);

   BEGIN

   SELECT Lineage INTO v_BaseEntityLineage FROM OrgUnits WHERE Id = p_BaseEntityId ;
   SELECT Lineage INTO v_MergedEntityLineage FROM OrgUnits WHERE Id = p_MergedEntityId ;

   UPDATE OrgUnits ORG
   SET ORG.Lineage = REPLACE(ORG.Lineage, v_MergedEntityLineage, v_BaseEntityLineage)
   WHERE ORG.Lineage LIKE '%' || v_MergedEntityLineage || '%' AND ORG.Id <> p_MergedEntityId;

   UPDATE OrgUnits
   SET ParentId = p_BaseEntityId
   WHERE ParentId = p_MergedEntityId;

   UPDATE UserProfileOrgUnits
   SET OrgUnit_Id = p_BaseEntityId 
   WHERE OrgUnit_Id = p_MergedEntityId;

   UPDATE Transactions 
   SET OrgUnitId = p_BaseEntityId
   WHERE OrgUnitId = p_MergedEntityId;

   UPDATE Transactions 
   SET EntityId = p_BaseEntityId
   WHERE EntityId = p_MergedEntityId;

   UPDATE TransactionAssignments
   SET FromEntityId = p_BaseEntityId 
   WHERE FromEntityId = p_MergedEntityId;

   UPDATE 
  (SELECT TransactionAssignments.ToUserId as ToU,TransactionAssignments.ToEntityId as ToE ,TransactionAssignments.TrayId as Tray , Transactions.TransactionCategoryId as TC
   FROM TransactionAssignments
   INNER JOIN Transactions
   ON TransactionAssignments.TransactionId = Transactions.Id
   WHERE Transactions.TransactionCategoryId <> 246 AND Transactions.StatusId <> 384 AND Transactions.StatusId <> 382 AND TransactionAssignments.ToEntityId = p_MergedEntityId
   ) t
   SET t.ToE = p_BaseEntityId , t.ToU = null , t.Tray = 5;

   UPDATE 
  (SELECT TransactionAssignments.ToUserId as ToU,TransactionAssignments.ToEntityId as ToE ,TransactionAssignments.TrayId as Tray , Transactions.TransactionCategoryId as TC
   FROM TransactionAssignments
   INNER JOIN Transactions
   ON TransactionAssignments.TransactionId = Transactions.Id
   WHERE Transactions.TransactionCategoryId <> 246 AND (Transactions.StatusId = 384 OR Transactions.StatusId = 382 ) AND TransactionAssignments.ToEntityId = p_MergedEntityId
   ) t
   SET t.ToE = p_BaseEntityId;

   UPDATE 
  (SELECT TransactionAssignments.ToEntityId as ToE , Transactions.TransactionCategoryId as TC 
   FROM TransactionAssignments
   INNER JOIN Transactions
   ON TransactionAssignments.TransactionId = Transactions.Id
   WHERE Transactions.TransactionCategoryId = 246 AND TransactionAssignments.ToEntityId = p_MergedEntityId
   ) t
   SET t.ToE = p_BaseEntityId;

   UPDATE TransactionCopies 
   SET EntityId = p_BaseEntityId
   WHERE EntityId = p_MergedEntityId;

   UPDATE TransactionCopies 
   SET FromEntityId = p_BaseEntityId
   WHERE FromEntityId = p_MergedEntityId;

   UPDATE TransactionExternalCopies 
   SET FromEntityId = p_BaseEntityId
   WHERE FromEntityId = p_MergedEntityId;

   UPDATE Tasks 
   SET FromOrgUnitId = p_BaseEntityId
   WHERE FromOrgUnitId = p_MergedEntityId;

   UPDATE Tasks 
   SET ToOrgUnitId = p_BaseEntityId
   WHERE ToOrgUnitId = p_MergedEntityId;

   UPDATE TransactionEntityDetails 
   SET EntityId = p_BaseEntityId
   WHERE EntityId = p_MergedEntityId;

   UPDATE TransactionFollowUps 
   SET EntityId = p_BaseEntityId
   WHERE EntityId = p_MergedEntityId;

   UPDATE TransactionPathDetails 
   SET OrgUnitId = p_BaseEntityId
   WHERE OrgUnitId = p_MergedEntityId;

   UPDATE TransactionPaths
   SET OrgUnitId = p_BaseEntityId
   WHERE OrgUnitId = p_MergedEntityId;

   UPDATE TransactionReservations
   SET EntityId = p_BaseEntityId
   WHERE EntityId = p_MergedEntityId;

   UPDATE UserDelegations
   SET OrgUnitId = p_BaseEntityId
   WHERE OrgUnitId = p_MergedEntityId;

   UPDATE DocumentInfo
   SET FromEntityId = p_BaseEntityId
   WHERE FromEntityId = p_MergedEntityId;

   UPDATE OrgUnits
   SET MANAGERID = p_ManagerId
   WHERE Id = p_BaseEntityId;

   UPDATE OrgUnits
   SET IsActive = 0 , ModefiedBy = p_UserId , ModefiedOn = SYSDATE 
   WHERE Id = p_MergedEntityId;
   END;
END;
/
create or replace PROCEDURE ERP_ENTITY_ADD_VIEW
AS
v_TimeStamp date;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPAddEntityTimeStamp';

      open c1 for
      SELECT * 
        FROM ENTITY_ADD_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ERP_ENTITY_MOVE_VIEW
AS
v_TimeStamp date;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPMoveEntityTimeStamp';

      open c1 for
      SELECT * 
        FROM ENTITY_MOVE_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ERP_ENTITY_UPDATE_NAME_VIEW
AS
v_TimeStamp date;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPUpdateEntityTimeStamp';

      open c1 for
      SELECT * 
        FROM ENTITY_UPDATE_NAME_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ERP_USER_DELEGATION_VIEW
AS
v_TimeStamp date null;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPDelegationUsersTimeStamp';

      open c1 for
      SELECT * 
        FROM USER_DELEGATION_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ERP_USERS_ADD_VIEW
AS
v_TimeStamp date;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPAddUsersTimeStamp';

      open c1 for
      SELECT * 
        FROM USERS_ADD_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ERP_USERS_DELETE_VIEW
AS
v_TimeStamp date;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPDeleteUsersTimeStamp';

      open c1 for
      SELECT * 
        FROM USERS_DELETE_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ERP_USERS_MOVE_VIEW
AS
v_TimeStamp date;
c1 SYS_REFCURSOR; 
BEGIN

      SELECT TO_DATE(VALUE, 'mm/dd/yyyy HH:MI:SS AM') INTO v_TimeStamp FROM settings WHERE KEY = 'ERPMoveUsersTimeStamp';

      open c1 for
      SELECT * 
        FROM USERS_MOVE_VIEW us 
        WHERE (v_TimeStamp is null or us."TimeStamp" > v_TimeStamp);

        DBMS_SQL.RETURN_RESULT(c1);
END;
/
create or replace PROCEDURE ADMIN_DELETE_USER_ERP
(
  p_UserProfileId      IN NUMBER,  
  p_ExternalOrgUnitId  IN NUMBER,
  p_LoggedInUser       IN NUMBER,
  p_TrayOrgUnit		   IN NUMBER,
  p_TraySaved		   IN NUMBER,
  p_TrayMyTransactions IN NUMBER,
  p_TrayDraftOutbound  IN NUMBER
)
AS
v_OrgUnitId number;
v_MainOrgUnitId number;
BEGIN
    SELECT Id Into v_OrgUnitId FROM OrgUnits where ExternalId = p_ExternalOrgUnitId;
    SELECT MAINORGUNITID INTO v_MainOrgUnitId FROM userprofiles where Id = p_ExternalOrgUnitId;

   IF v_MainOrgUnitId <> v_OrgUnitId THEN
   BEGIN
      UPDATE TransactionAssignments
         SET TrayId = p_TrayOrgUnit,
             ToUserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TrayId = p_TrayMyTransactions
        AND ToUserId = p_UserProfileId
        AND ToEntityId = v_OrgUnitId;

        UPDATE TransactionAssignments
         SET TrayId = p_TrayOrgUnit,
             ToUserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  TrayId = p_TrayDraftOutbound
        AND ToUserId = p_UserProfileId
        AND ToEntityId = v_OrgUnitId;

      UPDATE TransactionCopies
         SET UserId = NULL,
             ModefiedBy = p_LoggedInUser,
             ModefiedOn = SYSDATE
       WHERE  UserId = p_UserProfileId
        AND EntityId = v_OrgUnitId;
        
        DELETE FROM UserProfileOrgUnits 
        WHERE UserProfile_Id = p_UserProfileId
            AND OrgUnit_Id = v_OrgUnitId;

   END;
   END IF;
END;