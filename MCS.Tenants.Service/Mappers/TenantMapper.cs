using System.Linq;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Tenants;

namespace MCS.Tenants.Service.Mappers
{
    public static class TenantMapper
    {
        public static Tenant ToTenant(this TenantDTO model)
        {
            if (model == null) return new Tenant();
            Tenant oTenant = new Tenant();
            oTenant.Id = model.Id;
            oTenant.Name = model.Name?.ToTenantLocalizationIdentifier();
            oTenant.DelegatedName = model.DelegatedName?.ToTenantLocalizationIdentifier();
            oTenant.DatabaseName = model.DatabaseName;
            oTenant.DelegatedEmail = model.DelegatedEmail;
            oTenant.DelegatedMobile = model.DelegatedMobile;
            oTenant.DelegatedUserName = model.DelegatedUserName;
            oTenant.HostName = model.HostName;
            oTenant.FromDate = model.FromDate;
            oTenant.FromDateH = model.FromDateH;
            oTenant.ToDate = model.ToDate;
            oTenant.ToDateH = model.ToDateH;
            oTenant.IsDeleted = model.IsDeleted;
            oTenant.IsActive = model.IsActive;
            oTenant.YesserCertificate = model.YesserCertificate;
            oTenant.Logo = model.Logo;
            oTenant.OrgUnitsCount = model.OrgUnitsCount;
            oTenant.UsersCount = model.UsersCount;
            return oTenant;
        }

        public static TenantDTO ToTenantDTO(this Tenant model)
        {
            if (model == null) return new TenantDTO();
            TenantDTO oTenantDTO = new TenantDTO();
            oTenantDTO.Id = model.Id;
            oTenantDTO.Name = model.Name?.ToTenantLocalizationIdentifierDTO();
            oTenantDTO.DelegatedName = model.DelegatedName?.ToTenantLocalizationIdentifierDTO();
            oTenantDTO.DatabaseName = model.DatabaseName;
            oTenantDTO.DelegatedEmail = model.DelegatedEmail;
            oTenantDTO.DelegatedMobile = model.DelegatedMobile;
            oTenantDTO.DelegatedUserName = model.DelegatedUserName;
            oTenantDTO.HostName = model.HostName;
            oTenantDTO.FromDate = model.FromDate;
            oTenantDTO.FromDateH = model.FromDateH;
            oTenantDTO.ToDate = model.ToDate;
            oTenantDTO.ToDateH = model.ToDateH;
            oTenantDTO.IsDeleted = model.IsDeleted;
            oTenantDTO.IsActive = model.IsActive;
            oTenantDTO.Logo = model.Logo;
            oTenantDTO.YesserCertificate = model.YesserCertificate;
            oTenantDTO.OrgUnitsCount = model.OrgUnitsCount;
            oTenantDTO.UsersCount = model.UsersCount;
            oTenantDTO.LocalName = model.LocalName;
            return oTenantDTO;
        }

        public static UserTenant ToTenant(this UserTenantDTO model)
        {
            if (model == null) return new UserTenant();
            UserTenant oUserTenant = new UserTenant();
            oUserTenant.Id = model.Id;
            oUserTenant.TenantId = model.TenantId;
            oUserTenant.UserName = model.UserName;
            return oUserTenant;
        }
        public static TenantLocalizationIdentifier ToTenantLocalizationIdentifier(this TenantLocalizationIdentifierDTO model)
        {
            if (model == null) return new TenantLocalizationIdentifier();
            TenantLocalizationIdentifier oTenantLocalizationIdentifier = new TenantLocalizationIdentifier();
            oTenantLocalizationIdentifier.Id = model.Id;
            oTenantLocalizationIdentifier.CreatedBy = model.CreatedBy;
            oTenantLocalizationIdentifier.CreatedOn = model.CreatedOn;
            oTenantLocalizationIdentifier.ModefiedBy = model.ModefiedBy;
            oTenantLocalizationIdentifier.ModefiedOn = model.ModefiedOn;
            oTenantLocalizationIdentifier.Localizations = model.Localizations?.Select(x => x.ToTenantLocalization()).ToList();

            return oTenantLocalizationIdentifier;
        }

