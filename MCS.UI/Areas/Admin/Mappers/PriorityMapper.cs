using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class PriorityMapper
    {
        public static List<PriorityDTO> Map(IList<PriorityVM> priorityVMs)
        {
            if (priorityVMs == null || !priorityVMs.Any())
            { return null; }
            List<PriorityDTO> priorityDTOs = priorityVMs
                .Select(b => new PriorityDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    HasDate = b.HasDate,
                    Id = b.Id,
                    LocalName = b.LocalName
                }).ToList();
            return priorityDTOs;
        }
        public static List<PriorityVM> Map(IList<PriorityDTO> priorityDTOs)
        {
            if (priorityDTOs == null || !priorityDTOs.Any())
            {
                return new List<PriorityVM>();
            }
            List<PriorityVM> priorityVMs = priorityDTOs
                .Select(b => new PriorityVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    HasDate = b.HasDate,
                    Id = b.Id,
                    LocalName = b.LocalName,
                    HasPriorityExceptions = b.HasPriorityExceptions,
                    LateForEntity = b.LateForEntity,
                    LateForUser = b.LateForUser
                }).ToList();
            return priorityVMs;
        }
        public static List<PriorityAddDTO> Map(IList<PriorityAddVM> priorityAddVMs)
        {
            if (priorityAddVMs == null || !priorityAddVMs.Any())
            { return null; }
            List<PriorityAddDTO> priorityAddDTOs = priorityAddVMs
                .Select(b => new PriorityAddDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    HasDate = b.HasDate

                }).ToList();
            return priorityAddDTOs;
        }
        public static PriorityAddDTO Map(PriorityAddVM priorityAddVM)
        {
            if (priorityAddVM != null)
            {
                PriorityAddDTO priorityAddDTO = new PriorityAddDTO()
                {
                    Description = LocalizationMapper.Map(priorityAddVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(priorityAddVM.TransactionCategories),
                    HasDate = priorityAddVM.HasDate

                };
                return priorityAddDTO;
            }
            return null;
        }
        public static PriorityAddVM Map(PriorityAddDTO priorityAddDTO)
        {
            if (priorityAddDTO != null)
            {
                PriorityAddVM priorityAddVM = new PriorityAddVM()
                {
                    Description = LocalizationMapper.Map(priorityAddDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(priorityAddDTO.TransactionCategories),
                    HasDate = priorityAddDTO.HasDate

                };
                return priorityAddVM;
            }
            return null;
        }
        public static List<PriorityEditDTO> Map(IList<PriorityEditVM> priorityEditVMs)
        {
            if (priorityEditVMs == null || !priorityEditVMs.Any())
            { return null; }
            List<PriorityEditDTO> priorityEditDTOs = priorityEditVMs
                .Select(b => new PriorityEditDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    HasDate = b.HasDate,
                    Id = b.Id
                }).ToList();
            return priorityEditDTOs;
        }
        public static List<PriorityEditVM> Map(IList<PriorityEditDTO> priorityEditDTOs)
        {
            if (priorityEditDTOs == null || !priorityEditDTOs.Any())
            { return null; }
            List<PriorityEditVM> priorityEditVMs = priorityEditDTOs
                .Select(b => new PriorityEditVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    HasDate = b.HasDate,
                    Id = b.Id,
                    HasPriorityExceptions = b.HasPriorityExceptions,
                    LateForEntity = b.LateForEntity,
                    LateForUser = b.LateForUser,
                    PriorityExceptions = PriorityExceptionMapper.Map(b.PriorityExceptions),
                    Sort = b.Sort,
                    ProcessPeriod = b.ProcessPeriod
                }).ToList();
            return priorityEditVMs;
        }
        public static PriorityEditVM Map(PriorityEditDTO priorityEditDTO)
        {
            if (priorityEditDTO != null)
            {
                PriorityEditVM priorityEditVM = new PriorityEditVM()
                {
                    Description = LocalizationMapper.Map(priorityEditDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(priorityEditDTO.TransactionCategories),
                    HasDate = priorityEditDTO.HasDate,
                    Id = priorityEditDTO.Id,
                    HasPriorityExceptions = priorityEditDTO.HasPriorityExceptions,
                    LateForEntity = priorityEditDTO.LateForEntity,
                    LateForUser = priorityEditDTO.LateForUser,
                    PriorityExceptions = PriorityExceptionMapper.Map(priorityEditDTO.PriorityExceptions),
                    Sort = priorityEditDTO.Sort,
                    ProcessPeriod = priorityEditDTO.ProcessPeriod
                };
                return priorityEditVM;
            }
            return null;
        }
        public static PriorityEditDTO Map(PriorityEditVM priorityEditVM)
        {
            if (priorityEditVM != null)
            {
                PriorityEditDTO priorityEditDTO = new PriorityEditDTO()
                {
                    Description = LocalizationMapper.Map(priorityEditVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(priorityEditVM.TransactionCategories),
                    HasDate = priorityEditVM.HasDate,
                    Id = priorityEditVM.Id,
                    HasPriorityExceptions = priorityEditVM.HasPriorityExceptions,
                    LateForEntity = priorityEditVM.LateForEntity,
                    LateForUser = priorityEditVM.LateForUser,
                    Sort = priorityEditVM.Sort,
                    ProcessPeriod = priorityEditVM.ProcessPeriod
                };
                return priorityEditDTO;
            }
            return null;
        }
    }
}