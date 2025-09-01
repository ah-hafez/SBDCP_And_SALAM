using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class CounterDetailVM
    {
        public int Id { get; set; }        
        [CustomRequired("Admin.Counter.InitialValue")]
        public int InitialValue { get; set; }
        public int Count { get; set; }
        public int LastTransactionNumber { get; set; }
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