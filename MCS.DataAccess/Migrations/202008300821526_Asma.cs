namespace MCS.DataAccess.OracleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Asma : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "MCS_IPA_DM.ACTIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ISASCOPY = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TYPE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TYPE_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id")
                .Index(t => t.TYPE_ID, name: "IX_Type_Id");
            
            CreateTable(
                "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.LOCALIZATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CULTUREID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TEXT = c.String(maxLength: 100),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.CULTURES", t => t.CULTUREID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.CULTUREID, name: "IX_CultureId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.CULTURES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SHORTNAME = c.String(maxLength: 50),
                        NAMEID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.NAMEID)
                .Index(t => t.NAMEID, name: "IX_NameId");
            
            CreateTable(
                "MCS_IPA_DM.LOOKUPS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SORT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ENUMREFERENCE = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.LOOKUPLOCALIZATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TEXT = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        CULTURE_ID = c.Decimal(precision: 10, scale: 0),
                        LOOKUP_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.CULTURES", t => t.CULTURE_ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.LOOKUP_ID)
                .Index(t => t.CULTURE_ID, name: "IX_Culture_Id")
                .Index(t => t.LOOKUP_ID, name: "IX_Lookup_Id");
            
            CreateTable(
                "MCS_IPA_DM.ASPNETROLES",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 1000),
                        NAME = c.String(nullable: false, maxLength: 256),
                        DISCRIMINATOR = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.ASPNETUSERS",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 1000),
                        EMAIL = c.String(maxLength: 256),
                        EMAILCONFIRMED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PASSWORDHASH = c.String(maxLength: 1000),
                        SECURITYSTAMP = c.String(maxLength: 1000),
                        PHONENUMBER = c.String(maxLength: 1000),
                        PHONENUMBERCONFIRMED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TWOFACTORENABLED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKOUTENDDATEUTC = c.DateTime(),
                        LOCKOUTENABLED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ACCESSFAILEDCOUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERNAME = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.ASPNETUSERCLAIMS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.String(nullable: false, maxLength: 1000),
                        CLAIMTYPE = c.String(maxLength: 1000),
                        CLAIMVALUE = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ASPNETUSERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.ASPNETUSERLOGINS",
                c => new
                    {
                        LOGINPROVIDER = c.String(nullable: false, maxLength: 1000),
                        PROVIDERKEY = c.String(nullable: false, maxLength: 1000),
                        USERID = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => new { t.LOGINPROVIDER, t.PROVIDERKEY, t.USERID })
                .ForeignKey("MCS_IPA_DM.ASPNETUSERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.ASSIGNMENTGROUPDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ORGUNIT_ID = c.Decimal(precision: 10, scale: 0),
                        USERPROFILE_ID = c.Decimal(precision: 10, scale: 0),
                        ASSIGNMENTGROUP_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNIT_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERPROFILE_ID)
                .ForeignKey("MCS_IPA_DM.ASSIGNMENTGROUPS", t => t.ASSIGNMENTGROUP_ID)
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id")
                .Index(t => t.USERPROFILE_ID, name: "IX_UserProfile_Id")
                .Index(t => t.ASSIGNMENTGROUP_ID, name: "IX_AssignmentGroup_Id");
            
            CreateTable(
                "MCS_IPA_DM.ORGUNITS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        MANAGERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ASSIGNMENTPAPERID = c.Decimal(precision: 10, scale: 0),
                        PARENTID = c.Decimal(precision: 10, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        NUMBER = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BARCODE = c.String(maxLength: 50),
                        ISVIRTUALUNIT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONSPROCESSINGPERIOD = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        JOINTOGENERALCOUNTER = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LINEAGE = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        COUNTER_ID = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPERID)
                .ForeignKey("MCS_IPA_DM.COUNTERS", t => t.COUNTER_ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.PARENTID)
                .Index(t => t.ASSIGNMENTPAPERID, name: "IX_AssignmentPaperId")
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.COUNTER_ID, name: "IX_Counter_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.ASSIGNMENTPAPERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ISCREATEGROUPALLOWED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.ASSIGNMENTPAPERACTIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ASSIGNMENTPAPER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPER_ID)
                .Index(t => t.ACTIONID, name: "IX_ActionId")
                .Index(t => t.ASSIGNMENTPAPER_ID, name: "IX_AssignmentPaper_Id");
            
            CreateTable(
                "MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ASSIGNMENTPAPER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
                .ForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPER_ID)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ASSIGNMENTPAPER_ID, name: "IX_AssignmentPaper_Id");
            
            CreateTable(
                "MCS_IPA_DM.USERPROFILES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        IDENTITYID = c.String(maxLength: 128),
                        USERNAME = c.String(maxLength: 50),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TITLEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CATEGORYID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONPROCESSINGPERIOD = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PHONENUMBER = c.String(maxLength: 20),
                        EMAIL = c.String(maxLength: 50),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        USERNATIONALID = c.String(nullable: false, maxLength: 1000),
                        ALLOWMOBILE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LASTACTIVITY = c.DateTimeOffset(precision: 6),
                        STATUS = c.Decimal(precision: 10, scale: 0),
                        MAINORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        GENDER = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISMANAGER = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GROUPID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DIRECTMANAGER_ID = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                        USERIMAGE_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.USERCATEGORIES", t => t.CATEGORYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.DIRECTMANAGER_ID)
                .ForeignKey("MCS_IPA_DM.GROUPS", t => t.GROUPID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TITLEID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.DOCUMENTS", t => t.USERIMAGE_ID)
                .Index(t => t.TITLEID, name: "IX_TitleId")
                .Index(t => t.CATEGORYID, name: "IX_CategoryId")
                .Index(t => t.GROUPID, name: "IX_GroupId")
                .Index(t => t.DIRECTMANAGER_ID, name: "IX_DirectManager_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id")
                .Index(t => t.USERIMAGE_ID, name: "IX_UserImage_Id");
            
            CreateTable(
                "MCS_IPA_DM.CHATROOMALLOWEDUSERS",
                c => new
                    {
                        ROOMID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.ROOMID, t.USERID })
                .ForeignKey("MCS_IPA_DM.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.CHATROOMS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        LASTNUDGED = c.DateTimeOffset(precision: 6),
                        NAME = c.String(maxLength: 200),
                        CLOSED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PRIVATE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ONETOONE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.NAME)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId");
            
            CreateTable(
                "MCS_IPA_DM.CHATMESSAGES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CONTENT = c.String(maxLength: 1000),
                        WHEN = c.DateTimeOffset(nullable: false, precision: 6),
                        HTMLENCODED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        MESSAGETYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        HTMLCONTENT = c.String(maxLength: 1000),
                        ROOMID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IMAGEURL = c.String(maxLength: 1000),
                        SOURCE = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.WHEN)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.CHATMESSAGESSTATUS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        LASTUPDATEDDATE = c.DateTimeOffset(nullable: false, precision: 6),
                        ROOMID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        MESSAGEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.CHATMESSAGES", t => t.MESSAGEID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.MESSAGEID, name: "IX_MessageId");
            
            CreateTable(
                "MCS_IPA_DM.CHATROOMOWNERS",
                c => new
                    {
                        ROOMID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.ROOMID, t.USERID })
                .ForeignKey("MCS_IPA_DM.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        NUMBER = c.Decimal(nullable: false, precision: 19, scale: 0),
                        YEAR = c.Decimal(nullable: false, precision: 10, scale: 0),
                        YEARH = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DOCUMENTNUMBER = c.String(maxLength: 1000),
                        REMARKS = c.String(maxLength: 1000),
                        SUBJECT = c.String(maxLength: 1000),
                        PRINTEDDELIVERYREPORT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DELIVERYREPORTNUMBER = c.String(maxLength: 50),
                        SIGNEDBYUSERID = c.Decimal(precision: 10, scale: 0),
                        STATUSID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        REJECTIONREASON = c.String(maxLength: 1000),
                        TRANSACTIONCATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SUGGESTEDTOPICID = c.Decimal(precision: 10, scale: 0),
                        ENTITYID = c.Decimal(precision: 10, scale: 0),
                        TOUSERID = c.Decimal(precision: 10, scale: 0),
                        PRIORITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CONFIDENTIALITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONTYPEID = c.Decimal(precision: 10, scale: 0),
                        LETTERTYPEID = c.Decimal(precision: 10, scale: 0),
                        EXTERNALPARTYID = c.Decimal(precision: 10, scale: 0),
                        EXTERNALPARTYMANAGERID = c.Decimal(precision: 10, scale: 0),
                        MAINDOCUMENTID = c.Decimal(precision: 10, scale: 0),
                        REMINDDATE = c.DateTime(),
                        REMINDDATEH = c.String(maxLength: 20),
                        OUTBOUNDDRAFTID = c.Decimal(precision: 10, scale: 0),
                        OUTBOUNDDRAFTEDITORTYPE = c.Decimal(precision: 10, scale: 0),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISSIGNED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DELIVERYMETHODID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        INBOUNDDATEH = c.String(maxLength: 1000),
                        POSTCODE = c.String(maxLength: 1000),
                        POBOX = c.String(maxLength: 1000),
                        ISDRAFT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISFORINDIVIDUAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SAVEDREASON = c.String(maxLength: 1000),
                        DELIVERYNUMBER = c.String(maxLength: 30),
                        REPORTERID = c.Decimal(precision: 10, scale: 0),
                        INBOUNDINTENDEDPERSON = c.String(maxLength: 1000),
                        RESERVATIONID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.CONFIDENTIALITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTIES", t => t.EXTERNALPARTYID)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTYMANAGERS", t => t.EXTERNALPARTYMANAGERID)
                .ForeignKey("MCS_IPA_DM.LETTERTYPES", t => t.LETTERTYPEID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.MAINDOCUMENTID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONRESERVATIONS", t => t.RESERVATIONID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.SIGNEDBYUSERID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.STATUSID)
                .ForeignKey("MCS_IPA_DM.SUGGESTEDTOPICS", t => t.SUGGESTEDTOPICID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TRANSACTIONCATEGORYID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
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
                "MCS_IPA_DM.TRANSACTIONASSIGNMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRAYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FROMUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOUSERID = c.Decimal(precision: 10, scale: 0),
                        PHYSICALUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ACTIONID = c.Decimal(precision: 10, scale: 0),
                        FROMENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PHYSICALENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESCRIPTION = c.String(maxLength: 1000),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        PHYSICALDATE = c.DateTime(nullable: false),
                        PHYSICALDATEH = c.String(maxLength: 1000),
                        VIEWED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISPOPULARIAZATION = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DELIVERYMETHODID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONPATHID = c.Decimal(precision: 10, scale: 0),
                        CURRENTPATHSTEP = c.Decimal(precision: 10, scale: 0),
                        DUEDATE = c.DateTime(nullable: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.PHYSICALENTITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.PHYSICALUSERID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONPATHS", t => t.TRANSACTIONPATHID)
                .ForeignKey("MCS_IPA_DM.TRAYS", t => t.TRAYID, cascadeDelete: true)
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
                "MCS_IPA_DM.TASKS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TOUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        DELIVERYDATE = c.DateTime(nullable: false),
                        DELIVERYDATEH = c.String(maxLength: 20),
                        ISEXCLUSIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TASKDESCRIPTION = c.String(maxLength: 1000),
                        STATUSDESCRIPTION = c.String(maxLength: 500),
                        PARENTID = c.Decimal(precision: 10, scale: 0),
                        STATUSID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LEVELLIMITATION = c.Decimal(precision: 10, scale: 0),
                        FROMUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FROMORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ACTIONID = c.Decimal(precision: 10, scale: 0),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        NUMBEROFNOTIFICATIONS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONASSIGNMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TASKS", t => t.PARENTID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.STATUSID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOORGUNITID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", t => t.TRANSACTIONASSIGNMENT_ID)
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
                "MCS_IPA_DM.TASKREMINDERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        TASK_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.TASKS", t => t.TASK_ID)
                .Index(t => t.TASK_ID, name: "IX_Task_Id");
            
            CreateTable(
                "MCS_IPA_DM.TASKSATTACHMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TASKID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DOCUMENTINFOID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENTINFOID)
                .ForeignKey("MCS_IPA_DM.TASKS", t => t.TASKID)
                .Index(t => t.TASKID, name: "IX_TaskId")
                .Index(t => t.DOCUMENTINFOID, name: "IX_DocumentInfoId");
            
            CreateTable(
                "MCS_IPA_DM.DOCUMENTINFO",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NAME = c.String(maxLength: 100),
                        SIZE = c.Decimal(nullable: false, precision: 19, scale: 0),
                        MIMETYPE = c.String(maxLength: 100),
                        ECMID = c.String(maxLength: 50),
                        FROMUSERID = c.Decimal(precision: 10, scale: 0),
                        FROMENTITYID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DOCUMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTS", t => t.DOCUMENT_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID)
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.DOCUMENT_ID, name: "IX_Document_Id");
            
            CreateTable(
                "MCS_IPA_DM.DOCUMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CONTENT = c.Binary(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONPATHS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NAME = c.String(maxLength: 1000),
                        USERID = c.Decimal(precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONTYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TRANSACTIONTYPEID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONPATHDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONPATHID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SORT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONPATHS", t => t.TRANSACTIONPATHID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
                .Index(t => t.TRANSACTIONPATHID, name: "IX_TransactionPathId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "MCS_IPA_DM.TRAYS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SORT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        NAME_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.NAME_ID)
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "MCS_IPA_DM.ATTACHMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESCRIPTION = c.String(maxLength: 1000),
                        ATTACHMENTSOURCE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DOCUMENTINFO_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENTINFO_ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ATTACHMENTTYPES", t => t.TYPEID, cascadeDelete: true)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.DOCUMENTINFO_ID, name: "IX_DocumentInfo_Id");
            
            CreateTable(
                "MCS_IPA_DM.ATTACHMENTTYPES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PRINTBARCODE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ARCHIVABLE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.PERMISSIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CODE = c.String(maxLength: 100),
                        ISUSERDEFINED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        WEIGHT = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        NAME_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.NAME_ID)
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "MCS_IPA_DM.GROUPS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ISUSERDEFINED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        GROUPNAME_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.GROUPNAME_ID)
                .Index(t => t.GROUPNAME_ID, name: "IX_GroupName_Id");
            
            CreateTable(
                "MCS_IPA_DM.USERGROUPS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GROUPID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.GROUPS", t => t.GROUPID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.GROUPID, name: "IX_GroupId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONCOPIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(precision: 10, scale: 0),
                        ENTITYID = c.Decimal(precision: 10, scale: 0),
                        FROMUSERID = c.Decimal(precision: 10, scale: 0),
                        FROMENTITYID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        STATUS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISSENT = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "MCS_IPA_DM.EXPLANATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 1000),
                        EXPLANATIONEDITORTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PERMISSIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FROMUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DOCUMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENT_ID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.DOCUMENT_ID, name: "IX_Document_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(precision: 10, scale: 0),
                        ENTITYID = c.Decimal(precision: 10, scale: 0),
                        FROMUSERID = c.Decimal(precision: 10, scale: 0),
                        FROMENTITYID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 1000),
                        VIEWED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        STATUS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTIES", t => t.ENTITYID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTYMANAGERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "MCS_IPA_DM.EXTERNALPARTIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NUMBER = c.String(maxLength: 10),
                        EMAIL = c.String(maxLength: 50),
                        PHONENUMBER = c.String(maxLength: 20),
                        FAX = c.String(maxLength: 20),
                        ISVIRTUAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PARTYTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PARENTID = c.Decimal(precision: 10, scale: 0),
                        YASSERREGISTERED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(precision: 1, scale: 0),
                        LINEAGE = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ADDRESS_ID = c.Decimal(precision: 10, scale: 0),
                        NAME_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.ADDRESS_ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.NAME_ID)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTIES", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.ADDRESS_ID, name: "IX_Address_Id")
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "MCS_IPA_DM.EXTERNALPARTYMANAGERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        NAME_ID = c.Decimal(precision: 10, scale: 0),
                        EXTERNALPARTY_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.NAME_ID)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTIES", t => t.EXTERNALPARTY_ID, cascadeDelete: true)
                .Index(t => t.NAME_ID, name: "IX_Name_Id")
                .Index(t => t.EXTERNALPARTY_ID, name: "IX_ExternalParty_Id");
            
            CreateTable(
                "MCS_IPA_DM.EXTERNALPARTYATTACHMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PARTYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        NAME = c.String(maxLength: 1000),
                        DOCUMENTINFOID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONEXTERNALCOPYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENTINFOID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTIES", t => t.PARTYID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", t => t.TRANSACTIONEXTERNALCOPYID)
                .Index(t => t.PARTYID, name: "IX_PartyId")
                .Index(t => t.DOCUMENTINFOID, name: "IX_DocumentInfoId")
                .Index(t => t.TRANSACTIONEXTERNALCOPYID, name: "IX_TransactionExternalCopyId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONFOLLOWUPS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATETO = c.DateTime(),
                        DATETOH = c.String(maxLength: 1000),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ENTITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId");
            
            CreateTable(
                "MCS_IPA_DM.LETTERTYPES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        LETTERLISTTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISPOPULARIZATION = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONLINKS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOTRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TOTRANSACTIONID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LINKS", t => t.TYPEID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.TOTRANSACTIONID, name: "IX_ToTransactionId");
            
            CreateTable(
                "MCS_IPA_DM.LINKS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONNAMES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        NAMEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.NAMES", t => t.NAMEID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.NAMEID, name: "IX_NameId");
            
            CreateTable(
                "MCS_IPA_DM.NAMES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CIVILID = c.String(maxLength: 10),
                        NATIONALITYID = c.Decimal(precision: 10, scale: 0),
                        FIRSTNAME = c.String(maxLength: 120),
                        MOBILENUMBER = c.String(maxLength: 20),
                        PHONE = c.String(maxLength: 15),
                        EMAIL = c.String(maxLength: 150),
                        ADDRESS = c.String(maxLength: 100),
                        OTHERINFORMATION = c.String(maxLength: 200),
                        TITLEID = c.Decimal(precision: 10, scale: 0),
                        RELATIVERELATION = c.String(maxLength: 1000),
                        CITY = c.String(maxLength: 1000),
                        GENDER = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.NATIONALITYID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TITLEID)
                .Index(t => t.NATIONALITYID, name: "IX_NationalityId")
                .Index(t => t.TITLEID, name: "IX_TitleId");
            
            CreateTable(
                "MCS_IPA_DM.PRIORITIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        HASDATE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LATEFORENTITY = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LATEFORUSER = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SORT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        HASPRIORITYEXCEPTIONS = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.PRIORITYEXCEPTIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PRIORITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERPROFILEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LATEONUSERSAFTER = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERPROFILEID, cascadeDelete: true)
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONRESERVATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        REASON = c.String(maxLength: 1000),
                        TRANSACTIONCATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TRANSACTIONCATEGORYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONSUBJECTCLASSIFICATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SUBJECTCLASSIFICATIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", t => t.SUBJECTCLASSIFICATIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .Index(t => t.SUBJECTCLASSIFICATIONID, name: "IX_SubjectClassificationId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId");
            
            CreateTable(
                "MCS_IPA_DM.SUBJECTCLASSIFICATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISGROUP = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PARENTID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.SUBJECTORGUNITS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        SUBJECTCLASSIFICATION_ID = c.Decimal(precision: 10, scale: 0),
                        SUGGESTEDTOPIC_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", t => t.SUBJECTCLASSIFICATION_ID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.SUGGESTEDTOPICS", t => t.SUGGESTEDTOPIC_ID, cascadeDelete: true)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.SUBJECTCLASSIFICATION_ID, name: "IX_SubjectClassification_Id")
                .Index(t => t.SUGGESTEDTOPIC_ID, name: "IX_SuggestedTopic_Id");
            
            CreateTable(
                "MCS_IPA_DM.SUGGESTEDTOPICS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISGROUP = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PARENTID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.SUGGESTEDTOPICS", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONTYPES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PERMISSIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ABBREVIATION_ID = c.Decimal(precision: 10, scale: 0),
                        COLOR_ID = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.ABBREVIATION_ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.COLOR_ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: true)
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.ABBREVIATION_ID, name: "IX_Abbreviation_Id")
                .Index(t => t.COLOR_ID, name: "IX_Color_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.CHATROOMUSERS",
                c => new
                    {
                        ROOMID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.ROOMID, t.USERID })
                .ForeignKey("MCS_IPA_DM.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.USERCATEGORIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        CATEGORYNAME_ID = c.Decimal(precision: 10, scale: 0),
                        PERMISSION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.CATEGORYNAME_ID)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.PERMISSION_ID)
                .Index(t => t.CATEGORYNAME_ID, name: "IX_CategoryName_Id")
                .Index(t => t.PERMISSION_ID, name: "IX_Permission_Id");
            
            CreateTable(
                "MCS_IPA_DM.USERCATEGORYTRAYS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERCATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        TARY_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.TRAYS", t => t.TARY_ID)
                .ForeignKey("MCS_IPA_DM.USERCATEGORIES", t => t.USERCATEGORYID, cascadeDelete: true)
                .Index(t => t.USERCATEGORYID, name: "IX_UserCategoryId")
                .Index(t => t.TARY_ID, name: "IX_Tary_Id");
            
            CreateTable(
                "MCS_IPA_DM.CHATCLIENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERAGENT = c.String(maxLength: 1000),
                        NAME = c.String(maxLength: 1000),
                        LASTACTIVITY = c.DateTimeOffset(nullable: false, precision: 6),
                        LASTCLIENTACTIVITY = c.DateTimeOffset(precision: 6),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CONNECTIONID = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.USERPERMISSIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERPROFILEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PERMISSIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        GROUPID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.ID, t.USERPROFILEID, t.PERMISSIONID })
                .ForeignKey("MCS_IPA_DM.GROUPS", t => t.GROUPID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERPROFILEID, cascadeDelete: true)
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.GROUPID, name: "IX_GroupId");
            
            CreateTable(
                "MCS_IPA_DM.BARCODEDESIGNS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        HTML = c.String(maxLength: 4000, unicode: false),
                        ISGENERAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        WIDTH = c.Decimal(nullable: false, precision: 10, scale: 0),
                        HEIGHT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ATTACHMENTHTML = c.String(maxLength: 4000, unicode: false),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ORGUNIT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TYPEID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNIT_ID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
            CreateTable(
                "MCS_IPA_DM.COUNTERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ISGENERAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        YEAR = c.String(maxLength: 1000),
                        RESETBYYEAR = c.Decimal(nullable: false, precision: 1, scale: 0),
                        OWNERENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DESCRIPTION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.DESCRIPTION_ID)
                .Index(t => t.DESCRIPTION_ID, name: "IX_Description_Id");
            
            CreateTable(
                "MCS_IPA_DM.COUNTERDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        INITIALVALUE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONTYPEID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        COUNTER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.COUNTERS", t => t.COUNTER_ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.COUNTER_ID, name: "IX_Counter_Id");
            
            CreateTable(
                "MCS_IPA_DM.ORGUNITLINKS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        FROMENTITY_ID = c.Decimal(precision: 10, scale: 0),
                        TOENTITY_ID = c.Decimal(precision: 10, scale: 0),
                        ORGUNIT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITY_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOENTITY_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNIT_ID, cascadeDelete: true)
                .Index(t => t.FROMENTITY_ID, name: "IX_FromEntity_Id")
                .Index(t => t.TOENTITY_ID, name: "IX_ToEntity_Id")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
            CreateTable(
                "MCS_IPA_DM.REPORTERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TOENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        TEXT = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOENTITYID)
                .Index(t => t.TOENTITYID, name: "IX_ToEntityId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        NUMBER = c.String(maxLength: 50),
                        TRANSACTIONASSIGNMENTHISTORYID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONHISTORYID = c.Decimal(precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 50),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DOCUMENTID = c.Decimal(precision: 10, scale: 0),
                        REPORTERID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONEXTERNALCOPYID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENTID)
                .ForeignKey("MCS_IPA_DM.REPORTERS", t => t.REPORTERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", t => t.TRANSACTIONASSIGNMENTHISTORYID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", t => t.TRANSACTIONEXTERNALCOPYID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", t => t.TRANSACTIONHISTORYID)
                .Index(t => t.TRANSACTIONASSIGNMENTHISTORYID, name: "IX_TransactionAssignmentHistoryId")
                .Index(t => t.TRANSACTIONHISTORYID, name: "IX_TransactionHistoryId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.DOCUMENTID, name: "IX_DocumentId")
                .Index(t => t.REPORTERID, name: "IX_ReporterId")
                .Index(t => t.TRANSACTIONEXTERNALCOPYID, name: "IX_TransactionExternalCopyId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRAYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FROMUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOUSERID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(precision: 10, scale: 0),
                        ACTIONID = c.Decimal(precision: 10, scale: 0),
                        FROMENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESCRIPTION = c.String(maxLength: 1000),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        EXPLANATIONID = c.Decimal(precision: 10, scale: 0),
                        USERDELEGATIONID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ACTIONS", t => t.ACTIONID)
                .ForeignKey("MCS_IPA_DM.EXPLANATIONS", t => t.EXPLANATIONID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCS_IPA_DM.TRAYS", t => t.TRAYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERDELEGATIONS", t => t.USERDELEGATIONID)
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
                "MCS_IPA_DM.USERDELEGATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        FROMDATE = c.DateTime(nullable: false),
                        TODATE = c.DateTime(nullable: false),
                        FROMDATEH = c.String(maxLength: 50),
                        TODATEH = c.String(maxLength: 50),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERPROFILEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PRIORITYID = c.Decimal(precision: 10, scale: 0),
                        CONFIDENTIALITYID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONTYPEID = c.Decimal(precision: 10, scale: 0),
                        USERPREFERENCEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        REJECTIONREASON = c.String(maxLength: 1000),
                        STATUSID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RECEIVECOPY = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.CONFIDENTIALITYID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PRIORITIES", t => t.PRIORITYID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.STATUSID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TRANSACTIONTYPEID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERPROFILEID)
                .ForeignKey("MCS_IPA_DM.USERPREFERENCES", t => t.USERPREFERENCEID, cascadeDelete: true)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.CONFIDENTIALITYID, name: "IX_ConfidentialityId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.USERPREFERENCEID, name: "IX_UserPreferenceId")
                .Index(t => t.STATUSID, name: "IX_StatusId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONHISTORIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SIGNEDBYUSERID = c.Decimal(precision: 10, scale: 0),
                        SIGNEDBYORGUNITID = c.Decimal(precision: 10, scale: 0),
                        STATUSID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESTINATIONID = c.Decimal(precision: 10, scale: 0),
                        EXPLANATIONID = c.Decimal(precision: 10, scale: 0),
                        DELIVERYMETHODID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PRIORITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CONFIDENTIALITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        REMARKS = c.String(maxLength: 1000),
                        SUBJECT = c.String(maxLength: 1000),
                        TRANSACTIONCATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONTYPEID = c.Decimal(precision: 10, scale: 0),
                        LETTERTYPEID = c.Decimal(precision: 10, scale: 0),
                        EXTERNALPARTYID = c.Decimal(precision: 10, scale: 0),
                        EXTERNALPARTYMANAGERID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PRINTEDDELIVERYREPORT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DELIVERYREPORTNUMBER = c.String(maxLength: 1000),
                        ATTCHMENTCOUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOENTITYID = c.Decimal(precision: 10, scale: 0),
                        TOUSERID = c.Decimal(precision: 10, scale: 0),
                        REMINDDATE = c.DateTime(),
                        REMINDDATEH = c.String(maxLength: 1000),
                        OUTBOUNDDRAFTID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.CONFIDENTIALITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.DESTINATIONID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.EXPLANATIONID)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTIES", t => t.EXTERNALPARTYID)
                .ForeignKey("MCS_IPA_DM.EXTERNALPARTYMANAGERS", t => t.EXTERNALPARTYMANAGERID)
                .ForeignKey("MCS_IPA_DM.LETTERTYPES", t => t.LETTERTYPEID)
                .ForeignKey("MCS_IPA_DM.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.SIGNEDBYORGUNITID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.SIGNEDBYUSERID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.STATUSID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TRANSACTIONCATEGORYID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID)
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
                "MCS_IPA_DM.ASSIGNMENTGROUPS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        OWNERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.OWNERID, cascadeDelete: true)
                .Index(t => t.OWNERID, name: "IX_OwnerId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.ATTACHMENTEXTENSIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        EXTENSIONNAME = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.AUDITDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PROPERTYNAME = c.String(maxLength: 100),
                        PROPERTYOLDVALUE = c.String(maxLength: 1000),
                        PROPERTYNEWVALUE = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        AUDIT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.AUDITS", t => t.AUDIT_ID)
                .Index(t => t.AUDIT_ID, name: "IX_Audit_Id");
            
            CreateTable(
                "MCS_IPA_DM.AUDITS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IPADDRESS = c.String(maxLength: 50),
                        DATE = c.DateTime(nullable: false),
                        OPERATIONTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ENTITYNAME = c.String(maxLength: 50),
                        PRIMARYKEYVALUE = c.String(maxLength: 1000),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.BARCODES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        VALUE = c.String(maxLength: 1000),
                        REFERENCEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        REFERENCETYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.REFERENCETYPEID, cascadeDelete: true)
                .Index(t => t.REFERENCETYPEID, name: "IX_ReferenceTypeId");
            
            CreateTable(
                "MCS_IPA_DM.CITIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.COLLABORATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SENDERID = c.Decimal(precision: 10, scale: 0),
                        RECEIVERID = c.Decimal(precision: 10, scale: 0),
                        TEXT = c.String(maxLength: 1000),
                        TRANSACTIONID = c.Decimal(precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        STATUS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ATTACHMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ATTACHMENTS", t => t.ATTACHMENT_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.RECEIVERID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.SENDERID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.SENDERID, name: "IX_SenderId")
                .Index(t => t.RECEIVERID, name: "IX_ReceiverId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ATTACHMENT_ID, name: "IX_Attachment_Id");
            
            CreateTable(
                "MCS_IPA_DM.DISTRIBUTIONLISTDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DISTRIBUTIONLISTID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTS", t => t.DISTRIBUTIONLISTID)
                .Index(t => t.DISTRIBUTIONLISTID, name: "IX_DistributionListId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId");
            
            CreateTable(
                "MCS_IPA_DM.DISTRIBUTIONLISTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.LOCALIZATIONIDENTIFIERID, name: "IX_LocalizationIdentifierId");
            
            CreateTable(
                "MCS_IPA_DM.DOCPROVIDERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PROVIDER_TYPE = c.String(maxLength: 50),
                        FILE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FILE_URL = c.String(maxLength: 50),
                        FILE_DOC_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FILE_STATUS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FILE_IS_MIGRATED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANS_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.DOCUMENTATTRIBUTES",
                c => new
                    {
                        DOCUMENTATTRIBUTEID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DOCUMENTNUMBER = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DOCUMENTSYSNUMBER = c.Decimal(precision: 10, scale: 0),
                        DOCUMENTTYPEID = c.Decimal(precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        HIJRIDATE = c.String(maxLength: 50),
                        SUBJECTID = c.Decimal(precision: 10, scale: 0),
                        CONFIDENTIALITYID = c.Decimal(precision: 10, scale: 0),
                        PRIORITYID = c.Decimal(precision: 10, scale: 0),
                        REMARKS = c.String(maxLength: 50),
                        DOCUMENTID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESTINATIONID = c.Decimal(precision: 10, scale: 0),
                        SOURCEID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.DOCUMENTATTRIBUTEID);
            
            CreateTable(
                "MCS_IPA_DM.ESCALATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONCATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PRIORITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ESCALATIONACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ESCALATIONTOID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ESCALATIONAFTERDAYS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.ESCALATIONACTIONID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.ESCALATIONTOID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.TRANSACTIONCATEGORYID, cascadeDelete: true)
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.ESCALATIONACTIONID, name: "IX_EscalationActionId")
                .Index(t => t.ESCALATIONTOID, name: "IX_EscalationToId");
            
            CreateTable(
                "MCS_IPA_DM.FOLLOWUPDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NOTES = c.String(maxLength: 1000),
                        TRANSACTIONFOLLOWUPID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", t => t.TRANSACTIONFOLLOWUPID, cascadeDelete: true)
                .Index(t => t.TRANSACTIONFOLLOWUPID, name: "IX_TransactionFollowUpId");
            
            CreateTable(
                "MCS_IPA_DM.FORMDEPARTMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        FORMID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DEPARTMENTID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.DEPARTMENTID)
                .ForeignKey("MCS_IPA_DM.FORMS", t => t.FORMID, cascadeDelete: true)
                .Index(t => t.FORMID, name: "IX_FormId")
                .Index(t => t.DEPARTMENTID, name: "IX_DepartmentId");
            
            CreateTable(
                "MCS_IPA_DM.FORMS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISLOCKED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LOCKEDBY = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONCATEGORIES = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISINTERNAL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        FORMCONTENT_ID = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.FORMCONTENT_ID)
                .ForeignKey("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.FORMCONTENT_ID, name: "IX_FormContent_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCS_IPA_DM.HUBATTACHMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESCRIPTION = c.String(maxLength: 1000),
                        EXTERNALATTACHEMENTID = c.String(maxLength: 1000),
                        ATTACHMENTID = c.String(maxLength: 1000),
                        ISATTACHMENT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DOCUMENTINFO_ID = c.Decimal(precision: 10, scale: 0),
                        HUBTRANSACTION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENTINFO_ID)
                .ForeignKey("MCS_IPA_DM.ATTACHMENTTYPES", t => t.TYPEID)
                .ForeignKey("MCS_IPA_DM.HUBTRANSACTIONS", t => t.HUBTRANSACTION_ID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.DOCUMENTINFO_ID, name: "IX_DocumentInfo_Id")
                .Index(t => t.HUBTRANSACTION_ID, name: "IX_HubTransaction_Id");
            
            CreateTable(
                "MCS_IPA_DM.HUBRECORDS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        OUTERTEXT = c.String(),
                        METHODNAME = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.HUBRELATEDPERSONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ADDRESS = c.String(maxLength: 1000),
                        EMAIL = c.String(maxLength: 1000),
                        NAME = c.String(maxLength: 1000),
                        NATIONALID = c.String(maxLength: 1000),
                        PHONENUMBER = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        HUBTRANSACTION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.HUBTRANSACTIONS", t => t.HUBTRANSACTION_ID)
                .Index(t => t.HUBTRANSACTION_ID, name: "IX_HubTransaction_Id");
            
            CreateTable(
                "MCS_IPA_DM.HUBRQUIDS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONNUMBER = c.Decimal(nullable: false, precision: 19, scale: 0),
                        RQUID = c.String(maxLength: 1000),
                        ISCOPY = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONSCOPYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.HUBTRANSACTIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONNUMBER = c.String(maxLength: 1000),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PRIORITYLEVELID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CONFIDENTIALITYLEVELID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESTINATIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RECORDDATE = c.DateTime(nullable: false),
                        HIJRIRECORDDATE = c.String(maxLength: 1000),
                        REMARKS = c.String(maxLength: 1000),
                        RQUID = c.Guid(nullable: false),
                        SUBJECT = c.String(maxLength: 1000),
                        REMINDERGDATE = c.DateTime(),
                        REMINDERHDATE = c.String(maxLength: 1000),
                        STATUS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CLASSIFICATION = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        NEWTRANSACTIONID = c.Decimal(precision: 19, scale: 0),
                        NEWTRANSACTIONTIMESTAMP = c.DateTime(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        MAINDOCUMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.MAINDOCUMENT_ID)
                .Index(t => t.MAINDOCUMENT_ID, name: "IX_MainDocument_Id");
            
            CreateTable(
                "MCS_IPA_DM.NOTIFICATIONDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SUBJECT = c.String(maxLength: 1000),
                        BODY = c.String(maxLength: 1000),
                        LINK = c.String(maxLength: 1000),
                        EMAIL = c.String(maxLength: 1000),
                        ISSENT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        FAILURECOUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        NOTIFICATIONTEMPLATETYPE_ID = c.Decimal(precision: 10, scale: 0),
                        NOTIFICATIONTYPE_ID = c.Decimal(precision: 10, scale: 0),
                        NOTIFICATION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.NOTIFICATIONTEMPLATETYPE_ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.NOTIFICATIONTYPE_ID)
                .ForeignKey("MCS_IPA_DM.NOTIFICATIONS", t => t.NOTIFICATION_ID)
                .Index(t => t.NOTIFICATIONTEMPLATETYPE_ID, name: "IX_NotificationTemplateType_Id")
                .Index(t => t.NOTIFICATIONTYPE_ID, name: "IX_NotificationType_Id")
                .Index(t => t.NOTIFICATION_ID, name: "IX_Notification_Id");
            
            CreateTable(
                "MCS_IPA_DM.NOTIFICATIONATTACHMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BINARY = c.Binary(),
                        FILENAME = c.String(maxLength: 100),
                        CONTENTTYPE = c.String(maxLength: 1000),
                        CONTENTLENGTH = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        NOTIFICATIONDETAIL_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.NOTIFICATIONDETAILS", t => t.NOTIFICATIONDETAIL_ID)
                .Index(t => t.NOTIFICATIONDETAIL_ID, name: "IX_NotificationDetail_Id");
            
            CreateTable(
                "MCS_IPA_DM.NOTIFICATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SOURCEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        ISREAD = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.SOURCEID, cascadeDelete: true)
                .Index(t => t.SOURCEID, name: "IX_SourceId");
            
            CreateTable(
                "MCS_IPA_DM.NOTIFICATIONUSERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        NOTIFICATION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.NOTIFICATIONS", t => t.NOTIFICATION_ID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.NOTIFICATION_ID, name: "IX_Notification_Id");
            
            CreateTable(
                "MCS_IPA_DM.RESOURCES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        RESOURCEID = c.String(nullable: false, maxLength: 1024),
                        VALUE = c.String(maxLength: 1000),
                        CULTURE = c.String(maxLength: 10),
                        RESOURCESET = c.String(maxLength: 512),
                        TYPE = c.String(maxLength: 512),
                        BINFILE = c.Binary(),
                        TEXTFILE = c.String(maxLength: 1000),
                        FILENAME = c.String(maxLength: 128),
                        COMMENT = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.SETTINGS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        KEY = c.String(maxLength: 1000),
                        VALUE = c.String(maxLength: 1000),
                        BLOBVALUE = c.Binary(),
                        TYPE = c.Decimal(precision: 10, scale: 0),
                        DESCRIPTION = c.String(maxLength: 1000),
                        MODELID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RESOURCEID = c.String(maxLength: 1000),
                        ISREADONLY = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.SIGNEDDELIVERYREPORTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 1000),
                        DOCUMENTID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.DOCUMENTINFO", t => t.DOCUMENTID)
                .Index(t => t.DOCUMENTID, name: "IX_DocumentId");
            
            CreateTable(
                "MCS_IPA_DM.SYSTEMDEFAULTVALUES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DEFAULTVALUEID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.TASKHISTORIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 20),
                        DELIVERYDATE = c.DateTime(nullable: false),
                        DELIVERYDATEH = c.String(maxLength: 20),
                        STATUSDESCRIPTION = c.String(maxLength: 500),
                        TASKDESCRIPTION = c.String(maxLength: 1000),
                        ISEXCLUSIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        FROMORGUNIT_ID = c.Decimal(precision: 10, scale: 0),
                        FROMUSER_ID = c.Decimal(precision: 10, scale: 0),
                        PARENT_ID = c.Decimal(precision: 10, scale: 0),
                        STATUS_ID = c.Decimal(precision: 10, scale: 0),
                        TOORGUNIT_ID = c.Decimal(precision: 10, scale: 0),
                        TOUSER_ID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMORGUNIT_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.FROMUSER_ID)
                .ForeignKey("MCS_IPA_DM.TASKS", t => t.PARENT_ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.STATUS_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOORGUNIT_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSER_ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTION_ID)
                .Index(t => t.FROMORGUNIT_ID, name: "IX_FromOrgUnit_Id")
                .Index(t => t.FROMUSER_ID, name: "IX_FromUser_Id")
                .Index(t => t.PARENT_ID, name: "IX_Parent_Id")
                .Index(t => t.STATUS_ID, name: "IX_Status_Id")
                .Index(t => t.TOORGUNIT_ID, name: "IX_ToOrgUnit_Id")
                .Index(t => t.TOUSER_ID, name: "IX_ToUser_Id")
                .Index(t => t.TRANSACTION_ID, name: "IX_Transaction_Id");
            
            CreateTable(
                "MCS_IPA_DM.TASKWORKFLOWS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        FROMENTITY_ID = c.Decimal(precision: 10, scale: 0),
                        TOENTITY_ID = c.Decimal(precision: 10, scale: 0),
                        TOUSER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.FROMENTITY_ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.TOENTITY_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.TOUSER_ID)
                .Index(t => t.FROMENTITY_ID, name: "IX_FromEntity_Id")
                .Index(t => t.TOENTITY_ID, name: "IX_ToEntity_Id")
                .Index(t => t.TOUSER_ID, name: "IX_ToUser_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONASSIGNEES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        ENTITY_ID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTION_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ENTITY_ID)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTION_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USER_ID)
                .Index(t => t.ENTITY_ID, name: "IX_Entity_Id")
                .Index(t => t.TRANSACTION_ID, name: "IX_Transaction_Id")
                .Index(t => t.USER_ID, name: "IX_User_Id");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONENTITYDETAILS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ENTITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ENTITYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ENTITYID, name: "IX_EntityId");
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONINDEXLOGS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONCATEGORYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANSACTIONTYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        NUMBER = c.Decimal(nullable: false, precision: 19, scale: 0),
                        BARCODE = c.String(maxLength: 50),
                        DATEH = c.String(maxLength: 50),
                        DATE = c.DateTime(nullable: false),
                        YEAR = c.Decimal(nullable: false, precision: 10, scale: 0),
                        YEARH = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PERMISSIONCODE = c.String(maxLength: 1000),
                        PRIORITYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PARTYID = c.Decimal(precision: 10, scale: 0),
                        ORGUNITID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SIGNEDBYUSERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DIRECTEDTOUSERID = c.Decimal(precision: 10, scale: 0),
                        STATUSID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LETTERTYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
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
                        SUBJECT = c.String(maxLength: 1000),
                        ASSIGNMENTS = c.String(maxLength: 50),
                        ISINDEXED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ISUPDATED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        WITHARCHIVING = c.Decimal(nullable: false, precision: 1, scale: 0),
                        COLOR = c.String(maxLength: 50),
                        SUBJECTCLASSIFICATIONS = c.String(maxLength: 500),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.TRANSACTIONLOGS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DATE = c.DateTime(nullable: false),
                        DATEH = c.String(maxLength: 1000),
                        TRANSACTIONID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        AUDITINGACTIONCODE_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.LOOKUPS", t => t.AUDITINGACTIONCODE_ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.AUDITINGACTIONCODE_ID, name: "IX_AuditingActionCode_Id");
            
            CreateTable(
                "MCS_IPA_DM.USERMOBILES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TOKEN = c.String(maxLength: 1000),
                        DEVICETOKEN = c.String(maxLength: 1000),
                        ACTIVATIONREQUESTCODE = c.String(maxLength: 1000),
                        ACTIVATAIONCODE = c.String(maxLength: 1000),
                        DEACTIVATIONREQUESTCODE = c.String(maxLength: 1000),
                        SIGNEDCERT = c.String(maxLength: 1000),
                        CA = c.String(maxLength: 1000),
                        CACRL = c.String(maxLength: 1000),
                        ISUPDATED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        UPDATEFLAGS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LASTLOGINDATE = c.DateTime(nullable: false),
                        LOGS = c.Binary(),
                        SETTINGS = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.USERPREFERENCES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CULTUREID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISDELEGATIONENABLED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SIGNATURE = c.Binary(),
                        MARKINGDOC = c.Binary(),
                        SIGNATUREPASSWORD = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SIGNATUREPASSWORDTEXT = c.String(maxLength: 1000),
                        FREETEXT = c.String(maxLength: 1000),
                        EMAIL = c.String(maxLength: 50),
                        USERPROFILEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        OTP = c.String(maxLength: 1000),
                        OTPCREATEDON = c.DateTime(),
                        NOTIFICATIONSUBSCRIPTIONS = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ASSIGNMENTPAPERID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPERID)
                .ForeignKey("MCS_IPA_DM.CULTURES", t => t.CULTUREID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERPROFILEID)
                .Index(t => t.CULTUREID, name: "IX_CultureId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.ASSIGNMENTPAPERID, name: "IX_AssignmentPaperId");
            
            CreateTable(
                "MCS_IPA_DM.USERTRAYPREFERENCES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRAYID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        USERPREFERENCE_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCS_IPA_DM.TRAYS", t => t.TRAYID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.USERPREFERENCES", t => t.USERPREFERENCE_ID)
                .Index(t => t.TRAYID, name: "IX_TrayId")
                .Index(t => t.USERPREFERENCE_ID, name: "IX_UserPreference_Id");
            
            CreateTable(
                "MCS_IPA_DM.YESSERMAPPINGS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        YESSERTYPEID = c.String(maxLength: 1000),
                        CLOUDTYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        EXPONENT = c.Binary(),
                        MODULUS = c.Binary(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCS_IPA_DM.ASPNETUSERROLES",
                c => new
                    {
                        ROLEID = c.String(nullable: false, maxLength: 1000),
                        USERID = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => new { t.ROLEID, t.USERID })
                .ForeignKey("MCS_IPA_DM.ASPNETROLES", t => t.ROLEID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ASPNETUSERS", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROLEID, name: "IX_RoleId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCS_IPA_DM.GROUPPERMISSIONS",
                c => new
                    {
                        GROUP_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PERMISSION_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.GROUP_ID, t.PERMISSION_ID })
                .ForeignKey("MCS_IPA_DM.GROUPS", t => t.GROUP_ID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.PERMISSIONS", t => t.PERMISSION_ID, cascadeDelete: true)
                .Index(t => t.GROUP_ID, name: "IX_Group_Id")
                .Index(t => t.PERMISSION_ID, name: "IX_Permission_Id");
            
            CreateTable(
                "MCS_IPA_DM.USERPROFILEORGUNITS",
                c => new
                    {
                        USERPROFILE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ORGUNIT_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.USERPROFILE_ID, t.ORGUNIT_ID })
                .ForeignKey("MCS_IPA_DM.USERPROFILES", t => t.USERPROFILE_ID, cascadeDelete: true)
                .ForeignKey("MCS_IPA_DM.ORGUNITS", t => t.ORGUNIT_ID, cascadeDelete: true)
                .Index(t => t.USERPROFILE_ID, name: "IX_UserProfile_Id")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("MCS_IPA_DM.USERTRAYPREFERENCES", "USERPREFERENCE_ID", "MCS_IPA_DM.USERPREFERENCES");
            DropForeignKey("MCS_IPA_DM.USERTRAYPREFERENCES", "TRAYID", "MCS_IPA_DM.TRAYS");
            DropForeignKey("MCS_IPA_DM.USERPREFERENCES", "USERPROFILEID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "USERPREFERENCEID", "MCS_IPA_DM.USERPREFERENCES");
            DropForeignKey("MCS_IPA_DM.USERPREFERENCES", "CULTUREID", "MCS_IPA_DM.CULTURES");
            DropForeignKey("MCS_IPA_DM.USERPREFERENCES", "ASSIGNMENTPAPERID", "MCS_IPA_DM.ASSIGNMENTPAPERS");
            DropForeignKey("MCS_IPA_DM.USERMOBILES", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONLOGS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONLOGS", "AUDITINGACTIONCODE_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONENTITYDETAILS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONENTITYDETAILS", "ENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNEES", "USER_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNEES", "TRANSACTION_ID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNEES", "ENTITY_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKWORKFLOWS", "TOUSER_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TASKWORKFLOWS", "TOENTITY_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKWORKFLOWS", "FROMENTITY_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "TRANSACTION_ID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "TOUSER_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "TOORGUNIT_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "STATUS_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "PARENT_ID", "MCS_IPA_DM.TASKS");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "FROMUSER_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TASKHISTORIES", "FROMORGUNIT_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.SIGNEDDELIVERYREPORTS", "DOCUMENTID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONUSERS", "NOTIFICATION_ID", "MCS_IPA_DM.NOTIFICATIONS");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONUSERS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONS", "SOURCEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONDETAILS", "NOTIFICATION_ID", "MCS_IPA_DM.NOTIFICATIONS");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONDETAILS", "NOTIFICATIONTYPE_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONDETAILS", "NOTIFICATIONTEMPLATETYPE_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.NOTIFICATIONATTACHMENTS", "NOTIFICATIONDETAIL_ID", "MCS_IPA_DM.NOTIFICATIONDETAILS");
            DropForeignKey("MCS_IPA_DM.HUBTRANSACTIONS", "MAINDOCUMENT_ID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.HUBRELATEDPERSONS", "HUBTRANSACTION_ID", "MCS_IPA_DM.HUBTRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.HUBATTACHMENTS", "HUBTRANSACTION_ID", "MCS_IPA_DM.HUBTRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.HUBATTACHMENTS", "TYPEID", "MCS_IPA_DM.ATTACHMENTTYPES");
            DropForeignKey("MCS_IPA_DM.HUBATTACHMENTS", "DOCUMENTINFO_ID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.FORMS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.FORMS", "FORMCONTENT_ID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.FORMDEPARTMENTS", "FORMID", "MCS_IPA_DM.FORMS");
            DropForeignKey("MCS_IPA_DM.FORMDEPARTMENTS", "DEPARTMENTID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.FOLLOWUPDETAILS", "TRANSACTIONFOLLOWUPID", "MCS_IPA_DM.TRANSACTIONFOLLOWUPS");
            DropForeignKey("MCS_IPA_DM.ESCALATIONS", "TRANSACTIONCATEGORYID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.ESCALATIONS", "PRIORITYID", "MCS_IPA_DM.PRIORITIES");
            DropForeignKey("MCS_IPA_DM.ESCALATIONS", "ESCALATIONTOID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.ESCALATIONS", "ESCALATIONACTIONID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTS", "LOCALIZATIONIDENTIFIERID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS", "DISTRIBUTIONLISTID", "MCS_IPA_DM.DISTRIBUTIONLISTS");
            DropForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.COLLABORATIONS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.COLLABORATIONS", "SENDERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.COLLABORATIONS", "RECEIVERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.COLLABORATIONS", "ATTACHMENT_ID", "MCS_IPA_DM.ATTACHMENTS");
            DropForeignKey("MCS_IPA_DM.CITIES", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.BARCODES", "REFERENCETYPEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.AUDITDETAILS", "AUDIT_ID", "MCS_IPA_DM.AUDITS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTGROUPS", "OWNERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTGROUPS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS", "ASSIGNMENTGROUP_ID", "MCS_IPA_DM.ASSIGNMENTGROUPS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS", "USERPROFILE_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS", "ORGUNIT_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.REPORTERS", "TOENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONHISTORYID", "MCS_IPA_DM.TRANSACTIONHISTORIES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "TRANSACTIONTYPEID", "MCS_IPA_DM.TRANSACTIONTYPES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "TRANSACTIONCATEGORYID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "TOUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "TOENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "STATUSID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "SIGNEDBYUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "SIGNEDBYORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "PRIORITYID", "MCS_IPA_DM.PRIORITIES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "LETTERTYPEID", "MCS_IPA_DM.LETTERTYPES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "EXTERNALPARTYMANAGERID", "MCS_IPA_DM.EXTERNALPARTYMANAGERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "EXTERNALPARTYID", "MCS_IPA_DM.EXTERNALPARTIES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "EXPLANATIONID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "DESTINATIONID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "DELIVERYMETHODID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONHISTORIES", "CONFIDENTIALITYID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONEXTERNALCOPYID", "MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONASSIGNMENTHISTORYID", "MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "USERDELEGATIONID", "MCS_IPA_DM.USERDELEGATIONS");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "USERPROFILEID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "TRANSACTIONTYPEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "STATUSID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "PRIORITYID", "MCS_IPA_DM.PRIORITIES");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.USERDELEGATIONS", "CONFIDENTIALITYID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "TRAYID", "MCS_IPA_DM.TRAYS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "TOUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "TOENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "FROMENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "EXPLANATIONID", "MCS_IPA_DM.EXPLANATIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "REPORTERID", "MCS_IPA_DM.REPORTERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "DOCUMENTID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.REPORTERS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.ORGUNITS", "PARENTID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.ORGUNITS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.ORGUNITLINKS", "ORGUNIT_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.ORGUNITLINKS", "TOENTITY_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.ORGUNITLINKS", "FROMENTITY_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.ORGUNITS", "COUNTER_ID", "MCS_IPA_DM.COUNTERS");
            DropForeignKey("MCS_IPA_DM.COUNTERS", "DESCRIPTION_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.COUNTERDETAILS", "TRANSACTIONTYPEID", "MCS_IPA_DM.TRANSACTIONTYPES");
            DropForeignKey("MCS_IPA_DM.COUNTERDETAILS", "COUNTER_ID", "MCS_IPA_DM.COUNTERS");
            DropForeignKey("MCS_IPA_DM.BARCODEDESIGNS", "ORGUNIT_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.BARCODEDESIGNS", "TYPEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.ORGUNITS", "ASSIGNMENTPAPERID", "MCS_IPA_DM.ASSIGNMENTPAPERS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES", "ASSIGNMENTPAPER_ID", "MCS_IPA_DM.ASSIGNMENTPAPERS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERPROFILES", "USERIMAGE_ID", "MCS_IPA_DM.DOCUMENTS");
            DropForeignKey("MCS_IPA_DM.USERPROFILES", "TITLEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.USERPERMISSIONS", "USERPROFILEID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERPERMISSIONS", "PERMISSIONID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.USERPERMISSIONS", "GROUPID", "MCS_IPA_DM.GROUPS");
            DropForeignKey("MCS_IPA_DM.USERPROFILEORGUNITS", "ORGUNIT_ID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.USERPROFILEORGUNITS", "USERPROFILE_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERPROFILES", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.USERPROFILES", "GROUPID", "MCS_IPA_DM.GROUPS");
            DropForeignKey("MCS_IPA_DM.USERPROFILES", "DIRECTMANAGER_ID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.CHATCLIENTS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERPROFILES", "CATEGORYID", "MCS_IPA_DM.USERCATEGORIES");
            DropForeignKey("MCS_IPA_DM.USERCATEGORIES", "PERMISSION_ID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.USERCATEGORYTRAYS", "USERCATEGORYID", "MCS_IPA_DM.USERCATEGORIES");
            DropForeignKey("MCS_IPA_DM.USERCATEGORYTRAYS", "TARY_ID", "MCS_IPA_DM.TRAYS");
            DropForeignKey("MCS_IPA_DM.USERCATEGORIES", "CATEGORYNAME_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.CHATROOMALLOWEDUSERS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.CHATROOMUSERS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.CHATROOMUSERS", "ROOMID", "MCS_IPA_DM.CHATROOMS");
            DropForeignKey("MCS_IPA_DM.CHATROOMS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "TRANSACTIONTYPEID", "MCS_IPA_DM.TRANSACTIONTYPES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", "PERMISSIONID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", "COLOR_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONTYPES", "ABBREVIATION_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "TRANSACTIONCATEGORYID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "TOUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "SUGGESTEDTOPICID", "MCS_IPA_DM.SUGGESTEDTOPICS");
            DropForeignKey("MCS_IPA_DM.SUBJECTORGUNITS", "SUGGESTEDTOPIC_ID", "MCS_IPA_DM.SUGGESTEDTOPICS");
            DropForeignKey("MCS_IPA_DM.SUGGESTEDTOPICS", "PARENTID", "MCS_IPA_DM.SUGGESTEDTOPICS");
            DropForeignKey("MCS_IPA_DM.SUGGESTEDTOPICS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONSUBJECTCLASSIFICATIONS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONSUBJECTCLASSIFICATIONS", "SUBJECTCLASSIFICATIONID", "MCS_IPA_DM.SUBJECTCLASSIFICATIONS");
            DropForeignKey("MCS_IPA_DM.SUBJECTORGUNITS", "SUBJECTCLASSIFICATION_ID", "MCS_IPA_DM.SUBJECTCLASSIFICATIONS");
            DropForeignKey("MCS_IPA_DM.SUBJECTORGUNITS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", "PARENTID", "MCS_IPA_DM.SUBJECTCLASSIFICATIONS");
            DropForeignKey("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "STATUSID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "SIGNEDBYUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONRESERVATIONS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "RESERVATIONID", "MCS_IPA_DM.TRANSACTIONRESERVATIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONRESERVATIONS", "TRANSACTIONCATEGORYID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONRESERVATIONS", "ENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "PRIORITYID", "MCS_IPA_DM.PRIORITIES");
            DropForeignKey("MCS_IPA_DM.PRIORITYEXCEPTIONS", "USERPROFILEID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.PRIORITYEXCEPTIONS", "PRIORITYID", "MCS_IPA_DM.PRIORITIES");
            DropForeignKey("MCS_IPA_DM.PRIORITYEXCEPTIONS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.PRIORITIES", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONNAMES", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONNAMES", "NAMEID", "MCS_IPA_DM.NAMES");
            DropForeignKey("MCS_IPA_DM.NAMES", "TITLEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.NAMES", "NATIONALITYID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "MAINDOCUMENTID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONLINKS", "TYPEID", "MCS_IPA_DM.LINKS");
            DropForeignKey("MCS_IPA_DM.LINKS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONLINKS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONLINKS", "TOTRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "LETTERTYPEID", "MCS_IPA_DM.LETTERTYPES");
            DropForeignKey("MCS_IPA_DM.LETTERTYPES", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", "ENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "EXTERNALPARTYMANAGERID", "MCS_IPA_DM.EXTERNALPARTYMANAGERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "EXTERNALPARTYID", "MCS_IPA_DM.EXTERNALPARTIES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "USERID", "MCS_IPA_DM.EXTERNALPARTYMANAGERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "FROMENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS", "TRANSACTIONEXTERNALCOPYID", "MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS", "PARTYID", "MCS_IPA_DM.EXTERNALPARTIES");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS", "DOCUMENTINFOID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "ENTITYID", "MCS_IPA_DM.EXTERNALPARTIES");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTYMANAGERS", "EXTERNALPARTY_ID", "MCS_IPA_DM.EXTERNALPARTIES");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTYMANAGERS", "NAME_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTIES", "PARENTID", "MCS_IPA_DM.EXTERNALPARTIES");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTIES", "NAME_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.EXTERNALPARTIES", "ADDRESS_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.EXPLANATIONS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.EXPLANATIONS", "PERMISSIONID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.EXPLANATIONS", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.EXPLANATIONS", "DOCUMENT_ID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "ENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "DELIVERYMETHODID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONCOPIES", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONCOPIES", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONCOPIES", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONCOPIES", "FROMENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONCOPIES", "ENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONCOPIES", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONS", "CONFIDENTIALITYID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.GROUPPERMISSIONS", "PERMISSION_ID", "MCS_IPA_DM.PERMISSIONS");
            DropForeignKey("MCS_IPA_DM.GROUPPERMISSIONS", "GROUP_ID", "MCS_IPA_DM.GROUPS");
            DropForeignKey("MCS_IPA_DM.USERGROUPS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.USERGROUPS", "GROUPID", "MCS_IPA_DM.GROUPS");
            DropForeignKey("MCS_IPA_DM.GROUPS", "GROUPNAME_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.PERMISSIONS", "NAME_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.ATTACHMENTS", "TYPEID", "MCS_IPA_DM.ATTACHMENTTYPES");
            DropForeignKey("MCS_IPA_DM.ATTACHMENTTYPES", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.ATTACHMENTS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.ATTACHMENTS", "DOCUMENTINFO_ID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "TRAYID", "MCS_IPA_DM.TRAYS");
            DropForeignKey("MCS_IPA_DM.TRAYS", "NAME_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "TRANSACTIONPATHID", "MCS_IPA_DM.TRANSACTIONPATHS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHS", "TRANSACTIONTYPEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "TRANSACTIONPATHID", "MCS_IPA_DM.TRANSACTIONPATHS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONPATHS", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "TOUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "TOENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKS", "TRANSACTIONASSIGNMENT_ID", "MCS_IPA_DM.TRANSACTIONASSIGNMENTS");
            DropForeignKey("MCS_IPA_DM.TASKS", "TRANSACTIONID", "MCS_IPA_DM.TRANSACTIONS");
            DropForeignKey("MCS_IPA_DM.TASKS", "TOUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TASKS", "TOORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKSATTACHMENTS", "TASKID", "MCS_IPA_DM.TASKS");
            DropForeignKey("MCS_IPA_DM.TASKSATTACHMENTS", "DOCUMENTINFOID", "MCS_IPA_DM.DOCUMENTINFO");
            DropForeignKey("MCS_IPA_DM.DOCUMENTINFO", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.DOCUMENTINFO", "FROMENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.DOCUMENTINFO", "DOCUMENT_ID", "MCS_IPA_DM.DOCUMENTS");
            DropForeignKey("MCS_IPA_DM.TASKS", "STATUSID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TASKREMINDERS", "TASK_ID", "MCS_IPA_DM.TASKS");
            DropForeignKey("MCS_IPA_DM.TASKS", "PARENTID", "MCS_IPA_DM.TASKS");
            DropForeignKey("MCS_IPA_DM.TASKS", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TASKS", "FROMORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TASKS", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "PHYSICALUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "PHYSICALENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "FROMUSERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "FROMENTITYID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "DELIVERYMETHODID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.CHATROOMOWNERS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.CHATROOMOWNERS", "ROOMID", "MCS_IPA_DM.CHATROOMS");
            DropForeignKey("MCS_IPA_DM.CHATMESSAGES", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.CHATMESSAGES", "ROOMID", "MCS_IPA_DM.CHATROOMS");
            DropForeignKey("MCS_IPA_DM.CHATMESSAGESSTATUS", "USERID", "MCS_IPA_DM.USERPROFILES");
            DropForeignKey("MCS_IPA_DM.CHATMESSAGESSTATUS", "ROOMID", "MCS_IPA_DM.CHATROOMS");
            DropForeignKey("MCS_IPA_DM.CHATMESSAGESSTATUS", "MESSAGEID", "MCS_IPA_DM.CHATMESSAGES");
            DropForeignKey("MCS_IPA_DM.CHATROOMALLOWEDUSERS", "ROOMID", "MCS_IPA_DM.CHATROOMS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES", "ORGUNITID", "MCS_IPA_DM.ORGUNITS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERACTIONS", "ASSIGNMENTPAPER_ID", "MCS_IPA_DM.ASSIGNMENTPAPERS");
            DropForeignKey("MCS_IPA_DM.ASSIGNMENTPAPERACTIONS", "ACTIONID", "MCS_IPA_DM.ACTIONS");
            DropForeignKey("MCS_IPA_DM.ASPNETUSERLOGINS", "USERID", "MCS_IPA_DM.ASPNETUSERS");
            DropForeignKey("MCS_IPA_DM.ASPNETUSERCLAIMS", "USERID", "MCS_IPA_DM.ASPNETUSERS");
            DropForeignKey("MCS_IPA_DM.ASPNETUSERROLES", "USERID", "MCS_IPA_DM.ASPNETUSERS");
            DropForeignKey("MCS_IPA_DM.ASPNETUSERROLES", "ROLEID", "MCS_IPA_DM.ASPNETROLES");
            DropForeignKey("MCS_IPA_DM.ACTIONS", "TYPE_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.ACTIONS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.LOCALIZATIONS", "LOCALIZATIONIDENTIFIER_ID", "MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCS_IPA_DM.LOCALIZATIONS", "CULTUREID", "MCS_IPA_DM.CULTURES");
            DropForeignKey("MCS_IPA_DM.CULTURES", "NAMEID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.LOOKUPLOCALIZATIONS", "LOOKUP_ID", "MCS_IPA_DM.LOOKUPS");
            DropForeignKey("MCS_IPA_DM.LOOKUPLOCALIZATIONS", "CULTURE_ID", "MCS_IPA_DM.CULTURES");
            DropIndex("MCS_IPA_DM.USERPROFILEORGUNITS", "IX_OrgUnit_Id");
            DropIndex("MCS_IPA_DM.USERPROFILEORGUNITS", "IX_UserProfile_Id");
            DropIndex("MCS_IPA_DM.GROUPPERMISSIONS", "IX_Permission_Id");
            DropIndex("MCS_IPA_DM.GROUPPERMISSIONS", "IX_Group_Id");
            DropIndex("MCS_IPA_DM.ASPNETUSERROLES", "IX_UserId");
            DropIndex("MCS_IPA_DM.ASPNETUSERROLES", "IX_RoleId");
            DropIndex("MCS_IPA_DM.USERTRAYPREFERENCES", "IX_UserPreference_Id");
            DropIndex("MCS_IPA_DM.USERTRAYPREFERENCES", "IX_TrayId");
            DropIndex("MCS_IPA_DM.USERPREFERENCES", "IX_AssignmentPaperId");
            DropIndex("MCS_IPA_DM.USERPREFERENCES", "IX_UserProfileId");
            DropIndex("MCS_IPA_DM.USERPREFERENCES", "IX_CultureId");
            DropIndex("MCS_IPA_DM.USERMOBILES", "IX_UserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONLOGS", "IX_AuditingActionCode_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONLOGS", "IX_UserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONENTITYDETAILS", "IX_EntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONENTITYDETAILS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNEES", "IX_User_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNEES", "IX_Transaction_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNEES", "IX_Entity_Id");
            DropIndex("MCS_IPA_DM.TASKWORKFLOWS", "IX_ToUser_Id");
            DropIndex("MCS_IPA_DM.TASKWORKFLOWS", "IX_ToEntity_Id");
            DropIndex("MCS_IPA_DM.TASKWORKFLOWS", "IX_FromEntity_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_Transaction_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_ToUser_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_ToOrgUnit_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_Status_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_Parent_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_FromUser_Id");
            DropIndex("MCS_IPA_DM.TASKHISTORIES", "IX_FromOrgUnit_Id");
            DropIndex("MCS_IPA_DM.SIGNEDDELIVERYREPORTS", "IX_DocumentId");
            DropIndex("MCS_IPA_DM.NOTIFICATIONUSERS", "IX_Notification_Id");
            DropIndex("MCS_IPA_DM.NOTIFICATIONUSERS", "IX_UserId");
            DropIndex("MCS_IPA_DM.NOTIFICATIONS", "IX_SourceId");
            DropIndex("MCS_IPA_DM.NOTIFICATIONATTACHMENTS", "IX_NotificationDetail_Id");
            DropIndex("MCS_IPA_DM.NOTIFICATIONDETAILS", "IX_Notification_Id");
            DropIndex("MCS_IPA_DM.NOTIFICATIONDETAILS", "IX_NotificationType_Id");
            DropIndex("MCS_IPA_DM.NOTIFICATIONDETAILS", "IX_NotificationTemplateType_Id");
            DropIndex("MCS_IPA_DM.HUBTRANSACTIONS", "IX_MainDocument_Id");
            DropIndex("MCS_IPA_DM.HUBRELATEDPERSONS", "IX_HubTransaction_Id");
            DropIndex("MCS_IPA_DM.HUBATTACHMENTS", "IX_HubTransaction_Id");
            DropIndex("MCS_IPA_DM.HUBATTACHMENTS", "IX_DocumentInfo_Id");
            DropIndex("MCS_IPA_DM.HUBATTACHMENTS", "IX_TypeId");
            DropIndex("MCS_IPA_DM.FORMS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.FORMS", "IX_FormContent_Id");
            DropIndex("MCS_IPA_DM.FORMDEPARTMENTS", "IX_DepartmentId");
            DropIndex("MCS_IPA_DM.FORMDEPARTMENTS", "IX_FormId");
            DropIndex("MCS_IPA_DM.FOLLOWUPDETAILS", "IX_TransactionFollowUpId");
            DropIndex("MCS_IPA_DM.ESCALATIONS", "IX_EscalationToId");
            DropIndex("MCS_IPA_DM.ESCALATIONS", "IX_EscalationActionId");
            DropIndex("MCS_IPA_DM.ESCALATIONS", "IX_PriorityId");
            DropIndex("MCS_IPA_DM.ESCALATIONS", "IX_TransactionCategoryId");
            DropIndex("MCS_IPA_DM.DISTRIBUTIONLISTS", "IX_LocalizationIdentifierId");
            DropIndex("MCS_IPA_DM.DISTRIBUTIONLISTS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.DISTRIBUTIONLISTS", "IX_UserId");
            DropIndex("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS", "IX_UserId");
            DropIndex("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS", "IX_DistributionListId");
            DropIndex("MCS_IPA_DM.COLLABORATIONS", "IX_Attachment_Id");
            DropIndex("MCS_IPA_DM.COLLABORATIONS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.COLLABORATIONS", "IX_ReceiverId");
            DropIndex("MCS_IPA_DM.COLLABORATIONS", "IX_SenderId");
            DropIndex("MCS_IPA_DM.CITIES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.BARCODES", "IX_ReferenceTypeId");
            DropIndex("MCS_IPA_DM.AUDITDETAILS", "IX_Audit_Id");
            DropIndex("MCS_IPA_DM.ASSIGNMENTGROUPS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.ASSIGNMENTGROUPS", "IX_OwnerId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_ToUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_ToEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_ExternalPartyManagerId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_ExternalPartyId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_LetterTypeId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_TransactionTypeId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_TransactionCategoryId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_ConfidentialityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_PriorityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_DeliveryMethodId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_ExplanationId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_DestinationId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_StatusId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_SignedByOrgUnitId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_SignedByUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONHISTORIES", "IX_UserId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_StatusId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_UserPreferenceId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_TransactionTypeId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_ConfidentialityId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_PriorityId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_UserProfileId");
            DropIndex("MCS_IPA_DM.USERDELEGATIONS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_UserDelegationId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_ExplanationId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_ToEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_FromEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_ActionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_ToUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES", "IX_TrayId");
            DropIndex("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "IX_TransactionExternalCopyId");
            DropIndex("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "IX_ReporterId");
            DropIndex("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "IX_DocumentId");
            DropIndex("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "IX_TransactionHistoryId");
            DropIndex("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS", "IX_TransactionAssignmentHistoryId");
            DropIndex("MCS_IPA_DM.REPORTERS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.REPORTERS", "IX_ToEntityId");
            DropIndex("MCS_IPA_DM.ORGUNITLINKS", "IX_OrgUnit_Id");
            DropIndex("MCS_IPA_DM.ORGUNITLINKS", "IX_ToEntity_Id");
            DropIndex("MCS_IPA_DM.ORGUNITLINKS", "IX_FromEntity_Id");
            DropIndex("MCS_IPA_DM.COUNTERDETAILS", "IX_Counter_Id");
            DropIndex("MCS_IPA_DM.COUNTERDETAILS", "IX_TransactionTypeId");
            DropIndex("MCS_IPA_DM.COUNTERS", "IX_Description_Id");
            DropIndex("MCS_IPA_DM.BARCODEDESIGNS", "IX_OrgUnit_Id");
            DropIndex("MCS_IPA_DM.BARCODEDESIGNS", "IX_TypeId");
            DropIndex("MCS_IPA_DM.USERPERMISSIONS", "IX_GroupId");
            DropIndex("MCS_IPA_DM.USERPERMISSIONS", "IX_PermissionId");
            DropIndex("MCS_IPA_DM.USERPERMISSIONS", "IX_UserProfileId");
            DropIndex("MCS_IPA_DM.CHATCLIENTS", "IX_UserId");
            DropIndex("MCS_IPA_DM.USERCATEGORYTRAYS", "IX_Tary_Id");
            DropIndex("MCS_IPA_DM.USERCATEGORYTRAYS", "IX_UserCategoryId");
            DropIndex("MCS_IPA_DM.USERCATEGORIES", "IX_Permission_Id");
            DropIndex("MCS_IPA_DM.USERCATEGORIES", "IX_CategoryName_Id");
            DropIndex("MCS_IPA_DM.CHATROOMUSERS", "IX_UserId");
            DropIndex("MCS_IPA_DM.CHATROOMUSERS", "IX_RoomId");
            DropIndex("MCS_IPA_DM.TRANSACTIONTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONTYPES", "IX_Color_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONTYPES", "IX_Abbreviation_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONTYPES", "IX_PermissionId");
            DropIndex("MCS_IPA_DM.SUGGESTEDTOPICS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.SUGGESTEDTOPICS", "IX_ParentId");
            DropIndex("MCS_IPA_DM.SUBJECTORGUNITS", "IX_SuggestedTopic_Id");
            DropIndex("MCS_IPA_DM.SUBJECTORGUNITS", "IX_SubjectClassification_Id");
            DropIndex("MCS_IPA_DM.SUBJECTORGUNITS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.SUBJECTCLASSIFICATIONS", "IX_ParentId");
            DropIndex("MCS_IPA_DM.TRANSACTIONSUBJECTCLASSIFICATIONS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONSUBJECTCLASSIFICATIONS", "IX_SubjectClassificationId");
            DropIndex("MCS_IPA_DM.TRANSACTIONRESERVATIONS", "IX_TransactionCategoryId");
            DropIndex("MCS_IPA_DM.TRANSACTIONRESERVATIONS", "IX_EntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONRESERVATIONS", "IX_UserId");
            DropIndex("MCS_IPA_DM.PRIORITYEXCEPTIONS", "IX_UserProfileId");
            DropIndex("MCS_IPA_DM.PRIORITYEXCEPTIONS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.PRIORITYEXCEPTIONS", "IX_PriorityId");
            DropIndex("MCS_IPA_DM.PRIORITIES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.NAMES", "IX_TitleId");
            DropIndex("MCS_IPA_DM.NAMES", "IX_NationalityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONNAMES", "IX_NameId");
            DropIndex("MCS_IPA_DM.TRANSACTIONNAMES", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.LINKS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONLINKS", "IX_ToTransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONLINKS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONLINKS", "IX_TypeId");
            DropIndex("MCS_IPA_DM.LETTERTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", "IX_EntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", "IX_UserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONFOLLOWUPS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS", "IX_TransactionExternalCopyId");
            DropIndex("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS", "IX_DocumentInfoId");
            DropIndex("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS", "IX_PartyId");
            DropIndex("MCS_IPA_DM.EXTERNALPARTYMANAGERS", "IX_ExternalParty_Id");
            DropIndex("MCS_IPA_DM.EXTERNALPARTYMANAGERS", "IX_Name_Id");
            DropIndex("MCS_IPA_DM.EXTERNALPARTIES", "IX_Name_Id");
            DropIndex("MCS_IPA_DM.EXTERNALPARTIES", "IX_Address_Id");
            DropIndex("MCS_IPA_DM.EXTERNALPARTIES", "IX_ParentId");
            DropIndex("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "IX_ActionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "IX_FromEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "IX_EntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES", "IX_UserId");
            DropIndex("MCS_IPA_DM.EXPLANATIONS", "IX_Document_Id");
            DropIndex("MCS_IPA_DM.EXPLANATIONS", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.EXPLANATIONS", "IX_PermissionId");
            DropIndex("MCS_IPA_DM.EXPLANATIONS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONCOPIES", "IX_ActionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONCOPIES", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONCOPIES", "IX_FromEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONCOPIES", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONCOPIES", "IX_EntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONCOPIES", "IX_UserId");
            DropIndex("MCS_IPA_DM.USERGROUPS", "IX_UserId");
            DropIndex("MCS_IPA_DM.USERGROUPS", "IX_GroupId");
            DropIndex("MCS_IPA_DM.GROUPS", "IX_GroupName_Id");
            DropIndex("MCS_IPA_DM.PERMISSIONS", "IX_Name_Id");
            DropIndex("MCS_IPA_DM.ATTACHMENTTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.ATTACHMENTS", "IX_DocumentInfo_Id");
            DropIndex("MCS_IPA_DM.ATTACHMENTS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.ATTACHMENTS", "IX_TypeId");
            DropIndex("MCS_IPA_DM.TRAYS", "IX_Name_Id");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "IX_ActionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "IX_UserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHDETAILS", "IX_TransactionPathId");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHS", "IX_TransactionTypeId");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.TRANSACTIONPATHS", "IX_UserId");
            DropIndex("MCS_IPA_DM.DOCUMENTINFO", "IX_Document_Id");
            DropIndex("MCS_IPA_DM.DOCUMENTINFO", "IX_FromEntityId");
            DropIndex("MCS_IPA_DM.DOCUMENTINFO", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.TASKSATTACHMENTS", "IX_DocumentInfoId");
            DropIndex("MCS_IPA_DM.TASKSATTACHMENTS", "IX_TaskId");
            DropIndex("MCS_IPA_DM.TASKREMINDERS", "IX_Task_Id");
            DropIndex("MCS_IPA_DM.TASKS", "IX_TransactionAssignment_Id");
            DropIndex("MCS_IPA_DM.TASKS", "IX_ActionId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_FromOrgUnitId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_StatusId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_ParentId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_ToOrgUnitId");
            DropIndex("MCS_IPA_DM.TASKS", "IX_ToUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_TransactionPathId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_DeliveryMethodId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_PhysicalEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_ToEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_FromEntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_ActionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_PhysicalUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_ToUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_FromUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONASSIGNMENTS", "IX_TrayId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_ReservationId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_DeliveryMethodId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_MainDocumentId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_ExternalPartyManagerId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_ExternalPartyId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_LetterTypeId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_TransactionTypeId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_ConfidentialityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_PriorityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_ToUserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_EntityId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_SuggestedTopicId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_UserId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_TransactionCategoryId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_StatusId");
            DropIndex("MCS_IPA_DM.TRANSACTIONS", "IX_SignedByUserId");
            DropIndex("MCS_IPA_DM.CHATROOMOWNERS", "IX_UserId");
            DropIndex("MCS_IPA_DM.CHATROOMOWNERS", "IX_RoomId");
            DropIndex("MCS_IPA_DM.CHATMESSAGESSTATUS", "IX_MessageId");
            DropIndex("MCS_IPA_DM.CHATMESSAGESSTATUS", "IX_UserId");
            DropIndex("MCS_IPA_DM.CHATMESSAGESSTATUS", "IX_RoomId");
            DropIndex("MCS_IPA_DM.CHATMESSAGES", "IX_UserId");
            DropIndex("MCS_IPA_DM.CHATMESSAGES", "IX_RoomId");
            DropIndex("MCS_IPA_DM.CHATMESSAGES", new[] { "WHEN" });
            DropIndex("MCS_IPA_DM.CHATROOMS", "IX_TransactionId");
            DropIndex("MCS_IPA_DM.CHATROOMS", new[] { "NAME" });
            DropIndex("MCS_IPA_DM.CHATROOMALLOWEDUSERS", "IX_UserId");
            DropIndex("MCS_IPA_DM.CHATROOMALLOWEDUSERS", "IX_RoomId");
            DropIndex("MCS_IPA_DM.USERPROFILES", "IX_UserImage_Id");
            DropIndex("MCS_IPA_DM.USERPROFILES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.USERPROFILES", "IX_DirectManager_Id");
            DropIndex("MCS_IPA_DM.USERPROFILES", "IX_GroupId");
            DropIndex("MCS_IPA_DM.USERPROFILES", "IX_CategoryId");
            DropIndex("MCS_IPA_DM.USERPROFILES", "IX_TitleId");
            DropIndex("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES", "IX_AssignmentPaper_Id");
            DropIndex("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES", "IX_UserId");
            DropIndex("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES", "IX_OrgUnitId");
            DropIndex("MCS_IPA_DM.ASSIGNMENTPAPERACTIONS", "IX_AssignmentPaper_Id");
            DropIndex("MCS_IPA_DM.ASSIGNMENTPAPERACTIONS", "IX_ActionId");
            DropIndex("MCS_IPA_DM.ORGUNITS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.ORGUNITS", "IX_Counter_Id");
            DropIndex("MCS_IPA_DM.ORGUNITS", "IX_ParentId");
            DropIndex("MCS_IPA_DM.ORGUNITS", "IX_AssignmentPaperId");
            DropIndex("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS", "IX_AssignmentGroup_Id");
            DropIndex("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS", "IX_UserProfile_Id");
            DropIndex("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS", "IX_OrgUnit_Id");
            DropIndex("MCS_IPA_DM.ASPNETUSERLOGINS", "IX_UserId");
            DropIndex("MCS_IPA_DM.ASPNETUSERCLAIMS", "IX_UserId");
            DropIndex("MCS_IPA_DM.LOOKUPLOCALIZATIONS", "IX_Lookup_Id");
            DropIndex("MCS_IPA_DM.LOOKUPLOCALIZATIONS", "IX_Culture_Id");
            DropIndex("MCS_IPA_DM.CULTURES", "IX_NameId");
            DropIndex("MCS_IPA_DM.LOCALIZATIONS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCS_IPA_DM.LOCALIZATIONS", "IX_CultureId");
            DropIndex("MCS_IPA_DM.ACTIONS", "IX_Type_Id");
            DropIndex("MCS_IPA_DM.ACTIONS", "IX_LocalizationIdentifier_Id");
            DropTable("MCS_IPA_DM.USERPROFILEORGUNITS");
            DropTable("MCS_IPA_DM.GROUPPERMISSIONS");
            DropTable("MCS_IPA_DM.ASPNETUSERROLES");
            DropTable("MCS_IPA_DM.YESSERMAPPINGS");
            DropTable("MCS_IPA_DM.USERTRAYPREFERENCES");
            DropTable("MCS_IPA_DM.USERPREFERENCES");
            DropTable("MCS_IPA_DM.USERMOBILES");
            DropTable("MCS_IPA_DM.TRANSACTIONLOGS");
            DropTable("MCS_IPA_DM.TRANSACTIONINDEXLOGS");
            DropTable("MCS_IPA_DM.TRANSACTIONENTITYDETAILS");
            DropTable("MCS_IPA_DM.TRANSACTIONASSIGNEES");
            DropTable("MCS_IPA_DM.TASKWORKFLOWS");
            DropTable("MCS_IPA_DM.TASKHISTORIES");
            DropTable("MCS_IPA_DM.SYSTEMDEFAULTVALUES");
            DropTable("MCS_IPA_DM.SIGNEDDELIVERYREPORTS");
            DropTable("MCS_IPA_DM.SETTINGS");
            DropTable("MCS_IPA_DM.RESOURCES");
            DropTable("MCS_IPA_DM.NOTIFICATIONUSERS");
            DropTable("MCS_IPA_DM.NOTIFICATIONS");
            DropTable("MCS_IPA_DM.NOTIFICATIONATTACHMENTS");
            DropTable("MCS_IPA_DM.NOTIFICATIONDETAILS");
            DropTable("MCS_IPA_DM.HUBTRANSACTIONS");
            DropTable("MCS_IPA_DM.HUBRQUIDS");
            DropTable("MCS_IPA_DM.HUBRELATEDPERSONS");
            DropTable("MCS_IPA_DM.HUBRECORDS");
            DropTable("MCS_IPA_DM.HUBATTACHMENTS");
            DropTable("MCS_IPA_DM.FORMS");
            DropTable("MCS_IPA_DM.FORMDEPARTMENTS");
            DropTable("MCS_IPA_DM.FOLLOWUPDETAILS");
            DropTable("MCS_IPA_DM.ESCALATIONS");
            DropTable("MCS_IPA_DM.DOCUMENTATTRIBUTES");
            DropTable("MCS_IPA_DM.DOCPROVIDERS");
            DropTable("MCS_IPA_DM.DISTRIBUTIONLISTS");
            DropTable("MCS_IPA_DM.DISTRIBUTIONLISTDETAILS");
            DropTable("MCS_IPA_DM.COLLABORATIONS");
            DropTable("MCS_IPA_DM.CITIES");
            DropTable("MCS_IPA_DM.BARCODES");
            DropTable("MCS_IPA_DM.AUDITS");
            DropTable("MCS_IPA_DM.AUDITDETAILS");
            DropTable("MCS_IPA_DM.ATTACHMENTEXTENSIONS");
            DropTable("MCS_IPA_DM.ASSIGNMENTGROUPS");
            DropTable("MCS_IPA_DM.TRANSACTIONHISTORIES");
            DropTable("MCS_IPA_DM.USERDELEGATIONS");
            DropTable("MCS_IPA_DM.TRANSACTIONASSIGNMENTHISTORIES");
            DropTable("MCS_IPA_DM.TRANSACTIONDELIVERYREPORTS");
            DropTable("MCS_IPA_DM.REPORTERS");
            DropTable("MCS_IPA_DM.ORGUNITLINKS");
            DropTable("MCS_IPA_DM.COUNTERDETAILS");
            DropTable("MCS_IPA_DM.COUNTERS");
            DropTable("MCS_IPA_DM.BARCODEDESIGNS");
            DropTable("MCS_IPA_DM.USERPERMISSIONS");
            DropTable("MCS_IPA_DM.CHATCLIENTS");
            DropTable("MCS_IPA_DM.USERCATEGORYTRAYS");
            DropTable("MCS_IPA_DM.USERCATEGORIES");
            DropTable("MCS_IPA_DM.CHATROOMUSERS");
            DropTable("MCS_IPA_DM.TRANSACTIONTYPES");
            DropTable("MCS_IPA_DM.SUGGESTEDTOPICS");
            DropTable("MCS_IPA_DM.SUBJECTORGUNITS");
            DropTable("MCS_IPA_DM.SUBJECTCLASSIFICATIONS");
            DropTable("MCS_IPA_DM.TRANSACTIONSUBJECTCLASSIFICATIONS");
            DropTable("MCS_IPA_DM.TRANSACTIONRESERVATIONS");
            DropTable("MCS_IPA_DM.PRIORITYEXCEPTIONS");
            DropTable("MCS_IPA_DM.PRIORITIES");
            DropTable("MCS_IPA_DM.NAMES");
            DropTable("MCS_IPA_DM.TRANSACTIONNAMES");
            DropTable("MCS_IPA_DM.LINKS");
            DropTable("MCS_IPA_DM.TRANSACTIONLINKS");
            DropTable("MCS_IPA_DM.LETTERTYPES");
            DropTable("MCS_IPA_DM.TRANSACTIONFOLLOWUPS");
            DropTable("MCS_IPA_DM.EXTERNALPARTYATTACHMENTS");
            DropTable("MCS_IPA_DM.EXTERNALPARTYMANAGERS");
            DropTable("MCS_IPA_DM.EXTERNALPARTIES");
            DropTable("MCS_IPA_DM.TRANSACTIONEXTERNALCOPIES");
            DropTable("MCS_IPA_DM.EXPLANATIONS");
            DropTable("MCS_IPA_DM.TRANSACTIONCOPIES");
            DropTable("MCS_IPA_DM.USERGROUPS");
            DropTable("MCS_IPA_DM.GROUPS");
            DropTable("MCS_IPA_DM.PERMISSIONS");
            DropTable("MCS_IPA_DM.ATTACHMENTTYPES");
            DropTable("MCS_IPA_DM.ATTACHMENTS");
            DropTable("MCS_IPA_DM.TRAYS");
            DropTable("MCS_IPA_DM.TRANSACTIONPATHDETAILS");
            DropTable("MCS_IPA_DM.TRANSACTIONPATHS");
            DropTable("MCS_IPA_DM.DOCUMENTS");
            DropTable("MCS_IPA_DM.DOCUMENTINFO");
            DropTable("MCS_IPA_DM.TASKSATTACHMENTS");
            DropTable("MCS_IPA_DM.TASKREMINDERS");
            DropTable("MCS_IPA_DM.TASKS");
            DropTable("MCS_IPA_DM.TRANSACTIONASSIGNMENTS");
            DropTable("MCS_IPA_DM.TRANSACTIONS");
            DropTable("MCS_IPA_DM.CHATROOMOWNERS");
            DropTable("MCS_IPA_DM.CHATMESSAGESSTATUS");
            DropTable("MCS_IPA_DM.CHATMESSAGES");
            DropTable("MCS_IPA_DM.CHATROOMS");
            DropTable("MCS_IPA_DM.CHATROOMALLOWEDUSERS");
            DropTable("MCS_IPA_DM.USERPROFILES");
            DropTable("MCS_IPA_DM.ASSIGNMENTPAPERBENEFICIARIES");
            DropTable("MCS_IPA_DM.ASSIGNMENTPAPERACTIONS");
            DropTable("MCS_IPA_DM.ASSIGNMENTPAPERS");
            DropTable("MCS_IPA_DM.ORGUNITS");
            DropTable("MCS_IPA_DM.ASSIGNMENTGROUPDETAILS");
            DropTable("MCS_IPA_DM.ASPNETUSERLOGINS");
            DropTable("MCS_IPA_DM.ASPNETUSERCLAIMS");
            DropTable("MCS_IPA_DM.ASPNETUSERS");
            DropTable("MCS_IPA_DM.ASPNETROLES");
            DropTable("MCS_IPA_DM.LOOKUPLOCALIZATIONS");
            DropTable("MCS_IPA_DM.LOOKUPS");
            DropTable("MCS_IPA_DM.CULTURES");
            DropTable("MCS_IPA_DM.LOCALIZATIONS");
            DropTable("MCS_IPA_DM.LOCALIZATIONIDENTIFIERS");
            DropTable("MCS_IPA_DM.ACTIONS");
        }
    }
}
