using MCS.UI.Areas.User.Models.Shared;
using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class FormVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public string LocalName { get; set; }
        public DocumentVM FormContentVM { get; set; }
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
        public string TransactionSource
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

                return string.Join(" / ", TransactionCategories); ;
            }
        }
    }
}