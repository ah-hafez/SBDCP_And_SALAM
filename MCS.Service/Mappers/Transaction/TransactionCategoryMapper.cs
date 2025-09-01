using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionCategoryMapper
    {
        public static TransactionCategories Map(IList<TransactionCategoryDTO> transactionCategoryDTOs)
        {
            if (transactionCategoryDTOs == null || !transactionCategoryDTOs.Any())
            {
                return TransactionCategories.None;
            }
            TransactionCategories sourceTransactionType = TransactionCategories.None;

            foreach (TransactionCategoryDTO transactionSourceDTO in transactionCategoryDTOs)
            {
                if (transactionSourceDTO.IsSelected)
                {
                    sourceTransactionType = sourceTransactionType ^ (TransactionCategories)transactionSourceDTO.Id;
                }
            }

            return sourceTransactionType;
        }

        public static TransactionCategories Map(Permission permission)
        {
            return TransactionCategories.None;
        }

        public static List<TransactionCategoryDTO> Map(TransactionCategories transactionCategories, string cultureName)
        {

            if (transactionCategories != TransactionCategories.None)
            {
                List<TransactionCategoryDTO> transactionSources = new List<TransactionCategoryDTO>();
                ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                foreach (TransactionCategories sourceTransactionType in Enum.GetValues(typeof(TransactionCategories)))
                {
                    if (sourceTransactionType == TransactionCategories.None)
                        continue;

                    TransactionCategoryDTO transactionSourceDTO = new TransactionCategoryDTO();

                    transactionSourceDTO.Text = lookupBL.GetLookupItem((int)EnumMapper.GetTransactionCategory(sourceTransactionType).LookupIdentity(LookupCategory.TransactionCategory,cultureName), cultureName).Text;
                    transactionSourceDTO.Id = (int)sourceTransactionType;

                    if (Convert.ToBoolean(transactionCategories & sourceTransactionType))
                    {
                        transactionSourceDTO.IsSelected = true;
                    }

                    transactionSources.Add(transactionSourceDTO);
                }

                return transactionSources;
            }
            return null;
        }
    }
}
