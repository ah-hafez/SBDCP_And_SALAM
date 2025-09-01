using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class FollowUpLookUpsVM
    {

        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public bool IsInternal { get; set; }
        public string LocalName { get; set; }
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public LookupVM Status { get; set; }
        public string TransactionCategory
        {
            get
            {
                List<string> transactionCategories = new List<string>();

                if (TransactionCategories != null)
                {
                    foreach (TransactionCategoryVM transactionCategoryVM in TransactionCategories)
                    {
                        if (transactionCategoryVM.IsSelected)
                        {
                            transactionCategories.Add(transactionCategoryVM.Text);
                        }
                    }
                }

                return string.Join(" / ", transactionCategories); ;
            }
        }
    }
}