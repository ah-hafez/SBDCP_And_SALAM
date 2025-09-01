using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class PriorityMapper
    {
        public static PriorityVM Map(PriorityDTO priorityDTO)
        {
            if (priorityDTO != null)
            {
                PriorityVM priorityVM = new PriorityVM()
                { 
                    Id = priorityDTO.Id,
                    LocalName = priorityDTO.LocalName,
                    HasDate = priorityDTO.HasDate,
                    TransactionCategories = TransactionCategoryMapper.Map(priorityDTO.TransactionCategories),
                    Description = LocalizationMapper.Map(priorityDTO.Description)
                };
                return priorityVM;
            }
            return new PriorityVM();
        }
        public static PriorityDTO Map(PriorityVM priorityVM)
        {
            if (priorityVM != null)
            {
                PriorityDTO priorityDTO = new PriorityDTO()
                { 
                    Id = priorityVM.Id,
                    LocalName = priorityVM.LocalName,
                    HasDate = priorityVM.HasDate,
                    TransactionCategories = TransactionCategoryMapper.Map(priorityVM.TransactionCategories),
                    Description = LocalizationMapper.Map(priorityVM.Description)
                };
                return priorityDTO;
            }
            return new PriorityDTO();
        }
        public static List<PriorityVM> Map(IList<PriorityDTO> priorityDTOs)
        {
            if (priorityDTOs == null || !priorityDTOs.Any())
            {
                return new List<PriorityVM>();
            }
            List<PriorityVM> priorityVM = priorityDTOs
                .Select(priorityDTO => new PriorityVM()
                {
                    Id = priorityDTO.Id,
                    LocalName = priorityDTO.LocalName,
                    HasDate = priorityDTO.HasDate,
                    TransactionCategories = TransactionCategoryMapper.Map(priorityDTO.TransactionCategories),
                    Description = LocalizationMapper.Map(priorityDTO.Description)
                }).ToList();
            return priorityVM;
        }
        public static List<PriorityDTO> Map(IList<PriorityVM> priorityVMs)
        {
            if (priorityVMs == null || !priorityVMs.Any())
            {
                return new List<PriorityDTO>();
            }
            List<PriorityDTO> priorityDTOs = priorityVMs
                .Select(priorityDTO => new PriorityDTO()
                {
                    Id = priorityDTO.Id,
                    LocalName = priorityDTO.LocalName,
                    HasDate = priorityDTO.HasDate,
                    TransactionCategories = TransactionCategoryMapper.Map(priorityDTO.TransactionCategories),
                    Description = LocalizationMapper.Map(priorityDTO.Description)
                }).ToList();
            return priorityDTOs;
        }
    }
}