using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class CounterMapper
    {
        public static CounterVM Map(CounterDTO counterDTO)
        {
            if (counterDTO != null)
            {
                CounterVM counterVM = new CounterVM()
                { 
                    CounterDetails = CounterDetailMapper.Map(counterDTO.CounterDetails),
                    Id = counterDTO.Id,
                    IsJoinToGeneralCounter = counterDTO.IsGeneral
                };
                return counterVM;
            }
            return new CounterVM();
        }
        public static CounterDTO Map(CounterVM counterVM)
        {
            if (counterVM != null)
            {
                CounterDTO counterDTO = new CounterDTO()
                { 
                    CounterDetails = CounterDetailMapper.Map(counterVM.CounterDetails),
                    Id = counterVM.Id,
                    IsGeneral = counterVM.IsJoinToGeneralCounter
                };
                return counterDTO;
            }
            return new CounterDTO();
        }
    }
}