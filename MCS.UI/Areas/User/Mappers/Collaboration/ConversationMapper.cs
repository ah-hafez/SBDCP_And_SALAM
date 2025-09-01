using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Collaboration;

namespace MCS.UI.Areas.User.Mappers.Collaboration
{
    public static class ConversationMapper
    {
        public static List<ConversationVM> Map(IList<ConversationDTO> conversationDTOs)
        {
            if (conversationDTOs == null || !conversationDTOs.Any())
            {
                return new List<ConversationVM>();
            }
            List<ConversationVM> conversationVMs = conversationDTOs
                .Select(conversationDTO => new ConversationVM()
                { 
                    Id = conversationDTO.Id,
                    Date = conversationDTO.Date,
                    DateH = conversationDTO.DateH,
                    ReceiverName = conversationDTO.ReceiverName,
                    SenderId = conversationDTO.SenderId,
                    SenderName = conversationDTO.SenderName,
                    Text = conversationDTO.Text
                }).ToList();

            return conversationVMs;
        }
        public static List<ConversationDTO> Map(IList<ConversationVM> conversationVMs)
        {
            if (conversationVMs == null || !conversationVMs.Any())
            {
                return new List<ConversationDTO>();
            }
            List<ConversationDTO> conversationDTOs = conversationVMs
                .Select(conversationVM => new ConversationDTO()
                {
                    Id = conversationVM.Id,
                    Date = conversationVM.Date,
                    DateH = conversationVM.DateH,
                    ReceiverName = conversationVM.ReceiverName,
                    SenderId = conversationVM.SenderId,
                    SenderName = conversationVM.SenderName,
                    Text = conversationVM.Text
                }).ToList();

            return conversationDTOs;
        }
    }
}