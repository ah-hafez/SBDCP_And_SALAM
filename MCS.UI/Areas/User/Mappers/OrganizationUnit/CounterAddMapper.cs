using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class CounterAddMapper
    {
        public static List<CounterAddVM> Map(IList<CounterAddDTO> counterAddDTOs)
        {
            if (counterAddDTOs == null || !counterAddDTOs.Any())
            {
                return new List<CounterAddVM>();
            }
            List<CounterAddVM> counterAddVMs = counterAddDTOs
                .Select(counterAddDTO => new CounterAddVM()
                {
                    CounterDetails = CounterDetailMapper.Map(counterAddDTO.CounterDetails),
                    Id = counterAddDTO.Id,
                    IsJoinToGeneralCounter = counterAddDTO.IsJoinToGeneralCounter
                }).ToList();

            return counterAddVMs;
        }
        public static List<CounterAddDTO> Map(IList<CounterAddVM> counterAddVMs)
        {
            if (counterAddVMs == null || !counterAddVMs.Any())
            {
                return new List<CounterAddDTO>();
            }
            List<CounterAddDTO> counterAddDTOs = counterAddVMs
                .Select(counterAddVM => new CounterAddDTO()
                {
                    CounterDetails = CounterDetailMapper.Map(counterAddVM.CounterDetails),
                    Id = counterAddVM.Id,
                    IsJoinToGeneralCounter = counterAddVM.IsJoinToGeneralCounter
                }).ToList();

            return counterAddDTOs;
        }
    }
}