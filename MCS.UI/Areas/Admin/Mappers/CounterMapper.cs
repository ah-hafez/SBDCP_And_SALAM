using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.OrgUnit;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class CounterMapper
    {
        public static List<CounterDetailDTO> Map(IList<CounterDetailVM> counterDetailVMs)
        {
            if (counterDetailVMs == null || !counterDetailVMs.Any())
            {
                return null;
            }
            List<CounterDetailDTO> counterDetailDTOs = counterDetailVMs
                .Select(b => new CounterDetailDTO
                {                    
                    Id = b.Id,
                    Count = b.Count,
                    InitialValue = b.InitialValue,
                    LastTransactionNumber = b.LastTransactionNumber,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)

                }).ToList();
            return counterDetailDTOs;
        }
        public static List<CounterDetailVM> Map(IList<CounterDetailDTO> counterDetailDTOs)
        {
            if (counterDetailDTOs == null || !counterDetailDTOs.Any())
            {
                return null;
            }
            List<CounterDetailVM> counterDetailVMs = counterDetailDTOs
                .Select(b => new CounterDetailVM
                {
                    Id = b.Id,
                    Count = b.Count,
                    InitialValue = b.InitialValue,
                    LastTransactionNumber = b.LastTransactionNumber,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return counterDetailVMs;
        }
        public static List<CounterDTO> Map(IList<CounterVM> counterVMs)
        {
            if (counterVMs == null || !counterVMs.Any())
            {
                return null;
            }
            List<CounterDTO> counterDTOs = counterVMs
                .Select(b => new CounterDTO
                {
                    CounterDetails = CounterMapper.Map(b.CounterDetails),
                    Id = b.CounterId,
                    IsGeneral = b.IsGeneral

                }).ToList();
            return counterDTOs;
        }
        public static List<CounterVM> Map(IList<CounterDTO> counterDTOs, string cultureName)
        {
            if (counterDTOs == null || !counterDTOs.Any())
            {
                return null;
            }
            List<CounterVM> counterVMs = counterDTOs
                .Select(b => new CounterVM
                {
                    CounterDetails = CounterMapper.Map(b.CounterDetails),
                    CounterId = b.Id,
                    IsGeneral = b.IsGeneral

                }).ToList();
            return counterVMs;
        }
        public static CounterVM Map(CounterDTO counterDTO, string cultureName)
        {
            if (counterDTO != null)
            {
                return new CounterVM
                {

                    CounterDetails = Map(counterDTO.CounterDetails),
                    CounterId = counterDTO.Id,
                    IsGeneral = counterDTO.IsGeneral,
                    OwnerEntityId = counterDTO.OwnerEntityId,
                    Description = LocalizationMapper.Map(counterDTO.Description)
                };
            }
            return null;
        }
        public static CounterDTO Map(CounterVM counterVM)
        {
            if (counterVM != null)
            {
                return new CounterDTO
                {
                    Id = counterVM.CounterId,
                    Year = counterVM.Year,
                    OwnerEntityId = counterVM.OwnerEntityId,
                    Description = LocalizationMapper.Map(counterVM.Description),
                    IsGeneral = counterVM.IsGeneral,
                    CounterDetails = Map(counterVM.CounterDetails)     
                };
            }
            return null;
        }
        public static List<CounterEditVM> Map(IList<CounterEditDTO> counterEditDTOs, string cultureName)
        {
            if (counterEditDTOs == null || !counterEditDTOs.Any())
            {
                return null;
            }
            List<CounterEditVM> counterEditVMs = counterEditDTOs
                .Select(b => new CounterEditVM
                {
                    CounterDetails = CounterMapper.Map(b.CounterDetails),
                    Id = b.Id,
                    IsGeneral = b.IsJoinToGeneralCounter,
                    Year = b.Year

                }).ToList();
            return counterEditVMs;
        }
        public static List<CounterEditDTO> Map(IList<CounterEditVM> counterEditVMs)
        {
            if (counterEditVMs == null || !counterEditVMs.Any())
            {
                return null;
            }
            List<CounterEditDTO> counterEditDTOs = counterEditVMs
                .Select(b => new CounterEditDTO
                {
                    CounterDetails = CounterMapper.Map(b.CounterDetails),
                    Id = b.Id,
                    IsJoinToGeneralCounter = b.IsGeneral,
                    Year = b.Year

                }).ToList();
            return counterEditDTOs;
        }
        public static List<CounterAddDTO> Map(IList<CounterAddVM> counterAddVMs)
        {
            if (counterAddVMs == null || !counterAddVMs.Any())
            {
                return null;
            }
            List<CounterAddDTO> counterAddDTOs = counterAddVMs
                .Select(b => new CounterAddDTO
                {
                    CounterDetails = CounterMapper.Map(b.CounterDetails),
                    Id = b.Id,
                    IsJoinToGeneralCounter = b.IsGeneral

                }).ToList();
            return counterAddDTOs;
        }

        public static CounterVM Map(CounterAddVM counterAddVM)
        {
            if (counterAddVM != null)
            {
                return new CounterVM
                {
                    CounterDetails = counterAddVM.CounterDetails,
                    CounterId = counterAddVM.Id,
                    IsGeneral = counterAddVM.IsGeneral
                };
            }
            return null;
        }
    }

}