namespace MCS.DataAccess.Tenants.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DBO.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DBO.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DBO.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "DBO.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("DBO.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "DBO.TenantCultures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShortName = c.String(maxLength: 50),
                        NameId = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantLookups", t => t.NameId)
                .Index(t => t.NameId);
            
            CreateTable(
                "DBO.TenantLookups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                        EnumReference = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DBO.TenantLookupLocalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                        Culture_Id = c.Int(),
                        Lookup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantCultures", t => t.Culture_Id)
                .ForeignKey("DBO.TenantLookups", t => t.Lookup_Id)
                .Index(t => t.Culture_Id)
                .Index(t => t.Lookup_Id);
            
            CreateTable(
                "DBO.TenantNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DelegatedEmail = c.String(),
                        SourceId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateH = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DBO.TenantNotificationDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(nullable: false),
                        Subject = c.String(),
                        Body = c.String(),
                        IsSent = c.Boolean(nullable: false),
                        FailureCount = c.Int(nullable: false),
                        Email = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                        Template_Id = c.Int(),
                        TenantNotification_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantNotificationTemplates", t => t.Template_Id)
                .ForeignKey("DBO.TenantNotifications", t => t.TenantNotification_Id)
                .Index(t => t.Template_Id)
                .Index(t => t.TenantNotification_Id);
            
            CreateTable(
                "DBO.TenantNotificationAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Binary = c.Binary(),
                        FileName = c.String(),
                        ContentType = c.String(),
                        ContentLength = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                        TenantNotificationDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantNotificationDetails", t => t.TenantNotificationDetail_Id)
                .Index(t => t.TenantNotificationDetail_Id);
            
            CreateTable(
                "DBO.TenantNotificationTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateH = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantLookups", t => t.TypeId, cascadeDelete: false)
                .Index(t => t.TypeId);
            
            CreateTable(
                "DBO.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceId = c.String(nullable: false, maxLength: 1024),
                        Value = c.String(),
                        Culture = c.String(maxLength: 10),
                        ResourceSet = c.String(maxLength: 512),
                        Type = c.String(maxLength: 512),
                        BinFile = c.Binary(),
                        TextFile = c.String(),
                        Filename = c.String(maxLength: 128),
                        Comment = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DBO.Tenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatabaseName = c.String(maxLength: 100),
                        HostName = c.String(maxLength: 100),
                        FromDate = c.DateTime(nullable: false),
                        FromDateH = c.String(maxLength: 20),
                        ToDate = c.DateTime(nullable: false),
                        ToDateH = c.String(maxLength: 20),
                        OrgUnitsCount = c.Int(),
                        UsersCount = c.Int(),
                        DelegatedUserName = c.String(maxLength: 50),
                        DelegatedEmail = c.String(maxLength: 50),
                        DelegatedMobile = c.String(maxLength: 20),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        YesserCertificate = c.Binary(),
                        YesserCode = c.String(),
                        YesserSourceID = c.String(),
                        YesserServiceID = c.String(),
                        YesserSourceName = c.String(),
                        SendingUsername = c.String(),
                        SendingPassword = c.String(),
                        RecievingUsername = c.String(),
                        RecievingPassword = c.String(),
                        Logo = c.Binary(),
                        ECMProfileId = c.String(),
                        ECMCategoryId = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                        DelegatedName_Id = c.Int(),
                        Name_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantLocalizationIdentifiers", t => t.DelegatedName_Id)
                .ForeignKey("DBO.TenantLocalizationIdentifiers", t => t.Name_Id)
                .Index(t => t.DelegatedName_Id)
                .Index(t => t.Name_Id);
            
            CreateTable(
                "DBO.TenantLocalizationIdentifiers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DBO.TenantLocalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CultureId = c.Int(nullable: false),
                        LocalizationIdentifierId = c.Int(nullable: false),
                        Text = c.String(maxLength: 100),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.TenantCultures", t => t.CultureId, cascadeDelete: false)
                .ForeignKey("DBO.TenantLocalizationIdentifiers", t => t.LocalizationIdentifierId, cascadeDelete: false)
                .Index(t => t.CultureId)
                .Index(t => t.LocalizationIdentifierId);
            
            CreateTable(
                "DBO.UserTenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        TenantId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModefiedOn = c.DateTime(),
                        ModefiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DBO.Tenants", t => t.TenantId, cascadeDelete: false)
                .Index(t => t.TenantId);
            
            CreateTable(
                "DBO.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("DBO.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("DBO.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DBO.UserTenants", "TenantId", "DBO.Tenants");
            DropForeignKey("DBO.Tenants", "Name_Id", "DBO.TenantLocalizationIdentifiers");
            DropForeignKey("DBO.Tenants", "DelegatedName_Id", "DBO.TenantLocalizationIdentifiers");
            DropForeignKey("DBO.TenantLocalizations", "LocalizationIdentifierId", "DBO.TenantLocalizationIdentifiers");
            DropForeignKey("DBO.TenantLocalizations", "CultureId", "DBO.TenantCultures");
            DropForeignKey("DBO.TenantNotificationDetails", "TenantNotification_Id", "DBO.TenantNotifications");
            DropForeignKey("DBO.TenantNotificationDetails", "Template_Id", "DBO.TenantNotificationTemplates");
            DropForeignKey("DBO.TenantNotificationTemplates", "TypeId", "DBO.TenantLookups");
            DropForeignKey("DBO.TenantNotificationAttachments", "TenantNotificationDetail_Id", "DBO.TenantNotificationDetails");
            DropForeignKey("DBO.TenantCultures", "NameId", "DBO.TenantLookups");
            DropForeignKey("DBO.TenantLookupLocalizations", "Lookup_Id", "DBO.TenantLookups");
            DropForeignKey("DBO.TenantLookupLocalizations", "Culture_Id", "DBO.TenantCultures");
            DropForeignKey("DBO.AspNetUserLogins", "UserId", "DBO.AspNetUsers");
            DropForeignKey("DBO.AspNetUserClaims", "UserId", "DBO.AspNetUsers");
            DropForeignKey("DBO.AspNetUserRoles", "UserId", "DBO.AspNetUsers");
            DropForeignKey("DBO.AspNetUserRoles", "RoleId", "DBO.AspNetRoles");
            DropIndex("DBO.AspNetUserRoles", new[] { "UserId" });
            DropIndex("DBO.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("DBO.UserTenants", new[] { "TenantId" });
            DropIndex("DBO.TenantLocalizations", new[] { "LocalizationIdentifierId" });
            DropIndex("DBO.TenantLocalizations", new[] { "CultureId" });
            DropIndex("DBO.Tenants", new[] { "Name_Id" });
            DropIndex("DBO.Tenants", new[] { "DelegatedName_Id" });
            DropIndex("DBO.TenantNotificationTemplates", new[] { "TypeId" });
            DropIndex("DBO.TenantNotificationAttachments", new[] { "TenantNotificationDetail_Id" });
            DropIndex("DBO.TenantNotificationDetails", new[] { "TenantNotification_Id" });
            DropIndex("DBO.TenantNotificationDetails", new[] { "Template_Id" });
            DropIndex("DBO.TenantLookupLocalizations", new[] { "Lookup_Id" });
            DropIndex("DBO.TenantLookupLocalizations", new[] { "Culture_Id" });
            DropIndex("DBO.TenantCultures", new[] { "NameId" });
            DropIndex("DBO.AspNetUserLogins", new[] { "UserId" });
            DropIndex("DBO.AspNetUserClaims", new[] { "UserId" });
            DropTable("DBO.AspNetUserRoles");
            DropTable("DBO.UserTenants");
            DropTable("DBO.TenantLocalizations");
            DropTable("DBO.TenantLocalizationIdentifiers");
            DropTable("DBO.Tenants");
            DropTable("DBO.Resources");
            DropTable("DBO.TenantNotificationTemplates");
            DropTable("DBO.TenantNotificationAttachments");
            DropTable("DBO.TenantNotificationDetails");
            DropTable("DBO.TenantNotifications");
            DropTable("DBO.TenantLookupLocalizations");
            DropTable("DBO.TenantLookups");
            DropTable("DBO.TenantCultures");
            DropTable("DBO.AspNetUserLogins");
            DropTable("DBO.AspNetUserClaims");
            DropTable("DBO.AspNetUsers");
            DropTable("DBO.AspNetRoles");
        }
    }
}
