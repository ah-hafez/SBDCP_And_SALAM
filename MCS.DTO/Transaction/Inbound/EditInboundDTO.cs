using System;
using System.Collections.Generic;

namespace MCS.DTO
{

    public class EditInboundDTO : TransactionDTO
    {
        public EditInboundBasicInfoDTO InboundBasicInfoEdit { get; set; }
        
        public override Common.TransactionCategory TransactionCategory
        {
            get { return Common.TransactionCategory.Inbound; }
        }

        public List<TransactionCopyDTO> Copies { get; set; }

        public IList<TransactionFollowUpDTO> FollowUps { get; set; }

        public int ModifiedByUserId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
    }
}
