using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class TransactionCategoryMapper
    {
        public static List<TransactionCategoryDTO> Map(IList<TransactionCategoryVM> transactionCategoryVMs)
        {
            if (transactionCategoryVMs == null || !transactionCategoryVMs.Any())
            { return null; }
            List<TransactionCategoryDTO> transactionSourceDTOs = transactionCategoryVMs
                .Select(b => new TransactionCategoryDTO
                { 
                    Id = b.Id, 
                    IsSelected = b.IsSelected,
                    Text = b.Text
                }).ToList();
            return transactionSourceDTOs;
        }
        public static List<TransactionCategoryVM> Map(IList<TransactionCategoryDTO> transactionCategoryDTOs)
        {
            if (transactionCategoryDTOs == null || !transactionCategoryDTOs.Any())
            { return null; }
            List<TransactionCategoryVM> transactionSourceVMs = transactionCategoryDTOs
                .Select(b => new TransactionCategoryVM
                {
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    Text = b.Text
                }).ToList();
            return transactionSourceVMs;
        }
        public static TransactionCategoryDTO Map(TransactionCategoryVM transactionCategoryVM)
        {
            if (transactionCategoryVM != null)
            {
                return new TransactionCategoryDTO
                {
                    Id = transactionCategoryVM.Id,
                    IsSelected = transactionCategoryVM.IsSelected,
                    Text = transactionCategoryVM.Text
                };
            }
            return null;
        }
        public static TransactionCategoryVM Map(TransactionCategoryDTO transactionCategoryDTO)
        {
            if (transactionCategoryDTO != null)
            {
                return new TransactionCategoryVM
                { 
                    Id = transactionCategoryDTO.Id,
                    IsSelected = transactionCategoryDTO.IsSelected,
                    Text = transactionCategoryDTO.Text
                };
            }
            return null;
        }
    }
}