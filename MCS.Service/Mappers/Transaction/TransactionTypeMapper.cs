using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionTypeMapper
    {
        public static BarcodeDesignType Map(TransactionCategory transactionCategory)
        {
            switch (transactionCategory)
            {
                case TransactionCategory.Inbound:
                    return BarcodeDesignType.Inbound;
                case TransactionCategory.ExternalOutbound:
                    return BarcodeDesignType.Outbound;
                case TransactionCategory.InternalOutbound:
                    return BarcodeDesignType.OutboundInternal;
                default:
                    break;
            }
            return BarcodeDesignType.None;
        }


        public static TransactionType Map(TransactionTypeAddDTO transactionTypeAddDTO)
        {

            if (transactionTypeAddDTO != null)
            {
                ILookupBL lookupBL = IoC.Resolve<ILookupBL>();
                IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                Permission permission = permissionBL.GetPermissionById(transactionTypeAddDTO.PermissionId);

                TransactionType transactionType = new TransactionType()
                {
                    TransactionCategories = TransactionCategoryMapper.Map(permission),
                    Color = lookupBL.GetLookupItem(transactionTypeAddDTO.ColorId),
                    LocalizationIdentifier = LocalizationIdentifierMapper.Map(transactionTypeAddDTO.Description),
                    Abbreviation = LocalizationIdentifierMapper.Map(transactionTypeAddDTO.Abbreviation),
                    Permission = permission
                };

                return transactionType;
            }
            return null;
        }

        public static TransactionType Map(TransactionTypeEditDTO transactionTypeEditDTO)
        {
            if (transactionTypeEditDTO != null)
            {
                IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                Permission permission = permissionBL.GetPermissionById(transactionTypeEditDTO.PermissionId);

                TransactionType transactionType = new TransactionType()
                {
                    Id = transactionTypeEditDTO.Id,
                    TransactionCategories = TransactionCategoryMapper.Map(permission),
                    Color = lookupBL.GetLookupItem(transactionTypeEditDTO.ColorId),
                    LocalizationIdentifier = LocalizationIdentifierMapper.Map(transactionTypeEditDTO.Description),
                    Abbreviation = LocalizationIdentifierMapper.Map(transactionTypeEditDTO.Abbreviation),
                    Permission = permission,
                    PermissionId = transactionTypeEditDTO.PermissionId
                };

                return transactionType;
            }
            return null;
        }

        public static TransactionTypeEditDTO Map(TransactionType transactionType, string cultureName)
        {
            if (transactionType != null)
            {
                TransactionTypeEditDTO transactionTypeEditDTO = new TransactionTypeEditDTO()
                {
                    Id = transactionType.Id,
                    ColorId = transactionType.Color.Id,
                    Description = LocalizationIdentifierMapper.Map(transactionType.LocalizationIdentifier.Localizations),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionType.TransactionCategories, cultureName),
                    PermissionId = transactionType.Permission.Id,
                };

                if (transactionType.Abbreviation != null)
                {
                    transactionTypeEditDTO.Abbreviation = LocalizationIdentifierMapper.Map(transactionType.Abbreviation.Localizations);
                }

                return transactionTypeEditDTO;
            }
            return null;
        }

        public static List<TransactionTypeDTO> Map(IList<TransactionType> transactionTypes, string cultureName)
        {
            if (transactionTypes == null || !transactionTypes.Any())
            {
                return null;
            }
            List<TransactionTypeDTO> transactionTypeDTOs = transactionTypes
                .Select(transactionType => new TransactionTypeDTO()
                {
                    Id = transactionType.Id,
                    LocalName = transactionType.Text,
                    TransactionCategories = TransactionCategoryMapper.Map(transactionType.TransactionCategories, cultureName),
                    Description = transactionType.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(transactionType.LocalizationIdentifier.Localizations) : null

                }).ToList();

            return transactionTypeDTOs;
        }
    }
}