        public static TenantLocalizationIdentifierDTO ToTenantLocalizationIdentifierDTO(this TenantLocalizationIdentifier model)
        {
            if (model == null) return new TenantLocalizationIdentifierDTO();
            TenantLocalizationIdentifierDTO oTenantLocalizationIdentifierDTO = new TenantLocalizationIdentifierDTO();
            oTenantLocalizationIdentifierDTO.Id = model.Id;
            oTenantLocalizationIdentifierDTO.CreatedBy = model.CreatedBy;
            oTenantLocalizationIdentifierDTO.CreatedOn = model.CreatedOn;
            oTenantLocalizationIdentifierDTO.ModefiedBy = model.ModefiedBy;
            oTenantLocalizationIdentifierDTO.ModefiedOn = model.ModefiedOn;
            oTenantLocalizationIdentifierDTO.Localizations = model.Localizations?.Select(x => x.ToTenantLocalizationDTO()).ToList();

            return oTenantLocalizationIdentifierDTO;
        }

        public static TenantLocalization ToTenantLocalization(this TenantLocalizationDTO model)
        {
            if (model == null) return new TenantLocalization();
            TenantLocalization oTenantLocalization = new TenantLocalization();
            oTenantLocalization.Id = model.Id;
            oTenantLocalization.CreatedBy = model.CreatedBy;
            oTenantLocalization.CreatedOn = model.CreatedOn;
            oTenantLocalization.ModefiedBy = model.ModefiedBy;
            oTenantLocalization.ModefiedOn = model.ModefiedOn;
            oTenantLocalization.CultureId = model.CultureId;
            oTenantLocalization.Culture = model.Culture?.ToTenantCulture();
            oTenantLocalization.Text = model.Text;
            oTenantLocalization.LocalizationIdentifier = model.LocalizationIdentifier?.ToTenantLocalizationIdentifier();

            return oTenantLocalization;
        }

        public static TenantLocalizationDTO ToTenantLocalizationDTO(this TenantLocalization model)
        {
            if (model == null) return new TenantLocalizationDTO();
            TenantLocalizationDTO oTenantLocalizationDTO = new TenantLocalizationDTO();
            oTenantLocalizationDTO.Id = model.Id;
            oTenantLocalizationDTO.CreatedBy = model.CreatedBy;
            oTenantLocalizationDTO.CreatedOn = model.CreatedOn;
            oTenantLocalizationDTO.ModefiedBy = model.ModefiedBy;
            oTenantLocalizationDTO.ModefiedOn = model.ModefiedOn;
            oTenantLocalizationDTO.CultureId = model.CultureId;
            oTenantLocalizationDTO.Culture = model.Culture?.ToTenantCultureDTO();
            oTenantLocalizationDTO.Text = model.Text;
            return oTenantLocalizationDTO;
        }


        public static TenantCulture ToTenantCulture(this TenantCultureDTO model)
        {
            if (model == null) return new TenantCulture();
            TenantCulture oTenantCulture = new TenantCulture();
            oTenantCulture.Id = model.Id;
            oTenantCulture.CreatedBy = model.CreatedBy;
            oTenantCulture.CreatedOn = model.CreatedOn;
            oTenantCulture.ModefiedBy = model.ModefiedBy;
            oTenantCulture.ModefiedOn = model.ModefiedOn;
            oTenantCulture.ShortName = model.ShortName;
            oTenantCulture.NameId = model.NameId;
            oTenantCulture.Name = model.Name?.ToTenantLookup();
            return oTenantCulture;
        }

        public static TenantCultureDTO ToTenantCultureDTO(this TenantCulture model)
        {
            if (model == null) return new TenantCultureDTO();
            TenantCultureDTO oTenantCultureDTO = new TenantCultureDTO();
            oTenantCultureDTO.Id = model.Id;
            oTenantCultureDTO.CreatedBy = model.CreatedBy;
            oTenantCultureDTO.CreatedOn = model.CreatedOn;
            oTenantCultureDTO.ModefiedBy = model.ModefiedBy;
            oTenantCultureDTO.ModefiedOn = model.ModefiedOn;
            oTenantCultureDTO.ShortName = model.ShortName;
            oTenantCultureDTO.NameId = model.NameId;
            return oTenantCultureDTO;
        }

