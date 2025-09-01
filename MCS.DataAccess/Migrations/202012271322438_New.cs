namespace MCS.DataAccess.OracleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "MCSDEMO.ACTIONS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TYPE_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id")
                .Index(t => t.TYPE_ID, name: "IX_Type_Id");
            
            CreateTable(
                "MCSDEMO.LOCALIZATIONIDENTIFIERS",
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
                "MCSDEMO.LOCALIZATIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CULTUREID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TEXT = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.CULTURES", t => t.CULTUREID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.CULTUREID, name: "IX_CultureId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.CULTURES",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.NAMEID)
                .Index(t => t.NAMEID, name: "IX_NameId");
            
            CreateTable(
                "MCSDEMO.LOOKUPS",
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
                "MCSDEMO.LOOKUPLOCALIZATIONS",
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
                .ForeignKey("MCSDEMO.CULTURES", t => t.CULTURE_ID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.LOOKUP_ID)
                .Index(t => t.CULTURE_ID, name: "IX_Culture_Id")
                .Index(t => t.LOOKUP_ID, name: "IX_Lookup_Id");
            
            CreateTable(
                "MCSDEMO.ASPNETROLES",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 1000),
                        NAME = c.String(nullable: false, maxLength: 256),
                        DISCRIMINATOR = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCSDEMO.ASPNETUSERS",
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
                "MCSDEMO.ASPNETUSERCLAIMS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        USERID = c.String(nullable: false, maxLength: 1000),
                        CLAIMTYPE = c.String(maxLength: 1000),
                        CLAIMVALUE = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.ASPNETUSERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.ASPNETUSERLOGINS",
                c => new
                    {
                        LOGINPROVIDER = c.String(nullable: false, maxLength: 1000),
                        PROVIDERKEY = c.String(nullable: false, maxLength: 1000),
                        USERID = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => new { t.LOGINPROVIDER, t.PROVIDERKEY, t.USERID })
                .ForeignKey("MCSDEMO.ASPNETUSERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.ASSIGNMENTGROUPDETAILS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNIT_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERPROFILE_ID)
                .ForeignKey("MCSDEMO.ASSIGNMENTGROUPS", t => t.ASSIGNMENTGROUP_ID)
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id")
                .Index(t => t.USERPROFILE_ID, name: "IX_UserProfile_Id")
                .Index(t => t.ASSIGNMENTGROUP_ID, name: "IX_AssignmentGroup_Id");
            
            CreateTable(
                "MCSDEMO.ORGUNITS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        MANAGERID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ASSIGNMENTPAPERID = c.Decimal(precision: 10, scale: 0),
                        PARENTID = c.Decimal(precision: 10, scale: 0),
                        ISACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        NUMBER = c.String(maxLength: 50),
                        BARCODE = c.String(maxLength: 50),
                        ISVIRTUALUNIT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TRANSACTIONSPROCESSINGPERIOD = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ISDELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        JOINTOGENERALCOUNTER = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LINEAGE = c.String(maxLength: 1000),
                        EXTERNALID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        COUNTER_ID = c.Decimal(precision: 10, scale: 0),
                        LOCALIZATIONIDENTIFIER_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPERID)
                .ForeignKey("MCSDEMO.COUNTERS", t => t.COUNTER_ID)
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.PARENTID)
                .Index(t => t.ASSIGNMENTPAPERID, name: "IX_AssignmentPaperId")
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.COUNTER_ID, name: "IX_Counter_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.ASSIGNMENTPAPERS",
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
                "MCSDEMO.ASSIGNMENTPAPERACTIONS",
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
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPER_ID)
                .Index(t => t.ACTIONID, name: "IX_ActionId")
                .Index(t => t.ASSIGNMENTPAPER_ID, name: "IX_AssignmentPaper_Id");
            
            CreateTable(
                "MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
                .ForeignKey("MCSDEMO.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPER_ID)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ASSIGNMENTPAPER_ID, name: "IX_AssignmentPaper_Id");
            
            CreateTable(
                "MCSDEMO.USERPROFILES",
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
                        EXTERNALID = c.Decimal(precision: 10, scale: 0),
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
                .ForeignKey("MCSDEMO.USERCATEGORIES", t => t.CATEGORYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.DIRECTMANAGER_ID)
                .ForeignKey("MCSDEMO.GROUPS", t => t.GROUPID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TITLEID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.DOCUMENTS", t => t.USERIMAGE_ID)
                .Index(t => t.TITLEID, name: "IX_TitleId")
                .Index(t => t.CATEGORYID, name: "IX_CategoryId")
                .Index(t => t.GROUPID, name: "IX_GroupId")
                .Index(t => t.DIRECTMANAGER_ID, name: "IX_DirectManager_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id")
                .Index(t => t.USERIMAGE_ID, name: "IX_UserImage_Id");
            
            CreateTable(
                "MCSDEMO.CHATROOMALLOWEDUSERS",
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
                .ForeignKey("MCSDEMO.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.CHATROOMS",
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
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.NAME)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId");
            
            CreateTable(
                "MCSDEMO.CHATMESSAGES",
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
                .ForeignKey("MCSDEMO.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.WHEN)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.CHATMESSAGESSTATUS",
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
                .ForeignKey("MCSDEMO.CHATMESSAGES", t => t.MESSAGEID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.MESSAGEID, name: "IX_MessageId");
            
            CreateTable(
                "MCSDEMO.CHATROOMOWNERS",
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
                .ForeignKey("MCSDEMO.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONS",
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
                        SOURCETYPEID = c.Decimal(precision: 10, scale: 0),
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
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.CONFIDENTIALITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.EXTERNALPARTYID)
                .ForeignKey("MCSDEMO.EXTERNALPARTYMANAGERS", t => t.EXTERNALPARTYMANAGERID)
                .ForeignKey("MCSDEMO.LETTERTYPES", t => t.LETTERTYPEID)
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.MAINDOCUMENTID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONRESERVATIONS", t => t.RESERVATIONID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.SIGNEDBYUSERID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.STATUSID)
                .ForeignKey("MCSDEMO.SUGGESTEDTOPICS", t => t.SUGGESTEDTOPICID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TRANSACTIONCATEGORYID)
                .ForeignKey("MCSDEMO.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
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
                "MCSDEMO.TRANSACTIONASSIGNMENTS",
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
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.PHYSICALENTITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.PHYSICALUSERID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCSDEMO.TRANSACTIONPATHS", t => t.TRANSACTIONPATHID)
                .ForeignKey("MCSDEMO.TRAYS", t => t.TRAYID, cascadeDelete: true)
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
                "MCSDEMO.TASKS",
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
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TASKS", t => t.PARENTID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.STATUSID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOORGUNITID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", t => t.TRANSACTIONASSIGNMENT_ID)
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
                "MCSDEMO.TASKREMINDERS",
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
                .ForeignKey("MCSDEMO.TASKS", t => t.TASK_ID)
                .Index(t => t.TASK_ID, name: "IX_Task_Id");
            
            CreateTable(
                "MCSDEMO.TASKSATTACHMENTS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENTINFOID)
                .ForeignKey("MCSDEMO.TASKS", t => t.TASKID)
                .Index(t => t.TASKID, name: "IX_TaskId")
                .Index(t => t.DOCUMENTINFOID, name: "IX_DocumentInfoId");
            
            CreateTable(
                "MCSDEMO.DOCUMENTINFO",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NAME = c.String(maxLength: 200),
                        SIZE = c.Decimal(nullable: false, precision: 19, scale: 0),
                        MIMETYPE = c.String(maxLength: 100),
                        ECMID = c.String(maxLength: 50),
                        FROMUSERID = c.Decimal(precision: 10, scale: 0),
                        FROMENTITYID = c.Decimal(precision: 10, scale: 0),
                        TRANSACTIONID = c.Decimal(precision: 10, scale: 0),
                        DOCUMENTTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DOCUMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.DOCUMENTS", t => t.DOCUMENT_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID)
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.DOCUMENT_ID, name: "IX_Document_Id");
            
            CreateTable(
                "MCSDEMO.DOCUMENTS",
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
                "MCSDEMO.TRANSACTIONPATHS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TRANSACTIONTYPEID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONPATHDETAILS",
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
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONPATHS", t => t.TRANSACTIONPATHID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
                .Index(t => t.TRANSACTIONPATHID, name: "IX_TransactionPathId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "MCSDEMO.TRAYS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.NAME_ID)
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "MCSDEMO.ATTACHMENTS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENTINFO_ID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ATTACHMENTTYPES", t => t.TYPEID, cascadeDelete: true)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.DOCUMENTINFO_ID, name: "IX_DocumentInfo_Id");
            
            CreateTable(
                "MCSDEMO.ATTACHMENTTYPES",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.PERMISSIONS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.NAME_ID)
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "MCSDEMO.GROUPS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.GROUPNAME_ID)
                .Index(t => t.GROUPNAME_ID, name: "IX_GroupName_Id");
            
            CreateTable(
                "MCSDEMO.USERGROUPS",
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
                .ForeignKey("MCSDEMO.GROUPS", t => t.GROUPID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.GROUPID, name: "IX_GroupId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONCOPIES",
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
                        SENTDATE = c.DateTime(),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "MCSDEMO.EXPLANATIONS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENT_ID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.DOCUMENT_ID, name: "IX_Document_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONEXTERNALCOPIES",
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
                        SENDEMAIL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.ENTITYID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.EXTERNALPARTYMANAGERS", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.FROMUSERID, name: "IX_FromUserId")
                .Index(t => t.FROMENTITYID, name: "IX_FromEntityId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ACTIONID, name: "IX_ActionId");
            
            CreateTable(
                "MCSDEMO.EXTERNALPARTIES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NUMBER = c.String(maxLength: 20),
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.ADDRESS_ID)
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.NAME_ID)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.ADDRESS_ID, name: "IX_Address_Id")
                .Index(t => t.NAME_ID, name: "IX_Name_Id");
            
            CreateTable(
                "MCSDEMO.EXTERNALPARTYMANAGERS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.NAME_ID)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.EXTERNALPARTY_ID, cascadeDelete: true)
                .Index(t => t.NAME_ID, name: "IX_Name_Id")
                .Index(t => t.EXTERNALPARTY_ID, name: "IX_ExternalParty_Id");
            
            CreateTable(
                "MCSDEMO.EXTERNALPARTYATTACHMENTS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENTINFOID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.PARTYID)
                .ForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", t => t.TRANSACTIONEXTERNALCOPYID)
                .Index(t => t.PARTYID, name: "IX_PartyId")
                .Index(t => t.DOCUMENTINFOID, name: "IX_DocumentInfoId")
                .Index(t => t.TRANSACTIONEXTERNALCOPYID, name: "IX_TransactionExternalCopyId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONFOLLOWUPS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ENTITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId");
            
            CreateTable(
                "MCSDEMO.LETTERTYPES",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONLINKS",
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
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TOTRANSACTIONID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LINKS", t => t.TYPEID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.TOTRANSACTIONID, name: "IX_ToTransactionId");
            
            CreateTable(
                "MCSDEMO.LINKS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONNAMES",
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
                .ForeignKey("MCSDEMO.NAMES", t => t.NAMEID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.NAMEID, name: "IX_NameId");
            
            CreateTable(
                "MCSDEMO.NAMES",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.NATIONALITYID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TITLEID)
                .Index(t => t.NATIONALITYID, name: "IX_NationalityId")
                .Index(t => t.TITLEID, name: "IX_TitleId");
            
            CreateTable(
                "MCSDEMO.PRIORITIES",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.PRIORITYEXCEPTIONS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERPROFILEID, cascadeDelete: true)
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONRESERVATIONS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ENTITYID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TRANSACTIONCATEGORYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ENTITYID, name: "IX_EntityId")
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONSUBJECTCLASSIFICATI",
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
                .ForeignKey("MCSDEMO.SUBJECTCLASSIFICATIONS", t => t.SUBJECTCLASSIFICATIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .Index(t => t.SUBJECTCLASSIFICATIONID, name: "IX_SubjectClassificationId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId");
            
            CreateTable(
                "MCSDEMO.SUBJECTCLASSIFICATIONS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.SUBJECTCLASSIFICATIONS", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.SUBJECTORGUNITS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.SUBJECTCLASSIFICATIONS", t => t.SUBJECTCLASSIFICATION_ID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.SUGGESTEDTOPICS", t => t.SUGGESTEDTOPIC_ID, cascadeDelete: true)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.SUBJECTCLASSIFICATION_ID, name: "IX_SubjectClassification_Id")
                .Index(t => t.SUGGESTEDTOPIC_ID, name: "IX_SuggestedTopic_Id");
            
            CreateTable(
                "MCSDEMO.SUGGESTEDTOPICS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.SUGGESTEDTOPICS", t => t.PARENTID)
                .Index(t => t.PARENTID, name: "IX_ParentId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONTYPES",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.ABBREVIATION_ID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.COLOR_ID)
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: true)
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.ABBREVIATION_ID, name: "IX_Abbreviation_Id")
                .Index(t => t.COLOR_ID, name: "IX_Color_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.CHATROOMUSERS",
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
                .ForeignKey("MCSDEMO.CHATROOMS", t => t.ROOMID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROOMID, name: "IX_RoomId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.USERCATEGORIES",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.CATEGORYNAME_ID)
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.PERMISSION_ID)
                .Index(t => t.CATEGORYNAME_ID, name: "IX_CategoryName_Id")
                .Index(t => t.PERMISSION_ID, name: "IX_Permission_Id");
            
            CreateTable(
                "MCSDEMO.USERCATEGORYTRAYS",
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
                .ForeignKey("MCSDEMO.TRAYS", t => t.TARY_ID)
                .ForeignKey("MCSDEMO.USERCATEGORIES", t => t.USERCATEGORYID, cascadeDelete: true)
                .Index(t => t.USERCATEGORYID, name: "IX_UserCategoryId")
                .Index(t => t.TARY_ID, name: "IX_Tary_Id");
            
            CreateTable(
                "MCSDEMO.CHATCLIENTS",
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
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.USERPERMISSIONS",
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
                .ForeignKey("MCSDEMO.GROUPS", t => t.GROUPID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.PERMISSIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERPROFILEID, cascadeDelete: true)
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.PERMISSIONID, name: "IX_PermissionId")
                .Index(t => t.GROUPID, name: "IX_GroupId");
            
            CreateTable(
                "MCSDEMO.BARCODEDESIGNS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TYPEID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNIT_ID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
            CreateTable(
                "MCSDEMO.COUNTERS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.DESCRIPTION_ID)
                .Index(t => t.DESCRIPTION_ID, name: "IX_Description_Id");
            
            CreateTable(
                "MCSDEMO.COUNTERDETAILS",
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
                .ForeignKey("MCSDEMO.COUNTERS", t => t.COUNTER_ID)
                .ForeignKey("MCSDEMO.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.COUNTER_ID, name: "IX_Counter_Id");
            
            CreateTable(
                "MCSDEMO.ORGUNITLINKS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITY_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOENTITY_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNIT_ID, cascadeDelete: true)
                .Index(t => t.FROMENTITY_ID, name: "IX_FromEntity_Id")
                .Index(t => t.TOENTITY_ID, name: "IX_ToEntity_Id")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
            CreateTable(
                "MCSDEMO.REPORTERS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOENTITYID)
                .Index(t => t.TOENTITYID, name: "IX_ToEntityId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONDELIVERYREPORTS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENTID)
                .ForeignKey("MCSDEMO.REPORTERS", t => t.REPORTERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", t => t.TRANSACTIONASSIGNMENTHISTORYID)
                .ForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", t => t.TRANSACTIONEXTERNALCOPYID)
                .ForeignKey("MCSDEMO.TRANSACTIONHISTORIES", t => t.TRANSACTIONHISTORYID)
                .Index(t => t.TRANSACTIONASSIGNMENTHISTORYID, name: "IX_TransactionAssignmentHistoryId")
                .Index(t => t.TRANSACTIONHISTORYID, name: "IX_TransactionHistoryId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.DOCUMENTID, name: "IX_DocumentId")
                .Index(t => t.REPORTERID, name: "IX_ReporterId")
                .Index(t => t.TRANSACTIONEXTERNALCOPYID, name: "IX_TransactionExternalCopyId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES",
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
                .ForeignKey("MCSDEMO.ACTIONS", t => t.ACTIONID)
                .ForeignKey("MCSDEMO.EXPLANATIONS", t => t.EXPLANATIONID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCSDEMO.TRAYS", t => t.TRAYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERDELEGATIONS", t => t.USERDELEGATIONID)
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
                "MCSDEMO.USERDELEGATIONS",
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
                        SHOWTRANSACTION = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.CONFIDENTIALITYID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PRIORITIES", t => t.PRIORITYID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.STATUSID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TRANSACTIONTYPEID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERPROFILEID)
                .ForeignKey("MCSDEMO.USERPREFERENCES", t => t.USERPREFERENCEID, cascadeDelete: true)
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.CONFIDENTIALITYID, name: "IX_ConfidentialityId")
                .Index(t => t.TRANSACTIONTYPEID, name: "IX_TransactionTypeId")
                .Index(t => t.USERPREFERENCEID, name: "IX_UserPreferenceId")
                .Index(t => t.STATUSID, name: "IX_StatusId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONHISTORIES",
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
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.CONFIDENTIALITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.DELIVERYMETHODID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.DESTINATIONID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.EXPLANATIONID)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.EXTERNALPARTYID)
                .ForeignKey("MCSDEMO.EXTERNALPARTYMANAGERS", t => t.EXTERNALPARTYMANAGERID)
                .ForeignKey("MCSDEMO.LETTERTYPES", t => t.LETTERTYPEID)
                .ForeignKey("MCSDEMO.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.SIGNEDBYORGUNITID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.SIGNEDBYUSERID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.STATUSID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOENTITYID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TRANSACTIONCATEGORYID)
                .ForeignKey("MCSDEMO.TRANSACTIONTYPES", t => t.TRANSACTIONTYPEID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID)
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
                "MCSDEMO.ASSIGNMENTGROUPS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.OWNERID, cascadeDelete: true)
                .Index(t => t.OWNERID, name: "IX_OwnerId")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.ATTACHMENTEXTENSIONS",
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
                "MCSDEMO.AUDITDETAILS",
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
                .ForeignKey("MCSDEMO.AUDITS", t => t.AUDIT_ID)
                .Index(t => t.AUDIT_ID, name: "IX_Audit_Id");
            
            CreateTable(
                "MCSDEMO.AUDITS",
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
                "MCSDEMO.BARCODES",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.REFERENCETYPEID, cascadeDelete: true)
                .Index(t => t.REFERENCETYPEID, name: "IX_ReferenceTypeId");
            
            CreateTable(
                "MCSDEMO.CITIES",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.COLLABORATIONS",
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
                .ForeignKey("MCSDEMO.ATTACHMENTS", t => t.ATTACHMENT_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.RECEIVERID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.SENDERID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.SENDERID, name: "IX_SenderId")
                .Index(t => t.RECEIVERID, name: "IX_ReceiverId")
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ATTACHMENT_ID, name: "IX_Attachment_Id");
            
            CreateTable(
                "MCSDEMO.DISTRIBUTIONLISTDETAILS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.DISTRIBUTIONLISTS", t => t.DISTRIBUTIONLISTID)
                .Index(t => t.DISTRIBUTIONLISTID, name: "IX_DistributionListId")
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId");
            
            CreateTable(
                "MCSDEMO.DISTRIBUTIONLISTS",
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
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNITID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.ORGUNITID, name: "IX_OrgUnitId")
                .Index(t => t.LOCALIZATIONIDENTIFIERID, name: "IX_LocalizationIdentifierId");
            
            CreateTable(
                "MCSDEMO.DOCPROVIDERS",
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
                "MCSDEMO.DOCUMENTATTRIBUTES",
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
                "MCSDEMO.ESCALATIONS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.ESCALATIONACTIONID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.ESCALATIONTOID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PRIORITIES", t => t.PRIORITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.TRANSACTIONCATEGORYID, cascadeDelete: true)
                .Index(t => t.TRANSACTIONCATEGORYID, name: "IX_TransactionCategoryId")
                .Index(t => t.PRIORITYID, name: "IX_PriorityId")
                .Index(t => t.ESCALATIONACTIONID, name: "IX_EscalationActionId")
                .Index(t => t.ESCALATIONTOID, name: "IX_EscalationToId");
            
            CreateTable(
                "MCSDEMO.FOLLOWUPDETAILS",
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
                .ForeignKey("MCSDEMO.TRANSACTIONFOLLOWUPS", t => t.TRANSACTIONFOLLOWUPID, cascadeDelete: true)
                .Index(t => t.TRANSACTIONFOLLOWUPID, name: "IX_TransactionFollowUpId");
            
            CreateTable(
                "MCSDEMO.FORMDEPARTMENTS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.DEPARTMENTID)
                .ForeignKey("MCSDEMO.FORMS", t => t.FORMID, cascadeDelete: true)
                .Index(t => t.FORMID, name: "IX_FormId")
                .Index(t => t.DEPARTMENTID, name: "IX_DepartmentId");
            
            CreateTable(
                "MCSDEMO.FORMS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.FORMCONTENT_ID)
                .ForeignKey("MCSDEMO.LOCALIZATIONIDENTIFIERS", t => t.LOCALIZATIONIDENTIFIER_ID)
                .Index(t => t.FORMCONTENT_ID, name: "IX_FormContent_Id")
                .Index(t => t.LOCALIZATIONIDENTIFIER_ID, name: "IX_LocalizationIdentifier_Id");
            
            CreateTable(
                "MCSDEMO.HUBATTACHMENTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TYPEID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DESCRIPTION = c.String(maxLength: 1000),
                        EXTERNALATTACHEMENTID = c.String(maxLength: 1000),
                        ATTACHEMENTID = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        DOCUMENTINFO_ID = c.Decimal(precision: 10, scale: 0),
                        HUBTRANSACTION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENTINFO_ID)
                .ForeignKey("MCSDEMO.ATTACHMENTTYPES", t => t.TYPEID)
                .ForeignKey("MCSDEMO.HUBTRANSACTIONS", t => t.HUBTRANSACTION_ID)
                .Index(t => t.TYPEID, name: "IX_TypeId")
                .Index(t => t.DOCUMENTINFO_ID, name: "IX_DocumentInfo_Id")
                .Index(t => t.HUBTRANSACTION_ID, name: "IX_HubTransaction_Id");
            
            CreateTable(
                "MCSDEMO.HUBRECORDS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        OUTERTEXT = c.String(maxLength: 1000),
                        METHODNAME = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCSDEMO.HUBRELATEDPERSONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ADDRESS = c.String(maxLength: 1000),
                        EMAIL = c.String(maxLength: 1000),
                        NAME = c.String(maxLength: 1000),
                        NATIONALID = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        HUBTRANSACTION_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.HUBTRANSACTIONS", t => t.HUBTRANSACTION_ID)
                .Index(t => t.HUBTRANSACTION_ID, name: "IX_HubTransaction_Id");
            
            CreateTable(
                "MCSDEMO.HUBRQUIDS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TRANSACTIONNUMBER = c.Decimal(nullable: false, precision: 19, scale: 0),
                        RQUID = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCSDEMO.HUBTRANSACTIONS",
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
                        DELIVERYTYPE = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                        MAINDOCUMENT_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.MAINDOCUMENT_ID)
                .Index(t => t.MAINDOCUMENT_ID, name: "IX_MainDocument_Id");
            
            CreateTable(
                "MCSDEMO.NOTIFICATIONDETAILS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.NOTIFICATIONTEMPLATETYPE_ID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.NOTIFICATIONTYPE_ID)
                .ForeignKey("MCSDEMO.NOTIFICATIONS", t => t.NOTIFICATION_ID)
                .Index(t => t.NOTIFICATIONTEMPLATETYPE_ID, name: "IX_NotificationTemplateType_Id")
                .Index(t => t.NOTIFICATIONTYPE_ID, name: "IX_NotificationType_Id")
                .Index(t => t.NOTIFICATION_ID, name: "IX_Notification_Id");
            
            CreateTable(
                "MCSDEMO.NOTIFICATIONATTACHMENTS",
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
                .ForeignKey("MCSDEMO.NOTIFICATIONDETAILS", t => t.NOTIFICATIONDETAIL_ID)
                .Index(t => t.NOTIFICATIONDETAIL_ID, name: "IX_NotificationDetail_Id");
            
            CreateTable(
                "MCSDEMO.NOTIFICATIONS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.SOURCEID, cascadeDelete: true)
                .Index(t => t.SOURCEID, name: "IX_SourceId");
            
            CreateTable(
                "MCSDEMO.NOTIFICATIONUSERS",
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
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.NOTIFICATIONS", t => t.NOTIFICATION_ID)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.NOTIFICATION_ID, name: "IX_Notification_Id");
            
            CreateTable(
                "MCSDEMO.RESOURCES",
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
                "MCSDEMO.SETTINGS",
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
                "MCSDEMO.SIGNEDDELIVERYREPORTS",
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
                .ForeignKey("MCSDEMO.DOCUMENTINFO", t => t.DOCUMENTID)
                .Index(t => t.DOCUMENTID, name: "IX_DocumentId");
            
            CreateTable(
                "MCSDEMO.SYSTEMDEFAULTVALUES",
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
                "MCSDEMO.TASKHISTORIES",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMORGUNIT_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.FROMUSER_ID)
                .ForeignKey("MCSDEMO.TASKS", t => t.PARENT_ID)
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.STATUS_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOORGUNIT_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSER_ID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTION_ID)
                .Index(t => t.FROMORGUNIT_ID, name: "IX_FromOrgUnit_Id")
                .Index(t => t.FROMUSER_ID, name: "IX_FromUser_Id")
                .Index(t => t.PARENT_ID, name: "IX_Parent_Id")
                .Index(t => t.STATUS_ID, name: "IX_Status_Id")
                .Index(t => t.TOORGUNIT_ID, name: "IX_ToOrgUnit_Id")
                .Index(t => t.TOUSER_ID, name: "IX_ToUser_Id")
                .Index(t => t.TRANSACTION_ID, name: "IX_Transaction_Id");
            
            CreateTable(
                "MCSDEMO.TASKWORKFLOWS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.FROMENTITY_ID)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.TOENTITY_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.TOUSER_ID)
                .Index(t => t.FROMENTITY_ID, name: "IX_FromEntity_Id")
                .Index(t => t.TOENTITY_ID, name: "IX_ToEntity_Id")
                .Index(t => t.TOUSER_ID, name: "IX_ToUser_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONASSIGNEES",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ENTITY_ID)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTION_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USER_ID)
                .Index(t => t.ENTITY_ID, name: "IX_Entity_Id")
                .Index(t => t.TRANSACTION_ID, name: "IX_Transaction_Id")
                .Index(t => t.USER_ID, name: "IX_User_Id");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONENTITYDETAILS",
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
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ENTITYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.TRANSACTIONS", t => t.TRANSACTIONID)
                .Index(t => t.TRANSACTIONID, name: "IX_TransactionId")
                .Index(t => t.ENTITYID, name: "IX_EntityId");
            
            CreateTable(
                "MCSDEMO.TRANSACTIONINDEXLOGS",
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
                "MCSDEMO.TRANSACTIONLOGS",
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
                .ForeignKey("MCSDEMO.LOOKUPS", t => t.AUDITINGACTIONCODE_ID)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId")
                .Index(t => t.AUDITINGACTIONCODE_ID, name: "IX_AuditingActionCode_Id");
            
            CreateTable(
                "MCSDEMO.USERMOBILES",
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
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.USERPREFERENCES",
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
                        FOLLOWUPORGID = c.Decimal(precision: 10, scale: 0),
                        FOLLOWUPUSERID = c.Decimal(precision: 10, scale: 0),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.ASSIGNMENTPAPERS", t => t.ASSIGNMENTPAPERID)
                .ForeignKey("MCSDEMO.CULTURES", t => t.CULTUREID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERPROFILEID)
                .Index(t => t.CULTUREID, name: "IX_CultureId")
                .Index(t => t.USERPROFILEID, name: "IX_UserProfileId")
                .Index(t => t.ASSIGNMENTPAPERID, name: "IX_AssignmentPaperId");
            
            CreateTable(
                "MCSDEMO.USERTRAYPREFERENCES",
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
                .ForeignKey("MCSDEMO.TRAYS", t => t.TRAYID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.USERPREFERENCES", t => t.USERPREFERENCE_ID)
                .Index(t => t.TRAYID, name: "IX_TrayId")
                .Index(t => t.USERPREFERENCE_ID, name: "IX_UserPreference_Id");
            
            CreateTable(
                "MCSDEMO.YESSERMAPPINGS",
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("MCSDEMO.EXTERNALPARTIES", t => t.CLOUDTYPEID, cascadeDelete: true)
                .Index(t => t.CLOUDTYPEID, name: "IX_CloudTypeId");
            
            CreateTable(
                "MCSDEMO.YESSERNEWENTITES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        YESSERID = c.String(maxLength: 1000),
                        NAMEAR = c.String(maxLength: 1000),
                        NAMEEN = c.String(maxLength: 1000),
                        CREATEDON = c.DateTime(nullable: false),
                        CREATEDBY = c.Decimal(precision: 10, scale: 0),
                        MODEFIEDON = c.DateTime(),
                        MODEFIEDBY = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "MCSDEMO.ASPNETUSERROLES",
                c => new
                    {
                        ROLEID = c.String(nullable: false, maxLength: 1000),
                        USERID = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => new { t.ROLEID, t.USERID })
                .ForeignKey("MCSDEMO.ASPNETROLES", t => t.ROLEID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ASPNETUSERS", t => t.USERID, cascadeDelete: true)
                .Index(t => t.ROLEID, name: "IX_RoleId")
                .Index(t => t.USERID, name: "IX_UserId");
            
            CreateTable(
                "MCSDEMO.GROUPPERMISSIONS",
                c => new
                    {
                        GROUP_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PERMISSION_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.GROUP_ID, t.PERMISSION_ID })
                .ForeignKey("MCSDEMO.GROUPS", t => t.GROUP_ID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.PERMISSIONS", t => t.PERMISSION_ID, cascadeDelete: true)
                .Index(t => t.GROUP_ID, name: "IX_Group_Id")
                .Index(t => t.PERMISSION_ID, name: "IX_Permission_Id");
            
            CreateTable(
                "MCSDEMO.USERPROFILEORGUNITS",
                c => new
                    {
                        USERPROFILE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ORGUNIT_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.USERPROFILE_ID, t.ORGUNIT_ID })
                .ForeignKey("MCSDEMO.USERPROFILES", t => t.USERPROFILE_ID, cascadeDelete: true)
                .ForeignKey("MCSDEMO.ORGUNITS", t => t.ORGUNIT_ID, cascadeDelete: true)
                .Index(t => t.USERPROFILE_ID, name: "IX_UserProfile_Id")
                .Index(t => t.ORGUNIT_ID, name: "IX_OrgUnit_Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("MCSDEMO.YESSERMAPPINGS", "CLOUDTYPEID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.USERTRAYPREFERENCES", "USERPREFERENCE_ID", "MCSDEMO.USERPREFERENCES");
            DropForeignKey("MCSDEMO.USERTRAYPREFERENCES", "TRAYID", "MCSDEMO.TRAYS");
            DropForeignKey("MCSDEMO.USERPREFERENCES", "USERPROFILEID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "USERPREFERENCEID", "MCSDEMO.USERPREFERENCES");
            DropForeignKey("MCSDEMO.USERPREFERENCES", "CULTUREID", "MCSDEMO.CULTURES");
            DropForeignKey("MCSDEMO.USERPREFERENCES", "ASSIGNMENTPAPERID", "MCSDEMO.ASSIGNMENTPAPERS");
            DropForeignKey("MCSDEMO.USERMOBILES", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONLOGS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONLOGS", "AUDITINGACTIONCODE_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONENTITYDETAILS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONENTITYDETAILS", "ENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNEES", "USER_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNEES", "TRANSACTION_ID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNEES", "ENTITY_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKWORKFLOWS", "TOUSER_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TASKWORKFLOWS", "TOENTITY_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKWORKFLOWS", "FROMENTITY_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "TRANSACTION_ID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "TOUSER_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "TOORGUNIT_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "STATUS_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "PARENT_ID", "MCSDEMO.TASKS");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "FROMUSER_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TASKHISTORIES", "FROMORGUNIT_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.SIGNEDDELIVERYREPORTS", "DOCUMENTID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.NOTIFICATIONUSERS", "NOTIFICATION_ID", "MCSDEMO.NOTIFICATIONS");
            DropForeignKey("MCSDEMO.NOTIFICATIONUSERS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.NOTIFICATIONS", "SOURCEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.NOTIFICATIONDETAILS", "NOTIFICATION_ID", "MCSDEMO.NOTIFICATIONS");
            DropForeignKey("MCSDEMO.NOTIFICATIONDETAILS", "NOTIFICATIONTYPE_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.NOTIFICATIONDETAILS", "NOTIFICATIONTEMPLATETYPE_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.NOTIFICATIONATTACHMENTS", "NOTIFICATIONDETAIL_ID", "MCSDEMO.NOTIFICATIONDETAILS");
            DropForeignKey("MCSDEMO.HUBTRANSACTIONS", "MAINDOCUMENT_ID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.HUBRELATEDPERSONS", "HUBTRANSACTION_ID", "MCSDEMO.HUBTRANSACTIONS");
            DropForeignKey("MCSDEMO.HUBATTACHMENTS", "HUBTRANSACTION_ID", "MCSDEMO.HUBTRANSACTIONS");
            DropForeignKey("MCSDEMO.HUBATTACHMENTS", "TYPEID", "MCSDEMO.ATTACHMENTTYPES");
            DropForeignKey("MCSDEMO.HUBATTACHMENTS", "DOCUMENTINFO_ID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.FORMS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.FORMS", "FORMCONTENT_ID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.FORMDEPARTMENTS", "FORMID", "MCSDEMO.FORMS");
            DropForeignKey("MCSDEMO.FORMDEPARTMENTS", "DEPARTMENTID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.FOLLOWUPDETAILS", "TRANSACTIONFOLLOWUPID", "MCSDEMO.TRANSACTIONFOLLOWUPS");
            DropForeignKey("MCSDEMO.ESCALATIONS", "TRANSACTIONCATEGORYID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.ESCALATIONS", "PRIORITYID", "MCSDEMO.PRIORITIES");
            DropForeignKey("MCSDEMO.ESCALATIONS", "ESCALATIONTOID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.ESCALATIONS", "ESCALATIONACTIONID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.DISTRIBUTIONLISTS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.DISTRIBUTIONLISTS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.DISTRIBUTIONLISTS", "LOCALIZATIONIDENTIFIERID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.DISTRIBUTIONLISTDETAILS", "DISTRIBUTIONLISTID", "MCSDEMO.DISTRIBUTIONLISTS");
            DropForeignKey("MCSDEMO.DISTRIBUTIONLISTDETAILS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.DISTRIBUTIONLISTDETAILS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.COLLABORATIONS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.COLLABORATIONS", "SENDERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.COLLABORATIONS", "RECEIVERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.COLLABORATIONS", "ATTACHMENT_ID", "MCSDEMO.ATTACHMENTS");
            DropForeignKey("MCSDEMO.CITIES", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.BARCODES", "REFERENCETYPEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.AUDITDETAILS", "AUDIT_ID", "MCSDEMO.AUDITS");
            DropForeignKey("MCSDEMO.ASSIGNMENTGROUPS", "OWNERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.ASSIGNMENTGROUPS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.ASSIGNMENTGROUPDETAILS", "ASSIGNMENTGROUP_ID", "MCSDEMO.ASSIGNMENTGROUPS");
            DropForeignKey("MCSDEMO.ASSIGNMENTGROUPDETAILS", "USERPROFILE_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.ASSIGNMENTGROUPDETAILS", "ORGUNIT_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.REPORTERS", "TOENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONHISTORYID", "MCSDEMO.TRANSACTIONHISTORIES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "TRANSACTIONTYPEID", "MCSDEMO.TRANSACTIONTYPES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "TRANSACTIONCATEGORYID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "TOUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "TOENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "STATUSID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "SIGNEDBYUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "SIGNEDBYORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "PRIORITYID", "MCSDEMO.PRIORITIES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "LETTERTYPEID", "MCSDEMO.LETTERTYPES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "EXTERNALPARTYMANAGERID", "MCSDEMO.EXTERNALPARTYMANAGERS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "EXTERNALPARTYID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "EXPLANATIONID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "DESTINATIONID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "DELIVERYMETHODID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONHISTORIES", "CONFIDENTIALITYID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONEXTERNALCOPYID", "MCSDEMO.TRANSACTIONEXTERNALCOPIES");
            DropForeignKey("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONASSIGNMENTHISTORYID", "MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "USERDELEGATIONID", "MCSDEMO.USERDELEGATIONS");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "USERPROFILEID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "TRANSACTIONTYPEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "STATUSID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "PRIORITYID", "MCSDEMO.PRIORITIES");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.USERDELEGATIONS", "CONFIDENTIALITYID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "TRAYID", "MCSDEMO.TRAYS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "TOUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "TOENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "FROMENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "EXPLANATIONID", "MCSDEMO.EXPLANATIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "REPORTERID", "MCSDEMO.REPORTERS");
            DropForeignKey("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "DOCUMENTID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.REPORTERS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.ORGUNITS", "PARENTID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.ORGUNITS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.ORGUNITLINKS", "ORGUNIT_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.ORGUNITLINKS", "TOENTITY_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.ORGUNITLINKS", "FROMENTITY_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.ORGUNITS", "COUNTER_ID", "MCSDEMO.COUNTERS");
            DropForeignKey("MCSDEMO.COUNTERS", "DESCRIPTION_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.COUNTERDETAILS", "TRANSACTIONTYPEID", "MCSDEMO.TRANSACTIONTYPES");
            DropForeignKey("MCSDEMO.COUNTERDETAILS", "COUNTER_ID", "MCSDEMO.COUNTERS");
            DropForeignKey("MCSDEMO.BARCODEDESIGNS", "ORGUNIT_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.BARCODEDESIGNS", "TYPEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.ORGUNITS", "ASSIGNMENTPAPERID", "MCSDEMO.ASSIGNMENTPAPERS");
            DropForeignKey("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES", "ASSIGNMENTPAPER_ID", "MCSDEMO.ASSIGNMENTPAPERS");
            DropForeignKey("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERPROFILES", "USERIMAGE_ID", "MCSDEMO.DOCUMENTS");
            DropForeignKey("MCSDEMO.USERPROFILES", "TITLEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.USERPERMISSIONS", "USERPROFILEID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERPERMISSIONS", "PERMISSIONID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.USERPERMISSIONS", "GROUPID", "MCSDEMO.GROUPS");
            DropForeignKey("MCSDEMO.USERPROFILEORGUNITS", "ORGUNIT_ID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.USERPROFILEORGUNITS", "USERPROFILE_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERPROFILES", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.USERPROFILES", "GROUPID", "MCSDEMO.GROUPS");
            DropForeignKey("MCSDEMO.USERPROFILES", "DIRECTMANAGER_ID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.CHATCLIENTS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERPROFILES", "CATEGORYID", "MCSDEMO.USERCATEGORIES");
            DropForeignKey("MCSDEMO.USERCATEGORIES", "PERMISSION_ID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.USERCATEGORYTRAYS", "USERCATEGORYID", "MCSDEMO.USERCATEGORIES");
            DropForeignKey("MCSDEMO.USERCATEGORYTRAYS", "TARY_ID", "MCSDEMO.TRAYS");
            DropForeignKey("MCSDEMO.USERCATEGORIES", "CATEGORYNAME_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.CHATROOMALLOWEDUSERS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.CHATROOMUSERS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.CHATROOMUSERS", "ROOMID", "MCSDEMO.CHATROOMS");
            DropForeignKey("MCSDEMO.CHATROOMS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "TRANSACTIONTYPEID", "MCSDEMO.TRANSACTIONTYPES");
            DropForeignKey("MCSDEMO.TRANSACTIONTYPES", "PERMISSIONID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONTYPES", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONTYPES", "COLOR_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONTYPES", "ABBREVIATION_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "TRANSACTIONCATEGORYID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "TOUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "SUGGESTEDTOPICID", "MCSDEMO.SUGGESTEDTOPICS");
            DropForeignKey("MCSDEMO.SUBJECTORGUNITS", "SUGGESTEDTOPIC_ID", "MCSDEMO.SUGGESTEDTOPICS");
            DropForeignKey("MCSDEMO.SUGGESTEDTOPICS", "PARENTID", "MCSDEMO.SUGGESTEDTOPICS");
            DropForeignKey("MCSDEMO.SUGGESTEDTOPICS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONSUBJECTCLASSIFICATI", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONSUBJECTCLASSIFICATI", "SUBJECTCLASSIFICATIONID", "MCSDEMO.SUBJECTCLASSIFICATIONS");
            DropForeignKey("MCSDEMO.SUBJECTORGUNITS", "SUBJECTCLASSIFICATION_ID", "MCSDEMO.SUBJECTCLASSIFICATIONS");
            DropForeignKey("MCSDEMO.SUBJECTORGUNITS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.SUBJECTCLASSIFICATIONS", "PARENTID", "MCSDEMO.SUBJECTCLASSIFICATIONS");
            DropForeignKey("MCSDEMO.SUBJECTCLASSIFICATIONS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "STATUSID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "SIGNEDBYUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONRESERVATIONS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "RESERVATIONID", "MCSDEMO.TRANSACTIONRESERVATIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONRESERVATIONS", "TRANSACTIONCATEGORYID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONRESERVATIONS", "ENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "PRIORITYID", "MCSDEMO.PRIORITIES");
            DropForeignKey("MCSDEMO.PRIORITYEXCEPTIONS", "USERPROFILEID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.PRIORITYEXCEPTIONS", "PRIORITYID", "MCSDEMO.PRIORITIES");
            DropForeignKey("MCSDEMO.PRIORITYEXCEPTIONS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.PRIORITIES", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONNAMES", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONNAMES", "NAMEID", "MCSDEMO.NAMES");
            DropForeignKey("MCSDEMO.NAMES", "TITLEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.NAMES", "NATIONALITYID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "MAINDOCUMENTID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.TRANSACTIONLINKS", "TYPEID", "MCSDEMO.LINKS");
            DropForeignKey("MCSDEMO.LINKS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONLINKS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONLINKS", "TOTRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "LETTERTYPEID", "MCSDEMO.LETTERTYPES");
            DropForeignKey("MCSDEMO.LETTERTYPES", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONFOLLOWUPS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONFOLLOWUPS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONFOLLOWUPS", "ENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "EXTERNALPARTYMANAGERID", "MCSDEMO.EXTERNALPARTYMANAGERS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "EXTERNALPARTYID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "USERID", "MCSDEMO.EXTERNALPARTYMANAGERS");
            DropForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "FROMENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.EXTERNALPARTYATTACHMENTS", "TRANSACTIONEXTERNALCOPYID", "MCSDEMO.TRANSACTIONEXTERNALCOPIES");
            DropForeignKey("MCSDEMO.EXTERNALPARTYATTACHMENTS", "PARTYID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.EXTERNALPARTYATTACHMENTS", "DOCUMENTINFOID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "ENTITYID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.EXTERNALPARTYMANAGERS", "EXTERNALPARTY_ID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.EXTERNALPARTYMANAGERS", "NAME_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.EXTERNALPARTIES", "PARENTID", "MCSDEMO.EXTERNALPARTIES");
            DropForeignKey("MCSDEMO.EXTERNALPARTIES", "NAME_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.EXTERNALPARTIES", "ADDRESS_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.EXPLANATIONS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.EXPLANATIONS", "PERMISSIONID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.EXPLANATIONS", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.EXPLANATIONS", "DOCUMENT_ID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "ENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "DELIVERYMETHODID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONCOPIES", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONCOPIES", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONCOPIES", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONCOPIES", "FROMENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONCOPIES", "ENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONCOPIES", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONS", "CONFIDENTIALITYID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.GROUPPERMISSIONS", "PERMISSION_ID", "MCSDEMO.PERMISSIONS");
            DropForeignKey("MCSDEMO.GROUPPERMISSIONS", "GROUP_ID", "MCSDEMO.GROUPS");
            DropForeignKey("MCSDEMO.USERGROUPS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.USERGROUPS", "GROUPID", "MCSDEMO.GROUPS");
            DropForeignKey("MCSDEMO.GROUPS", "GROUPNAME_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.PERMISSIONS", "NAME_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.ATTACHMENTS", "TYPEID", "MCSDEMO.ATTACHMENTTYPES");
            DropForeignKey("MCSDEMO.ATTACHMENTTYPES", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.ATTACHMENTS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.ATTACHMENTS", "DOCUMENTINFO_ID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "TRAYID", "MCSDEMO.TRAYS");
            DropForeignKey("MCSDEMO.TRAYS", "NAME_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "TRANSACTIONPATHID", "MCSDEMO.TRANSACTIONPATHS");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHS", "TRANSACTIONTYPEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHDETAILS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHDETAILS", "TRANSACTIONPATHID", "MCSDEMO.TRANSACTIONPATHS");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHDETAILS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHDETAILS", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONPATHS", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "TOUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "TOENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKS", "TRANSACTIONASSIGNMENT_ID", "MCSDEMO.TRANSACTIONASSIGNMENTS");
            DropForeignKey("MCSDEMO.TASKS", "TRANSACTIONID", "MCSDEMO.TRANSACTIONS");
            DropForeignKey("MCSDEMO.TASKS", "TOUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TASKS", "TOORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKSATTACHMENTS", "TASKID", "MCSDEMO.TASKS");
            DropForeignKey("MCSDEMO.TASKSATTACHMENTS", "DOCUMENTINFOID", "MCSDEMO.DOCUMENTINFO");
            DropForeignKey("MCSDEMO.DOCUMENTINFO", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.DOCUMENTINFO", "FROMENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.DOCUMENTINFO", "DOCUMENT_ID", "MCSDEMO.DOCUMENTS");
            DropForeignKey("MCSDEMO.TASKS", "STATUSID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TASKREMINDERS", "TASK_ID", "MCSDEMO.TASKS");
            DropForeignKey("MCSDEMO.TASKS", "PARENTID", "MCSDEMO.TASKS");
            DropForeignKey("MCSDEMO.TASKS", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TASKS", "FROMORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TASKS", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "PHYSICALUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "PHYSICALENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "FROMUSERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "FROMENTITYID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "DELIVERYMETHODID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.TRANSACTIONASSIGNMENTS", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.CHATROOMOWNERS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.CHATROOMOWNERS", "ROOMID", "MCSDEMO.CHATROOMS");
            DropForeignKey("MCSDEMO.CHATMESSAGES", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.CHATMESSAGES", "ROOMID", "MCSDEMO.CHATROOMS");
            DropForeignKey("MCSDEMO.CHATMESSAGESSTATUS", "USERID", "MCSDEMO.USERPROFILES");
            DropForeignKey("MCSDEMO.CHATMESSAGESSTATUS", "ROOMID", "MCSDEMO.CHATROOMS");
            DropForeignKey("MCSDEMO.CHATMESSAGESSTATUS", "MESSAGEID", "MCSDEMO.CHATMESSAGES");
            DropForeignKey("MCSDEMO.CHATROOMALLOWEDUSERS", "ROOMID", "MCSDEMO.CHATROOMS");
            DropForeignKey("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES", "ORGUNITID", "MCSDEMO.ORGUNITS");
            DropForeignKey("MCSDEMO.ASSIGNMENTPAPERACTIONS", "ASSIGNMENTPAPER_ID", "MCSDEMO.ASSIGNMENTPAPERS");
            DropForeignKey("MCSDEMO.ASSIGNMENTPAPERACTIONS", "ACTIONID", "MCSDEMO.ACTIONS");
            DropForeignKey("MCSDEMO.ASPNETUSERLOGINS", "USERID", "MCSDEMO.ASPNETUSERS");
            DropForeignKey("MCSDEMO.ASPNETUSERCLAIMS", "USERID", "MCSDEMO.ASPNETUSERS");
            DropForeignKey("MCSDEMO.ASPNETUSERROLES", "USERID", "MCSDEMO.ASPNETUSERS");
            DropForeignKey("MCSDEMO.ASPNETUSERROLES", "ROLEID", "MCSDEMO.ASPNETROLES");
            DropForeignKey("MCSDEMO.ACTIONS", "TYPE_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.ACTIONS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.LOCALIZATIONS", "LOCALIZATIONIDENTIFIER_ID", "MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropForeignKey("MCSDEMO.LOCALIZATIONS", "CULTUREID", "MCSDEMO.CULTURES");
            DropForeignKey("MCSDEMO.CULTURES", "NAMEID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.LOOKUPLOCALIZATIONS", "LOOKUP_ID", "MCSDEMO.LOOKUPS");
            DropForeignKey("MCSDEMO.LOOKUPLOCALIZATIONS", "CULTURE_ID", "MCSDEMO.CULTURES");
            DropIndex("MCSDEMO.USERPROFILEORGUNITS", "IX_OrgUnit_Id");
            DropIndex("MCSDEMO.USERPROFILEORGUNITS", "IX_UserProfile_Id");
            DropIndex("MCSDEMO.GROUPPERMISSIONS", "IX_Permission_Id");
            DropIndex("MCSDEMO.GROUPPERMISSIONS", "IX_Group_Id");
            DropIndex("MCSDEMO.ASPNETUSERROLES", "IX_UserId");
            DropIndex("MCSDEMO.ASPNETUSERROLES", "IX_RoleId");
            DropIndex("MCSDEMO.YESSERMAPPINGS", "IX_CloudTypeId");
            DropIndex("MCSDEMO.USERTRAYPREFERENCES", "IX_UserPreference_Id");
            DropIndex("MCSDEMO.USERTRAYPREFERENCES", "IX_TrayId");
            DropIndex("MCSDEMO.USERPREFERENCES", "IX_AssignmentPaperId");
            DropIndex("MCSDEMO.USERPREFERENCES", "IX_UserProfileId");
            DropIndex("MCSDEMO.USERPREFERENCES", "IX_CultureId");
            DropIndex("MCSDEMO.USERMOBILES", "IX_UserId");
            DropIndex("MCSDEMO.TRANSACTIONLOGS", "IX_AuditingActionCode_Id");
            DropIndex("MCSDEMO.TRANSACTIONLOGS", "IX_UserId");
            DropIndex("MCSDEMO.TRANSACTIONENTITYDETAILS", "IX_EntityId");
            DropIndex("MCSDEMO.TRANSACTIONENTITYDETAILS", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNEES", "IX_User_Id");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNEES", "IX_Transaction_Id");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNEES", "IX_Entity_Id");
            DropIndex("MCSDEMO.TASKWORKFLOWS", "IX_ToUser_Id");
            DropIndex("MCSDEMO.TASKWORKFLOWS", "IX_ToEntity_Id");
            DropIndex("MCSDEMO.TASKWORKFLOWS", "IX_FromEntity_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_Transaction_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_ToUser_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_ToOrgUnit_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_Status_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_Parent_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_FromUser_Id");
            DropIndex("MCSDEMO.TASKHISTORIES", "IX_FromOrgUnit_Id");
            DropIndex("MCSDEMO.SIGNEDDELIVERYREPORTS", "IX_DocumentId");
            DropIndex("MCSDEMO.NOTIFICATIONUSERS", "IX_Notification_Id");
            DropIndex("MCSDEMO.NOTIFICATIONUSERS", "IX_UserId");
            DropIndex("MCSDEMO.NOTIFICATIONS", "IX_SourceId");
            DropIndex("MCSDEMO.NOTIFICATIONATTACHMENTS", "IX_NotificationDetail_Id");
            DropIndex("MCSDEMO.NOTIFICATIONDETAILS", "IX_Notification_Id");
            DropIndex("MCSDEMO.NOTIFICATIONDETAILS", "IX_NotificationType_Id");
            DropIndex("MCSDEMO.NOTIFICATIONDETAILS", "IX_NotificationTemplateType_Id");
            DropIndex("MCSDEMO.HUBTRANSACTIONS", "IX_MainDocument_Id");
            DropIndex("MCSDEMO.HUBRELATEDPERSONS", "IX_HubTransaction_Id");
            DropIndex("MCSDEMO.HUBATTACHMENTS", "IX_HubTransaction_Id");
            DropIndex("MCSDEMO.HUBATTACHMENTS", "IX_DocumentInfo_Id");
            DropIndex("MCSDEMO.HUBATTACHMENTS", "IX_TypeId");
            DropIndex("MCSDEMO.FORMS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.FORMS", "IX_FormContent_Id");
            DropIndex("MCSDEMO.FORMDEPARTMENTS", "IX_DepartmentId");
            DropIndex("MCSDEMO.FORMDEPARTMENTS", "IX_FormId");
            DropIndex("MCSDEMO.FOLLOWUPDETAILS", "IX_TransactionFollowUpId");
            DropIndex("MCSDEMO.ESCALATIONS", "IX_EscalationToId");
            DropIndex("MCSDEMO.ESCALATIONS", "IX_EscalationActionId");
            DropIndex("MCSDEMO.ESCALATIONS", "IX_PriorityId");
            DropIndex("MCSDEMO.ESCALATIONS", "IX_TransactionCategoryId");
            DropIndex("MCSDEMO.DISTRIBUTIONLISTS", "IX_LocalizationIdentifierId");
            DropIndex("MCSDEMO.DISTRIBUTIONLISTS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.DISTRIBUTIONLISTS", "IX_UserId");
            DropIndex("MCSDEMO.DISTRIBUTIONLISTDETAILS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.DISTRIBUTIONLISTDETAILS", "IX_UserId");
            DropIndex("MCSDEMO.DISTRIBUTIONLISTDETAILS", "IX_DistributionListId");
            DropIndex("MCSDEMO.COLLABORATIONS", "IX_Attachment_Id");
            DropIndex("MCSDEMO.COLLABORATIONS", "IX_TransactionId");
            DropIndex("MCSDEMO.COLLABORATIONS", "IX_ReceiverId");
            DropIndex("MCSDEMO.COLLABORATIONS", "IX_SenderId");
            DropIndex("MCSDEMO.CITIES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.BARCODES", "IX_ReferenceTypeId");
            DropIndex("MCSDEMO.AUDITDETAILS", "IX_Audit_Id");
            DropIndex("MCSDEMO.ASSIGNMENTGROUPS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.ASSIGNMENTGROUPS", "IX_OwnerId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_ToUserId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_ToEntityId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_ExternalPartyManagerId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_ExternalPartyId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_LetterTypeId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_TransactionTypeId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_TransactionCategoryId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_ConfidentialityId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_PriorityId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_DeliveryMethodId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_ExplanationId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_DestinationId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_StatusId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_SignedByOrgUnitId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_SignedByUserId");
            DropIndex("MCSDEMO.TRANSACTIONHISTORIES", "IX_UserId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_StatusId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_UserPreferenceId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_TransactionTypeId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_ConfidentialityId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_PriorityId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_UserProfileId");
            DropIndex("MCSDEMO.USERDELEGATIONS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_UserDelegationId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_ExplanationId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_ToEntityId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_FromEntityId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_ActionId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_ToUserId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_FromUserId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES", "IX_TrayId");
            DropIndex("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "IX_TransactionExternalCopyId");
            DropIndex("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "IX_ReporterId");
            DropIndex("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "IX_DocumentId");
            DropIndex("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "IX_TransactionHistoryId");
            DropIndex("MCSDEMO.TRANSACTIONDELIVERYREPORTS", "IX_TransactionAssignmentHistoryId");
            DropIndex("MCSDEMO.REPORTERS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.REPORTERS", "IX_ToEntityId");
            DropIndex("MCSDEMO.ORGUNITLINKS", "IX_OrgUnit_Id");
            DropIndex("MCSDEMO.ORGUNITLINKS", "IX_ToEntity_Id");
            DropIndex("MCSDEMO.ORGUNITLINKS", "IX_FromEntity_Id");
            DropIndex("MCSDEMO.COUNTERDETAILS", "IX_Counter_Id");
            DropIndex("MCSDEMO.COUNTERDETAILS", "IX_TransactionTypeId");
            DropIndex("MCSDEMO.COUNTERS", "IX_Description_Id");
            DropIndex("MCSDEMO.BARCODEDESIGNS", "IX_OrgUnit_Id");
            DropIndex("MCSDEMO.BARCODEDESIGNS", "IX_TypeId");
            DropIndex("MCSDEMO.USERPERMISSIONS", "IX_GroupId");
            DropIndex("MCSDEMO.USERPERMISSIONS", "IX_PermissionId");
            DropIndex("MCSDEMO.USERPERMISSIONS", "IX_UserProfileId");
            DropIndex("MCSDEMO.CHATCLIENTS", "IX_UserId");
            DropIndex("MCSDEMO.USERCATEGORYTRAYS", "IX_Tary_Id");
            DropIndex("MCSDEMO.USERCATEGORYTRAYS", "IX_UserCategoryId");
            DropIndex("MCSDEMO.USERCATEGORIES", "IX_Permission_Id");
            DropIndex("MCSDEMO.USERCATEGORIES", "IX_CategoryName_Id");
            DropIndex("MCSDEMO.CHATROOMUSERS", "IX_UserId");
            DropIndex("MCSDEMO.CHATROOMUSERS", "IX_RoomId");
            DropIndex("MCSDEMO.TRANSACTIONTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.TRANSACTIONTYPES", "IX_Color_Id");
            DropIndex("MCSDEMO.TRANSACTIONTYPES", "IX_Abbreviation_Id");
            DropIndex("MCSDEMO.TRANSACTIONTYPES", "IX_PermissionId");
            DropIndex("MCSDEMO.SUGGESTEDTOPICS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.SUGGESTEDTOPICS", "IX_ParentId");
            DropIndex("MCSDEMO.SUBJECTORGUNITS", "IX_SuggestedTopic_Id");
            DropIndex("MCSDEMO.SUBJECTORGUNITS", "IX_SubjectClassification_Id");
            DropIndex("MCSDEMO.SUBJECTORGUNITS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.SUBJECTCLASSIFICATIONS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.SUBJECTCLASSIFICATIONS", "IX_ParentId");
            DropIndex("MCSDEMO.TRANSACTIONSUBJECTCLASSIFICATI", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONSUBJECTCLASSIFICATI", "IX_SubjectClassificationId");
            DropIndex("MCSDEMO.TRANSACTIONRESERVATIONS", "IX_TransactionCategoryId");
            DropIndex("MCSDEMO.TRANSACTIONRESERVATIONS", "IX_EntityId");
            DropIndex("MCSDEMO.TRANSACTIONRESERVATIONS", "IX_UserId");
            DropIndex("MCSDEMO.PRIORITYEXCEPTIONS", "IX_UserProfileId");
            DropIndex("MCSDEMO.PRIORITYEXCEPTIONS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.PRIORITYEXCEPTIONS", "IX_PriorityId");
            DropIndex("MCSDEMO.PRIORITIES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.NAMES", "IX_TitleId");
            DropIndex("MCSDEMO.NAMES", "IX_NationalityId");
            DropIndex("MCSDEMO.TRANSACTIONNAMES", "IX_NameId");
            DropIndex("MCSDEMO.TRANSACTIONNAMES", "IX_TransactionId");
            DropIndex("MCSDEMO.LINKS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.TRANSACTIONLINKS", "IX_ToTransactionId");
            DropIndex("MCSDEMO.TRANSACTIONLINKS", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONLINKS", "IX_TypeId");
            DropIndex("MCSDEMO.LETTERTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.TRANSACTIONFOLLOWUPS", "IX_EntityId");
            DropIndex("MCSDEMO.TRANSACTIONFOLLOWUPS", "IX_UserId");
            DropIndex("MCSDEMO.TRANSACTIONFOLLOWUPS", "IX_TransactionId");
            DropIndex("MCSDEMO.EXTERNALPARTYATTACHMENTS", "IX_TransactionExternalCopyId");
            DropIndex("MCSDEMO.EXTERNALPARTYATTACHMENTS", "IX_DocumentInfoId");
            DropIndex("MCSDEMO.EXTERNALPARTYATTACHMENTS", "IX_PartyId");
            DropIndex("MCSDEMO.EXTERNALPARTYMANAGERS", "IX_ExternalParty_Id");
            DropIndex("MCSDEMO.EXTERNALPARTYMANAGERS", "IX_Name_Id");
            DropIndex("MCSDEMO.EXTERNALPARTIES", "IX_Name_Id");
            DropIndex("MCSDEMO.EXTERNALPARTIES", "IX_Address_Id");
            DropIndex("MCSDEMO.EXTERNALPARTIES", "IX_ParentId");
            DropIndex("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "IX_ActionId");
            DropIndex("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "IX_FromEntityId");
            DropIndex("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "IX_FromUserId");
            DropIndex("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "IX_EntityId");
            DropIndex("MCSDEMO.TRANSACTIONEXTERNALCOPIES", "IX_UserId");
            DropIndex("MCSDEMO.EXPLANATIONS", "IX_Document_Id");
            DropIndex("MCSDEMO.EXPLANATIONS", "IX_FromUserId");
            DropIndex("MCSDEMO.EXPLANATIONS", "IX_PermissionId");
            DropIndex("MCSDEMO.EXPLANATIONS", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONCOPIES", "IX_ActionId");
            DropIndex("MCSDEMO.TRANSACTIONCOPIES", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONCOPIES", "IX_FromEntityId");
            DropIndex("MCSDEMO.TRANSACTIONCOPIES", "IX_FromUserId");
            DropIndex("MCSDEMO.TRANSACTIONCOPIES", "IX_EntityId");
            DropIndex("MCSDEMO.TRANSACTIONCOPIES", "IX_UserId");
            DropIndex("MCSDEMO.USERGROUPS", "IX_UserId");
            DropIndex("MCSDEMO.USERGROUPS", "IX_GroupId");
            DropIndex("MCSDEMO.GROUPS", "IX_GroupName_Id");
            DropIndex("MCSDEMO.PERMISSIONS", "IX_Name_Id");
            DropIndex("MCSDEMO.ATTACHMENTTYPES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.ATTACHMENTS", "IX_DocumentInfo_Id");
            DropIndex("MCSDEMO.ATTACHMENTS", "IX_TransactionId");
            DropIndex("MCSDEMO.ATTACHMENTS", "IX_TypeId");
            DropIndex("MCSDEMO.TRAYS", "IX_Name_Id");
            DropIndex("MCSDEMO.TRANSACTIONPATHDETAILS", "IX_ActionId");
            DropIndex("MCSDEMO.TRANSACTIONPATHDETAILS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.TRANSACTIONPATHDETAILS", "IX_UserId");
            DropIndex("MCSDEMO.TRANSACTIONPATHDETAILS", "IX_TransactionPathId");
            DropIndex("MCSDEMO.TRANSACTIONPATHS", "IX_TransactionTypeId");
            DropIndex("MCSDEMO.TRANSACTIONPATHS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.TRANSACTIONPATHS", "IX_UserId");
            DropIndex("MCSDEMO.DOCUMENTINFO", "IX_Document_Id");
            DropIndex("MCSDEMO.DOCUMENTINFO", "IX_FromEntityId");
            DropIndex("MCSDEMO.DOCUMENTINFO", "IX_FromUserId");
            DropIndex("MCSDEMO.TASKSATTACHMENTS", "IX_DocumentInfoId");
            DropIndex("MCSDEMO.TASKSATTACHMENTS", "IX_TaskId");
            DropIndex("MCSDEMO.TASKREMINDERS", "IX_Task_Id");
            DropIndex("MCSDEMO.TASKS", "IX_TransactionAssignment_Id");
            DropIndex("MCSDEMO.TASKS", "IX_ActionId");
            DropIndex("MCSDEMO.TASKS", "IX_TransactionId");
            DropIndex("MCSDEMO.TASKS", "IX_FromOrgUnitId");
            DropIndex("MCSDEMO.TASKS", "IX_FromUserId");
            DropIndex("MCSDEMO.TASKS", "IX_StatusId");
            DropIndex("MCSDEMO.TASKS", "IX_ParentId");
            DropIndex("MCSDEMO.TASKS", "IX_ToOrgUnitId");
            DropIndex("MCSDEMO.TASKS", "IX_ToUserId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_TransactionPathId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_DeliveryMethodId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_PhysicalEntityId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_ToEntityId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_FromEntityId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_ActionId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_TransactionId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_PhysicalUserId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_ToUserId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_FromUserId");
            DropIndex("MCSDEMO.TRANSACTIONASSIGNMENTS", "IX_TrayId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_ReservationId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_DeliveryMethodId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_MainDocumentId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_ExternalPartyManagerId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_ExternalPartyId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_LetterTypeId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_TransactionTypeId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_ConfidentialityId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_PriorityId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_ToUserId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_EntityId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_SuggestedTopicId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_OrgUnitId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_UserId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_TransactionCategoryId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_StatusId");
            DropIndex("MCSDEMO.TRANSACTIONS", "IX_SignedByUserId");
            DropIndex("MCSDEMO.CHATROOMOWNERS", "IX_UserId");
            DropIndex("MCSDEMO.CHATROOMOWNERS", "IX_RoomId");
            DropIndex("MCSDEMO.CHATMESSAGESSTATUS", "IX_MessageId");
            DropIndex("MCSDEMO.CHATMESSAGESSTATUS", "IX_UserId");
            DropIndex("MCSDEMO.CHATMESSAGESSTATUS", "IX_RoomId");
            DropIndex("MCSDEMO.CHATMESSAGES", "IX_UserId");
            DropIndex("MCSDEMO.CHATMESSAGES", "IX_RoomId");
            DropIndex("MCSDEMO.CHATMESSAGES", new[] { "WHEN" });
            DropIndex("MCSDEMO.CHATROOMS", "IX_TransactionId");
            DropIndex("MCSDEMO.CHATROOMS", new[] { "NAME" });
            DropIndex("MCSDEMO.CHATROOMALLOWEDUSERS", "IX_UserId");
            DropIndex("MCSDEMO.CHATROOMALLOWEDUSERS", "IX_RoomId");
            DropIndex("MCSDEMO.USERPROFILES", "IX_UserImage_Id");
            DropIndex("MCSDEMO.USERPROFILES", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.USERPROFILES", "IX_DirectManager_Id");
            DropIndex("MCSDEMO.USERPROFILES", "IX_GroupId");
            DropIndex("MCSDEMO.USERPROFILES", "IX_CategoryId");
            DropIndex("MCSDEMO.USERPROFILES", "IX_TitleId");
            DropIndex("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES", "IX_AssignmentPaper_Id");
            DropIndex("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES", "IX_UserId");
            DropIndex("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES", "IX_OrgUnitId");
            DropIndex("MCSDEMO.ASSIGNMENTPAPERACTIONS", "IX_AssignmentPaper_Id");
            DropIndex("MCSDEMO.ASSIGNMENTPAPERACTIONS", "IX_ActionId");
            DropIndex("MCSDEMO.ORGUNITS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.ORGUNITS", "IX_Counter_Id");
            DropIndex("MCSDEMO.ORGUNITS", "IX_ParentId");
            DropIndex("MCSDEMO.ORGUNITS", "IX_AssignmentPaperId");
            DropIndex("MCSDEMO.ASSIGNMENTGROUPDETAILS", "IX_AssignmentGroup_Id");
            DropIndex("MCSDEMO.ASSIGNMENTGROUPDETAILS", "IX_UserProfile_Id");
            DropIndex("MCSDEMO.ASSIGNMENTGROUPDETAILS", "IX_OrgUnit_Id");
            DropIndex("MCSDEMO.ASPNETUSERLOGINS", "IX_UserId");
            DropIndex("MCSDEMO.ASPNETUSERCLAIMS", "IX_UserId");
            DropIndex("MCSDEMO.LOOKUPLOCALIZATIONS", "IX_Lookup_Id");
            DropIndex("MCSDEMO.LOOKUPLOCALIZATIONS", "IX_Culture_Id");
            DropIndex("MCSDEMO.CULTURES", "IX_NameId");
            DropIndex("MCSDEMO.LOCALIZATIONS", "IX_LocalizationIdentifier_Id");
            DropIndex("MCSDEMO.LOCALIZATIONS", "IX_CultureId");
            DropIndex("MCSDEMO.ACTIONS", "IX_Type_Id");
            DropIndex("MCSDEMO.ACTIONS", "IX_LocalizationIdentifier_Id");
            DropTable("MCSDEMO.USERPROFILEORGUNITS");
            DropTable("MCSDEMO.GROUPPERMISSIONS");
            DropTable("MCSDEMO.ASPNETUSERROLES");
            DropTable("MCSDEMO.YESSERNEWENTITES");
            DropTable("MCSDEMO.YESSERMAPPINGS");
            DropTable("MCSDEMO.USERTRAYPREFERENCES");
            DropTable("MCSDEMO.USERPREFERENCES");
            DropTable("MCSDEMO.USERMOBILES");
            DropTable("MCSDEMO.TRANSACTIONLOGS");
            DropTable("MCSDEMO.TRANSACTIONINDEXLOGS");
            DropTable("MCSDEMO.TRANSACTIONENTITYDETAILS");
            DropTable("MCSDEMO.TRANSACTIONASSIGNEES");
            DropTable("MCSDEMO.TASKWORKFLOWS");
            DropTable("MCSDEMO.TASKHISTORIES");
            DropTable("MCSDEMO.SYSTEMDEFAULTVALUES");
            DropTable("MCSDEMO.SIGNEDDELIVERYREPORTS");
            DropTable("MCSDEMO.SETTINGS");
            DropTable("MCSDEMO.RESOURCES");
            DropTable("MCSDEMO.NOTIFICATIONUSERS");
            DropTable("MCSDEMO.NOTIFICATIONS");
            DropTable("MCSDEMO.NOTIFICATIONATTACHMENTS");
            DropTable("MCSDEMO.NOTIFICATIONDETAILS");
            DropTable("MCSDEMO.HUBTRANSACTIONS");
            DropTable("MCSDEMO.HUBRQUIDS");
            DropTable("MCSDEMO.HUBRELATEDPERSONS");
            DropTable("MCSDEMO.HUBRECORDS");
            DropTable("MCSDEMO.HUBATTACHMENTS");
            DropTable("MCSDEMO.FORMS");
            DropTable("MCSDEMO.FORMDEPARTMENTS");
            DropTable("MCSDEMO.FOLLOWUPDETAILS");
            DropTable("MCSDEMO.ESCALATIONS");
            DropTable("MCSDEMO.DOCUMENTATTRIBUTES");
            DropTable("MCSDEMO.DOCPROVIDERS");
            DropTable("MCSDEMO.DISTRIBUTIONLISTS");
            DropTable("MCSDEMO.DISTRIBUTIONLISTDETAILS");
            DropTable("MCSDEMO.COLLABORATIONS");
            DropTable("MCSDEMO.CITIES");
            DropTable("MCSDEMO.BARCODES");
            DropTable("MCSDEMO.AUDITS");
            DropTable("MCSDEMO.AUDITDETAILS");
            DropTable("MCSDEMO.ATTACHMENTEXTENSIONS");
            DropTable("MCSDEMO.ASSIGNMENTGROUPS");
            DropTable("MCSDEMO.TRANSACTIONHISTORIES");
            DropTable("MCSDEMO.USERDELEGATIONS");
            DropTable("MCSDEMO.TRANSACTIONASSIGNMENTHISTORIES");
            DropTable("MCSDEMO.TRANSACTIONDELIVERYREPORTS");
            DropTable("MCSDEMO.REPORTERS");
            DropTable("MCSDEMO.ORGUNITLINKS");
            DropTable("MCSDEMO.COUNTERDETAILS");
            DropTable("MCSDEMO.COUNTERS");
            DropTable("MCSDEMO.BARCODEDESIGNS");
            DropTable("MCSDEMO.USERPERMISSIONS");
            DropTable("MCSDEMO.CHATCLIENTS");
            DropTable("MCSDEMO.USERCATEGORYTRAYS");
            DropTable("MCSDEMO.USERCATEGORIES");
            DropTable("MCSDEMO.CHATROOMUSERS");
            DropTable("MCSDEMO.TRANSACTIONTYPES");
            DropTable("MCSDEMO.SUGGESTEDTOPICS");
            DropTable("MCSDEMO.SUBJECTORGUNITS");
            DropTable("MCSDEMO.SUBJECTCLASSIFICATIONS");
            DropTable("MCSDEMO.TRANSACTIONSUBJECTCLASSIFICATI");
            DropTable("MCSDEMO.TRANSACTIONRESERVATIONS");
            DropTable("MCSDEMO.PRIORITYEXCEPTIONS");
            DropTable("MCSDEMO.PRIORITIES");
            DropTable("MCSDEMO.NAMES");
            DropTable("MCSDEMO.TRANSACTIONNAMES");
            DropTable("MCSDEMO.LINKS");
            DropTable("MCSDEMO.TRANSACTIONLINKS");
            DropTable("MCSDEMO.LETTERTYPES");
            DropTable("MCSDEMO.TRANSACTIONFOLLOWUPS");
            DropTable("MCSDEMO.EXTERNALPARTYATTACHMENTS");
            DropTable("MCSDEMO.EXTERNALPARTYMANAGERS");
            DropTable("MCSDEMO.EXTERNALPARTIES");
            DropTable("MCSDEMO.TRANSACTIONEXTERNALCOPIES");
            DropTable("MCSDEMO.EXPLANATIONS");
            DropTable("MCSDEMO.TRANSACTIONCOPIES");
            DropTable("MCSDEMO.USERGROUPS");
            DropTable("MCSDEMO.GROUPS");
            DropTable("MCSDEMO.PERMISSIONS");
            DropTable("MCSDEMO.ATTACHMENTTYPES");
            DropTable("MCSDEMO.ATTACHMENTS");
            DropTable("MCSDEMO.TRAYS");
            DropTable("MCSDEMO.TRANSACTIONPATHDETAILS");
            DropTable("MCSDEMO.TRANSACTIONPATHS");
            DropTable("MCSDEMO.DOCUMENTS");
            DropTable("MCSDEMO.DOCUMENTINFO");
            DropTable("MCSDEMO.TASKSATTACHMENTS");
            DropTable("MCSDEMO.TASKREMINDERS");
            DropTable("MCSDEMO.TASKS");
            DropTable("MCSDEMO.TRANSACTIONASSIGNMENTS");
            DropTable("MCSDEMO.TRANSACTIONS");
            DropTable("MCSDEMO.CHATROOMOWNERS");
            DropTable("MCSDEMO.CHATMESSAGESSTATUS");
            DropTable("MCSDEMO.CHATMESSAGES");
            DropTable("MCSDEMO.CHATROOMS");
            DropTable("MCSDEMO.CHATROOMALLOWEDUSERS");
            DropTable("MCSDEMO.USERPROFILES");
            DropTable("MCSDEMO.ASSIGNMENTPAPERBENEFICIARIES");
            DropTable("MCSDEMO.ASSIGNMENTPAPERACTIONS");
            DropTable("MCSDEMO.ASSIGNMENTPAPERS");
            DropTable("MCSDEMO.ORGUNITS");
            DropTable("MCSDEMO.ASSIGNMENTGROUPDETAILS");
            DropTable("MCSDEMO.ASPNETUSERLOGINS");
            DropTable("MCSDEMO.ASPNETUSERCLAIMS");
            DropTable("MCSDEMO.ASPNETUSERS");
            DropTable("MCSDEMO.ASPNETROLES");
            DropTable("MCSDEMO.LOOKUPLOCALIZATIONS");
            DropTable("MCSDEMO.LOOKUPS");
            DropTable("MCSDEMO.CULTURES");
            DropTable("MCSDEMO.LOCALIZATIONS");
            DropTable("MCSDEMO.LOCALIZATIONIDENTIFIERS");
            DropTable("MCSDEMO.ACTIONS");
        }
    }
}
