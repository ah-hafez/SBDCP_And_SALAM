using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class LinkMapper
    {
        public static List<LinkDTO> Map(IList<LinkVM> linkVMs)
        {
            if (linkVMs == null || !linkVMs.Any())
            { return null; }
            List<LinkDTO> linkDTOs = linkVMs
                .Select(b => new LinkDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id,
                    IsInternal = b.IsInternal,
                    LocalName = b.LocalName

                }).ToList();
            return linkDTOs;
        }
        public static List<LinkVM> Map(IList<LinkDTO> linkDTOs)
        {
            if (linkDTOs == null || !linkDTOs.Any())
            {
                return new List<LinkVM>();
            }
            List<LinkVM> linkVMs = linkDTOs
                .Select(b => new LinkVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id,
                    IsInternal = b.IsInternal,
                    LocalName = b.LocalName,
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return linkVMs;
        }
        public static List<LinkAddDTO> Map(IList<LinkAddVM> linkAddVMs)
        {
            if (linkAddVMs == null || !linkAddVMs.Any())
            { return null; }
            List<LinkAddDTO> linkAddDTOs = linkAddVMs
                .Select(b => new LinkAddDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)

                }).ToList();
            return linkAddDTOs;
        }
        public static LinkAddDTO Map(LinkAddVM linkAddVM)
        {
            LinkAddDTO linkAddDTO = new LinkAddDTO()
            {
                Description = LocalizationMapper.Map(linkAddVM.Description),
                TransactionCategories = TransactionCategoryMapper.Map(linkAddVM.TransactionCategories)

            };
            return linkAddDTO;
        }
        public static LinkAddVM Map(LinkAddDTO linkAddDTO)
        {
            if (linkAddDTO != null)
            {
                LinkAddVM linkAddVM = new LinkAddVM()
                {
                    Description = LocalizationMapper.Map(linkAddDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories)

                };
                return linkAddVM;
            }
            return null;
        }
        public static List<LinkEditDTO> Map(IList<LinkEditVM> linkEditVMs)
        {
            if (linkEditVMs == null || !linkEditVMs.Any())
            { return null; }
            List<LinkEditDTO> linkEditDTOs = linkEditVMs
                .Select(b => new LinkEditDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id

                }).ToList();
            return linkEditDTOs;
        }
        public static List<LinkEditVM> Map(IList<LinkEditDTO> linkEditDTOs)
        {
            if (linkEditDTOs == null || !linkEditDTOs.Any())
            { return null; }
            List<LinkEditVM> linkEditVMs = linkEditDTOs
                .Select(b => new LinkEditVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id

                }).ToList();
            return linkEditVMs;
        }
        public static LinkEditVM Map(LinkEditDTO linkEditDTO)
        {
            if (linkEditDTO != null)
            {
                LinkEditVM linkEditVM = new LinkEditVM()
                {
                    Description = LocalizationMapper.Map(linkEditDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories),
                    Id = linkEditDTO.Id,
                    IsActive = linkEditDTO.IsActive,
                    IsLocked = linkEditDTO.IsLocked,
                    LockedBy = linkEditDTO.LockedBy
                };
                return linkEditVM;
            }
            return null;
        }
        public static LinkEditDTO Map(LinkEditVM linkEditVM)
        {
            if (linkEditVM != null)
            {
                LinkEditDTO linkEditDTO = new LinkEditDTO()
                {
                    Description = LocalizationMapper.Map(linkEditVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(linkEditVM.TransactionCategories),
                    Id = linkEditVM.Id

                };
                return linkEditDTO;
            }
            return null;
        }

    }
}