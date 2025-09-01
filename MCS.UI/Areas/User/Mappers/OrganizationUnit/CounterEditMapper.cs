using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class CounterEditVMCounterEditMapper
    {
        public static List<CounterEditVM> Map(IList<CounterEditDTO> counterEditDTOs)
        {
            if (counterEditDTOs == null || !counterEditDTOs.Any())
            {
                return new List<CounterEditVM>();
            }
            List<CounterEditVM> counterEditVMs = counterEditDTOs
                .Select(counterEditDTO => new CounterEditVM()
                { 
                    CounterDetails = CounterDetailMapper.Map(counterEditDTO.CounterDetails),
                    Id = counterEditDTO.Id,
                    IsJoinToGeneralCounter = counterEditDTO.IsJoinToGeneralCounter,
                    Year = counterEditDTO.Year
                }).ToList();

            return counterEditVMs;
        }
        public static List<CounterEditDTO> Map(IList<CounterEditVM> counterEditVMs)
        {
            if (counterEditVMs == null || !counterEditVMs.Any())
            {
                return new List<CounterEditDTO>();
            }
            List<CounterEditDTO> counterEditDTOs = counterEditVMs
                .Select(counterEditVM => new CounterEditDTO()
                { 
                    CounterDetails = CounterDetailMapper.Map(counterEditVM.CounterDetails),
                    Id = counterEditVM.Id,
                    IsJoinToGeneralCounter = counterEditVM.IsJoinToGeneralCounter,
                    Year = counterEditVM.Year
                }).ToList();

            return counterEditDTOs;
        }
    }

}