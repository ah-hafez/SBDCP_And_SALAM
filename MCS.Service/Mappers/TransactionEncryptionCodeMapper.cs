using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionEncryptionCodeMapper
    {


        public static TransactionEncryptionCodeDTO Map(TransactionEncryptionCode b)
        {
            if (b == null)
            {
                return new TransactionEncryptionCodeDTO();
            }

            TransactionEncryptionCodeDTO transactionEncryptionCodeDTO = new TransactionEncryptionCodeDTO()
            {
                Id = b.Id,
                TransactionId = b.TransactionId,
                EncryptionChannel = b.EncryptionChannel,
                OrgUnitId = b.OrgUnitId,
                OrgUnit = new OrgUnitDTO { Name = b.OrgUnit.LocalName, Id = b.OrgUnit.Id },
                UserId = b.UserId,
                User = new UserProfileDTO { LocalName = b.User.LocalName, Id = b.User.Id },
                Code = b.Code,
                CreatedBy = b.CreatedBy,
                CreatedOn = b.CreatedOn,
                ModefiedBy = b.ModefiedBy,
                ModefiedOn = b.ModefiedOn,
                CodeExpireDate = b.CodeExpireDate,
            };

            return transactionEncryptionCodeDTO;

        }


        public static TransactionEncryptionCode Map(TransactionEncryptionCodeDTO b)
        {
            if (b == null)
            {
                return new TransactionEncryptionCode();
            }

            TransactionEncryptionCode transactionEncryptionCode = new TransactionEncryptionCode()
            {
                Id = b.Id,
                TransactionId = b.TransactionId,
                Code = b.Code,
                EncryptionChannel = b.EncryptionChannel,
                OrgUnitId = b.OrgUnitId,
                UserId = b.UserId,
                CreatedBy = b.CreatedBy,
                CreatedOn = b.CreatedOn,
                ModefiedBy = b.ModefiedBy,
                ModefiedOn = b.ModefiedOn,
                CodeExpireDate = b.CodeExpireDate,
            };

            return transactionEncryptionCode;

        }


    }
}