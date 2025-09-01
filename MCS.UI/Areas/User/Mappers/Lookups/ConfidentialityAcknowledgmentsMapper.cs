using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class ConfidentialityAcknowledgmentsMapper
    {
        public static List<ConfidentialityAcknowledgmentsVM> Map(IList<ConfidentialityAcknowledgmentsDTO> confidentialityAcknowledgmentsDTO)
        {
            if (confidentialityAcknowledgmentsDTO == null || !confidentialityAcknowledgmentsDTO.Any())
            {
                return new List<ConfidentialityAcknowledgmentsVM>();
            }
            List<ConfidentialityAcknowledgmentsVM> confidentialityAcknowledgmentsVM = confidentialityAcknowledgmentsDTO
                .Select(attachmentTypeDTO => new ConfidentialityAcknowledgmentsVM()
                { 
                    Id = attachmentTypeDTO.Id,
                    IsMandatary = attachmentTypeDTO.IsMandatary,
                    Description = LocalizationMapper.Map(attachmentTypeDTO.Description),
                    LocalName = attachmentTypeDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeDTO.TransactionCategories)
                }).ToList();

            return confidentialityAcknowledgmentsVM;
        }
        public static List<ConfidentialityAcknowledgmentsDTO> Map(IList<ConfidentialityAcknowledgmentsVM> confidentialityAcknowledgmentsVMs)
        {
            if (confidentialityAcknowledgmentsVMs == null || !confidentialityAcknowledgmentsVMs.Any())
            {
                return new List<ConfidentialityAcknowledgmentsDTO>();
            }
            List<ConfidentialityAcknowledgmentsDTO> confidentialityAcknowledgmentsDTOs = confidentialityAcknowledgmentsVMs
                .Select(confidentialityAcknowledgmentsVM => new ConfidentialityAcknowledgmentsDTO()
                {
                    Id = confidentialityAcknowledgmentsVM.Id,
                    IsMandatary = confidentialityAcknowledgmentsVM.IsMandatary,
                    Description = LocalizationMapper.Map(confidentialityAcknowledgmentsVM.Description),
                    LocalName = confidentialityAcknowledgmentsVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(confidentialityAcknowledgmentsVM.TransactionCategories)
                }).ToList();

            return confidentialityAcknowledgmentsDTOs;
        }
        public static List<ConfidentialityAcknowledgmentsAddVM> Map(IList<ConfidentialityAcknowledgmentsAddDTO> confidentialityAcknowledgmentsAddDTOs)
        {
            if (confidentialityAcknowledgmentsAddDTOs == null || !confidentialityAcknowledgmentsAddDTOs.Any())
            {
                return new List<ConfidentialityAcknowledgmentsAddVM>();
            }
            List<ConfidentialityAcknowledgmentsAddVM> confidentialityAcknowledgmentsAddVMs = confidentialityAcknowledgmentsAddDTOs
                .Select(confidentialityAcknowledgmentsVM => new ConfidentialityAcknowledgmentsAddVM()
                {
                    IsMandatary = confidentialityAcknowledgmentsVM.IsMandatary,
                    Description = LocalizationMapper.Map(confidentialityAcknowledgmentsVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(confidentialityAcknowledgmentsVM.TransactionCategories) 
                }).ToList();

            return confidentialityAcknowledgmentsAddVMs;
        }
        public static List<ConfidentialityAcknowledgmentsAddDTO> Map(IList<ConfidentialityAcknowledgmentsAddVM> confidentialityAcknowledgmentsAddVMs)
        {
            if (confidentialityAcknowledgmentsAddVMs == null || !confidentialityAcknowledgmentsAddVMs.Any())
            {
                return new List<ConfidentialityAcknowledgmentsAddDTO>();
            }
            List<ConfidentialityAcknowledgmentsAddDTO> confidentialityAcknowledgmentsAddDTOs = confidentialityAcknowledgmentsAddVMs
                .Select(confidentialityAcknowledgmentsAddVM => new ConfidentialityAcknowledgmentsAddDTO()
                {
                    IsMandatary = confidentialityAcknowledgmentsAddVM.IsMandatary,
                    Description = LocalizationMapper.Map(confidentialityAcknowledgmentsAddVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(confidentialityAcknowledgmentsAddVM.TransactionCategories) 
                }).ToList();

            return confidentialityAcknowledgmentsAddDTOs;

        }
        public static List<ConfidentialityAcknowledgmentsEditVM> Map(IList<ConfidentialityAcknowledgmentsEditDTO> confidentialityAcknowledgmentsEditDTOs)
        {
            if (confidentialityAcknowledgmentsEditDTOs == null || !confidentialityAcknowledgmentsEditDTOs.Any())
            {
                return new List<ConfidentialityAcknowledgmentsEditVM>();
            }
            List<ConfidentialityAcknowledgmentsEditVM> confidentialityAcknowledgmentsEditVMs = confidentialityAcknowledgmentsEditDTOs
                .Select(confidentialityAcknowledgmentsEditDTO => new ConfidentialityAcknowledgmentsEditVM()
                {
                    Id = confidentialityAcknowledgmentsEditDTO.Id,
                    Description = LocalizationMapper.Map(confidentialityAcknowledgmentsEditDTO.Description),
                    IsMandatary = confidentialityAcknowledgmentsEditDTO.IsMandatary,
                    TransactionCategories = TransactionCategoryMapper.Map(confidentialityAcknowledgmentsEditDTO.TransactionCategories) 
                }).ToList();

            return confidentialityAcknowledgmentsEditVMs;
        }
        public static List<ConfidentialityAcknowledgmentsEditDTO> Map(IList<ConfidentialityAcknowledgmentsEditVM> confidentialityAcknowledgmentsEditVMs)
        {
            if (confidentialityAcknowledgmentsEditVMs == null || !confidentialityAcknowledgmentsEditVMs.Any())
            {
                return new List<ConfidentialityAcknowledgmentsEditDTO>();
            }
            List<ConfidentialityAcknowledgmentsEditDTO> confidentialityAcknowledgmentsEditDTO = confidentialityAcknowledgmentsEditVMs
                .Select(confidentialityAcknowledgmentsEditVM => new ConfidentialityAcknowledgmentsEditDTO()
                { 
                    Id = confidentialityAcknowledgmentsEditVM.Id,
                    Description = LocalizationMapper.Map(confidentialityAcknowledgmentsEditVM.Description) ,
                    TransactionCategories = TransactionCategoryMapper.Map(confidentialityAcknowledgmentsEditVM.TransactionCategories),
                    IsMandatary = confidentialityAcknowledgmentsEditVM.IsMandatary
                }).ToList();

            return confidentialityAcknowledgmentsEditDTO;
        }

    }
}