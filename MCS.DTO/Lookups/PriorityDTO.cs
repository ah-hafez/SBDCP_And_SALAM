using System.Collections.Generic;

namespace MCS.DTO
{
    public class PriorityDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public string LocalName { get; set; }
        public bool HasDate { get; set; }
        public int LateForEntity { get; set; }
        public int LateForUser { get; set; }
        public bool HasPriorityExceptions { get; set; }
        public int Sort { get; set; }
        public int ProcessPeriod { get; set; }
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
        public string TransactionSource
        {
            get
            {
                List<string> transactionCategories = new List<string>();

                if (TransactionCategories != null)
                {
                    foreach (TransactionCategoryDTO transactionCategoryDTO in TransactionCategories)
                    {
                        if (transactionCategoryDTO.IsSelected)
                        {
                            transactionCategories.Add(transactionCategoryDTO.Text);
                        }
                    }
                }

                return string.Join(" / ", transactionCategories); 
            }
        }
    }
}
