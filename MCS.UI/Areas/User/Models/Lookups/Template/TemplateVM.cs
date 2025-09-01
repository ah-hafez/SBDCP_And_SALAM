using MCS.UI.Areas.User.Models.Shared;
using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class TemplateVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public string LocalName { get; set; }
        public DocumentVM FormContentVM { get; set; }
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
        public int StatusId { get; set; }
        public LookupVM Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
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