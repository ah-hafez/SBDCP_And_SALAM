using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class FollowUpLookUpsMapper
    {
        public static List<FollowUpLookUpDTO> Map(IList<FollowUpLookUpsVM> followUpLookUpsVMs)
        {
            if (followUpLookUpsVMs == null || !followUpLookUpsVMs.Any())
            { return null; }
            List<FollowUpLookUpDTO> FollowUpLookUpsDTOs = followUpLookUpsVMs
                .Select(b => new FollowUpLookUpDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id,
                    IsInternal = b.IsInternal,
                    LocalName = b.LocalName

                }).ToList();
            return FollowUpLookUpsDTOs;
        }


        public static List<FollowUpLookUpsVM> Map(IList<FollowUpLookUpDTO> followUpLookUpsDTOs)
        {
            if (followUpLookUpsDTOs == null || !followUpLookUpsDTOs.Any())
            {
                return new List<FollowUpLookUpsVM>();
            }
            List<FollowUpLookUpsVM> followUpPriorityTypesVMs = followUpLookUpsDTOs
                .Select(b => new FollowUpLookUpsVM
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
            return followUpPriorityTypesVMs;
        }
        public static List<FollowUpLookUpAddDTO> Map(IList<FollowUpLookUpsAddVM> followUpLookUpsAddVMs)
        {
            if (followUpLookUpsAddVMs == null || !followUpLookUpsAddVMs.Any())
            { return null; }
            List<FollowUpLookUpAddDTO> FollowUpLookUpsAddDTOs = followUpLookUpsAddVMs
                .Select(b => new FollowUpLookUpAddDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)

                }).ToList();
            return FollowUpLookUpsAddDTOs;
        }
        public static FollowUpLookUpAddDTO Map(FollowUpLookUpsAddVM followUpLookUpsAddVM)
        {
            FollowUpLookUpAddDTO FollowUpLookUpsAddDTOs = new FollowUpLookUpAddDTO()
            {
                Description = LocalizationMapper.Map(followUpLookUpsAddVM.Description),
                TransactionCategories = TransactionCategoryMapper.Map(followUpLookUpsAddVM.TransactionCategories)

            };
            return FollowUpLookUpsAddDTOs;
        }

        public static FollowUpLookUpsAddVM Map(FollowUpLookUpAddDTO FollowUpLookUpsAddDTOs)
        {
            if (FollowUpLookUpsAddDTOs != null)
            {
                FollowUpLookUpsAddVM followUpLookUpsAddVM = new FollowUpLookUpsAddVM()
                {
                    Description = LocalizationMapper.Map(FollowUpLookUpsAddDTOs.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(FollowUpLookUpsAddDTOs.TransactionCategories)

                };
                return followUpLookUpsAddVM;
            }
            return null;
        }
        public static List<FollowUpLookUpEditDTO> Map(IList<FollowUpLookUpsEditVM> followUpLookUpsEditVMs)
        {
            if (followUpLookUpsEditVMs == null || !followUpLookUpsEditVMs.Any())
            { return null; }
            List<FollowUpLookUpEditDTO> followUpLookUpsEditDTOs = followUpLookUpsEditVMs
                .Select(b => new FollowUpLookUpEditDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id

                }).ToList();
            return followUpLookUpsEditDTOs;
        }
        public static List<FollowUpLookUpsEditVM> Map(IList<FollowUpLookUpEditDTO> followUpLookUpsEditDTOs)
        {
            if (followUpLookUpsEditDTOs == null || !followUpLookUpsEditDTOs.Any())
            { return null; }
            List<FollowUpLookUpsEditVM> followUpLookUpsEditVMs = followUpLookUpsEditDTOs
                .Select(b => new FollowUpLookUpsEditVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Id = b.Id

                }).ToList();
            return followUpLookUpsEditVMs;
        }
        public static FollowUpLookUpsEditVM Map(FollowUpLookUpEditDTO followUpLookUpsEditDTOs)
        {
            if (followUpLookUpsEditDTOs != null)
            {
                FollowUpLookUpsEditVM followUpLookUpsEditVMs = new FollowUpLookUpsEditVM()
                {
                    Description = LocalizationMapper.Map(followUpLookUpsEditDTOs.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(followUpLookUpsEditDTOs.TransactionCategories),
                    Id = followUpLookUpsEditDTOs.Id,
                    IsActive = followUpLookUpsEditDTOs.IsActive,
                    IsLocked = followUpLookUpsEditDTOs.IsLocked,
                    LockedBy = followUpLookUpsEditDTOs.LockedBy
                };
                return followUpLookUpsEditVMs;
            }
            return null;
        }
        public static FollowUpLookUpEditDTO Map(FollowUpLookUpsEditVM followUpLookUpsEditVMs)
        {
            if (followUpLookUpsEditVMs != null)
            {
                FollowUpLookUpEditDTO followUpLookUpsEditDTOs = new FollowUpLookUpEditDTO()
                {
                    Description = LocalizationMapper.Map(followUpLookUpsEditVMs.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(followUpLookUpsEditVMs.TransactionCategories),
                    Id = followUpLookUpsEditVMs.Id

                };
                return followUpLookUpsEditDTOs;
            }
            return null;
        }
    }
}