        public static TenantLookup ToTenantLookup(this TenantLookupDTO model)
        {
            if (model == null) return new TenantLookup();
            TenantLookup oTenantLookup = new TenantLookup();
            oTenantLookup.Id = model.Id;
            oTenantLookup.CreatedBy = model.CreatedBy;
            oTenantLookup.CreatedOn = model.CreatedOn;
            oTenantLookup.ModefiedBy = model.ModefiedBy;
            oTenantLookup.ModefiedOn = model.ModefiedOn;
            oTenantLookup.CategoryId = model.CategoryId;
            oTenantLookup.IsActive = model.IsActive;
            oTenantLookup.Sort = model.Sort;
            oTenantLookup.EnumReference = model.EnumReference;
            oTenantLookup.Localizations = model.Localizations?.Select(x => x.ToTenantLookupLocalization()).ToList();
            oTenantLookup.Text = model.Text;
            return oTenantLookup;
        }

        public static TenantLookupDTO ToTenantLookupDTO(this TenantLookup model)
        {
            if (model == null) return new TenantLookupDTO();
            TenantLookupDTO oTenantLookupDTO = new TenantLookupDTO();
            oTenantLookupDTO.Id = model.Id;
            oTenantLookupDTO.CreatedBy = model.CreatedBy;
            oTenantLookupDTO.CreatedOn = model.CreatedOn;
            oTenantLookupDTO.ModefiedBy = model.ModefiedBy;
            oTenantLookupDTO.ModefiedOn = model.ModefiedOn;
            oTenantLookupDTO.CategoryId = model.CategoryId;
            oTenantLookupDTO.IsActive = model.IsActive;
            oTenantLookupDTO.Sort = model.Sort;
            oTenantLookupDTO.EnumReference = model.EnumReference;
            oTenantLookupDTO.Localizations = model.Localizations?.Select(x => x.ToTenantLookupLocalizationDTO()).ToList();
            oTenantLookupDTO.Text = model.Text;
            return oTenantLookupDTO;
        }

        public static TenantLookupLocalization ToTenantLookupLocalization(this TenantLookupLocalizationDTO model)
        {
            if (model == null) return new TenantLookupLocalization();
            TenantLookupLocalization oTenantLookupLocalization = new TenantLookupLocalization();
            oTenantLookupLocalization.Id = model.Id;
            oTenantLookupLocalization.CreatedBy = model.CreatedBy;
            oTenantLookupLocalization.CreatedOn = model.CreatedOn;
            oTenantLookupLocalization.ModefiedBy = model.ModefiedBy;
            oTenantLookupLocalization.ModefiedOn = model.ModefiedOn;
            oTenantLookupLocalization.Text = model.Text;
            oTenantLookupLocalization.Lookup = model.Lookup.ToTenantLookup();
            oTenantLookupLocalization.Culture = model.Culture.ToTenantCulture();

            return oTenantLookupLocalization;
        }

        public static TenantLookupLocalizationDTO ToTenantLookupLocalizationDTO(this TenantLookupLocalization model)
        {
            if (model == null) return new TenantLookupLocalizationDTO();
            TenantLookupLocalizationDTO oTenantLookupLocalizationDTO = new TenantLookupLocalizationDTO();
            oTenantLookupLocalizationDTO.Id = model.Id;
            oTenantLookupLocalizationDTO.CreatedBy = model.CreatedBy;
            oTenantLookupLocalizationDTO.CreatedOn = model.CreatedOn;
            oTenantLookupLocalizationDTO.ModefiedBy = model.ModefiedBy;
            oTenantLookupLocalizationDTO.ModefiedOn = model.ModefiedOn;
            oTenantLookupLocalizationDTO.Text = model.Text;
            //   oTenantLookupLocalizationDTO.Lookup = model.Lookup.ToTenantLookupDTO();
            oTenantLookupLocalizationDTO.Culture = model.Culture.ToTenantCultureDTO();

            return oTenantLookupLocalizationDTO;
        }

