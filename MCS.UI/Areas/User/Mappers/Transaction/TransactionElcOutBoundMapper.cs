using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionElcOutBoundMapper
    {
        public static TransactionElcOutBoundDTO Map(TransactionElcOutBoundVm transactionElcOutBoundVm)
        {
            if (transactionElcOutBoundVm == null)
            {
                return null;
            }
            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO()
            {
                Id = transactionElcOutBoundVm.Id,
                TransactionId = transactionElcOutBoundVm.TransactionId,
                EntityId = transactionElcOutBoundVm.EntityId,
                //Entity = OrgUnitMapper.Map(transactionElcOutBoundVm.Entity),
                UserId = transactionElcOutBoundVm.UserId,
               // User = transactionElcOutBoundVm.MapUserProfile(transactionElcOutBoundVm.User),
                Ishidden = transactionElcOutBoundVm.Ishidden,
                CreatedBy = transactionElcOutBoundVm.CreatedBy,
                CreatedOn = transactionElcOutBoundVm.CreatedOn,

            };
            return transactionElcOutBoundDTO;
        }

        public static TransactionElcOutBoundVm Map(TransactionElcOutBoundDTO transactionElcOutBoundDTO)
        {
            if (transactionElcOutBoundDTO == null)
            {
                return null;
            }
            TransactionElcOutBoundVm transactionElcOutBoundVm = new TransactionElcOutBoundVm()
            {

                Id = transactionElcOutBoundDTO.Id,
                TransactionId = transactionElcOutBoundDTO.TransactionId,
                EntityId = transactionElcOutBoundDTO.EntityId,
                EntityName = transactionElcOutBoundDTO.Entity.Name != null ? transactionElcOutBoundDTO.Entity.Name  : "" ,
                UserId = transactionElcOutBoundDTO.UserId,
                UserName = transactionElcOutBoundDTO.User.LocalName != null ? transactionElcOutBoundDTO.User.LocalName  : "",
                Ishidden = transactionElcOutBoundDTO.Ishidden,
                CreatedBy = transactionElcOutBoundDTO.CreatedBy,
                CreatedOn = transactionElcOutBoundDTO.CreatedOn,
            }; 
            return transactionElcOutBoundVm;
        }

    }
}