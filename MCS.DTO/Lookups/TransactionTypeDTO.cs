using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionTypeDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public string LocalName { get; set; }
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
