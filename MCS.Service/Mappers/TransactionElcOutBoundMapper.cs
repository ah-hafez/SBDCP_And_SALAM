using System;
using System.Collections.Generic;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionElcOutBoundMapper
    {
        public static TransactionElcOutBoundDTO Map(TransactionElcOutBound transactionElcOutBound)
        {
            if (transactionElcOutBound == null)
            {
                return null;
            }
            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO()
            {
                
                TransactionId = transactionElcOutBound.TransactionId,
                EntityId =transactionElcOutBound.EntityId,
                UserId = transactionElcOutBound.UserId,
                Ishidden = transactionElcOutBound.Ishidden,
                CreatedBy = transactionElcOutBound.CreatedBy.HasValue ? transactionElcOutBound.CreatedBy.Value : 0,
                CreatedOn = transactionElcOutBound.CreatedOn, 

            }; 
            return transactionElcOutBoundDTO;
        }

        public static TransactionElcOutBound Map(TransactionElcOutBoundDTO transactionElcOutBoundDTO)
        {
            if (transactionElcOutBoundDTO == null)
            {
                return null;
            }
            TransactionElcOutBound transactionElcOutBound = new TransactionElcOutBound()
            {

               
                TransactionId = transactionElcOutBoundDTO.TransactionId,
                EntityId = transactionElcOutBoundDTO.EntityId,
                UserId = transactionElcOutBoundDTO.UserId,
                Ishidden = transactionElcOutBoundDTO.Ishidden,
                CreatedBy = transactionElcOutBoundDTO.CreatedBy,
                CreatedOn = transactionElcOutBoundDTO.CreatedOn,
                  
            };
             

            return transactionElcOutBound;
        }

    }
}