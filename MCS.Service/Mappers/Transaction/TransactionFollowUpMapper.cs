using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionFollowUpMapper
    {
        public static List<TransactionFollowUpDTO> Map(IList<TransactionFollowUp> oListTransactionFollowUp)
        {
            if (oListTransactionFollowUp == null || !oListTransactionFollowUp.Any())
            {
                return new List<TransactionFollowUpDTO>();
            }

            List<TransactionFollowUpDTO> oListTransactionFollowUpDTO = oListTransactionFollowUp
                .Select(fdomain => new TransactionFollowUpDTO()
                {
                    Id = fdomain.Id, 
                    DateTo = fdomain.DateTo,
                    DateToH = fdomain.DateToH,
                    TransactionId = fdomain.TransactionId,
                    CreatingUserId = fdomain.CreatingUserId,
                    CreatingUser = new UserProfileDTO { LocalName = fdomain.CreatingUser.LocalName },
                    CreatingEntityId = fdomain.CreatingEntityId,
                    CreatingEntity = new OrgUnitDTO { Name = fdomain.CreatingEntity.LocalName },
                    FollowUpEntityId = fdomain.FollowUpEntityId,
                    FollowUpEntity = new OrgUnitDTO { Name = fdomain.FollowUpEntity.LocalName },
                    FollowUpUserId = fdomain.FollowUpUserId,
                    FollowUpUser = fdomain.FollowUpUserId.HasValue ? new UserProfileDTO { LocalName = fdomain.FollowUpUser.LocalName } : null,
                    CreationDate = fdomain.CreationDate,
                    FollowUpExpireDate = fdomain.FollowUpExpireDate,
                    FollowUpExpireDateHj = fdomain.FollowUpExpireDateHj,
                    Notes = fdomain.Notes,
                    Active = fdomain.Active,
                    ProccessPeriod = fdomain.ProccessPeriod,
                    ProccessPeriodDate = fdomain.ProccessPeriodDate,
                    FollowUpProccessNote = fdomain.FollowUpProccessNote,
                    FollowUpCompletionDate = fdomain.FollowUpCompletionDate,
                    FollowUpCompletionDateHj = fdomain.FollowUpCompletionDateHj,
                    FollowUpReceiveDate = fdomain.FollowUpReceiveDate,
                    FollowUpReason = fdomain.FollowUpReason,
                    FollowUpTypeId = fdomain.FollowUpTypeId,
                    FollowUpStatusId = fdomain.FollowUpStatusId,
                    FollowUpMethodId = fdomain.FollowUpMethodId,
                    FollowUpPriortyId = fdomain.FollowUpPriortyId,
                    FollowUpProccessId = fdomain.FollowUpProccessId,
                    FollowUpProgressId = fdomain.FollowUpProgressId,
                    IsCopy = fdomain.IsCopy,
                    IsReminder = fdomain.IsReminder,
                    IsEscalated = fdomain.IsEscalated,
                    HasChild = fdomain.HasChild,
                    IsImportant = fdomain.IsImportant,
                    ParentId = fdomain.ParentId,
                }).ToList();

            return oListTransactionFollowUpDTO;
        }

        public static IList<TransactionFollowUp> Map(IList<TransactionFollowUpDTO> oListTransactionFollowUpDTO)
        {
            if (oListTransactionFollowUpDTO == null || !oListTransactionFollowUpDTO.Any())
            {
                return new List<TransactionFollowUp>();
            }

            List<TransactionFollowUp> list = oListTransactionFollowUpDTO
                .Select(fdto => new TransactionFollowUp()
                {
                    Id = fdto.Id,
                    DateTo = fdto.DateTo,
                    DateToH = fdto.DateToH,
                    TransactionId = fdto.TransactionId,
                    CreatingUserId = fdto.CreatingUserId,
                    CreatingUser = new UserProfile { LocalName = fdto.CreatingUser.LocalName },
                    CreatingEntityId = fdto.CreatingEntityId,
                    CreatingEntity = new OrgUnit { LocalName = fdto.CreatingEntity.Name },
                    FollowUpEntityId = fdto.FollowUpEntityId,
                    FollowUpEntity = new OrgUnit { LocalName = fdto.FollowUpEntity.Name },
                    FollowUpUserId = fdto.FollowUpUserId,
                    FollowUpUser = fdto.FollowUpUserId .HasValue ? new UserProfile { LocalName = fdto.FollowUpUser.LocalName } : null , 
                    CreationDate = fdto.CreationDate,
                    FollowUpExpireDate = fdto.FollowUpExpireDate,
                    FollowUpExpireDateHj = fdto.FollowUpExpireDateHj,
                    Notes = fdto.Notes,
                    Active = fdto.Active,
                    ProccessPeriod = fdto.ProccessPeriod,
                    ProccessPeriodDate = fdto.ProccessPeriodDate,
                    FollowUpProccessNote = fdto.FollowUpProccessNote,
                    FollowUpCompletionDate = fdto.FollowUpCompletionDate,
                    FollowUpCompletionDateHj = fdto.FollowUpCompletionDateHj,
                    FollowUpReceiveDate = fdto.FollowUpReceiveDate,
                    FollowUpReason = fdto.FollowUpReason,
                    FollowUpTypeId = fdto.FollowUpTypeId,
                    FollowUpStatusId = fdto.FollowUpStatusId,
                    FollowUpMethodId = fdto.FollowUpMethodId,
                    FollowUpPriortyId = fdto.FollowUpPriortyId,
                    FollowUpProccessId = fdto.FollowUpProccessId,
                    FollowUpProgressId = fdto.FollowUpProgressId,
                    IsCopy = fdto.IsCopy,
                    IsReminder = fdto.IsReminder,
                    IsEscalated = fdto.IsEscalated,
                    HasChild = fdto.HasChild,
                    IsImportant = fdto.IsImportant,
                    ParentId = fdto.ParentId,
                }).ToList();

            return list;
        }

        public static TransactionFollowUp Map(TransactionFollowUpDTO TransactionFollowUpDTO)
        {
            if (TransactionFollowUpDTO == null)
            {
                return new TransactionFollowUp();
            }

            TransactionFollowUp oTransactionFollowUp = new TransactionFollowUp()
            {
                Id = TransactionFollowUpDTO.Id,
                TransactionId = TransactionFollowUpDTO.TransactionId,
                DateTo = TransactionFollowUpDTO.DateTo,
                DateToH = TransactionFollowUpDTO.DateToH,
                IsDeleted = false,
                CreatingUserId = TransactionFollowUpDTO.CreatingUserId,
                //CreatingUser =  new UserProfile { LocalName = TransactionFollowUpDTO.CreatingUser.LocalName }, 
                CreatingEntityId = TransactionFollowUpDTO.CreatingEntityId,
                //CreatingEntity = new OrgUnit { LocalName  = TransactionFollowUpDTO.CreatingEntity.Name },
                FollowUpEntityId = TransactionFollowUpDTO.FollowUpEntityId,
                //  FollowUpEntity = new OrgUnit { LocalName = TransactionFollowUpDTO.CreatingEntity.Name },
                FollowUpUserId = TransactionFollowUpDTO.FollowUpUserId,
                // FollowUpUser = new UserProfile { LocalName = TransactionFollowUpDTO.FollowUpUser.LocalName },
                CreationDate = TransactionFollowUpDTO.CreationDate,
                FollowUpExpireDate = TransactionFollowUpDTO.FollowUpExpireDate,
                FollowUpExpireDateHj = TransactionFollowUpDTO.FollowUpExpireDateHj,
                Notes = TransactionFollowUpDTO.Notes,
                Active = TransactionFollowUpDTO.Active,
                ProccessPeriod = TransactionFollowUpDTO.ProccessPeriod,
                ProccessPeriodDate = TransactionFollowUpDTO.ProccessPeriodDate,
                FollowUpProccessNote = TransactionFollowUpDTO.FollowUpProccessNote,
                FollowUpCompletionDate = TransactionFollowUpDTO.FollowUpCompletionDate,
                FollowUpCompletionDateHj = TransactionFollowUpDTO.FollowUpCompletionDateHj,
                FollowUpReceiveDate = TransactionFollowUpDTO.FollowUpReceiveDate,
                FollowUpReason = TransactionFollowUpDTO.FollowUpReason,
                FollowUpTypeId = TransactionFollowUpDTO.FollowUpTypeId,
                FollowUpStatusId = TransactionFollowUpDTO.FollowUpStatusId,
                FollowUpMethodId = TransactionFollowUpDTO.FollowUpMethodId,
                FollowUpPriortyId = TransactionFollowUpDTO.FollowUpPriortyId,
                FollowUpProccessId = TransactionFollowUpDTO.FollowUpProccessId,
                FollowUpProgressId = TransactionFollowUpDTO.FollowUpProgressId,
                IsCopy = TransactionFollowUpDTO.IsCopy,
                IsReminder = TransactionFollowUpDTO.IsReminder,
                IsEscalated = TransactionFollowUpDTO.IsEscalated,
                HasChild = TransactionFollowUpDTO.HasChild,
                IsImportant = TransactionFollowUpDTO.IsImportant,
                ParentId = TransactionFollowUpDTO.ParentId,

            };

            return oTransactionFollowUp;
        }
        public static TransactionFollowUp VipMap(PrivateFollowupDto TransactionFollowUpDTO)
        {
            if (TransactionFollowUpDTO == null)
            {
                return new TransactionFollowUp();
            }

            TransactionFollowUp oTransactionFollowUp = new TransactionFollowUp()
            {

                TransactionId = TransactionFollowUpDTO.TransactionId,
                DateTo = TransactionFollowUpDTO.DateTo,
                DateToH = TransactionFollowUpDTO.DateToH,
                IsDeleted = false,
                CreatingUserId = TransactionFollowUpDTO.CreatingUserId,
                CreatingEntityId = TransactionFollowUpDTO.CreatingEntityId,
                FollowUpEntityId = TransactionFollowUpDTO.FollowUpEntityId,
                FollowUpUserId = TransactionFollowUpDTO.UserId,
                CreationDate = TransactionFollowUpDTO.CreationDate,
                FollowUpExpireDate = TransactionFollowUpDTO.FollowUpExpireDate,
                FollowUpExpireDateHj = TransactionFollowUpDTO.FollowUpExpireDateHj,
                Active = TransactionFollowUpDTO.Active,
                ProccessPeriod = TransactionFollowUpDTO.PeriodId,
                FollowUpTypeId = TransactionFollowUpDTO.FollowUpTypeId,
                FollowUpStatusId = TransactionFollowUpDTO.FollowUpStatusId,
                IsCopy = false,
                IsReminder = false,
                IsEscalated = false,
                HasChild = false,
                IsImportant = TransactionFollowUpDTO.IsImportant,


            };

            return oTransactionFollowUp;
        }

        public static TransactionFollowUp VipPublicMap(PublicFollowupDto TransactionFollowUpDTO)
        {
            if (TransactionFollowUpDTO == null)
            {
                return new TransactionFollowUp();
            }

            TransactionFollowUp oTransactionFollowUp = new TransactionFollowUp()
            {

                TransactionId = TransactionFollowUpDTO.TransactionId,
                FollowUpUserId = TransactionFollowUpDTO.FollowUpUserId,
                FollowUpEntityId = TransactionFollowUpDTO.FollowUpEntityId, 
                DateTo = TransactionFollowUpDTO.DateTo,
                DateToH = TransactionFollowUpDTO.DateToH,
                IsDeleted = false,
                CreatingUserId = TransactionFollowUpDTO.CreatingUserId,
                CreatingEntityId = TransactionFollowUpDTO.CreatingEntityId, 
                CreationDate = TransactionFollowUpDTO.CreationDate,
                FollowUpExpireDate = TransactionFollowUpDTO.FollowUpExpireDate,
                FollowUpExpireDateHj = TransactionFollowUpDTO.FollowUpExpireDateHj,
                Active = TransactionFollowUpDTO.Active,
                ProccessPeriod = TransactionFollowUpDTO.PeriodId,
                FollowUpTypeId = TransactionFollowUpDTO.FollowUpTypeId,
                FollowUpStatusId = TransactionFollowUpDTO.FollowUpStatusId,
                IsCopy = false,
                IsReminder = false,
                IsEscalated = false,
                HasChild = false,
                IsImportant = TransactionFollowUpDTO.IsImportant


            };

            return oTransactionFollowUp;
        }

        public static TransactionFollowUp VipPrivateMap(PrivateFollowupDto TransactionFollowUpDTO)
        {
            if (TransactionFollowUpDTO == null)
            {
                return new TransactionFollowUp();
            }

            TransactionFollowUp oTransactionFollowUp = new TransactionFollowUp()
            {

                TransactionId = TransactionFollowUpDTO.TransactionId,
                FollowUpUserId = TransactionFollowUpDTO.FollowUpUserId,
                FollowUpEntityId = TransactionFollowUpDTO.FollowUpEntityId, 
                DateTo = TransactionFollowUpDTO.DateTo,
                DateToH = TransactionFollowUpDTO.DateToH,
                IsDeleted = false,
                CreatingUserId = TransactionFollowUpDTO.CreatingUserId,
                CreatingEntityId = TransactionFollowUpDTO.CreatingEntityId, 
                CreationDate = TransactionFollowUpDTO.CreationDate,
                FollowUpExpireDate = TransactionFollowUpDTO.FollowUpExpireDate,
                FollowUpExpireDateHj = TransactionFollowUpDTO.FollowUpExpireDateHj,
                Active = TransactionFollowUpDTO.Active,
                ProccessPeriod = TransactionFollowUpDTO.PeriodId,
                FollowUpTypeId = TransactionFollowUpDTO.FollowUpTypeId,
                FollowUpStatusId = TransactionFollowUpDTO.FollowUpStatusId,
                IsCopy = false,
                IsReminder = false,
                IsEscalated = false,
                HasChild = false,
                IsImportant = TransactionFollowUpDTO.IsImportant,


            };

            return oTransactionFollowUp;
        }

        public static TransactionFollowUpDTO Map(TransactionFollowUp TransactionFollowUpDTO)
        {
            if (TransactionFollowUpDTO == null)
            {
                return new TransactionFollowUpDTO();
            }

            TransactionFollowUpDTO oTransactionFollowUp = new TransactionFollowUpDTO();

            oTransactionFollowUp.Id = TransactionFollowUpDTO.Id;
            oTransactionFollowUp.TransactionId = TransactionFollowUpDTO.TransactionId; 
            oTransactionFollowUp.DateTo = TransactionFollowUpDTO.DateTo;
            oTransactionFollowUp.DateToH = TransactionFollowUpDTO.DateToH;
            oTransactionFollowUp.IsDeleted = false;
            oTransactionFollowUp.CreatingUserId = TransactionFollowUpDTO.CreatingUserId;
            oTransactionFollowUp.CreatingUser = new UserProfileDTO { LocalName = TransactionFollowUpDTO.CreatingUser.LocalName };
            oTransactionFollowUp.CreatingEntityId = TransactionFollowUpDTO.CreatingEntityId;
            oTransactionFollowUp.CreatingEntity = new OrgUnitDTO { Name = TransactionFollowUpDTO.CreatingEntity.LocalName };
            oTransactionFollowUp.FollowUpEntityId = TransactionFollowUpDTO.FollowUpEntityId;
            oTransactionFollowUp.FollowUpEntity = new OrgUnitDTO { Name = TransactionFollowUpDTO.FollowUpEntity.LocalName };
            oTransactionFollowUp.FollowUpUserId = TransactionFollowUpDTO.FollowUpUserId;
            // oTransactionFollowUp.FollowUpUser = new UserProfileDTO { LocalName = TransactionFollowUpDTO.FollowUpUser.LocalName };
            oTransactionFollowUp.CreationDate = TransactionFollowUpDTO.CreationDate;
            oTransactionFollowUp.FollowUpExpireDate = TransactionFollowUpDTO.FollowUpExpireDate;
            oTransactionFollowUp.FollowUpExpireDateHj = TransactionFollowUpDTO.FollowUpExpireDateHj;
            oTransactionFollowUp.Notes = TransactionFollowUpDTO.Notes;
            oTransactionFollowUp.Active = TransactionFollowUpDTO.Active;
            oTransactionFollowUp.ProccessPeriod = TransactionFollowUpDTO.ProccessPeriod;
            oTransactionFollowUp.ProccessPeriodDate = TransactionFollowUpDTO.ProccessPeriodDate;
            oTransactionFollowUp.FollowUpProccessNote = TransactionFollowUpDTO.FollowUpProccessNote;
            oTransactionFollowUp.FollowUpCompletionDate = TransactionFollowUpDTO.FollowUpCompletionDate;
            oTransactionFollowUp.FollowUpCompletionDateHj = TransactionFollowUpDTO.FollowUpCompletionDateHj;
            oTransactionFollowUp.FollowUpReceiveDate = TransactionFollowUpDTO.FollowUpReceiveDate;
            oTransactionFollowUp.FollowUpReason = TransactionFollowUpDTO.FollowUpReason;
            oTransactionFollowUp.FollowUpTypeId = TransactionFollowUpDTO.FollowUpTypeId;
            oTransactionFollowUp.FollowUpStatusId = TransactionFollowUpDTO.FollowUpStatusId;
            oTransactionFollowUp.FollowUpMethodId = TransactionFollowUpDTO.FollowUpMethodId;
            oTransactionFollowUp.FollowUpPriortyId = TransactionFollowUpDTO.FollowUpPriortyId;
            oTransactionFollowUp.FollowUpProccessId = TransactionFollowUpDTO.FollowUpProccessId;
            oTransactionFollowUp.FollowUpProgressId = TransactionFollowUpDTO.FollowUpProgressId;
            oTransactionFollowUp.IsCopy = TransactionFollowUpDTO.IsCopy;
            oTransactionFollowUp.IsReminder = TransactionFollowUpDTO.IsReminder;
            oTransactionFollowUp.IsEscalated = TransactionFollowUpDTO.IsEscalated;
            oTransactionFollowUp.HasChild = TransactionFollowUpDTO.HasChild;
            oTransactionFollowUp.IsImportant = TransactionFollowUpDTO.IsImportant;
            oTransactionFollowUp.ParentId = TransactionFollowUpDTO.ParentId;




            return oTransactionFollowUp;
        }


        public static IList<FollowUpDetailsDTO> MapToFollowUpDetailsDTO(IList<FollowUpDetails> oFollowUpDetails)
        {
            if (oFollowUpDetails == null || !oFollowUpDetails.Any())
            {
                return new List<FollowUpDetailsDTO>();
            }

            List<FollowUpDetailsDTO> oListFollowUpDetailsDTO = oFollowUpDetails
                .Select(fdDomain => new FollowUpDetailsDTO()
                {
                    Id = fdDomain.Id,
                    TransactionFollowUpId = fdDomain.TransactionFollowUpId,
                    Notes = fdDomain.Notes,
                    CreatedOn = fdDomain.CreatedOn,
                    FollowUp = new TransactionFollowUpDTO { Id = fdDomain.TransactionFollowUpId, FollowUpUser = new UserProfileDTO { Id = fdDomain.TransactionFollowUp.FollowUpUser.Id, LocalName = fdDomain.TransactionFollowUp.FollowUpUser.LocalName } },
                }).ToList();

            return oListFollowUpDetailsDTO;
        }
    }
}