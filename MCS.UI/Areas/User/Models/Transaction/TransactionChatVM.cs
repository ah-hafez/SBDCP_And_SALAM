using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionChatVM : EntityBase
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.Chat.Users")]
        public string ChatUsers { get; set; }

        [CustomDisplayName("User.Transaction.Chat.DatetimeHJ")]
        public string DateTimeHJ { get; set; }

        public AjaxGrid<TransactionChatVM> TransactionChats { get; set; } = (AjaxGrid<TransactionChatVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionChatVM>(), 1, 0, false);
    }
}