using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class CounterDetailMapper
    {
        public static List<CounterDetailVM> Map(IList<CounterDetailDTO> counterDetailDTOs)
        {
            if (counterDetailDTOs == null || !counterDetailDTOs.Any())
            {
                return new List<CounterDetailVM>();
            }
            List<CounterDetailVM> counterDetailVMs = counterDetailDTOs
                .Select(counterDetailDTO => new CounterDetailVM()
                {
                    Id = counterDetailDTO.Id,
                    InitialValue = counterDetailDTO.InitialValue,
                    LastTransactionNumber = counterDetailDTO.LastTransactionNumber,
                    TransactionCategories = TransactionCategoryMapper.Map(counterDetailDTO.TransactionCategories),
                }).ToList();

            return counterDetailVMs;
        }
        public static List<CounterDetailVM> Map_New(IList<CounterDetailDTO> counterDetailDTOs)
        {
            if (counterDetailDTOs == null || !counterDetailDTOs.Any())
            {
                return new List<CounterDetailVM>();
            }
            List<CounterDetailVM> counterDetailVMs = counterDetailDTOs
                .Select(counterDetailDTO => new CounterDetailVM()
                {
                    Id = counterDetailDTO.Id,
                    InitialValue = counterDetailDTO.InitialValue,
                    LastTransactionNumber = counterDetailDTO.Count-1,
                    TransactionCategories = TransactionCategoryMapper.Map(counterDetailDTO.TransactionCategories),
                }).ToList();

            return counterDetailVMs;
        }
        public static List<CounterDetailDTO> Map(IList<CounterDetailVM> counterDetailVMs)
        {
            if (counterDetailVMs == null || !counterDetailVMs.Any())
            {
                return new List<CounterDetailDTO>();
            }
            List<CounterDetailDTO> counterDetailDTOs = counterDetailVMs
                .Select(counterDetailVM => new CounterDetailDTO()
                {
                    Id = counterDetailVM.Id,
                    InitialValue = counterDetailVM.InitialValue,
                    LastTransactionNumber = counterDetailVM.LastTransactionNumber,
                    TransactionCategories = TransactionCategoryMapper.Map(counterDetailVM.TransactionCategories),
                }).ToList();

            return counterDetailDTOs;
        }
    }
}