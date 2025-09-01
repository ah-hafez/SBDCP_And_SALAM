using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class UserClearanceMapper
    {
        public static UsersClearanceDTO Map(UsersClearance checkUsersClearance)
        {
            if (checkUsersClearance == null)
            {
                return null;
            }
            return new UsersClearanceDTO
            {
                InboundTransactionsCount = checkUsersClearance.InboundTransactionsCount,
                OutboundTransactionsCount = checkUsersClearance.OutboundTransactionsCount,
                SavedTransactionsCount = checkUsersClearance.SavedTransactionsCount,
                UserId = checkUsersClearance.UserId,
                UserName = checkUsersClearance.UserName
            };
        }
        public static List<UsersClearanceDTO> Map(List<UsersClearance> checkUsersClearances)
        {
            if (checkUsersClearances == null)
            {
                return null;
            }
            return checkUsersClearances.Select(uc => Map(uc)).ToList();
        }
    }
}