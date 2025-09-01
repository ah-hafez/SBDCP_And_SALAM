using System.Collections.Generic;

namespace MCS.DTO
{
    public class ConfidentialityAcknowledgmentsDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public string LocalName { get; set; }
        public bool IsMandatary { get; set; }
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
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
