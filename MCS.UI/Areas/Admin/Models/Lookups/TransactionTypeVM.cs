using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class TransactionTypeVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public string LocalName { get; set; }
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
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