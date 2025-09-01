namespace MCS.DataAccess.OracleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ini : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ACTIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISASCOPY = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(nullable: false),
                        TYPE_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.LOOKUPS", t => t.TYPE_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id")
                .Index(t => t.TYPE_ID, name: "IX_Type_Id");
            
            CreateTable(
                "dbo.LOCALIZATIONIDENTIFIERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LOCALIZATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CULTUREID = c.Int(nullable: false),
                        TEXT = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CULTURES", t => t.CULTUREID, cascadeDelete: false)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.CULTUREID, name: "IX_CultureId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.CULTURES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SHORTNAME = c.String(maxLength: 50),
                        NAMEID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.NAMEID)
                .Index(t => t.NAMEID, name: "IX_NameId");
            
            CreateTable(
                "dbo.LOOKUPS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CATEGORYID = c.Int(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        SORT = c.Int(nullable: false),
                        ENUMREFERENCE = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LOOKUPLOCALIZATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TEXT = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        CULTURE_ID = c.Int(),
                        LOOKUP_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CULTURES", t => t.CULTURE_ID)
                .ForeignKey("dbo.LOOKUPS", t => t.LOOKUP_ID)
                .Index(t => t.CULTURE_ID, name: "IX_Culture_Id")
                .Index(t => t.LOOKUP_ID, name: "IX_Lookup_Id");
            
            CreateTable(
                "dbo.ASPNETROLES",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NAME = c.String(nullable: false, maxLength: 256),
                        DISCRIMINATOR = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ASPNETUSERS",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        EMAIL = c.String(maxLength: 256),
                        EMAILCONFIRMED = c.Boolean(nullable: false),
                        PASSWORDHASH = c.String(),
                        SECURITYSTAMP = c.String(),
                        PHONENUMBER = c.String(),
                        PHONENUMBERCONFIRMED = c.Boolean(nullable: false),
                        TWOFACTORENABLED = c.Boolean(nullable: false),
                        LOCKOUTENDDATEUTC = c.DateTime(),
                        LOCKOUTENABLED = c.Boolean(nullable: false),
                        ACCESSFAILEDCOUNT = c.Int(nullable: false),
                        USERNAME = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ASPNETUSERCLAIMS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.String(nullable: false, maxLength: 128),
                        CLAIMTYPE = c.String(),
                        CLAIMVALUE = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ASPNETUSERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.ASPNETUSERLOGINS",
                c => new
                    {
                        LOGINPROVIDER = c.String(nullable: false, maxLength: 128),
                        PROVIDERKEY = c.String(nullable: false, maxLength: 128),
                        USERID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LOGINPROVIDER, t.PROVIDERKEY, t.USERID })
                .ForeignKey("dbo.ASPNETUSERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.ASSIGNMENTGROUPDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ORGUNIT_ID = c.Int(),
                        USERPROFILE_ID = c.Int(),
                        ASSIGNMENTGROUP_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNIT_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERPROFILE_ID)
                .ForeignKey("dbo.ASSIGNMENTGROUPS", t => t.ASSIGNMENTGROUP_ID)
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id")
                .Index(t => t.USERPROFILE_ID, name: "IX_UserProfile_Id")
                .Index(t => t.ASSIGNMENTGROUP_ID, name: "IX_AssignmentGroup_Id");
            
            CreateTable(
                "dbo.ORGUNITS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MANAGERID = c.Int(nullable: false),
                        ASSIGNMENTPAPERID = c.Int(),
                        PARENTID = c.Int(),
                        ISACTIVE = c.Boolean(nullable: false),
                        NUMBER = c.String(maxLength: 50),
                        BARCODE = c.String(maxLength: 50),
                        ISVIRTUALUNIT = c.Boolean(nullable: false),
                        TRANSACTIONSPROCESSINGPERIOD = c.Int(nullable: false),
                        ISDELETED = c.Boolean(nullable: false),
                        JOINTOGENERALCOUNTER = c.Boolean(nullable: false),
                        LINEAGE = c.String(),
                        EXTERNALID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        COUNTER_ID = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPERID)
                .ForeignKey("dbo.COUNTERS", t => t.COUNTER_ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.PARENTID)
                .Index(t => t.ASSIGNMENTPAPERID, name: "IX_AssignmentPaperId")
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.COUNTER_ID, name: "IX_Counter_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.ASSIGNMENTPAPERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISCREATEGROUPALLOWED = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ASSIGNMENTPAPERACTIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ACTIONID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ASSIGNMENTPAPER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPER_ID)
                .Index(t => t.ACTIONID, name: "IX_ActionId")
                .Index(t => t.ASSIGNMENTPAPER_ID, name: "IX_AssignmentPaper_Id");
            
            CreateTable(
                "dbo.ASSIGNMENTPAPERBENEFICIARIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ORGUNITID = c.Int(nullable: false),
                        USERID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ASSIGNMENTPAPER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .ForeignKey("dbo.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPER_ID)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ASSIGNMENTPAPER_ID, name: "IX_AssignmentPaper_Id");
            
            CreateTable(
                "dbo.USERPROFILES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDENTITYID = c.String(maxLength: 128),
                        USERNAME = c.String(maxLength: 50),
                        ISACTIVE = c.Boolean(nullable: false),
                        TITLEID = c.Int(nullable: false),
                        CATEGORYID = c.Int(),
                        TRANSACTIONPROCESSINGPERIOD = c.Int(nullable: false),
                        PHONENUMBER = c.String(maxLength: 20),
                        EMAIL = c.String(maxLength: 50),
                        ISDELETED = c.Boolean(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        USERNATIONALID = c.String(nullable: false),
                        ALLOWMOBILE = c.Boolean(nullable: false),
                        LASTACTIVITY = c.DateTimeOffset(precision: 7),
                        STATUS = c.Int(),
                        MAINORGUNITID = c.Int(nullable: false),
                        GENDER = c.Int(nullable: false),
                        ISMANAGER = c.Boolean(nullable: false),
                        EXTERNALID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        DIRECTMANAGER_ID = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                        USERIMAGE_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERCATEGORIES", t => t.CATEGORYID)
                .ForeignKey("dbo.USERPROFILES", t => t.DIRECTMANAGER_ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.LOOKUPS", t => t.TITLEID, cascadeDelete: false)
                .ForeignKey("dbo.DOCUMENTS", t => t.USERIMAGE_ID)
                .Index(t => t.TITLEID, name: "IX_TitleId")
                .Index(t => t.CATEGORYID, name: "IX_CategoryId")
                .Index(t => t.DIRECTMANAGER_ID, name: "IX_DirectManager_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id")
                .Index(t => t.USERIMAGE_ID, name: "IX_UserImage_Id");
            
            CreateTable(
                "dbo.CHATROOMALLOWEDUSERS",
                c => new
                    {
                        ROOMID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => new { t.ROOMID, t.USERID })
                //.ForeignKey("dbo.CHATROOMS", t => t.ROOMID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                //.Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.CHATROOMS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LASTNUDGED = c.DateTimeOffset(precision: 7),
                        NAME = c.String(maxLength: 200),
                        CLOSED = c.Boolean(nullable: false),
                        PRIVATE = c.Boolean(nullable: false),
                        ONETOONE = c.Boolean(nullable: false),
                        TRANSACTIONID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.NAME)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId");
            
            CreateTable(
                "dbo.CHATMESSAGES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CONTENT = c.String(),
                        WHEN = c.DateTimeOffset(nullable: false, precision: 7),
                        HTMLENCODED = c.Boolean(nullable: false),
                        MESSAGETYPE = c.Int(nullable: false),
                        HTMLCONTENT = c.String(),
                        ROOMID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        IMAGEURL = c.String(),
                        SOURCE = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                //.ForeignKey("dbo.CHATROOMS", t => t.ROOMID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.WHEN)
                //.Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.CHATMESSAGESSTATUS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LASTUPDATEDDATE = c.DateTimeOffset(nullable: false, precision: 7),
                        ROOMID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        MESSAGEID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                //.ForeignKey("dbo.CHATMESSAGES", t => t.MESSAGEID, cascadeDelete: false)
                //.ForeignKey("dbo.CHATROOMS", t => t.ROOMID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                //.Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
                //.Index(t => t.MESSAGEID, name: "IX_MessageId");
            
            CreateTable(
                "dbo.CHATROOMOWNERS",
                c => new
                    {
                        ROOMID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => new { t.ROOMID, t.USERID })
                .ForeignKey("dbo.CHATROOMS", t => t.ROOMID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.TRANSACTIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        NUMBER = c.Long(nullable: false),
                        YEAR = c.Int(nullable: false),
                        YEARH = c.Int(nullable: false),
                        DOCUMENTNUMBER = c.String(),
                        REMARKS = c.String(),
                        SUBJECT = c.String(),
                        PRINTEDDELIVERYREPORT = c.Boolean(nullable: false),
                        DELIVERYREPORTNUMBER = c.String(maxLength: 50),
                        SIGNEDBYUSERID = c.Int(),
                        STATUSID = c.Int(nullable: false),
                        REJECTIONREASON = c.String(),
                        TRANSACTIONCATEGORYID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        ORGUNITID = c.Int(nullable: false),
                        SUGGESTEDTOPICID = c.Int(),
                        ENTITYID = c.Int(),
                        TOUSERID = c.Int(),
                        PRIORITYID = c.Int(nullable: false),
                        CONFIDENTIALITYID = c.Int(nullable: false),
                        SOURCETYPEID = c.Int(),
                        TRANSACTIONTYPEID = c.Int(),
                        LETTERTYPEID = c.Int(),
                        EXTERNALPARTYID = c.Int(),
                        EXTERNALPARTYMANAGERID = c.Int(),
                        MAINDOCUMENTID = c.Int(),
                        REMINDDATE = c.DateTime(),
                        REMINDDATEH = c.String(maxLength: 20),
                        OUTBOUNDDRAFTID = c.Int(),
                        OUTBOUNDDRAFTEDITORTYPE = c.Int(),
                        ISDELETED = c.Boolean(nullable: false),
                        ISSIGNED = c.Boolean(nullable: false),
                        DELIVERYMETHODID = c.Int(nullable: false),
                        INBOUNDDATEH = c.String(),
                        POSTCODE = c.String(),
                        POBOX = c.String(),
                        ISDRAFT = c.Boolean(nullable: false),
                        ISFORINDIVIDUAL = c.Boolean(nullable: false),
                        SAVEDREASON = c.String(),
                        DELIVERYNUMBER = c.String(maxLength: 30),
                        REPORTERID = c.Int(),
                        INBOUNDINTENDEDPERSON = c.String(),
                        RESERVATIONID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PERMISSIONS", t => t.CONFIDENTIALITYID, cascadeDelete: false)
                .ForeignKey("dbo.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.EXTERNALPARTYID)
                .ForeignKey("dbo.EXTERNALPARTYMANAGERS", t => t.EXTERNALPARTYMANAGERID)
                .ForeignKey("dbo.LETTERTYPES", t => t.LETTERTYPEID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.MAINDOCUMENTID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.PRIORITIES", t => t.PRIORITYID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONRESERVATIONS", t => t.RESERVATIONID)
                .ForeignKey("dbo.USERPROFILES", t => t.SIGNEDBYUSERID)
                .ForeignKey("dbo.LOOKUPS", t => t.STATUSID)
                .ForeignKey("dbo.SUGGESTEDTOPICS", t => t.SUGGESTEDTOPICID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("dbo.LOOKUPS", t => t.TRANSACTIONCATEGORYID)
                .ForeignKey("dbo.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .Index(t => t.SIGNEDBYUSERID, name: "IX_SignedByUserId")
                .Index(t => t.STATUSID, name: "IX_StatusId")
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.SUGGESTEDTOPICID, name: "IX_SuggestedTopicId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.TOUSERID, name: "IX_ToUserId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.CONFIDENTIALITYID, name: "IX_ConfidentialityId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.LETTERTYPEID, name: "IX_LetterTypeId")
                .Index(t => t.EXTERNALPARTYID, name: "IX_ExternalPartyId")
                .Index(t => t.EXTERNALPARTYMANAGERID, name: "IX_ExternalPartyManagerId")
                .Index(t => t.MAINDOCUMENTID, name: "IX_MainDocumentId")
                .Index(t => t.DELIVERYMETHODID, name: "IX_DeliveryMethodId")
                .Index(t => t.RESERVATIONID, name: "IX_ReservationId");
            
            CreateTable(
                "dbo.TRANSACTIONASSIGNMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRAYID = c.Int(nullable: false),
                        FROMUSERID = c.Int(nullable: false),
                        TOUSERID = c.Int(),
                        PHYSICALUSERID = c.Int(nullable: false),
                        TRANSACTIONID = c.Int(nullable: false),
                        ACTIONID = c.Int(),
                        FROMENTITYID = c.Int(nullable: false),
                        TOENTITYID = c.Int(nullable: false),
                        PHYSICALENTITYID = c.Int(nullable: false),
                        DESCRIPTION = c.String(),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        PHYSICALDATE = c.DateTime(nullable: false),
                        PHYSICALDATEH = c.String(),
                        VIEWED = c.Boolean(nullable: false),
                        ISPOPULARIAZATION = c.Boolean(nullable: false),
                        DELIVERYMETHODID = c.Int(nullable: false),
                        TRANSACTIONPATHID = c.Int(),
                        CURRENTPATHSTEP = c.Int(),
                        DUEDATE = c.DateTime(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID)
                .ForeignKey("dbo.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("dbo.ORGUNITS", t => t.PHYSICALENTITYID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.PHYSICALUSERID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("dbo.TRANSACTIONPATHS", t => t.TRANSACTIONPATHID)
                .ForeignKey("dbo.TRAYS", t => t.TRAYID, cascadeDelete: false)
                .Index(t => t.TRAYID, name: "IX_TrayId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.TOUSERID, name: "IX_ToUserId")
                .Index(t => t.PHYSICALUSERID, name: "IX_PhysicalUserId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TOENTITYID, name: "IX_ToEntityId")
                .Index(t => t.PHYSICALENTITYID, name: "IX_PhysicalEntityId")
                .Index(t => t.DELIVERYMETHODID, name: "IX_DeliveryMethodId")
                .Index(t => t.TRANSACTIONPATHID, name: "IX_TransactionPathId");
            
            CreateTable(
                "dbo.TASKS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TOUSERID = c.Int(nullable: false),
                        TOORGUNITID = c.Int(nullable: false),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        DELIVERYDATE = c.DateTime(nullable: false),
                        DELIVERYDATEH = c.String(maxLength: 20),
                        ISEXCLUSIVE = c.Boolean(nullable: false),
                        TASKDESCRIPTION = c.String(),
                        STATUSDESCRIPTION = c.String(maxLength: 500),
                        PARENTID = c.Int(),
                        STATUSID = c.Int(nullable: false),
                        LEVELLIMITATION = c.Int(),
                        FROMUSERID = c.Int(nullable: false),
                        FROMORGUNITID = c.Int(nullable: false),
                        TRANSACTIONID = c.Int(nullable: false),
                        ACTIONID = c.Int(),
                        ISDELETED = c.Boolean(nullable: false),
                        NUMBEROFNOTIFICATIONS = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        TRANSACTIONASSIGNMENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID, cascadeDelete: false)
                .ForeignKey("dbo.TASKS", t => t.PARENTID)
                .ForeignKey("dbo.LOOKUPS", t => t.STATUSID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOORGUNITID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("dbo.TRANSACTIONASSIGNMENTS", t => t.TRANSACTIONASSIGNMENT_ID)
                .Index(t => t.TOUSERID, name: "IX_ToUserId")
                .Index(t => t.TOORGUNITID, name: "IX_ToOrgUnitId")
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.STATUSID, name: "IX_StatusId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMORGUNITID, name: "IX_FromOrgUnitId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId")
                .Index(t => t.TRANSACTIONASSIGNMENT_ID, name: "IX_TransactionAssignment_Id");
            
            CreateTable(
                "dbo.TASKREMINDERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        TASK_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TASKS", t => t.TASK_ID)
                .Index(t => t.TASK_ID, name: "IX_Task_Id");
            
            CreateTable(
                "dbo.TASKSATTACHMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TASKID = c.Int(nullable: false),
                        DOCUMENTINFOID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENTINFOID)
                .ForeignKey("dbo.TASKS", t => t.TASKID)
                .Index(t => t.TASKID, name: "IX_TaskId")
                .Index(t => t.DOCUMENTINFOID, name: "IX_DocumentInfoId");
            
            CreateTable(
                "dbo.DOCUMENTINFO",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NAME = c.String(maxLength: 200),
                        SIZE = c.Long(nullable: false),
                        MIMETYPE = c.String(maxLength: 100),
                        ECMID = c.String(maxLength: 50),
                        FROMUSERID = c.Int(),
                        FROMENTITYID = c.Int(),
                        TRANSACTIONID = c.Int(),
                        DOCUMENTTYPE = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        DOCUMENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTS", t => t.DOCUMENT_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID)
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.DOCUMENT_ID, name: "IX_Document_Id");
            
            CreateTable(
                "dbo.DOCUMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CONTENT = c.Binary(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TRANSACTIONPATHS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NAME = c.String(),
                        USERID = c.Int(),
                        ORGUNITID = c.Int(nullable: false),
                        TRANSACTIONTYPEID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.LOOKUPS", t => t.TRANSACTIONTYPEID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId");
            
            CreateTable(
                "dbo.TRANSACTIONPATHDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONPATHID = c.Int(nullable: false),
                        USERID = c.Int(),
                        ORGUNITID = c.Int(nullable: false),
                        ACTIONID = c.Int(nullable: false),
                        SORT = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONPATHS", t => t.TRANSACTIONPATHID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .Index(t => t.TRANSACTIONPATHID, name: "IX_TransactionPathId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "dbo.TRAYS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SORT = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        NAME_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.NAME_ID)
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "dbo.ATTACHMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TYPEID = c.Int(nullable: false),
                        COUNT = c.Int(nullable: false),
                        DESCRIPTION = c.String(),
                        ATTACHMENTSOURCE = c.Int(nullable: false),
                        TRANSACTIONID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        DOCUMENTINFO_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENTINFO_ID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.ATTACHMENTTYPES", t => t.TYPEID, cascadeDelete: false)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.DOCUMENTINFO_ID, name: "IX_DocumentInfo_Id");
            
            CreateTable(
                "dbo.ATTACHMENTTYPES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PRINTBARCODE = c.Boolean(nullable: false),
                        ARCHIVABLE = c.Boolean(nullable: false),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.PERMISSIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CODE = c.String(maxLength: 100),
                        ISUSERDEFINED = c.Boolean(nullable: false),
                        WEIGHT = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        NAME_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.NAME_ID)
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "dbo.GROUPS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISUSERDEFINED = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        GROUPNAME_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.GROUPNAME_ID)
                .Index(t => t.GROUPNAME_ID, name: "IX_GroupName_Id");
            
            CreateTable(
                "dbo.USERGROUPS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GROUPID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GROUPS", t => t.GROUPID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.GROUPID, name: "IX_GroupId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.TRANSACTIONCOPIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(),
                        ENTITYID = c.Int(),
                        FROMUSERID = c.Int(),
                        FROMENTITYID = c.Int(),
                        TRANSACTIONID = c.Int(nullable: false),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        STATUS = c.Int(nullable: false),
                        ACTIONID = c.Int(nullable: false),
                        ISSENT = c.Int(),
                        SENTDATE = c.DateTime(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "dbo.EXPLANATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONID = c.Int(nullable: false),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(),
                        EXPLANATIONEDITORTYPE = c.Int(nullable: false),
                        PERMISSIONID = c.Int(nullable: false),
                        FROMUSERID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        DOCUMENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENT_ID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID, cascadeDelete: false)
                .ForeignKey("dbo.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.DOCUMENT_ID, name: "IX_Document_Id");
            
            CreateTable(
                "dbo.TRANSACTIONEXTERNALCOPIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(),
                        ENTITYID = c.Int(),
                        FROMUSERID = c.Int(),
                        FROMENTITYID = c.Int(),
                        TRANSACTIONID = c.Int(nullable: false),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(),
                        VIEWED = c.Boolean(nullable: false),
                        ACTIONID = c.Int(nullable: false),
                        STATUS = c.Int(nullable: false),
                        SENDEMAIL = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.ENTITYID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.EXTERNALPARTYMANAGERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "dbo.EXTERNALPARTIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NUMBER = c.String(maxLength: 20),
                        EMAIL = c.String(maxLength: 50),
                        PHONENUMBER = c.String(maxLength: 20),
                        FAX = c.String(maxLength: 20),
                        ISVIRTUAL = c.Boolean(nullable: false),
                        PARTYTYPE = c.Int(nullable: false),
                        PARENTID = c.Int(),
                        YASSERREGISTERED = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(),
                        LINEAGE = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ADDRESS_ID = c.Int(),
                        NAME_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.ADDRESS_ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.NAME_ID)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.ADDRESS_ID, name: "IX_Address_Id")
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "dbo.EXTERNALPARTYMANAGERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        NAME_ID = c.Int(),
                        EXTERNALPARTY_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.NAME_ID)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.EXTERNALPARTY_ID, cascadeDelete: false)
                .Index(t => t.NAME_ID, name: "IX_Name_Id")
                .Index(t => t.EXTERNALPARTY_ID, name: "IX_ExternalParty_Id");
            
            CreateTable(
                "dbo.EXTERNALPARTYATTACHMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PARTYID = c.Int(nullable: false),
                        NAME = c.String(),
                        DOCUMENTINFOID = c.Int(nullable: false),
                        TRANSACTIONEXTERNALCOPYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENTINFOID, cascadeDelete: false)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.PARTYID)
                .ForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", t => t.TRANSACTIONEXTERNALCOPYID)
                .Index(t => t.PARTYID, name: "IX_PartyId")
                .Index(t => t.DOCUMENTINFOID, name: "IX_DocumentInfoId")
                .Index(t => t.TRANSACTIONEXTERNALCOPYID, name: "IX_TransactionExternalCopyId");
            
            CreateTable(
                "dbo.TRANSACTIONFOLLOWUPS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        ENTITYID = c.Int(nullable: false),
                        DATETO = c.DateTime(),
                        DATETOH = c.String(),
                        ISDELETED = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ENTITYID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId");
            
            CreateTable(
                "dbo.LETTERTYPES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LETTERLISTTYPE = c.Int(nullable: false),
                        ISPOPULARIZATION = c.Boolean(nullable: false),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.TRANSACTIONLINKS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TYPEID = c.Int(nullable: false),
                        TRANSACTIONID = c.Int(nullable: false),
                        TOTRANSACTIONID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TOTRANSACTIONID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.LINKS", t => t.TYPEID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.TOTRANSACTIONID, name: "IX_ToTransactionId");
            
            CreateTable(
                "dbo.LINKS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.TRANSACTIONNAMES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONID = c.Int(nullable: false),
                        NAMEID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NAMES", t => t.NAMEID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.NAMEID, name: "IX_NameId");
            
            CreateTable(
                "dbo.NAMES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CIVILID = c.String(maxLength: 10),
                        NATIONALITYID = c.Int(),
                        FIRSTNAME = c.String(maxLength: 120),
                        MOBILENUMBER = c.String(maxLength: 20),
                        PHONE = c.String(maxLength: 15),
                        EMAIL = c.String(maxLength: 150),
                        ADDRESS = c.String(maxLength: 100),
                        OTHERINFORMATION = c.String(maxLength: 200),
                        TITLEID = c.Int(),
                        RELATIVERELATION = c.String(),
                        CITY = c.String(),
                        GENDER = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.NATIONALITYID)
                .ForeignKey("dbo.LOOKUPS", t => t.TITLEID)
                .Index(t => t.NATIONALITYID, name: "IX_NationalityId")
                .Index(t => t.TITLEID, name: "IX_TitleId");
            
            CreateTable(
                "dbo.PRIORITIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HASDATE = c.Boolean(nullable: false),
                        LATEFORENTITY = c.Int(nullable: false),
                        LATEFORUSER = c.Int(nullable: false),
                        SORT = c.Int(nullable: false),
                        HASPRIORITYEXCEPTIONS = c.Boolean(nullable: false),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.PRIORITYEXCEPTIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PRIORITYID = c.Int(nullable: false),
                        ORGUNITID = c.Int(nullable: false),
                        USERPROFILEID = c.Int(nullable: false),
                        LATEONUSERSAFTER = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.PRIORITIES", t => t.PRIORITYID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERPROFILEID, cascadeDelete: false)
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId");
            
            CreateTable(
                "dbo.TRANSACTIONRESERVATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        ENTITYID = c.Int(nullable: false),
                        COUNT = c.Int(nullable: false),
                        REASON = c.String(),
                        TRANSACTIONCATEGORYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("dbo.LOOKUPS", t => t.TRANSACTIONCATEGORYID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId");
            
            CreateTable(
                "dbo.TRANSACTIONSUBJECTCLASSIFICATI",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SUBJECTCLASSIFICATIONID = c.Int(nullable: false),
                        TRANSACTIONID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SUBJECTCLASSIFICATIONS", t => t.SUBJECTCLASSIFICATIONID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .Index(t => t.SUBJECTCLASSIFICATIONID, name: "IX_SubjectClassificationId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId");
            
            CreateTable(
                "dbo.SUBJECTCLASSIFICATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ISGROUP = c.Boolean(nullable: false),
                        PARENTID = c.Int(),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.SUBJECTCLASSIFICATIONS", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.SUBJECTORGUNITS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ORGUNITID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        SUBJECTCLASSIFICATION_ID = c.Int(),
                        SUGGESTEDTOPIC_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.SUBJECTCLASSIFICATIONS", t => t.SUBJECTCLASSIFICATION_ID, cascadeDelete: false)
                .ForeignKey("dbo.SUGGESTEDTOPICS", t => t.SUGGESTEDTOPIC_ID, cascadeDelete: false)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.SUBJECTCLASSIFICATION_ID, name: "IX_SubjectClassification_Id")
                .Index(t => t.SUGGESTEDTOPIC_ID, name: "IX_SuggestedTopic_Id");
            
            CreateTable(
                "dbo.SUGGESTEDTOPICS",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ISGROUP = c.Boolean(nullable: false),
                        PARENTID = c.Int(),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.SUGGESTEDTOPICS", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.TRANSACTIONTYPES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PERMISSIONID = c.Int(nullable: false),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ABBREVIATION_ID = c.Int(),
                        COLOR_ID = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.ABBREVIATION_ID)
                .ForeignKey("dbo.LOOKUPS", t => t.COLOR_ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: false)
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.ABBREVIATION_ID, name: "IX_Abbreviation_Id")
                .Index(t => t.COLOR_ID, name: "IX_Color_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.CHATROOMUSERS",
                c => new
                    {
                        ROOMID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => new { t.ROOMID, t.USERID })
                .ForeignKey("dbo.CHATROOMS", t => t.ROOMID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.USERCATEGORIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        CATEGORYNAME_ID = c.Int(),
                        PERMISSION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.CATEGORYNAME_ID)
                .ForeignKey("dbo.PERMISSIONS", t => t.PERMISSION_ID)
                .Index(t => t.CATEGORYNAME_ID, name: "IX_CategoryName_Id")
                .Index(t => t.PERMISSION_ID, name: "IX_Permission_Id");
            
            CreateTable(
                "dbo.USERCATEGORYTRAYS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERCATEGORYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        TARY_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TRAYS", t => t.TARY_ID)
                .ForeignKey("dbo.USERCATEGORIES", t => t.USERCATEGORYID, cascadeDelete: false)
                .Index(t => t.USERCATEGORYID, name: "IX_UserCategoryId")
                .Index(t => t.TARY_ID, name: "IX_Tary_Id");
            
            CreateTable(
                "dbo.CHATCLIENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERAGENT = c.String(),
                        NAME = c.String(),
                        LASTACTIVITY = c.DateTimeOffset(nullable: false, precision: 7),
                        LASTCLIENTACTIVITY = c.DateTimeOffset(precision: 7),
                        USERID = c.Int(nullable: false),
                        CONNECTIONID = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.USERPERMISSIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERPROFILEID = c.Int(nullable: false),
                        PERMISSIONID = c.Int(nullable: false),
                        GROUPID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => new { t.ID, t.USERPROFILEID, t.PERMISSIONID })
                .ForeignKey("dbo.GROUPS", t => t.GROUPID, cascadeDelete: false)
                .ForeignKey("dbo.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERPROFILEID, cascadeDelete: false)
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.GROUPID, name: "IX_GroupId");
            
            CreateTable(
                "dbo.BARCODEDESIGNS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HTML = c.String(maxLength: 4000, unicode: false),
                        ISGENERAL = c.Boolean(nullable: false),
                        TYPEID = c.Int(nullable: false),
                        WIDTH = c.Int(nullable: false),
                        HEIGHT = c.Int(nullable: false),
                        ATTACHMENTHTML = c.String(maxLength: 4000, unicode: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ORGUNIT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.TYPEID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNIT_ID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
            CreateTable(
                "dbo.COUNTERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISGENERAL = c.Boolean(nullable: false),
                        YEAR = c.String(),
                        RESETBYYEAR = c.Boolean(nullable: false),
                        OWNERENTITYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        DESCRIPTION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.DESCRIPTION_ID)
                .Index(t => t.DESCRIPTION_ID, name: "IX_Description_Id");
            
            CreateTable(
                "dbo.COUNTERDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        INITIALVALUE = c.Int(nullable: false),
                        COUNT = c.Int(nullable: false),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        TRANSACTIONTYPEID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        COUNTER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.COUNTERS", t => t.COUNTER_ID)
                .ForeignKey("dbo.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.COUNTER_ID, name: "IX_Counter_Id");
            
            CreateTable(
                "dbo.ORGUNITLINKS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        FROMENTITY_ID = c.Int(),
                        TOENTITY_ID = c.Int(),
                        ORGUNIT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITY_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOENTITY_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNIT_ID, cascadeDelete: false)
                .Index(t => t.FROMENTITY_ID, name: "IX_FromEntity_Id")
                .Index(t => t.TOENTITY_ID, name: "IX_ToEntity_Id")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
            CreateTable(
                "dbo.REPORTERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TOENTITYID = c.Int(nullable: false),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISDELETED = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        TEXT = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOENTITYID)
                .Index(t => t.TOENTITYID, name: "IX_ToEntityId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.TRANSACTIONDELIVERYREPORTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        NUMBER = c.String(maxLength: 50),
                        TRANSACTIONASSIGNMENTHISTORYID = c.Int(),
                        TRANSACTIONHISTORYID = c.Int(),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 50),
                        TRANSACTIONID = c.Int(nullable: false),
                        DOCUMENTID = c.Int(),
                        REPORTERID = c.Int(),
                        TRANSACTIONEXTERNALCOPYID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENTID)
                .ForeignKey("dbo.REPORTERS", t => t.REPORTERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", t => t.TRANSACTIONASSIGNMENTHISTORYID)
                .ForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", t => t.TRANSACTIONEXTERNALCOPYID)
                .ForeignKey("dbo.TRANSACTIONHISTORIES", t => t.TRANSACTIONHISTORYID)
                .Index(t => t.TRANSACTIONASSIGNMENTHISTORYID, name: "IX_TransactionAssignmentHistoryId")
                .Index(t => t.TRANSACTIONHISTORYID, name: "IX_TransactionHistoryId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.DOCUMENTID, name: "IX_DocumentId")
                .Index(t => t.REPORTERID, name: "IX_ReporterId")
                .Index(t => t.TRANSACTIONEXTERNALCOPYID, name: "IX_TransactionExternalCopyId");
            
            CreateTable(
                "dbo.TRANSACTIONASSIGNMENTHISTORIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRAYID = c.Int(nullable: false),
                        FROMUSERID = c.Int(nullable: false),
                        TOUSERID = c.Int(),
                        TRANSACTIONID = c.Int(),
                        ACTIONID = c.Int(),
                        FROMENTITYID = c.Int(nullable: false),
                        TOENTITYID = c.Int(nullable: false),
                        DESCRIPTION = c.String(),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        EXPLANATIONID = c.Int(),
                        USERDELEGATIONID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ACTIONS", t => t.ACTIONID)
                .ForeignKey("dbo.EXPLANATIONS", t => t.EXPLANATIONID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITYID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSERID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("dbo.TRAYS", t => t.TRAYID, cascadeDelete: false)
                .ForeignKey("dbo.USERDELEGATIONS", t => t.USERDELEGATIONID)
                .Index(t => t.TRAYID, name: "IX_TrayId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.TOUSERID, name: "IX_ToUserId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TOENTITYID, name: "IX_ToEntityId")
                .Index(t => t.EXPLANATIONID, name: "IX_ExplanationId")
                .Index(t => t.USERDELEGATIONID, name: "IX_UserDelegationId");
            
            CreateTable(
                "dbo.USERDELEGATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FROMDATE = c.DateTime(nullable: false),
                        TODATE = c.DateTime(nullable: false),
                        FROMDATEH = c.String(maxLength: 50),
                        TODATEH = c.String(maxLength: 50),
                        ORGUNITID = c.Int(nullable: false),
                        USERPROFILEID = c.Int(nullable: false),
                        PRIORITYID = c.Int(),
                        CONFIDENTIALITYID = c.Int(),
                        TRANSACTIONTYPEID = c.Int(),
                        USERPREFERENCEID = c.Int(nullable: false),
                        REJECTIONREASON = c.String(),
                        STATUSID = c.Int(nullable: false),
                        RECEIVECOPY = c.Boolean(nullable: false),
                        SHOWTRANSACTION = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PERMISSIONS", t => t.CONFIDENTIALITYID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.PRIORITIES", t => t.PRIORITYID)
                .ForeignKey("dbo.LOOKUPS", t => t.STATUSID, cascadeDelete: false)
                .ForeignKey("dbo.LOOKUPS", t => t.TRANSACTIONTYPEID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERPROFILEID)
                .ForeignKey("dbo.USERPREFERENCES", t => t.USERPREFERENCEID, cascadeDelete: false)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.CONFIDENTIALITYID, name: "IX_ConfidentialityId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.USERPREFERENCEID, name: "IX_UserPreferenceId")
                .Index(t => t.STATUSID, name: "IX_StatusId");
            
            CreateTable(
                "dbo.TRANSACTIONHISTORIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        SIGNEDBYUSERID = c.Int(),
                        SIGNEDBYORGUNITID = c.Int(),
                        STATUSID = c.Int(nullable: false),
                        DESTINATIONID = c.Int(),
                        EXPLANATIONID = c.Int(),
                        DELIVERYMETHODID = c.Int(nullable: false),
                        PRIORITYID = c.Int(nullable: false),
                        CONFIDENTIALITYID = c.Int(nullable: false),
                        REMARKS = c.String(),
                        SUBJECT = c.String(),
                        TRANSACTIONCATEGORYID = c.Int(nullable: false),
                        TRANSACTIONTYPEID = c.Int(),
                        LETTERTYPEID = c.Int(),
                        EXTERNALPARTYID = c.Int(),
                        EXTERNALPARTYMANAGERID = c.Int(),
                        TRANSACTIONID = c.Int(nullable: false),
                        PRINTEDDELIVERYREPORT = c.Boolean(nullable: false),
                        DELIVERYREPORTNUMBER = c.String(),
                        ATTCHMENTCOUNT = c.Int(nullable: false),
                        TOENTITYID = c.Int(),
                        TOUSERID = c.Int(),
                        REMINDDATE = c.DateTime(),
                        REMINDDATEH = c.String(),
                        OUTBOUNDDRAFTID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PERMISSIONS", t => t.CONFIDENTIALITYID, cascadeDelete: false)
                .ForeignKey("dbo.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.DESTINATIONID)
                .ForeignKey("dbo.LOOKUPS", t => t.EXPLANATIONID)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.EXTERNALPARTYID)
                .ForeignKey("dbo.EXTERNALPARTYMANAGERS", t => t.EXTERNALPARTYMANAGERID)
                .ForeignKey("dbo.LETTERTYPES", t => t.LETTERTYPEID)
                .ForeignKey("dbo.PRIORITIES", t => t.PRIORITYID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.SIGNEDBYORGUNITID)
                .ForeignKey("dbo.USERPROFILES", t => t.SIGNEDBYUSERID)
                .ForeignKey("dbo.LOOKUPS", t => t.STATUSID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("dbo.LOOKUPS", t => t.TRANSACTIONCATEGORYID)
                .ForeignKey("dbo.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.SIGNEDBYUSERID, name: "IX_SignedByUserId")
                .Index(t => t.SIGNEDBYORGUNITID, name: "IX_SignedByOrgUnitId")
                .Index(t => t.STATUSID, name: "IX_StatusId")
                .Index(t => t.DESTINATIONID, name: "IX_DestinationId")
                .Index(t => t.EXPLANATIONID, name: "IX_ExplanationId")
                .Index(t => t.DELIVERYMETHODID, name: "IX_DeliveryMethodId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.CONFIDENTIALITYID, name: "IX_ConfidentialityId")
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.LETTERTYPEID, name: "IX_LetterTypeId")
                .Index(t => t.EXTERNALPARTYID, name: "IX_ExternalPartyId")
                .Index(t => t.EXTERNALPARTYMANAGERID, name: "IX_ExternalPartyManagerId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.TOENTITYID, name: "IX_ToEntityId")
                .Index(t => t.TOUSERID, name: "IX_ToUserId");
            
            CreateTable(
                "dbo.ASSIGNMENTGROUPS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OWNERID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.OWNERID, cascadeDelete: false)
                .Index(t => t.OWNERID, name: "IX_OwnerId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.ATTACHMENTEXTENSIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EXTENSIONNAME = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AUDITDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PROPERTYNAME = c.String(maxLength: 100),
                        PROPERTYOLDVALUE = c.String(maxLength: 1000),
                        PROPERTYNEWVALUE = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        AUDIT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AUDITS", t => t.AUDIT_ID)
                .Index(t => t.AUDIT_ID, name: "IX_Audit_Id");
            
            CreateTable(
                "dbo.AUDITS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        IPADDRESS = c.String(maxLength: 50),
                        DATE = c.DateTime(nullable: false),
                        OPERATIONTYPE = c.Int(nullable: false),
                        ENTITYNAME = c.String(maxLength: 50),
                        PRIMARYKEYVALUE = c.String(),
                        TRANSACTIONID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BARCODES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VALUE = c.String(),
                        REFERENCEID = c.Int(nullable: false),
                        REFERENCETYPEID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.REFERENCETYPEID, cascadeDelete: false)
                .Index(t => t.REFERENCETYPEID, name: "IX_ReferenceTypeId");
            
            CreateTable(
                "dbo.CITIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CITYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.COLLABORATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SENDERID = c.Int(),
                        RECEIVERID = c.Int(),
                        TEXT = c.String(maxLength: 1000),
                        TRANSACTIONID = c.Int(),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        STATUS = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ATTACHMENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ATTACHMENTS", t => t.ATTACHMENT_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.RECEIVERID)
                .ForeignKey("dbo.USERPROFILES", t => t.SENDERID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.SENDERID, name: "IX_SenderId")
                .Index(t => t.RECEIVERID, name: "IX_ReceiverId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ATTACHMENT_ID, name: "IX_Attachment_Id");
            
            CreateTable(
                "dbo.DISTRIBUTIONLISTDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DISTRIBUTIONLISTID = c.Int(nullable: false),
                        USERID = c.Int(nullable: false),
                        ORGUNITID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .ForeignKey("dbo.DISTRIBUTIONLISTS", t => t.DISTRIBUTIONLISTID)
                .Index(t => t.DISTRIBUTIONLISTID, name: "IX_DistributionListId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId");
            
            CreateTable(
                "dbo.DISTRIBUTIONLISTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        ORGUNITID = c.Int(nullable: false),
                        LOCALIZATIONIDENTIFIERID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIERID, cascadeDelete: false)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNITID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.LOCALIZATIONIDENTIFIERID, name: "IX_LocalizationIdentifierId");
            
            CreateTable(
                "dbo.DOCPROVIDERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PROVIDER_TYPE = c.String(maxLength: 50),
                        FILE_ID = c.Int(nullable: false),
                        FILE_URL = c.String(maxLength: 50),
                        FILE_DOC_ID = c.Int(nullable: false),
                        FILE_STATUS = c.Int(nullable: false),
                        FILE_IS_MIGRATED = c.Boolean(nullable: false),
                        TRANS_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DOCUMENTATTRIBUTES",
                c => new
                    {
                        DOCUMENTATTRIBUTEID = c.Int(nullable: false, identity: true),
                        DOCUMENTNUMBER = c.Int(nullable: false),
                        DOCUMENTSYSNUMBER = c.Int(),
                        DOCUMENTTYPEID = c.Int(),
                        DATE = c.DateTime(nullable: false),
                        HIJRIDATE = c.String(maxLength: 50),
                        SUBJECTID = c.Int(),
                        CONFIDENTIALITYID = c.Int(),
                        PRIORITYID = c.Int(),
                        REMARKS = c.String(maxLength: 50),
                        DOCUMENTID = c.Int(nullable: false),
                        DESTINATIONID = c.Int(),
                        SOURCEID = c.Int(),
                    })
                .PrimaryKey(t => t.DOCUMENTATTRIBUTEID);
            
            CreateTable(
                "dbo.ESCALATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONCATEGORYID = c.Int(nullable: false),
                        PRIORITYID = c.Int(nullable: false),
                        ESCALATIONACTIONID = c.Int(nullable: false),
                        ESCALATIONTOID = c.Int(nullable: false),
                        ESCALATIONAFTERDAYS = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.ESCALATIONACTIONID, cascadeDelete: false)
                .ForeignKey("dbo.LOOKUPS", t => t.ESCALATIONTOID, cascadeDelete: false)
                .ForeignKey("dbo.PRIORITIES", t => t.PRIORITYID, cascadeDelete: false)
                .ForeignKey("dbo.LOOKUPS", t => t.TRANSACTIONCATEGORYID, cascadeDelete: false)
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.ESCALATIONACTIONID, name: "IX_EscalationActionId")
                .Index(t => t.ESCALATIONTOID, name: "IX_EscalationToId");
            
            CreateTable(
                "dbo.FOLLOWUPDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NOTES = c.String(),
                        TRANSACTIONFOLLOWUPID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TRANSACTIONFOLLOWUPS", t => t.TRANSACTIONFOLLOWUPID, cascadeDelete: false)
                .Index(t => t.TRANSACTIONFOLLOWUPID, name: "IX_TransactionFollowUpId");
            
            CreateTable(
                "dbo.FORMDEPARTMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FORMID = c.Int(nullable: false),
                        DEPARTMENTID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.DEPARTMENTID)
                .ForeignKey("dbo.FORMS", t => t.FORMID, cascadeDelete: false)
                .Index(t => t.FORMID, name: "IX_FormId")
                .Index(t => t.DEPARTMENTID, name: "IX_DepartmentId");
            
            CreateTable(
                "dbo.FORMS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISACTIVE = c.Boolean(nullable: false),
                        ISLOCKED = c.Boolean(nullable: false),
                        LOCKEDBY = c.Int(),
                        TRANSACTIONCATEGORIES = c.Int(nullable: false),
                        ISINTERNAL = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        FORMCONTENT_ID = c.Int(),
                        LOCALIZATIONIDENTIFIER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.FORMCONTENT_ID)
                .ForeignKey("dbo.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.FORMCONTENT_ID, name: "IX_FormContent_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "dbo.HUBATTACHMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TYPEID = c.Int(nullable: false),
                        COUNT = c.Int(nullable: false),
                        DESCRIPTION = c.String(),
                        EXTERNALATTACHEMENTID = c.String(),
                        ATTACHEMENTID = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        DOCUMENTINFO_ID = c.Int(),
                        HUBTRANSACTION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENTINFO_ID)
                .ForeignKey("dbo.ATTACHMENTTYPES", t => t.TYPEID)
                .ForeignKey("dbo.HUBTRANSACTIONS", t => t.HUBTRANSACTION_ID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.DOCUMENTINFO_ID, name: "IX_DocumentInfo_Id")
                .Index(t => t.HUBTRANSACTION_ID, name: "IX_HubTransaction_Id");
            
            CreateTable(
                "dbo.HUBRECORDS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OUTERTEXT = c.String(),
                        METHODNAME = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HUBRELATEDPERSONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ADDRESS = c.String(),
                        EMAIL = c.String(),
                        NAME = c.String(),
                        NATIONALID = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        HUBTRANSACTION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HUBTRANSACTIONS", t => t.HUBTRANSACTION_ID)
                .Index(t => t.HUBTRANSACTION_ID, name: "IX_HubTransaction_Id");
            
            CreateTable(
                "dbo.HUBRQUIDS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONNUMBER = c.Long(nullable: false),
                        RQUID = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HUBTRANSACTIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONNUMBER = c.String(),
                        ORGUNITID = c.Int(nullable: false),
                        PRIORITYLEVELID = c.Int(nullable: false),
                        CONFIDENTIALITYLEVELID = c.Int(nullable: false),
                        DESTINATIONID = c.Int(nullable: false),
                        RECORDDATE = c.DateTime(nullable: false),
                        HIJRIRECORDDATE = c.String(),
                        REMARKS = c.String(),
                        RQUID = c.Guid(nullable: false),
                        SUBJECT = c.String(),
                        REMINDERGDATE = c.DateTime(),
                        REMINDERHDATE = c.String(),
                        STATUS = c.Int(nullable: false),
                        CLASSIFICATION = c.Int(nullable: false),
                        ISDELETED = c.Boolean(nullable: false),
                        NEWTRANSACTIONID = c.Long(),
                        NEWTRANSACTIONTIMESTAMP = c.DateTime(),
                        DELIVERYTYPE = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        MAINDOCUMENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.MAINDOCUMENT_ID)
                .Index(t => t.MAINDOCUMENT_ID, name: "IX_MainDocument_Id");
            
            CreateTable(
                "dbo.NOTIFICATIONDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SUBJECT = c.String(),
                        BODY = c.String(),
                        LINK = c.String(),
                        EMAIL = c.String(),
                        ISSENT = c.Boolean(nullable: false),
                        FAILURECOUNT = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        NOTIFICATIONTEMPLATETYPE_ID = c.Int(),
                        NOTIFICATIONTYPE_ID = c.Int(),
                        NOTIFICATION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.NOTIFICATIONTEMPLATETYPE_ID)
                .ForeignKey("dbo.LOOKUPS", t => t.NOTIFICATIONTYPE_ID)
                .ForeignKey("dbo.NOTIFICATIONS", t => t.NOTIFICATION_ID)
                .Index(t => t.NOTIFICATIONTEMPLATETYPE_ID, name: "IX_NotificationTemplateType_Id")
                .Index(t => t.NOTIFICATIONTYPE_ID, name: "IX_NotificationType_Id")
                .Index(t => t.NOTIFICATION_ID, name: "IX_Notification_Id");
            
            CreateTable(
                "dbo.NOTIFICATIONATTACHMENTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BINARY = c.Binary(),
                        FILENAME = c.String(maxLength: 100),
                        CONTENTTYPE = c.String(),
                        CONTENTLENGTH = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        NOTIFICATIONDETAIL_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NOTIFICATIONDETAILS", t => t.NOTIFICATIONDETAIL_ID)
                .Index(t => t.NOTIFICATIONDETAIL_ID, name: "IX_NotificationDetail_Id");
            
            CreateTable(
                "dbo.NOTIFICATIONS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SOURCEID = c.Int(nullable: false),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        ISREAD = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.SOURCEID, cascadeDelete: false)
                .Index(t => t.SOURCEID, name: "IX_SourceId");
            
            CreateTable(
                "dbo.NOTIFICATIONUSERS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        NOTIFICATION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .ForeignKey("dbo.NOTIFICATIONS", t => t.NOTIFICATION_ID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.NOTIFICATION_ID, name: "IX_Notification_Id");
            
            CreateTable(
                "dbo.RESOURCES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RESOURCEID = c.String(nullable: false, maxLength: 1024),
                        VALUE = c.String(),
                        CULTURE = c.String(maxLength: 10),
                        RESOURCESET = c.String(maxLength: 512),
                        TYPE = c.String(maxLength: 512),
                        BINFILE = c.Binary(),
                        TEXTFILE = c.String(),
                        FILENAME = c.String(maxLength: 128),
                        COMMENT = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SETTINGS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KEY = c.String(),
                        VALUE = c.String(),
                        BLOBVALUE = c.Binary(),
                        TYPE = c.Int(),
                        DESCRIPTION = c.String(),
                        MODELID = c.Int(nullable: false),
                        RESOURCEID = c.String(),
                        ISREADONLY = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SIGNEDDELIVERYREPORTS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(),
                        DOCUMENTID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DOCUMENTINFO", t => t.DOCUMENTID)
                .Index(t => t.DOCUMENTID, name: "IX_DocumentId");
            
            CreateTable(
                "dbo.SYSTEMDEFAULTVALUES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CATEGORYID = c.Int(nullable: false),
                        TYPEID = c.Int(nullable: false),
                        DEFAULTVALUEID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TASKHISTORIES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        DELIVERYDATE = c.DateTime(nullable: false),
                        DELIVERYDATEH = c.String(maxLength: 20),
                        STATUSDESCRIPTION = c.String(maxLength: 500),
                        TASKDESCRIPTION = c.String(),
                        ISEXCLUSIVE = c.Boolean(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        FROMORGUNIT_ID = c.Int(),
                        FROMUSER_ID = c.Int(),
                        PARENT_ID = c.Int(),
                        STATUS_ID = c.Int(),
                        TOORGUNIT_ID = c.Int(),
                        TOUSER_ID = c.Int(),
                        TRANSACTION_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMORGUNIT_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.FROMUSER_ID)
                .ForeignKey("dbo.TASKS", t => t.PARENT_ID)
                .ForeignKey("dbo.LOOKUPS", t => t.STATUS_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOORGUNIT_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSER_ID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTION_ID)
                .Index(t => t.FROMORGUNIT_ID, name: "IX_FromOrgUnit_Id")
                .Index(t => t.FROMUSER_ID, name: "IX_FromUser_Id")
                .Index(t => t.PARENT_ID, name: "IX_Parent_Id")
                .Index(t => t.STATUS_ID, name: "IX_Status_Id")
                .Index(t => t.TOORGUNIT_ID, name: "IX_ToOrgUnit_Id")
                .Index(t => t.TOUSER_ID, name: "IX_ToUser_Id")
                .Index(t => t.TRANSACTION_ID, name: "IX_Transaction_Id");
            
            CreateTable(
                "dbo.TASKWORKFLOWS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        FROMENTITY_ID = c.Int(),
                        TOENTITY_ID = c.Int(),
                        TOUSER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.FROMENTITY_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.TOENTITY_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.TOUSER_ID)
                .Index(t => t.FROMENTITY_ID, name: "IX_FromEntity_Id")
                .Index(t => t.TOENTITY_ID, name: "IX_ToEntity_Id")
                .Index(t => t.TOUSER_ID, name: "IX_ToUser_Id");
            
            CreateTable(
                "dbo.TRANSACTIONASSIGNEES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        ENTITY_ID = c.Int(),
                        TRANSACTION_ID = c.Int(),
                        USER_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ENTITY_ID)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTION_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.USER_ID)
                .Index(t => t.ENTITY_ID, name: "IX_Entity_Id")
                .Index(t => t.TRANSACTION_ID, name: "IX_Transaction_Id")
                .Index(t => t.USER_ID, name: "IX_User_Id");
            
            CreateTable(
                "dbo.TRANSACTIONENTITYDETAILS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSACTIONID = c.Int(nullable: false),
                        ENTITYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ENTITYID, cascadeDelete: false)
                .ForeignKey("dbo.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ENTITYID, name: "IX_EntityId");
            
            CreateTable(
                "dbo.TRANSACTIONINDEXLOGS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRANSID = c.Int(nullable: false),
                        TRANSACTIONCATEGORYID = c.Int(nullable: false),
                        TRANSACTIONTYPEID = c.Int(nullable: false),
                        NUMBER = c.Long(nullable: false),
                        BARCODE = c.String(maxLength: 50),
                        DATEH = c.String(maxLength: 50),
                        DATE = c.DateTime(nullable: false),
                        YEAR = c.Int(nullable: false),
                        YEARH = c.Int(nullable: false),
                        PERMISSIONCODE = c.String(),
                        PRIORITYID = c.Int(nullable: false),
                        PARTYID = c.Int(),
                        ORGUNITID = c.Int(nullable: false),
                        SIGNEDBYUSERID = c.Int(nullable: false),
                        DIRECTEDTOUSERID = c.Int(),
                        STATUSID = c.Int(nullable: false),
                        LETTERTYPEID = c.Int(nullable: false),
                        ORGUNITNAMEAR = c.String(maxLength: 50),
                        ORGUNITNAMEEN = c.String(maxLength: 50),
                        TYPENAMEAR = c.String(maxLength: 50),
                        TYPENAMEEN = c.String(maxLength: 50),
                        PARTYNAMEAR = c.String(maxLength: 50),
                        PARTYNAMEEN = c.String(maxLength: 50),
                        SIGNEDBYNAMEAR = c.String(maxLength: 50),
                        SIGNEDBYNAMEEN = c.String(maxLength: 50),
                        CONFIDENTIALITYNAMEAR = c.String(maxLength: 50),
                        CONFIDENTIALITYNAMEEN = c.String(maxLength: 50),
                        PRIORITYNAMEAR = c.String(maxLength: 50),
                        PRIORITYNAMEEN = c.String(maxLength: 50),
                        STATUSNAMEAR = c.String(maxLength: 50),
                        STATUSNAMEEN = c.String(maxLength: 50),
                        TRANSACTIONTYPENAMEAR = c.String(maxLength: 50),
                        TRANSACTIONTYPENAMEEN = c.String(maxLength: 50),
                        SUBJECT = c.String(),
                        ASSIGNMENTS = c.String(maxLength: 50),
                        ISINDEXED = c.Boolean(nullable: false),
                        ISUPDATED = c.Boolean(nullable: false),
                        WITHARCHIVING = c.Boolean(nullable: false),
                        COLOR = c.String(maxLength: 50),
                        SUBJECTCLASSIFICATIONS = c.String(maxLength: 500),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TRANSACTIONLOGS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(),
                        TRANSACTIONID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        AUDITINGACTIONCODE_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LOOKUPS", t => t.AUDITINGACTIONCODE_ID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.AUDITINGACTIONCODE_ID, name: "IX_AuditingActionCode_Id");
            
            CreateTable(
                "dbo.USERMOBILES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Int(nullable: false),
                        TOKEN = c.String(),
                        DEVICETOKEN = c.String(),
                        ACTIVATIONREQUESTCODE = c.String(),
                        ACTIVATAIONCODE = c.String(),
                        DEACTIVATIONREQUESTCODE = c.String(),
                        SIGNEDCERT = c.String(),
                        CA = c.String(),
                        CACRL = c.String(),
                        ISUPDATED = c.Boolean(nullable: false),
                        UPDATEFLAGS = c.Int(nullable: false),
                        LASTLOGINDATE = c.DateTime(nullable: false),
                        LOGS = c.Binary(),
                        SETTINGS = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERPROFILES", t => t.USERID, cascadeDelete: false)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.USERPREFERENCES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CULTUREID = c.Int(nullable: false),
                        ISDELEGATIONENABLED = c.Boolean(nullable: false),
                        SIGNATURE = c.Binary(),
                        MARKINGDOC = c.Binary(),
                        SIGNATUREPASSWORD = c.Boolean(nullable: false),
                        SIGNATUREPASSWORDTEXT = c.String(),
                        FREETEXT = c.String(),
                        EMAIL = c.String(maxLength: 50),
                        USERPROFILEID = c.Int(nullable: false),
                        OTP = c.String(),
                        OTPCREATEDON = c.DateTime(),
                        NOTIFICATIONSUBSCRIPTIONS = c.Int(nullable: false),
                        ASSIGNMENTPAPERID = c.Int(),
                        FOLLOWUPORGID = c.Int(),
                        FOLLOWUPUSERID = c.Int(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPERID)
                .ForeignKey("dbo.CULTURES", t => t.CULTUREID, cascadeDelete: false)
                .ForeignKey("dbo.USERPROFILES", t => t.USERPROFILEID)
                .Index(t => t.CULTUREID, name: "IX_CultureId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.ASSIGNMENTPAPERID, name: "IX_AssignmentPaperId");
            
            CreateTable(
                "dbo.USERPREFERENCEFOLLOWUPS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FOLLOWUPORGID = c.Int(),
                        FOLLOWUPUSERID = c.Int(),
                        ORGUNITID = c.Int(nullable: false),
                        USERPREFERENCEID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERPREFERENCES", t => t.USERPREFERENCEID, cascadeDelete: false)
                .Index(t => t.USERPREFERENCEID, name: "IX_UserPreferenceId");
            
            CreateTable(
                "dbo.USERTRAYPREFERENCES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TRAYID = c.Int(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                        USERPREFERENCE_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TRAYS", t => t.TRAYID, cascadeDelete: false)
                .ForeignKey("dbo.USERPREFERENCES", t => t.USERPREFERENCE_ID)
                .Index(t => t.TRAYID, name: "IX_TrayId")
                .Index(t => t.USERPREFERENCE_ID, name: "IX_UserPreference_Id");
            
            CreateTable(
                "dbo.YESSERMAPPINGS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TYPEID = c.Int(nullable: false),
                        YESSERTYPEID = c.String(),
                        CLOUDTYPEID = c.Int(nullable: false),
                        EXPONENT = c.Binary(),
                        MODULUS = c.Binary(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EXTERNALPARTIES", t => t.CLOUDTYPEID, cascadeDelete: false)
                .Index(t => t.CLOUDTYPEID, name: "IX_CloudTypeId");
            
            CreateTable(
                "dbo.YESSERNEWENTITES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        YESSERID = c.String(),
                        NAMEAR = c.String(),
                        NAMEEN = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Int(),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ASPNETUSERROLES",
                c => new
                    {
                        ROLEID = c.String(nullable: false, maxLength: 128),
                        USERID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ROLEID, t.USERID })
                .ForeignKey("dbo.ASPNETROLES", t => t.ROLEID)
                .ForeignKey("dbo.ASPNETUSERS", t => t.USERID)
                .Index(t => t.ROLEID, name: "IX_RoleId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "dbo.GROUPPERMISSIONS",
                c => new
                    {
                        GROUP_ID = c.Int(nullable: false),
                        PERMISSION_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GROUP_ID, t.PERMISSION_ID })
                .ForeignKey("dbo.GROUPS", t => t.GROUP_ID)
                .ForeignKey("dbo.PERMISSIONS", t => t.PERMISSION_ID)
                .Index(t => t.GROUP_ID, name: "IX_Group_Id")
                .Index(t => t.PERMISSION_ID, name: "IX_Permission_Id");
            
            CreateTable(
                "dbo.USERPROFILEORGUNITS",
                c => new
                    {
                        USERPROFILE_ID = c.Int(nullable: false),
                        ORGUNIT_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.USERPROFILE_ID, t.ORGUNIT_ID })
                .ForeignKey("dbo.USERPROFILES", t => t.USERPROFILE_ID)
                .ForeignKey("dbo.ORGUNITS", t => t.ORGUNIT_ID)
                .Index(t => t.USERPROFILE_ID, name: "IX_UserProfile_Id")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.YESSERMAPPINGS", "CLOUDTYPEID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.USERTRAYPREFERENCES", "USERPREFERENCE_ID", "dbo.USERPREFERENCES");
            DropForeignKey("dbo.USERTRAYPREFERENCES", "TRAYID", "dbo.TRAYS");
            DropForeignKey("dbo.USERPREFERENCES", "USERPROFILEID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERPREFERENCEFOLLOWUPS", "USERPREFERENCEID", "dbo.USERPREFERENCES");
            DropForeignKey("dbo.USERDELEGATIONS", "USERPREFERENCEID", "dbo.USERPREFERENCES");
            DropForeignKey("dbo.USERPREFERENCES", "CULTUREID", "dbo.CULTURES");
            DropForeignKey("dbo.USERPREFERENCES", "ASSIGNMENTPAPERID", "dbo.ASSIGNMENTPAPERS");
            DropForeignKey("dbo.USERMOBILES", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONLOGS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONLOGS", "AUDITINGACTIONCODE_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONENTITYDETAILS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONENTITYDETAILS", "ENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONASSIGNEES", "USER_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONASSIGNEES", "TRANSACTION_ID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONASSIGNEES", "ENTITY_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKWORKFLOWS", "TOUSER_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TASKWORKFLOWS", "TOENTITY_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKWORKFLOWS", "FROMENTITY_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKHISTORIES", "TRANSACTION_ID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TASKHISTORIES", "TOUSER_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TASKHISTORIES", "TOORGUNIT_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKHISTORIES", "STATUS_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TASKHISTORIES", "PARENT_ID", "dbo.TASKS");
            DropForeignKey("dbo.TASKHISTORIES", "FROMUSER_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TASKHISTORIES", "FROMORGUNIT_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.SIGNEDDELIVERYREPORTS", "DOCUMENTID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.NOTIFICATIONUSERS", "NOTIFICATION_ID", "dbo.NOTIFICATIONS");
            DropForeignKey("dbo.NOTIFICATIONUSERS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.NOTIFICATIONS", "SOURCEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.NOTIFICATIONDETAILS", "NOTIFICATION_ID", "dbo.NOTIFICATIONS");
            DropForeignKey("dbo.NOTIFICATIONDETAILS", "NOTIFICATIONTYPE_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.NOTIFICATIONDETAILS", "NOTIFICATIONTEMPLATETYPE_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.NOTIFICATIONATTACHMENTS", "NOTIFICATIONDETAIL_ID", "dbo.NOTIFICATIONDETAILS");
            DropForeignKey("dbo.HUBTRANSACTIONS", "MAINDOCUMENT_ID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.HUBRELATEDPERSONS", "HUBTRANSACTION_ID", "dbo.HUBTRANSACTIONS");
            DropForeignKey("dbo.HUBATTACHMENTS", "HUBTRANSACTION_ID", "dbo.HUBTRANSACTIONS");
            DropForeignKey("dbo.HUBATTACHMENTS", "TYPEID", "dbo.ATTACHMENTTYPES");
            DropForeignKey("dbo.HUBATTACHMENTS", "DOCUMENTINFO_ID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.FORMS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.FORMS", "FORMCONTENT_ID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.FORMDEPARTMENTS", "FORMID", "dbo.FORMS");
            DropForeignKey("dbo.FORMDEPARTMENTS", "DEPARTMENTID", "dbo.ORGUNITS");
            DropForeignKey("dbo.FOLLOWUPDETAILS", "TRANSACTIONFOLLOWUPID", "dbo.TRANSACTIONFOLLOWUPS");
            DropForeignKey("dbo.ESCALATIONS", "TRANSACTIONCATEGORYID", "dbo.LOOKUPS");
            DropForeignKey("dbo.ESCALATIONS", "PRIORITYID", "dbo.PRIORITIES");
            DropForeignKey("dbo.ESCALATIONS", "ESCALATIONTOID", "dbo.LOOKUPS");
            DropForeignKey("dbo.ESCALATIONS", "ESCALATIONACTIONID", "dbo.LOOKUPS");
            DropForeignKey("dbo.DISTRIBUTIONLISTS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.DISTRIBUTIONLISTS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.DISTRIBUTIONLISTS", "LOCALIZATIONIDENTIFIERID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.DISTRIBUTIONLISTDETAILS", "DISTRIBUTIONLISTID", "dbo.DISTRIBUTIONLISTS");
            DropForeignKey("dbo.DISTRIBUTIONLISTDETAILS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.DISTRIBUTIONLISTDETAILS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.COLLABORATIONS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.COLLABORATIONS", "SENDERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.COLLABORATIONS", "RECEIVERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.COLLABORATIONS", "ATTACHMENT_ID", "dbo.ATTACHMENTS");
            DropForeignKey("dbo.CITIES", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.BARCODES", "REFERENCETYPEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.AUDITDETAILS", "AUDIT_ID", "dbo.AUDITS");
            DropForeignKey("dbo.ASSIGNMENTGROUPS", "OWNERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.ASSIGNMENTGROUPS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.ASSIGNMENTGROUPDETAILS", "ASSIGNMENTGROUP_ID", "dbo.ASSIGNMENTGROUPS");
            DropForeignKey("dbo.ASSIGNMENTGROUPDETAILS", "USERPROFILE_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.ASSIGNMENTGROUPDETAILS", "ORGUNIT_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.REPORTERS", "TOENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONHISTORYID", "dbo.TRANSACTIONHISTORIES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "TRANSACTIONTYPEID", "dbo.TRANSACTIONTYPES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "TRANSACTIONCATEGORYID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "TOUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "TOENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "STATUSID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "SIGNEDBYUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "SIGNEDBYORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "PRIORITYID", "dbo.PRIORITIES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "LETTERTYPEID", "dbo.LETTERTYPES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "EXTERNALPARTYMANAGERID", "dbo.EXTERNALPARTYMANAGERS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "EXTERNALPARTYID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "EXPLANATIONID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "DESTINATIONID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "DELIVERYMETHODID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONHISTORIES", "CONFIDENTIALITYID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONEXTERNALCOPYID", "dbo.TRANSACTIONEXTERNALCOPIES");
            DropForeignKey("dbo.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONASSIGNMENTHISTORYID", "dbo.TRANSACTIONASSIGNMENTHISTORIES");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "USERDELEGATIONID", "dbo.USERDELEGATIONS");
            DropForeignKey("dbo.USERDELEGATIONS", "USERPROFILEID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERDELEGATIONS", "TRANSACTIONTYPEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.USERDELEGATIONS", "STATUSID", "dbo.LOOKUPS");
            DropForeignKey("dbo.USERDELEGATIONS", "PRIORITYID", "dbo.PRIORITIES");
            DropForeignKey("dbo.USERDELEGATIONS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.USERDELEGATIONS", "CONFIDENTIALITYID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "TRAYID", "dbo.TRAYS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "TOUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "TOENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "FROMENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "EXPLANATIONID", "dbo.EXPLANATIONS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTHISTORIES", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONDELIVERYREPORTS", "REPORTERID", "dbo.REPORTERS");
            DropForeignKey("dbo.TRANSACTIONDELIVERYREPORTS", "DOCUMENTID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.REPORTERS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.ORGUNITS", "PARENTID", "dbo.ORGUNITS");
            DropForeignKey("dbo.ORGUNITS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.ORGUNITLINKS", "ORGUNIT_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.ORGUNITLINKS", "TOENTITY_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.ORGUNITLINKS", "FROMENTITY_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.ORGUNITS", "COUNTER_ID", "dbo.COUNTERS");
            DropForeignKey("dbo.COUNTERS", "DESCRIPTION_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.COUNTERDETAILS", "TRANSACTIONTYPEID", "dbo.TRANSACTIONTYPES");
            DropForeignKey("dbo.COUNTERDETAILS", "COUNTER_ID", "dbo.COUNTERS");
            DropForeignKey("dbo.BARCODEDESIGNS", "ORGUNIT_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.BARCODEDESIGNS", "TYPEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.ORGUNITS", "ASSIGNMENTPAPERID", "dbo.ASSIGNMENTPAPERS");
            DropForeignKey("dbo.ASSIGNMENTPAPERBENEFICIARIES", "ASSIGNMENTPAPER_ID", "dbo.ASSIGNMENTPAPERS");
            DropForeignKey("dbo.ASSIGNMENTPAPERBENEFICIARIES", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERPROFILES", "USERIMAGE_ID", "dbo.DOCUMENTS");
            DropForeignKey("dbo.USERPROFILES", "TITLEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.USERPERMISSIONS", "USERPROFILEID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERPERMISSIONS", "PERMISSIONID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.USERPERMISSIONS", "GROUPID", "dbo.GROUPS");
            DropForeignKey("dbo.USERPROFILEORGUNITS", "ORGUNIT_ID", "dbo.ORGUNITS");
            DropForeignKey("dbo.USERPROFILEORGUNITS", "USERPROFILE_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERPROFILES", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.USERPROFILES", "DIRECTMANAGER_ID", "dbo.USERPROFILES");
            DropForeignKey("dbo.CHATCLIENTS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERPROFILES", "CATEGORYID", "dbo.USERCATEGORIES");
            DropForeignKey("dbo.USERCATEGORIES", "PERMISSION_ID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.USERCATEGORYTRAYS", "USERCATEGORYID", "dbo.USERCATEGORIES");
            DropForeignKey("dbo.USERCATEGORYTRAYS", "TARY_ID", "dbo.TRAYS");
            DropForeignKey("dbo.USERCATEGORIES", "CATEGORYNAME_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.CHATROOMALLOWEDUSERS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.CHATROOMUSERS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.CHATROOMUSERS", "ROOMID", "dbo.CHATROOMS");
            DropForeignKey("dbo.CHATROOMS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONS", "TRANSACTIONTYPEID", "dbo.TRANSACTIONTYPES");
            DropForeignKey("dbo.TRANSACTIONTYPES", "PERMISSIONID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.TRANSACTIONTYPES", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONTYPES", "COLOR_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONTYPES", "ABBREVIATION_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONS", "TRANSACTIONCATEGORYID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONS", "TOUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONS", "SUGGESTEDTOPICID", "dbo.SUGGESTEDTOPICS");
            DropForeignKey("dbo.SUBJECTORGUNITS", "SUGGESTEDTOPIC_ID", "dbo.SUGGESTEDTOPICS");
            DropForeignKey("dbo.SUGGESTEDTOPICS", "PARENTID", "dbo.SUGGESTEDTOPICS");
            DropForeignKey("dbo.SUGGESTEDTOPICS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONSUBJECTCLASSIFICATI", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONSUBJECTCLASSIFICATI", "SUBJECTCLASSIFICATIONID", "dbo.SUBJECTCLASSIFICATIONS");
            DropForeignKey("dbo.SUBJECTORGUNITS", "SUBJECTCLASSIFICATION_ID", "dbo.SUBJECTCLASSIFICATIONS");
            DropForeignKey("dbo.SUBJECTORGUNITS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.SUBJECTCLASSIFICATIONS", "PARENTID", "dbo.SUBJECTCLASSIFICATIONS");
            DropForeignKey("dbo.SUBJECTCLASSIFICATIONS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONS", "STATUSID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONS", "SIGNEDBYUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONRESERVATIONS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONS", "RESERVATIONID", "dbo.TRANSACTIONRESERVATIONS");
            DropForeignKey("dbo.TRANSACTIONRESERVATIONS", "TRANSACTIONCATEGORYID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONRESERVATIONS", "ENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONS", "PRIORITYID", "dbo.PRIORITIES");
            DropForeignKey("dbo.PRIORITYEXCEPTIONS", "USERPROFILEID", "dbo.USERPROFILES");
            DropForeignKey("dbo.PRIORITYEXCEPTIONS", "PRIORITYID", "dbo.PRIORITIES");
            DropForeignKey("dbo.PRIORITYEXCEPTIONS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.PRIORITIES", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONNAMES", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONNAMES", "NAMEID", "dbo.NAMES");
            DropForeignKey("dbo.NAMES", "TITLEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.NAMES", "NATIONALITYID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONS", "MAINDOCUMENTID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.TRANSACTIONLINKS", "TYPEID", "dbo.LINKS");
            DropForeignKey("dbo.LINKS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONLINKS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONLINKS", "TOTRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONS", "LETTERTYPEID", "dbo.LETTERTYPES");
            DropForeignKey("dbo.LETTERTYPES", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONFOLLOWUPS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONFOLLOWUPS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONFOLLOWUPS", "ENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONS", "EXTERNALPARTYMANAGERID", "dbo.EXTERNALPARTYMANAGERS");
            DropForeignKey("dbo.TRANSACTIONS", "EXTERNALPARTYID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", "USERID", "dbo.EXTERNALPARTYMANAGERS");
            DropForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", "FROMENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.EXTERNALPARTYATTACHMENTS", "TRANSACTIONEXTERNALCOPYID", "dbo.TRANSACTIONEXTERNALCOPIES");
            DropForeignKey("dbo.EXTERNALPARTYATTACHMENTS", "PARTYID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.EXTERNALPARTYATTACHMENTS", "DOCUMENTINFOID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", "ENTITYID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.EXTERNALPARTYMANAGERS", "EXTERNALPARTY_ID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.EXTERNALPARTYMANAGERS", "NAME_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.EXTERNALPARTIES", "PARENTID", "dbo.EXTERNALPARTIES");
            DropForeignKey("dbo.EXTERNALPARTIES", "NAME_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.EXTERNALPARTIES", "ADDRESS_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.TRANSACTIONEXTERNALCOPIES", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.EXPLANATIONS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.EXPLANATIONS", "PERMISSIONID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.EXPLANATIONS", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.EXPLANATIONS", "DOCUMENT_ID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.TRANSACTIONS", "ENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONS", "DELIVERYMETHODID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONCOPIES", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONCOPIES", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONCOPIES", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONCOPIES", "FROMENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONCOPIES", "ENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONCOPIES", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.TRANSACTIONS", "CONFIDENTIALITYID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.GROUPPERMISSIONS", "PERMISSION_ID", "dbo.PERMISSIONS");
            DropForeignKey("dbo.GROUPPERMISSIONS", "GROUP_ID", "dbo.GROUPS");
            DropForeignKey("dbo.USERGROUPS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.USERGROUPS", "GROUPID", "dbo.GROUPS");
            DropForeignKey("dbo.GROUPS", "GROUPNAME_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.PERMISSIONS", "NAME_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.ATTACHMENTS", "TYPEID", "dbo.ATTACHMENTTYPES");
            DropForeignKey("dbo.ATTACHMENTTYPES", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.ATTACHMENTS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.ATTACHMENTS", "DOCUMENTINFO_ID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "TRAYID", "dbo.TRAYS");
            DropForeignKey("dbo.TRAYS", "NAME_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "TRANSACTIONPATHID", "dbo.TRANSACTIONPATHS");
            DropForeignKey("dbo.TRANSACTIONPATHS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONPATHS", "TRANSACTIONTYPEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONPATHDETAILS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONPATHDETAILS", "TRANSACTIONPATHID", "dbo.TRANSACTIONPATHS");
            DropForeignKey("dbo.TRANSACTIONPATHDETAILS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONPATHDETAILS", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.TRANSACTIONPATHS", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "TOUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "TOENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKS", "TRANSACTIONASSIGNMENT_ID", "dbo.TRANSACTIONASSIGNMENTS");
            DropForeignKey("dbo.TASKS", "TRANSACTIONID", "dbo.TRANSACTIONS");
            DropForeignKey("dbo.TASKS", "TOUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TASKS", "TOORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKSATTACHMENTS", "TASKID", "dbo.TASKS");
            DropForeignKey("dbo.TASKSATTACHMENTS", "DOCUMENTINFOID", "dbo.DOCUMENTINFO");
            DropForeignKey("dbo.DOCUMENTINFO", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.DOCUMENTINFO", "FROMENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.DOCUMENTINFO", "DOCUMENT_ID", "dbo.DOCUMENTS");
            DropForeignKey("dbo.TASKS", "STATUSID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TASKREMINDERS", "TASK_ID", "dbo.TASKS");
            DropForeignKey("dbo.TASKS", "PARENTID", "dbo.TASKS");
            DropForeignKey("dbo.TASKS", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TASKS", "FROMORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TASKS", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "PHYSICALUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "PHYSICALENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "FROMUSERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "FROMENTITYID", "dbo.ORGUNITS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "DELIVERYMETHODID", "dbo.LOOKUPS");
            DropForeignKey("dbo.TRANSACTIONASSIGNMENTS", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.CHATROOMOWNERS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.CHATROOMOWNERS", "ROOMID", "dbo.CHATROOMS");
            DropForeignKey("dbo.CHATMESSAGES", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.CHATMESSAGES", "ROOMID", "dbo.CHATROOMS");
            DropForeignKey("dbo.CHATMESSAGESSTATUS", "USERID", "dbo.USERPROFILES");
            DropForeignKey("dbo.CHATMESSAGESSTATUS", "ROOMID", "dbo.CHATROOMS");
            DropForeignKey("dbo.CHATMESSAGESSTATUS", "MESSAGEID", "dbo.CHATMESSAGES");
            DropForeignKey("dbo.CHATROOMALLOWEDUSERS", "ROOMID", "dbo.CHATROOMS");
            DropForeignKey("dbo.ASSIGNMENTPAPERBENEFICIARIES", "ORGUNITID", "dbo.ORGUNITS");
            DropForeignKey("dbo.ASSIGNMENTPAPERACTIONS", "ASSIGNMENTPAPER_ID", "dbo.ASSIGNMENTPAPERS");
            DropForeignKey("dbo.ASSIGNMENTPAPERACTIONS", "ACTIONID", "dbo.ACTIONS");
            DropForeignKey("dbo.ASPNETUSERLOGINS", "USERID", "dbo.ASPNETUSERS");
            DropForeignKey("dbo.ASPNETUSERCLAIMS", "USERID", "dbo.ASPNETUSERS");
            DropForeignKey("dbo.ASPNETUSERROLES", "USERID", "dbo.ASPNETUSERS");
            DropForeignKey("dbo.ASPNETUSERROLES", "ROLEID", "dbo.ASPNETROLES");
            DropForeignKey("dbo.ACTIONS", "TYPE_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.ACTIONS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.LOCALIZATIONS", "LOCALIZATIONIDENTIFIER_ID", "dbo.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("dbo.LOCALIZATIONS", "CULTUREID", "dbo.CULTURES");
            DropForeignKey("dbo.CULTURES", "NAMEID", "dbo.LOOKUPS");
            DropForeignKey("dbo.LOOKUPLOCALIZATIONS", "LOOKUP_ID", "dbo.LOOKUPS");
            DropForeignKey("dbo.LOOKUPLOCALIZATIONS", "CULTURE_ID", "dbo.CULTURES");
            DropIndex("dbo.USERPROFILEORGUNITS", "IX_OrgUnit_Id");
            DropIndex("dbo.USERPROFILEORGUNITS", "IX_UserProfile_Id");
            DropIndex("dbo.GROUPPERMISSIONS", "IX_Permission_Id");
            DropIndex("dbo.GROUPPERMISSIONS", "IX_Group_Id");
            DropIndex("dbo.ASPNETUSERROLES", "IX_UserId");
            DropIndex("dbo.ASPNETUSERROLES", "IX_RoleId");
            DropIndex("dbo.YESSERMAPPINGS", "IX_CloudTypeId");
            DropIndex("dbo.USERTRAYPREFERENCES", "IX_UserPreference_Id");
            DropIndex("dbo.USERTRAYPREFERENCES", "IX_TrayId");
            DropIndex("dbo.USERPREFERENCEFOLLOWUPS", "IX_UserPreferenceId");
            DropIndex("dbo.USERPREFERENCES", "IX_AssignmentPaperId");
            DropIndex("dbo.USERPREFERENCES", "IX_UserProfileId");
            DropIndex("dbo.USERPREFERENCES", "IX_CultureId");
            DropIndex("dbo.USERMOBILES", "IX_UserId");
            DropIndex("dbo.TRANSACTIONLOGS", "IX_AuditingActionCode_Id");
            DropIndex("dbo.TRANSACTIONLOGS", "IX_UserId");
            DropIndex("dbo.TRANSACTIONENTITYDETAILS", "IX_EntityId");
            DropIndex("dbo.TRANSACTIONENTITYDETAILS", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONASSIGNEES", "IX_User_Id");
            DropIndex("dbo.TRANSACTIONASSIGNEES", "IX_Transaction_Id");
            DropIndex("dbo.TRANSACTIONASSIGNEES", "IX_Entity_Id");
            DropIndex("dbo.TASKWORKFLOWS", "IX_ToUser_Id");
            DropIndex("dbo.TASKWORKFLOWS", "IX_ToEntity_Id");
            DropIndex("dbo.TASKWORKFLOWS", "IX_FromEntity_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_Transaction_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_ToUser_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_ToOrgUnit_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_Status_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_Parent_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_FromUser_Id");
            DropIndex("dbo.TASKHISTORIES", "IX_FromOrgUnit_Id");
            DropIndex("dbo.SIGNEDDELIVERYREPORTS", "IX_DocumentId");
            DropIndex("dbo.NOTIFICATIONUSERS", "IX_Notification_Id");
            DropIndex("dbo.NOTIFICATIONUSERS", "IX_UserId");
            DropIndex("dbo.NOTIFICATIONS", "IX_SourceId");
            DropIndex("dbo.NOTIFICATIONATTACHMENTS", "IX_NotificationDetail_Id");
            DropIndex("dbo.NOTIFICATIONDETAILS", "IX_Notification_Id");
            DropIndex("dbo.NOTIFICATIONDETAILS", "IX_NotificationType_Id");
            DropIndex("dbo.NOTIFICATIONDETAILS", "IX_NotificationTemplateType_Id");
            DropIndex("dbo.HUBTRANSACTIONS", "IX_MainDocument_Id");
            DropIndex("dbo.HUBRELATEDPERSONS", "IX_HubTransaction_Id");
            DropIndex("dbo.HUBATTACHMENTS", "IX_HubTransaction_Id");
            DropIndex("dbo.HUBATTACHMENTS", "IX_DocumentInfo_Id");
            DropIndex("dbo.HUBATTACHMENTS", "IX_TypeId");
            DropIndex("dbo.FORMS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.FORMS", "IX_FormContent_Id");
            DropIndex("dbo.FORMDEPARTMENTS", "IX_DepartmentId");
            DropIndex("dbo.FORMDEPARTMENTS", "IX_FormId");
            DropIndex("dbo.FOLLOWUPDETAILS", "IX_TransactionFollowUpId");
            DropIndex("dbo.ESCALATIONS", "IX_EscalationToId");
            DropIndex("dbo.ESCALATIONS", "IX_EscalationActionId");
            DropIndex("dbo.ESCALATIONS", "IX_PriorityId");
            DropIndex("dbo.ESCALATIONS", "IX_TransactionCategoryId");
            DropIndex("dbo.DISTRIBUTIONLISTS", "IX_LocalizationIdentifierId");
            DropIndex("dbo.DISTRIBUTIONLISTS", "IX_OrgUnitId");
            DropIndex("dbo.DISTRIBUTIONLISTS", "IX_UserId");
            DropIndex("dbo.DISTRIBUTIONLISTDETAILS", "IX_OrgUnitId");
            DropIndex("dbo.DISTRIBUTIONLISTDETAILS", "IX_UserId");
            DropIndex("dbo.DISTRIBUTIONLISTDETAILS", "IX_DistributionListId");
            DropIndex("dbo.COLLABORATIONS", "IX_Attachment_Id");
            DropIndex("dbo.COLLABORATIONS", "IX_TransactionId");
            DropIndex("dbo.COLLABORATIONS", "IX_ReceiverId");
            DropIndex("dbo.COLLABORATIONS", "IX_SenderId");
            DropIndex("dbo.CITIES", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.BARCODES", "IX_ReferenceTypeId");
            DropIndex("dbo.AUDITDETAILS", "IX_Audit_Id");
            DropIndex("dbo.ASSIGNMENTGROUPS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.ASSIGNMENTGROUPS", "IX_OwnerId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_ToUserId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_ToEntityId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_ExternalPartyManagerId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_ExternalPartyId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_LetterTypeId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_TransactionTypeId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_TransactionCategoryId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_ConfidentialityId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_PriorityId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_DeliveryMethodId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_ExplanationId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_DestinationId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_StatusId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_SignedByOrgUnitId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_SignedByUserId");
            DropIndex("dbo.TRANSACTIONHISTORIES", "IX_UserId");
            DropIndex("dbo.USERDELEGATIONS", "IX_StatusId");
            DropIndex("dbo.USERDELEGATIONS", "IX_UserPreferenceId");
            DropIndex("dbo.USERDELEGATIONS", "IX_TransactionTypeId");
            DropIndex("dbo.USERDELEGATIONS", "IX_ConfidentialityId");
            DropIndex("dbo.USERDELEGATIONS", "IX_PriorityId");
            DropIndex("dbo.USERDELEGATIONS", "IX_UserProfileId");
            DropIndex("dbo.USERDELEGATIONS", "IX_OrgUnitId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_UserDelegationId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_ExplanationId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_ToEntityId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_FromEntityId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_ActionId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_ToUserId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_FromUserId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTHISTORIES", "IX_TrayId");
            DropIndex("dbo.TRANSACTIONDELIVERYREPORTS", "IX_TransactionExternalCopyId");
            DropIndex("dbo.TRANSACTIONDELIVERYREPORTS", "IX_ReporterId");
            DropIndex("dbo.TRANSACTIONDELIVERYREPORTS", "IX_DocumentId");
            DropIndex("dbo.TRANSACTIONDELIVERYREPORTS", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONDELIVERYREPORTS", "IX_TransactionHistoryId");
            DropIndex("dbo.TRANSACTIONDELIVERYREPORTS", "IX_TransactionAssignmentHistoryId");
            DropIndex("dbo.REPORTERS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.REPORTERS", "IX_ToEntityId");
            DropIndex("dbo.ORGUNITLINKS", "IX_OrgUnit_Id");
            DropIndex("dbo.ORGUNITLINKS", "IX_ToEntity_Id");
            DropIndex("dbo.ORGUNITLINKS", "IX_FromEntity_Id");
            DropIndex("dbo.COUNTERDETAILS", "IX_Counter_Id");
            DropIndex("dbo.COUNTERDETAILS", "IX_TransactionTypeId");
            DropIndex("dbo.COUNTERS", "IX_Description_Id");
            DropIndex("dbo.BARCODEDESIGNS", "IX_OrgUnit_Id");
            DropIndex("dbo.BARCODEDESIGNS", "IX_TypeId");
            DropIndex("dbo.USERPERMISSIONS", "IX_GroupId");
            DropIndex("dbo.USERPERMISSIONS", "IX_PermissionId");
            DropIndex("dbo.USERPERMISSIONS", "IX_UserProfileId");
            DropIndex("dbo.CHATCLIENTS", "IX_UserId");
            DropIndex("dbo.USERCATEGORYTRAYS", "IX_Tary_Id");
            DropIndex("dbo.USERCATEGORYTRAYS", "IX_UserCategoryId");
            DropIndex("dbo.USERCATEGORIES", "IX_Permission_Id");
            DropIndex("dbo.USERCATEGORIES", "IX_CategoryName_Id");
            DropIndex("dbo.CHATROOMUSERS", "IX_UserId");
            DropIndex("dbo.CHATROOMUSERS", "IX_RoomId");
            DropIndex("dbo.TRANSACTIONTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.TRANSACTIONTYPES", "IX_Color_Id");
            DropIndex("dbo.TRANSACTIONTYPES", "IX_Abbreviation_Id");
            DropIndex("dbo.TRANSACTIONTYPES", "IX_PermissionId");
            DropIndex("dbo.SUGGESTEDTOPICS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.SUGGESTEDTOPICS", "IX_ParentId");
            DropIndex("dbo.SUBJECTORGUNITS", "IX_SuggestedTopic_Id");
            DropIndex("dbo.SUBJECTORGUNITS", "IX_SubjectClassification_Id");
            DropIndex("dbo.SUBJECTORGUNITS", "IX_OrgUnitId");
            DropIndex("dbo.SUBJECTCLASSIFICATIONS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.SUBJECTCLASSIFICATIONS", "IX_ParentId");
            DropIndex("dbo.TRANSACTIONSUBJECTCLASSIFICATI", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONSUBJECTCLASSIFICATI", "IX_SubjectClassificationId");
            DropIndex("dbo.TRANSACTIONRESERVATIONS", "IX_TransactionCategoryId");
            DropIndex("dbo.TRANSACTIONRESERVATIONS", "IX_EntityId");
            DropIndex("dbo.TRANSACTIONRESERVATIONS", "IX_UserId");
            DropIndex("dbo.PRIORITYEXCEPTIONS", "IX_UserProfileId");
            DropIndex("dbo.PRIORITYEXCEPTIONS", "IX_OrgUnitId");
            DropIndex("dbo.PRIORITYEXCEPTIONS", "IX_PriorityId");
            DropIndex("dbo.PRIORITIES", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.NAMES", "IX_TitleId");
            DropIndex("dbo.NAMES", "IX_NationalityId");
            DropIndex("dbo.TRANSACTIONNAMES", "IX_NameId");
            DropIndex("dbo.TRANSACTIONNAMES", "IX_TransactionId");
            DropIndex("dbo.LINKS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.TRANSACTIONLINKS", "IX_ToTransactionId");
            DropIndex("dbo.TRANSACTIONLINKS", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONLINKS", "IX_TypeId");
            DropIndex("dbo.LETTERTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.TRANSACTIONFOLLOWUPS", "IX_EntityId");
            DropIndex("dbo.TRANSACTIONFOLLOWUPS", "IX_UserId");
            DropIndex("dbo.TRANSACTIONFOLLOWUPS", "IX_TransactionId");
            DropIndex("dbo.EXTERNALPARTYATTACHMENTS", "IX_TransactionExternalCopyId");
            DropIndex("dbo.EXTERNALPARTYATTACHMENTS", "IX_DocumentInfoId");
            DropIndex("dbo.EXTERNALPARTYATTACHMENTS", "IX_PartyId");
            DropIndex("dbo.EXTERNALPARTYMANAGERS", "IX_ExternalParty_Id");
            DropIndex("dbo.EXTERNALPARTYMANAGERS", "IX_Name_Id");
            DropIndex("dbo.EXTERNALPARTIES", "IX_Name_Id");
            DropIndex("dbo.EXTERNALPARTIES", "IX_Address_Id");
            DropIndex("dbo.EXTERNALPARTIES", "IX_ParentId");
            DropIndex("dbo.TRANSACTIONEXTERNALCOPIES", "IX_ActionId");
            DropIndex("dbo.TRANSACTIONEXTERNALCOPIES", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONEXTERNALCOPIES", "IX_FromEntityId");
            DropIndex("dbo.TRANSACTIONEXTERNALCOPIES", "IX_FromUserId");
            DropIndex("dbo.TRANSACTIONEXTERNALCOPIES", "IX_EntityId");
            DropIndex("dbo.TRANSACTIONEXTERNALCOPIES", "IX_UserId");
            DropIndex("dbo.EXPLANATIONS", "IX_Document_Id");
            DropIndex("dbo.EXPLANATIONS", "IX_FromUserId");
            DropIndex("dbo.EXPLANATIONS", "IX_PermissionId");
            DropIndex("dbo.EXPLANATIONS", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONCOPIES", "IX_ActionId");
            DropIndex("dbo.TRANSACTIONCOPIES", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONCOPIES", "IX_FromEntityId");
            DropIndex("dbo.TRANSACTIONCOPIES", "IX_FromUserId");
            DropIndex("dbo.TRANSACTIONCOPIES", "IX_EntityId");
            DropIndex("dbo.TRANSACTIONCOPIES", "IX_UserId");
            DropIndex("dbo.USERGROUPS", "IX_UserId");
            DropIndex("dbo.USERGROUPS", "IX_GroupId");
            DropIndex("dbo.GROUPS", "IX_GroupName_Id");
            DropIndex("dbo.PERMISSIONS", "IX_Name_Id");
            DropIndex("dbo.ATTACHMENTTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.ATTACHMENTS", "IX_DocumentInfo_Id");
            DropIndex("dbo.ATTACHMENTS", "IX_TransactionId");
            DropIndex("dbo.ATTACHMENTS", "IX_TypeId");
            DropIndex("dbo.TRAYS", "IX_Name_Id");
            DropIndex("dbo.TRANSACTIONPATHDETAILS", "IX_ActionId");
            DropIndex("dbo.TRANSACTIONPATHDETAILS", "IX_OrgUnitId");
            DropIndex("dbo.TRANSACTIONPATHDETAILS", "IX_UserId");
            DropIndex("dbo.TRANSACTIONPATHDETAILS", "IX_TransactionPathId");
            DropIndex("dbo.TRANSACTIONPATHS", "IX_TransactionTypeId");
            DropIndex("dbo.TRANSACTIONPATHS", "IX_OrgUnitId");
            DropIndex("dbo.TRANSACTIONPATHS", "IX_UserId");
            DropIndex("dbo.DOCUMENTINFO", "IX_Document_Id");
            DropIndex("dbo.DOCUMENTINFO", "IX_FromEntityId");
            DropIndex("dbo.DOCUMENTINFO", "IX_FromUserId");
            DropIndex("dbo.TASKSATTACHMENTS", "IX_DocumentInfoId");
            DropIndex("dbo.TASKSATTACHMENTS", "IX_TaskId");
            DropIndex("dbo.TASKREMINDERS", "IX_Task_Id");
            DropIndex("dbo.TASKS", "IX_TransactionAssignment_Id");
            DropIndex("dbo.TASKS", "IX_ActionId");
            DropIndex("dbo.TASKS", "IX_TransactionId");
            DropIndex("dbo.TASKS", "IX_FromOrgUnitId");
            DropIndex("dbo.TASKS", "IX_FromUserId");
            DropIndex("dbo.TASKS", "IX_StatusId");
            DropIndex("dbo.TASKS", "IX_ParentId");
            DropIndex("dbo.TASKS", "IX_ToOrgUnitId");
            DropIndex("dbo.TASKS", "IX_ToUserId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_TransactionPathId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_DeliveryMethodId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_PhysicalEntityId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_ToEntityId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_FromEntityId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_ActionId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_TransactionId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_PhysicalUserId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_ToUserId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_FromUserId");
            DropIndex("dbo.TRANSACTIONASSIGNMENTS", "IX_TrayId");
            DropIndex("dbo.TRANSACTIONS", "IX_ReservationId");
            DropIndex("dbo.TRANSACTIONS", "IX_DeliveryMethodId");
            DropIndex("dbo.TRANSACTIONS", "IX_MainDocumentId");
            DropIndex("dbo.TRANSACTIONS", "IX_ExternalPartyManagerId");
            DropIndex("dbo.TRANSACTIONS", "IX_ExternalPartyId");
            DropIndex("dbo.TRANSACTIONS", "IX_LetterTypeId");
            DropIndex("dbo.TRANSACTIONS", "IX_TransactionTypeId");
            DropIndex("dbo.TRANSACTIONS", "IX_ConfidentialityId");
            DropIndex("dbo.TRANSACTIONS", "IX_PriorityId");
            DropIndex("dbo.TRANSACTIONS", "IX_ToUserId");
            DropIndex("dbo.TRANSACTIONS", "IX_EntityId");
            DropIndex("dbo.TRANSACTIONS", "IX_SuggestedTopicId");
            DropIndex("dbo.TRANSACTIONS", "IX_OrgUnitId");
            DropIndex("dbo.TRANSACTIONS", "IX_UserId");
            DropIndex("dbo.TRANSACTIONS", "IX_TransactionCategoryId");
            DropIndex("dbo.TRANSACTIONS", "IX_StatusId");
            DropIndex("dbo.TRANSACTIONS", "IX_SignedByUserId");
            DropIndex("dbo.CHATROOMOWNERS", "IX_UserId");
            DropIndex("dbo.CHATROOMOWNERS", "IX_RoomId");
            DropIndex("dbo.CHATMESSAGESSTATUS", "IX_MessageId");
            DropIndex("dbo.CHATMESSAGESSTATUS", "IX_UserId");
            DropIndex("dbo.CHATMESSAGESSTATUS", "IX_RoomId");
            DropIndex("dbo.CHATMESSAGES", "IX_UserId");
            DropIndex("dbo.CHATMESSAGES", "IX_RoomId");
            DropIndex("dbo.CHATMESSAGES", new[] { "WHEN" });
            DropIndex("dbo.CHATROOMS", "IX_TransactionId");
            DropIndex("dbo.CHATROOMS", new[] { "NAME" });
            DropIndex("dbo.CHATROOMALLOWEDUSERS", "IX_UserId");
            DropIndex("dbo.CHATROOMALLOWEDUSERS", "IX_RoomId");
            DropIndex("dbo.USERPROFILES", "IX_UserImage_Id");
            DropIndex("dbo.USERPROFILES", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.USERPROFILES", "IX_DirectManager_Id");
            DropIndex("dbo.USERPROFILES", "IX_CategoryId");
            DropIndex("dbo.USERPROFILES", "IX_TitleId");
            DropIndex("dbo.ASSIGNMENTPAPERBENEFICIARIES", "IX_AssignmentPaper_Id");
            DropIndex("dbo.ASSIGNMENTPAPERBENEFICIARIES", "IX_UserId");
            DropIndex("dbo.ASSIGNMENTPAPERBENEFICIARIES", "IX_OrgUnitId");
            DropIndex("dbo.ASSIGNMENTPAPERACTIONS", "IX_AssignmentPaper_Id");
            DropIndex("dbo.ASSIGNMENTPAPERACTIONS", "IX_ActionId");
            DropIndex("dbo.ORGUNITS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.ORGUNITS", "IX_Counter_Id");
            DropIndex("dbo.ORGUNITS", "IX_ParentId");
            DropIndex("dbo.ORGUNITS", "IX_AssignmentPaperId");
            DropIndex("dbo.ASSIGNMENTGROUPDETAILS", "IX_AssignmentGroup_Id");
            DropIndex("dbo.ASSIGNMENTGROUPDETAILS", "IX_UserProfile_Id");
            DropIndex("dbo.ASSIGNMENTGROUPDETAILS", "IX_OrgUnit_Id");
            DropIndex("dbo.ASPNETUSERLOGINS", "IX_UserId");
            DropIndex("dbo.ASPNETUSERCLAIMS", "IX_UserId");
            DropIndex("dbo.LOOKUPLOCALIZATIONS", "IX_Lookup_Id");
            DropIndex("dbo.LOOKUPLOCALIZATIONS", "IX_Culture_Id");
            DropIndex("dbo.CULTURES", "IX_NameId");
            DropIndex("dbo.LOCALIZATIONS", "IX_LocalizationIdentifier_Id");
            DropIndex("dbo.LOCALIZATIONS", "IX_CultureId");
            DropIndex("dbo.ACTIONS", "IX_Type_Id");
            DropIndex("dbo.ACTIONS", "IX_LocalizationIdentifier_Id");
            DropTable("dbo.USERPROFILEORGUNITS");
            DropTable("dbo.GROUPPERMISSIONS");
            DropTable("dbo.ASPNETUSERROLES");
            DropTable("dbo.YESSERNEWENTITES");
            DropTable("dbo.YESSERMAPPINGS");
            DropTable("dbo.USERTRAYPREFERENCES");
            DropTable("dbo.USERPREFERENCEFOLLOWUPS");
            DropTable("dbo.USERPREFERENCES");
            DropTable("dbo.USERMOBILES");
            DropTable("dbo.TRANSACTIONLOGS");
            DropTable("dbo.TRANSACTIONINDEXLOGS");
            DropTable("dbo.TRANSACTIONENTITYDETAILS");
            DropTable("dbo.TRANSACTIONASSIGNEES");
            DropTable("dbo.TASKWORKFLOWS");
            DropTable("dbo.TASKHISTORIES");
            DropTable("dbo.SYSTEMDEFAULTVALUES");
            DropTable("dbo.SIGNEDDELIVERYREPORTS");
            DropTable("dbo.SETTINGS");
            DropTable("dbo.RESOURCES");
            DropTable("dbo.NOTIFICATIONUSERS");
            DropTable("dbo.NOTIFICATIONS");
            DropTable("dbo.NOTIFICATIONATTACHMENTS");
            DropTable("dbo.NOTIFICATIONDETAILS");
            DropTable("dbo.HUBTRANSACTIONS");
            DropTable("dbo.HUBRQUIDS");
            DropTable("dbo.HUBRELATEDPERSONS");
            DropTable("dbo.HUBRECORDS");
            DropTable("dbo.HUBATTACHMENTS");
            DropTable("dbo.FORMS");
            DropTable("dbo.FORMDEPARTMENTS");
            DropTable("dbo.FOLLOWUPDETAILS");
            DropTable("dbo.ESCALATIONS");
            DropTable("dbo.DOCUMENTATTRIBUTES");
            DropTable("dbo.DOCPROVIDERS");
            DropTable("dbo.DISTRIBUTIONLISTS");
            DropTable("dbo.DISTRIBUTIONLISTDETAILS");
            DropTable("dbo.COLLABORATIONS");
            DropTable("dbo.CITIES");
            DropTable("dbo.BARCODES");
            DropTable("dbo.AUDITS");
            DropTable("dbo.AUDITDETAILS");
            DropTable("dbo.ATTACHMENTEXTENSIONS");
            DropTable("dbo.ASSIGNMENTGROUPS");
            DropTable("dbo.TRANSACTIONHISTORIES");
            DropTable("dbo.USERDELEGATIONS");
            DropTable("dbo.TRANSACTIONASSIGNMENTHISTORIES");
            DropTable("dbo.TRANSACTIONDELIVERYREPORTS");
            DropTable("dbo.REPORTERS");
            DropTable("dbo.ORGUNITLINKS");
            DropTable("dbo.COUNTERDETAILS");
            DropTable("dbo.COUNTERS");
            DropTable("dbo.BARCODEDESIGNS");
            DropTable("dbo.USERPERMISSIONS");
            DropTable("dbo.CHATCLIENTS");
            DropTable("dbo.USERCATEGORYTRAYS");
            DropTable("dbo.USERCATEGORIES");
            DropTable("dbo.CHATROOMUSERS");
            DropTable("dbo.TRANSACTIONTYPES");
            DropTable("dbo.SUGGESTEDTOPICS");
            DropTable("dbo.SUBJECTORGUNITS");
            DropTable("dbo.SUBJECTCLASSIFICATIONS");
            DropTable("dbo.TRANSACTIONSUBJECTCLASSIFICATI");
            DropTable("dbo.TRANSACTIONRESERVATIONS");
            DropTable("dbo.PRIORITYEXCEPTIONS");
            DropTable("dbo.PRIORITIES");
            DropTable("dbo.NAMES");
            DropTable("dbo.TRANSACTIONNAMES");
            DropTable("dbo.LINKS");
            DropTable("dbo.TRANSACTIONLINKS");
            DropTable("dbo.LETTERTYPES");
            DropTable("dbo.TRANSACTIONFOLLOWUPS");
            DropTable("dbo.EXTERNALPARTYATTACHMENTS");
            DropTable("dbo.EXTERNALPARTYMANAGERS");
            DropTable("dbo.EXTERNALPARTIES");
            DropTable("dbo.TRANSACTIONEXTERNALCOPIES");
            DropTable("dbo.EXPLANATIONS");
            DropTable("dbo.TRANSACTIONCOPIES");
            DropTable("dbo.USERGROUPS");
            DropTable("dbo.GROUPS");
            DropTable("dbo.PERMISSIONS");
            DropTable("dbo.ATTACHMENTTYPES");
            DropTable("dbo.ATTACHMENTS");
            DropTable("dbo.TRAYS");
            DropTable("dbo.TRANSACTIONPATHDETAILS");
            DropTable("dbo.TRANSACTIONPATHS");
            DropTable("dbo.DOCUMENTS");
            DropTable("dbo.DOCUMENTINFO");
            DropTable("dbo.TASKSATTACHMENTS");
            DropTable("dbo.TASKREMINDERS");
            DropTable("dbo.TASKS");
            DropTable("dbo.TRANSACTIONASSIGNMENTS");
            DropTable("dbo.TRANSACTIONS");
            DropTable("dbo.CHATROOMOWNERS");
            DropTable("dbo.CHATMESSAGESSTATUS");
            DropTable("dbo.CHATMESSAGES");
            DropTable("dbo.CHATROOMS");
            DropTable("dbo.CHATROOMALLOWEDUSERS");
            DropTable("dbo.USERPROFILES");
            DropTable("dbo.ASSIGNMENTPAPERBENEFICIARIES");
            DropTable("dbo.ASSIGNMENTPAPERACTIONS");
            DropTable("dbo.ASSIGNMENTPAPERS");
            DropTable("dbo.ORGUNITS");
            DropTable("dbo.ASSIGNMENTGROUPDETAILS");
            DropTable("dbo.ASPNETUSERLOGINS");
            DropTable("dbo.ASPNETUSERCLAIMS");
            DropTable("dbo.ASPNETUSERS");
            DropTable("dbo.ASPNETROLES");
            DropTable("dbo.LOOKUPLOCALIZATIONS");
            DropTable("dbo.LOOKUPS");
            DropTable("dbo.CULTURES");
            DropTable("dbo.LOCALIZATIONS");
            DropTable("dbo.LOCALIZATIONIDENTIFIERS");
            DropTable("dbo.ACTIONS");
        }
    }
}
