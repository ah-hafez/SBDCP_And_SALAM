using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class TransactionCategoryMapper
    {
        public static List<TransactionCategoryDTO> Map(IList<TransactionCategoryVM> transactionCategoryVMs)
        {
            if (transactionCategoryVMs == null || !transactionCategoryVMs.Any())
            {
                return new List<TransactionCategoryDTO>();
            }
            List<TransactionCategoryDTO> transactionSourceDTOs = transactionCategoryVMs
                .Select(transactionCategoryVM => new TransactionCategoryDTO
                { 
                    Id = transactionCategoryVM.Id,
                    IsSelected = transactionCategoryVM.IsSelected,
                    Text = transactionCategoryVM.Text
                }).ToList();
            return transactionSourceDTOs;
        }

        public static List<TransactionCategoryVM> Map(IList<TransactionCategoryDTO> transactionCategoryDTOs)
        {
            if (transactionCategoryDTOs == null || !transactionCategoryDTOs.Any())
            {
                return new List<TransactionCategoryVM>();
            }
            List<TransactionCategoryVM> transactionCategoryVMs = transactionCategoryDTOs
                .Select(b => new TransactionCategoryVM
                { 
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    Text = b.Text
                }).ToList();
            return transactionCategoryVMs;
        }
        public static TransactionCategoryDTO Map(TransactionCategoryVM transactionCategoryVM)
        {
            if (transactionCategoryVM != null)
            {
                return new TransactionCategoryDTO()
                {
                    Id = transactionCategoryVM.Id,
                    IsSelected = transactionCategoryVM.IsSelected,
                    Text = transactionCategoryVM.Text
                };
            }
            return new TransactionCategoryDTO();
        }
        public static TransactionCategoryVM Map(TransactionCategoryDTO transactionCategoryDTO)
        {
            if (transactionCategoryDTO != null)
            {
                return new TransactionCategoryVM()
                { 
                    Id = transactionCategoryDTO.Id,
                    IsSelected = transactionCategoryDTO.IsSelected,
                    Text = transactionCategoryDTO.Text
                };
            }
            return new TransactionCategoryVM();
        }
    }
}