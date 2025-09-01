using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers
{
    public static class TransactionAddressMapper
    {
        public static TransactionAddressVM Map(TransactionAddressDTO transactionAddressDTO)
        {
            if (transactionAddressDTO == null)
            {
                return new TransactionAddressVM();
            }

            TransactionAddressVM transactionAddressVM = new TransactionAddressVM
            {
                DirectedTo = transactionAddressDTO.DirectedTo,
                DirectedToOrgUnit = transactionAddressDTO.DirectedToOrgUnit,
                DocumentType = transactionAddressDTO.DocumentType,
                TransactionDate = transactionAddressDTO.TransactionDate,
                Transactionnumber = transactionAddressDTO.Transactionnumber,
                ShipmentNumber = transactionAddressDTO.ShipmentNumber
            };

            return transactionAddressVM;
        }
    }
}