        public static TenantNotification ToTenantNotification(this TenantNotificationDTO model)
        {
            if (model == null) return new TenantNotification();
            TenantNotification oTenantNotification = new TenantNotification();
            oTenantNotification.Id = model.Id;
            oTenantNotification.CreatedBy = model.CreatedBy;
            oTenantNotification.CreatedOn = model.CreatedOn;
            oTenantNotification.ModefiedBy = model.ModefiedBy;
            oTenantNotification.ModefiedOn = model.ModefiedOn;
            oTenantNotification.Details = model.Details?.Select(x => x.ToTenantNotificationDetail()).ToList();
            oTenantNotification.DelegatedEmail = model.DelegatedEmail;
            oTenantNotification.SourceId = model.SourceId;
            oTenantNotification.Date = model.Date;
            oTenantNotification.DateH = model.DateH;

            return oTenantNotification;
        }

        public static TenantNotificationDTO ToTenantNotificationDTO(this TenantNotification model)
        {
            if (model == null) return new TenantNotificationDTO();
            TenantNotificationDTO oTenantNotificationDTO = new TenantNotificationDTO();
            oTenantNotificationDTO.Id = model.Id;
            oTenantNotificationDTO.CreatedBy = model.CreatedBy;
            oTenantNotificationDTO.CreatedOn = model.CreatedOn;
            oTenantNotificationDTO.ModefiedBy = model.ModefiedBy;
            oTenantNotificationDTO.ModefiedOn = model.ModefiedOn;
            oTenantNotificationDTO.Details = model.Details?.Select(x => x.ToTenantNotificationDetailDTO()).ToList();
            oTenantNotificationDTO.DelegatedEmail = model.DelegatedEmail;
            oTenantNotificationDTO.SourceId = model.SourceId;
            oTenantNotificationDTO.Date = model.Date;
            oTenantNotificationDTO.DateH = model.DateH;

            return oTenantNotificationDTO;
        }

        public static TenantNotificationDetail ToTenantNotificationDetail(this TenantNotificationDetailDTO model)
        {
            if (model == null) return new TenantNotificationDetail();
            TenantNotificationDetail oTenantNotificationDetail = new TenantNotificationDetail();
            oTenantNotificationDetail.Id = model.Id;
            oTenantNotificationDetail.CreatedBy = model.CreatedBy;
            oTenantNotificationDetail.CreatedOn = model.CreatedOn;
            oTenantNotificationDetail.ModefiedBy = model.ModefiedBy;
            oTenantNotificationDetail.ModefiedOn = model.ModefiedOn;
            oTenantNotificationDetail.TypeId = model.TypeId;
            oTenantNotificationDetail.Subject = model.Subject;
            oTenantNotificationDetail.Body = model.Body;
            oTenantNotificationDetail.Attachments = model.Attachments?.Select(x => x.ToTenantNotificationAttachment()).ToList();
            oTenantNotificationDetail.Template = model.Template?.ToTenantNotificationTemplate();

            return oTenantNotificationDetail;
        }

        public static TenantNotificationDetailDTO ToTenantNotificationDetailDTO(this TenantNotificationDetail model)
        {
            if (model == null) return new TenantNotificationDetailDTO();
            TenantNotificationDetailDTO oTenantNotificationDetailDTO = new TenantNotificationDetailDTO();
            oTenantNotificationDetailDTO.Id = model.Id;
            oTenantNotificationDetailDTO.CreatedBy = model.CreatedBy;
            oTenantNotificationDetailDTO.CreatedOn = model.CreatedOn;
            oTenantNotificationDetailDTO.ModefiedBy = model.ModefiedBy;
            oTenantNotificationDetailDTO.ModefiedOn = model.ModefiedOn;
            oTenantNotificationDetailDTO.TypeId = model.TypeId;
            oTenantNotificationDetailDTO.Subject = model.Subject;
            oTenantNotificationDetailDTO.Body = model.Body;
            oTenantNotificationDetailDTO.Attachments = model.Attachments?.Select(x => x.ToTenantNotificationAttachmentDTO()).ToList();
            oTenantNotificationDetailDTO.Template = model.Template?.ToTenantNotificationTemplateDTO();

            return oTenantNotificationDetailDTO;
        }

