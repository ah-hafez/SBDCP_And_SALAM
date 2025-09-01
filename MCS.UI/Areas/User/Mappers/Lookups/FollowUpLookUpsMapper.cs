using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
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
       

     
       
    }
}