using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionLightMapper
    {
        public static TransactionEditVM Map(TransactionEditDTO transactionEditDTO)
        {
            if (transactionEditDTO == null)
            {
                return new TransactionEditVM();
            }
            TransactionEditVM transactionEditVM = new TransactionEditVM()
            {
                DeliveryNumber = transactionEditDTO.DeliveryNumber,
                Id = transactionEditDTO.Id,
                TransactionCategory = transactionEditDTO.TransactionCategory
            };
            return transactionEditVM;
        }
        public static TransactionEditDTO Map(TransactionEditVM transactionEditVM)
        {
            if (transactionEditVM == null)
            {
                return new TransactionEditDTO();
            }
            TransactionEditDTO transactionEditDTO = new TransactionEditDTO()
            {
                DeliveryNumber = transactionEditVM.DeliveryNumber,
                Id = transactionEditVM.Id,
                TransactionCategory = transactionEditVM.TransactionCategory
            };
            return transactionEditDTO;
        }

        public static TransactionLightDTO Map(TransactionLightVM transactionLightVM)
        {
            if (transactionLightVM == null)
            {
                return new TransactionLightDTO();
            }
            TransactionLightDTO transactionLightDTO = new TransactionLightDTO()
            {
                Id = transactionLightVM.Id,
                TransactionCategory = transactionLightVM.TransactionCategory,
                Barcode = transactionLightVM.Barcode,
                Number = transactionLightVM.Number,
                UserId = transactionLightVM.UserId,
                EntityId = transactionLightVM.EntityId
            };
            return transactionLightDTO;
        }
    }
}