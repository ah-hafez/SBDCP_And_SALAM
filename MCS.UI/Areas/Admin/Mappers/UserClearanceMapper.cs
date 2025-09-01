using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Actions;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class UserClearanceMapper
    {
        public static UsersClearanceVM Map(UsersClearanceDTO usersClearanceDTO)
        {
            if (usersClearanceDTO == null)
            {
                return null;
            }
            return new UsersClearanceVM
            {
                InboundTransactionsCount = usersClearanceDTO.InboundTransactionsCount,
                OutboundTransactionsCount = usersClearanceDTO.OutboundTransactionsCount,
                SavedTransactionsCount = usersClearanceDTO.SavedTransactionsCount,
                UserId = usersClearanceDTO.UserId,
                UserName = usersClearanceDTO.UserName
            };
        }
        public static List<UsersClearanceVM> Map(List<UsersClearanceDTO> usersClearanceDTOs)
        {
            if (usersClearanceDTOs == null)
            {
                return null;
            }
            return usersClearanceDTOs.Select(uc => Map(uc)).ToList();
        }
    }
}