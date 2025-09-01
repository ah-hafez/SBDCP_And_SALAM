using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class LinkMapper
    {
        public static List<LinkVM> Map(IList<LinkDTO> linkDTOs)
        {
            if (linkDTOs == null || !linkDTOs.Any())
            {
                return new List<LinkVM>();
            }
            List<LinkVM> linkVMs = linkDTOs
                .Select(linkDTO => new LinkVM()
                { 
                    Id = linkDTO.Id,
                    Description = LocalizationMapper.Map(linkDTO.Description),
                    IsInternal = linkDTO.IsInternal,
                    LocalName = linkDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(linkDTO.TransactionCategories)
                }).ToList();
            return linkVMs;
        }
        public static List<LinkDTO> Map(IList<LinkVM> linkVMs)
        {
            if (linkVMs == null || !linkVMs.Any())
            {
                return new List<LinkDTO>();
            }
            List<LinkDTO> linkDTOs = linkVMs
                .Select(linkVM => new LinkDTO()
                {
                    Id = linkVM.Id,
                    Description = LocalizationMapper.Map(linkVM.Description),
                    IsInternal = linkVM.IsInternal,
                    LocalName = linkVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(linkVM.TransactionCategories)
                }).ToList();
            return linkDTOs;
        }
        public static List<LinkAddDTO> Map(IList<LinkAddVM> linkAddVMs)
        {
            if (linkAddVMs == null || !linkAddVMs.Any())
            {
                return new List<LinkAddDTO>();
            }
            List<LinkAddDTO> linkAddDTOs = linkAddVMs
                .Select(linkAddVM => new LinkAddDTO()
                {
                    Description = LocalizationMapper.Map(linkAddVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkAddVM.TransactionCategories)
                }).ToList();
            return linkAddDTOs;
        }
        public static List<LinkAddVM> Map(IList<LinkAddDTO> linkAddDTOs)
        {
            if (linkAddDTOs == null || !linkAddDTOs.Any())
            {
                return new List<LinkAddVM>();
            }
            List<LinkAddVM> linkAddVMs = linkAddDTOs
                .Select(linkAddDTO => new LinkAddVM()
                {
                    Description = LocalizationMapper.Map(linkAddDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories)
                }).ToList();
            return linkAddVMs;
        }
        public static List<LinkEditVM> Map(IList<LinkEditDTO> linkEditDTOs)
        {
            if (linkEditDTOs == null || !linkEditDTOs.Any())
            {
                return new List<LinkEditVM>();
            }
            List<LinkEditVM> linkEditVMs = linkEditDTOs
                .Select(linkEditDTO => new LinkEditVM()
                { 
                    Id = linkEditDTO.Id,
                    Description = LocalizationMapper.Map(linkEditDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories)
                }).ToList();
            return linkEditVMs;
        }
        public static List<LinkEditDTO> Map(IList<LinkEditVM> linkEditVMs)
        {
            if (linkEditVMs == null || !linkEditVMs.Any())
            {
                return new List<LinkEditDTO>();
            }
            List<LinkEditDTO> linkEditDTOs = linkEditVMs
                .Select(linkEditVM => new LinkEditDTO()
                {
                    Id = linkEditVM.Id,
                    Description = LocalizationMapper.Map(linkEditVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkEditVM.TransactionCategories)
                }).ToList();
            return linkEditDTOs;
        }
    }
}