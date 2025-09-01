using System;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.UserManagement;

namespace MCS.UI.Areas.User.Models.Assignment
{
    public class UserTransactionsTrayVM
    {
        public int Id { get; set; }
        public string DateH { get; set; }
        public DateTime Date { get; set; }
        public long Number { get; set; }
        public string DocumentNumber { get; set; }
        public int ConfedentialityId { get; set; }
        public int TransactionCategoryId { get; set; }
        public PriorityVM PriorityLevel { get; set; }
        public int StatusId { get; set; }
        public UserProfileVM ToUser { get; set; }
        public UserProfileVM FromUser { get; set; }
        public OrgUnitVM FromEntity { get; set; }
        public OrgUnitVM ToEntity { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public bool Islate { get; set; }
    }
}