        public static TenantNotificationAttachment ToTenantNotificationAttachment(this TenantNotificationAttachmentDTO model)
        {
            if (model == null) return new TenantNotificationAttachment();
            TenantNotificationAttachment oTenantNotificationDetail = new TenantNotificationAttachment();
            oTenantNotificationDetail.Id = model.Id;
            oTenantNotificationDetail.CreatedBy = model.CreatedBy;
            oTenantNotificationDetail.CreatedOn = model.CreatedOn;
            oTenantNotificationDetail.ModefiedBy = model.ModefiedBy;
            oTenantNotificationDetail.ModefiedOn = model.ModefiedOn;
            oTenantNotificationDetail.Binary = model.Binary;
            oTenantNotificationDetail.FileName = model.FileName;
            oTenantNotificationDetail.ContentType = model.ContentType;
            oTenantNotificationDetail.ContentLength = model.ContentLength;

            return oTenantNotificationDetail;
        }

        public static TenantNotificationAttachmentDTO ToTenantNotificationAttachmentDTO(this TenantNotificationAttachment model)
        {
            if (model == null) return new TenantNotificationAttachmentDTO();
            TenantNotificationAttachmentDTO oTenantNotificationAttachmentDTO = new TenantNotificationAttachmentDTO();
            oTenantNotificationAttachmentDTO.Id = model.Id;
            oTenantNotificationAttachmentDTO.CreatedBy = model.CreatedBy;
            oTenantNotificationAttachmentDTO.CreatedOn = model.CreatedOn;
            oTenantNotificationAttachmentDTO.ModefiedBy = model.ModefiedBy;
            oTenantNotificationAttachmentDTO.ModefiedOn = model.ModefiedOn;
            oTenantNotificationAttachmentDTO.Binary = model.Binary;
            oTenantNotificationAttachmentDTO.FileName = model.FileName;
            oTenantNotificationAttachmentDTO.ContentType = model.ContentType;
            oTenantNotificationAttachmentDTO.ContentLength = model.ContentLength;

            return oTenantNotificationAttachmentDTO;
        }

        public static TenantNotificationTemplate ToTenantNotificationTemplate(this TenantNotificationTemplateDTO model)
        {
            if (model == null) return new TenantNotificationTemplate();
            TenantNotificationTemplate oTenantNotificationTemplate = new TenantNotificationTemplate();
            oTenantNotificationTemplate.Id = model.Id;
            oTenantNotificationTemplate.CreatedBy = model.CreatedBy;
            oTenantNotificationTemplate.CreatedOn = model.CreatedOn;
            oTenantNotificationTemplate.ModefiedBy = model.ModefiedBy;
            oTenantNotificationTemplate.ModefiedOn = model.ModefiedOn;
            oTenantNotificationTemplate.TypeId = model.TypeId;
            oTenantNotificationTemplate.Type = model.Type?.ToTenantLookup();
            oTenantNotificationTemplate.Date = model.Date;
            oTenantNotificationTemplate.DateH = model.DateH;

            return oTenantNotificationTemplate;
        }

        public static TenantNotificationTemplateDTO ToTenantNotificationTemplateDTO(this TenantNotificationTemplate model)
        {
            if (model == null) return new TenantNotificationTemplateDTO();
            TenantNotificationTemplateDTO oTenantNotificationTemplateDTO = new TenantNotificationTemplateDTO();
            oTenantNotificationTemplateDTO.Id = model.Id;
            oTenantNotificationTemplateDTO.CreatedBy = model.CreatedBy;
            oTenantNotificationTemplateDTO.CreatedOn = model.CreatedOn;
            oTenantNotificationTemplateDTO.ModefiedBy = model.ModefiedBy;
            oTenantNotificationTemplateDTO.ModefiedOn = model.ModefiedOn;
            oTenantNotificationTemplateDTO.TypeId = model.TypeId;
            oTenantNotificationTemplateDTO.Type = model.Type?.ToTenantLookupDTO();
            oTenantNotificationTemplateDTO.Date = model.Date;
            oTenantNotificationTemplateDTO.DateH = model.DateH;

            return oTenantNotificationTemplateDTO;
        }
    }
}