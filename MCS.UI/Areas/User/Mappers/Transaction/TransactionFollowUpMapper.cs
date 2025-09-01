using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Localization;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionFollowUpMapper
    {
        public static List<TransactionFollowUpVM> Map(IList<TransactionFollowUpDTO> oListTransactionFollowUpDTO)
        {
            if (oListTransactionFollowUpDTO == null || !oListTransactionFollowUpDTO.Any())
            {
                return new List<TransactionFollowUpVM>();
            }

            List<TransactionFollowUpVM> oListTransactionFollowUpVM = oListTransactionFollowUpDTO
                .Select(fdto => new TransactionFollowUpVM()
                {
                    Id = fdto.Id, 
                    DateTo = fdto.DateTo,
                    DateToH = fdto.DateToH, 
                    IsDeleted = fdto.IsDeleted,
                    TransactionId = fdto.TransactionId,
                    CreatingUserId = fdto.CreatingUserId,
                    CreatingUserName = fdto.CreatingUser.LocalName,
                    CreatingEntityId = fdto.CreatingEntityId,
                    CreatingEntityName = fdto.CreatingEntity.Name + '/' + fdto.CreatingUser.LocalName,
                    FollowUpEntityId = fdto.FollowUpEntityId,
                    FollowUpEntityName = fdto.FollowUpEntity.Name,
                    FollowUpUserId = fdto.FollowUpUserId,
                    FollowUpUserName = fdto.FollowUpUser != null ? fdto.FollowUpUser.LocalName : "---",
                    EmployeeEntityName = fdto.FollowUpEntity.Name != null ? fdto.FollowUpEntity.Name : "---",
                    CreationDate = fdto.CreationDate,
                    CreationDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(fdto.CreationDate),
                    FollowUpExpireDate = fdto.FollowUpExpireDate,
                    FollowUpExpireDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(fdto.FollowUpExpireDate),
                    Notes = fdto.Notes,
                    Active = fdto.Active,
                    ProccessPeriod = fdto.ProccessPeriod,
                    ProccessPeriodDate = fdto.ProccessPeriodDate,
                    FollowUpProccessNote = fdto.FollowUpProccessNote,
                    FollowUpCompletionDate = fdto.FollowUpCompletionDate,
                    FollowUpReceiveDate = fdto.FollowUpReceiveDate,
                    FollowUpReceiveDateHj = fdto.FollowUpReceiveDate.HasValue ? DateTimeUtility.ConvertToUmAlQuraCalendar(fdto.FollowUpReceiveDate.Value) : "",
                    FollowUpReason = fdto.FollowUpReason,
                    FollowUpTypeId = fdto.FollowUpTypeId,
                    FollowUpType = MapFollowUpTypes(fdto.FollowUpTypeId),
                    FollowUpStatusId = fdto.FollowUpStatusId,
                    FollowUpStatus = MapFollowUpStatus(fdto.FollowUpStatusId, fdto.FollowUpExpireDate),
                    FollowUpMethodId = fdto.FollowUpMethodId,
                    FollowUpPriortyId = fdto.FollowUpPriortyId,
                    FollowUpProccessId = fdto.FollowUpProccessId,
                    FollowUpSourceId = fdto.FollowUpSourceId,
                    FollowUpProgressId = fdto.FollowUpProgressId,
                    IsCopy = fdto.IsCopy,
                    IsReminder = fdto.IsReminder,
                    IsEscalated = fdto.IsEscalated,
                    IsImportant = fdto.IsImportant,
                    HasChild = fdto.HasChild,
                    ParentId = fdto.ParentId
                }).ToList();


            TransactionFollowUpVM oTransactionFollowUpVM = new TransactionFollowUpVM();
            oTransactionFollowUpVM.FollowUps = oListTransactionFollowUpVM;

            return oListTransactionFollowUpVM;
        }

        private static string MapFollowUpTypes(int FollowUpTypeId)
        {
            string FollowUpType = string.Empty;
            switch (FollowUpTypeId)
            {
                case 1:
                    FollowUpType = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.FollowUp.PrivetFollowUp");
                    break;
                case 2:
                    FollowUpType = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.FollowUp.PublicFollowUp");
                    break;
                case 3:
                    FollowUpType = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.FollowUp.SecondaryFollowUp");
                    break;
                default:
                    FollowUpType = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.FollowUp.PublicFollowUp");
                    break;
                    return FollowUpType;
            }

            return FollowUpType;
        }

        private static string MapFollowUpStatus(int StatusId, DateTime FollowUpExpireDate)
        {


            if (FollowUpExpireDate < DateTime.Now)
                return ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Delayed");

            string FollowUpStatus = string.Empty;
            switch (StatusId)
            {
                case 1:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.New");
                    break;
                case 2:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.UnderProcessing");
                    break;
                case 3:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Completed");
                    break;
                case 4:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Delayed");

                    break;
                case 5:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Cancled");
                    break;
                case 6:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.UnderFollowupSecondLevel");
                    break;
                case 10:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Completed");
                    break;
                default:
                    FollowUpStatus = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.UnderProcessing");
                    break;
                    return FollowUpStatus;
            }

            return FollowUpStatus;


        }
        public static TransactionFollowUpVM Map(TransactionFollowUpDTO oListTransactionFollowUpDTO)
        {
            if (oListTransactionFollowUpDTO == null)
            {
                return new TransactionFollowUpVM();
            }

            TransactionFollowUpVM oListTransactionFollowUpVM = new TransactionFollowUpVM();

            oListTransactionFollowUpVM.Id = oListTransactionFollowUpDTO.Id; 
            oListTransactionFollowUpVM.DateTo = oListTransactionFollowUpDTO.DateTo;
            oListTransactionFollowUpVM.DateToH = oListTransactionFollowUpDTO.DateToH; 
            oListTransactionFollowUpVM.IsDeleted = oListTransactionFollowUpDTO.IsDeleted;
            oListTransactionFollowUpVM.TransactionId = oListTransactionFollowUpDTO.TransactionId;
            oListTransactionFollowUpVM.CreatingUserId = oListTransactionFollowUpDTO.CreatingUserId;
            oListTransactionFollowUpVM.CreatingEntityId = oListTransactionFollowUpDTO.CreatingEntityId;
            oListTransactionFollowUpVM.FollowUpEntityId = oListTransactionFollowUpDTO.FollowUpEntityId;
            oListTransactionFollowUpVM.FollowUpEntityName = oListTransactionFollowUpDTO.FollowUpEntity.Name;
            oListTransactionFollowUpVM.FollowUpUserId = oListTransactionFollowUpDTO.FollowUpUserId.HasValue ? oListTransactionFollowUpDTO.FollowUpUserId.Value : 0;
            oListTransactionFollowUpVM.FollowUpUserName = oListTransactionFollowUpDTO.FollowUpUser.LocalName;
            oListTransactionFollowUpVM.CreationDate = oListTransactionFollowUpDTO.CreationDate;
            oListTransactionFollowUpVM.FollowUpExpireDate = oListTransactionFollowUpDTO.FollowUpExpireDate;
            oListTransactionFollowUpVM.Notes = oListTransactionFollowUpDTO.Notes;
            oListTransactionFollowUpVM.Active = oListTransactionFollowUpDTO.Active;
            oListTransactionFollowUpVM.ProccessPeriod = oListTransactionFollowUpDTO.ProccessPeriod;
            oListTransactionFollowUpVM.ProccessPeriodDate = oListTransactionFollowUpDTO.ProccessPeriodDate;
            oListTransactionFollowUpVM.FollowUpProccessNote = oListTransactionFollowUpDTO.FollowUpProccessNote;
            oListTransactionFollowUpVM.FollowUpCompletionDate = oListTransactionFollowUpDTO.FollowUpCompletionDate;
            oListTransactionFollowUpVM.FollowUpReceiveDate = oListTransactionFollowUpDTO.FollowUpReceiveDate;
            oListTransactionFollowUpVM.FollowUpReason = oListTransactionFollowUpDTO.FollowUpReason;
            oListTransactionFollowUpVM.FollowUpTypeId = oListTransactionFollowUpDTO.FollowUpTypeId;
            oListTransactionFollowUpVM.FollowUpStatusId = oListTransactionFollowUpDTO.FollowUpStatusId;
            oListTransactionFollowUpVM.FollowUpMethodId = oListTransactionFollowUpDTO.FollowUpMethodId;
            oListTransactionFollowUpVM.FollowUpPriortyId = oListTransactionFollowUpDTO.FollowUpPriortyId;
            oListTransactionFollowUpVM.FollowUpProccessId = oListTransactionFollowUpDTO.FollowUpProccessId;
            oListTransactionFollowUpVM.FollowUpSourceId = oListTransactionFollowUpDTO.FollowUpSourceId;
            oListTransactionFollowUpVM.FollowUpProgressId = oListTransactionFollowUpDTO.FollowUpProgressId;
            oListTransactionFollowUpVM.IsCopy = oListTransactionFollowUpDTO.IsCopy;
            oListTransactionFollowUpVM.IsReminder = oListTransactionFollowUpDTO.IsReminder;
            oListTransactionFollowUpVM.IsEscalated = oListTransactionFollowUpDTO.IsEscalated;
            oListTransactionFollowUpVM.IsImportant = oListTransactionFollowUpDTO.IsImportant;
            oListTransactionFollowUpVM.HasChild = oListTransactionFollowUpDTO.HasChild;
            oListTransactionFollowUpVM.ParentId = oListTransactionFollowUpDTO.ParentId;


            return oListTransactionFollowUpVM;
        }
        public static List<TransactionFollowUpDTO> Map(IList<TransactionFollowUpVM> oListTransactionFollowUpVM)
        {
            if (oListTransactionFollowUpVM == null || !oListTransactionFollowUpVM.Any())
            {
                return new List<TransactionFollowUpDTO>();
            }

            List<TransactionFollowUpDTO> oListTransactionFollowUpDTO = new List<TransactionFollowUpDTO>();
            foreach (var item in oListTransactionFollowUpVM)
            {
                oListTransactionFollowUpDTO.Add(Map(item));
            }

            return oListTransactionFollowUpDTO;
        }

        public static TransactionFollowUpDTO Map(TransactionFollowUpVM oTransactionFollowUpVM)
        {
            if (oTransactionFollowUpVM == null)
            {
                return new TransactionFollowUpDTO();
            }

            TransactionFollowUpDTO oTransactionFollowUpDTO = new TransactionFollowUpDTO
            {
                TransactionId = oTransactionFollowUpVM.TransactionId, 
                Id = oTransactionFollowUpVM.Id, 
                DateTo = oTransactionFollowUpVM.DateTo,
                DateToH = oTransactionFollowUpVM.DateToH,
                IsDeleted = oTransactionFollowUpVM.IsDeleted,
                CreatedBy = SessionInfo.CurrentUser.Id,
                CreatingUserId = oTransactionFollowUpVM.CreatingUserId,
                CreatingEntityId = oTransactionFollowUpVM.CreatingEntityId,
                FollowUpEntityId = oTransactionFollowUpVM.FollowUpEntityId,
                FollowUpUserId = oTransactionFollowUpVM.FollowUpUserId,
                CreationDate = oTransactionFollowUpVM.CreationDate,
                FollowUpExpireDate = oTransactionFollowUpVM.FollowUpExpireDate,
                Notes = oTransactionFollowUpVM.Notes,
                Active = oTransactionFollowUpVM.Active,
                ProccessPeriod = oTransactionFollowUpVM.ProccessPeriod,
                ProccessPeriodDate = oTransactionFollowUpVM.ProccessPeriodDate,
                FollowUpProccessNote = oTransactionFollowUpVM.FollowUpProccessNote,
                FollowUpCompletionDate = oTransactionFollowUpVM.FollowUpCompletionDate,
                FollowUpReceiveDate = oTransactionFollowUpVM.FollowUpReceiveDate,
                FollowUpReason = oTransactionFollowUpVM.FollowUpReason,
                FollowUpTypeId = oTransactionFollowUpVM.FollowUpTypeId,
                FollowUpStatusId = oTransactionFollowUpVM.FollowUpStatusId,
                FollowUpMethodId = oTransactionFollowUpVM.FollowUpMethodId,
                FollowUpPriortyId = oTransactionFollowUpVM.FollowUpPriortyId,
                FollowUpProccessId = oTransactionFollowUpVM.FollowUpProccessId,
                FollowUpSourceId = oTransactionFollowUpVM.FollowUpSourceId,
                FollowUpProgressId = oTransactionFollowUpVM.FollowUpProgressId,
                IsCopy = oTransactionFollowUpVM.IsCopy,
                IsReminder = oTransactionFollowUpVM.IsReminder,
                IsEscalated = oTransactionFollowUpVM.IsEscalated,
                IsImportant = oTransactionFollowUpVM.IsImportant,
                HasChild = oTransactionFollowUpVM.HasChild,
                ParentId = oTransactionFollowUpVM.ParentId,

            };


            return oTransactionFollowUpDTO;
        }
        public static TransactionFollowUpDTO Map(VIPTransactionFollowUpVM oTransactionFollowUpVM)
        {
            if (oTransactionFollowUpVM == null)
            {
                return new TransactionFollowUpDTO();
            }

            TransactionFollowUpDTO oTransactionFollowUpDTO = new TransactionFollowUpDTO
            {
                TransactionId = oTransactionFollowUpVM.TransactionId, 
                Id = oTransactionFollowUpVM.Id, 
                DateTo = oTransactionFollowUpVM.DateTo,
                DateToH = oTransactionFollowUpVM.DateToH,
                IsDeleted = oTransactionFollowUpVM.IsDeleted,
                CreatedBy = SessionInfo.CurrentUser.Id,
                CreatingUserId = oTransactionFollowUpVM.CreatingUserId,
                CreatingEntityId = oTransactionFollowUpVM.CreatingEntityId,
                FollowUpEntityId = oTransactionFollowUpVM.FollowUpEntityId,
                FollowUpUserId = oTransactionFollowUpVM.FollowUpUserId,
                CreationDate = oTransactionFollowUpVM.CreationDate,
                FollowUpExpireDate = oTransactionFollowUpVM.FollowUpExpireDate,
                Notes = oTransactionFollowUpVM.Notes,
                Active = oTransactionFollowUpVM.Active,
                ProccessPeriod = oTransactionFollowUpVM.ProccessPeriod??0,
                ProccessPeriodDate = oTransactionFollowUpVM.ProccessPeriodDate,
                FollowUpProccessNote = oTransactionFollowUpVM.FollowUpProccessNote,
                FollowUpCompletionDate = oTransactionFollowUpVM.FollowUpCompletionDate,
                FollowUpReceiveDate = oTransactionFollowUpVM.FollowUpReceiveDate,
                FollowUpReason = oTransactionFollowUpVM.FollowUpReason,
                FollowUpTypeId = oTransactionFollowUpVM.FollowUpTypeId,
                FollowUpStatusId = oTransactionFollowUpVM.FollowUpStatusId,
                FollowUpMethodId = oTransactionFollowUpVM.FollowUpMethodId,
                FollowUpPriortyId = oTransactionFollowUpVM.FollowUpPriortyId,
                FollowUpProccessId = oTransactionFollowUpVM.FollowUpProccessId??0,
                FollowUpSourceId = oTransactionFollowUpVM.FollowUpSourceId,
                FollowUpProgressId = oTransactionFollowUpVM.FollowUpProgressId,
                IsCopy = oTransactionFollowUpVM.IsCopy,
                IsReminder = oTransactionFollowUpVM.IsReminder,
                IsEscalated = oTransactionFollowUpVM.IsEscalated,
                IsImportant = oTransactionFollowUpVM.IsImportant,
                HasChild = oTransactionFollowUpVM.HasChild,
                ParentId = oTransactionFollowUpVM.ParentId,

            };


            return oTransactionFollowUpDTO;
        }


        public static List<FollowUpDetailsVM> MapToFollowUpDetails(List<FollowUpDetailsDTO> oListFollowUpDetailsDTO)
        {
            if (oListFollowUpDetailsDTO == null || !oListFollowUpDetailsDTO.Any())
            {
                return new List<FollowUpDetailsVM>();
            }

            List<FollowUpDetailsVM> oListFollowUpDetailsVM = oListFollowUpDetailsDTO
                .Select(fdto => new FollowUpDetailsVM()
                {
                    Id = fdto.Id,
                    CreatedOn = DateTimeUtility.ConvertToUmAlQuraCalendar(fdto.CreatedOn),
                    UserName = fdto.FollowUp.FollowUpUser.LocalName,
                    Notes = fdto.Notes
                }).ToList();

            return oListFollowUpDetailsVM;
        }




        public static FollowUpCertificateVM MapToFollowUpCertificate(TransactionFollowUpDTO oListTransactionFollowUpVM)
        {
            if (oListTransactionFollowUpVM == null)
            {
                return new FollowUpCertificateVM();
            }
            FollowUpCertificateVM oFollowUpCertificateVM = new FollowUpCertificateVM();

            oFollowUpCertificateVM.FollowUpId = oListTransactionFollowUpVM.Id;
            oFollowUpCertificateVM.TransactionId = oListTransactionFollowUpVM.TransactionId;
            oFollowUpCertificateVM.CreatingUserId = oListTransactionFollowUpVM.CreatingUserId;
            oFollowUpCertificateVM.CreatingUserName = oListTransactionFollowUpVM.CreatingUser.LocalName != null ? oListTransactionFollowUpVM.CreatingUser.LocalName : "---";
            oFollowUpCertificateVM.CreatingEntityId = oListTransactionFollowUpVM.CreatingEntityId;
            oFollowUpCertificateVM.CreatingEntityName = oListTransactionFollowUpVM.CreatingEntity.Name;
            oFollowUpCertificateVM.FollowUpUserId = oListTransactionFollowUpVM.FollowUpUserId.HasValue ? oListTransactionFollowUpVM.FollowUpUserId.Value : 0;
            //oFollowUpCertificateVM.FollowUpUserName = oListTransactionFollowUpVM.FollowUpUser.LocalName;
            oFollowUpCertificateVM.FollowUpEntityId = oListTransactionFollowUpVM.FollowUpEntityId;
            oFollowUpCertificateVM.FollowUpEntityName = oListTransactionFollowUpVM.FollowUpEntity.Name;
            oFollowUpCertificateVM.CreationDate = oListTransactionFollowUpVM.CreationDate;
            oFollowUpCertificateVM.CreationDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(oListTransactionFollowUpVM.CreationDate);
            oFollowUpCertificateVM.FollowUpExpireDate = oListTransactionFollowUpVM.FollowUpExpireDate;
            oFollowUpCertificateVM.FollowUpExpireDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(oListTransactionFollowUpVM.FollowUpExpireDate);
            oFollowUpCertificateVM.FollowUpReason = oListTransactionFollowUpVM.FollowUpReason;
            oFollowUpCertificateVM.FollowUpTypeId = oListTransactionFollowUpVM.FollowUpTypeId;
            oFollowUpCertificateVM.FollowUpType = MapFollowUpTypes(oListTransactionFollowUpVM.FollowUpTypeId);
            oFollowUpCertificateVM.FollowUpStatusId = oListTransactionFollowUpVM.FollowUpStatusId;
            //oFollowUpCertificateVM.FollowUpStatus = oListTransactionFollowUpVM.FollowUpStatus;
            oFollowUpCertificateVM.FollowUpMethodId = oListTransactionFollowUpVM.FollowUpMethodId;
            oFollowUpCertificateVM.FollowUpPriortyId = oListTransactionFollowUpVM.FollowUpPriortyId;
            oFollowUpCertificateVM.FollowUpProccessId = oListTransactionFollowUpVM.FollowUpProccessId;
            oFollowUpCertificateVM.FollowUpSourceId = oListTransactionFollowUpVM.FollowUpSourceId;
            oFollowUpCertificateVM.FollowUpProgressId = oListTransactionFollowUpVM.FollowUpProgressId;
            oFollowUpCertificateVM.FollowUpProccessNote = oListTransactionFollowUpVM.FollowUpProccessNote;
            oFollowUpCertificateVM.Notes = oListTransactionFollowUpVM.Notes;
            oFollowUpCertificateVM.Active = oListTransactionFollowUpVM.Active;
            oFollowUpCertificateVM.ProccessPeriod = oListTransactionFollowUpVM.ProccessPeriod;
            oFollowUpCertificateVM.ProccessPeriodDate = oListTransactionFollowUpVM.ProccessPeriodDate;
            oFollowUpCertificateVM.FollowUpCompletionDate = oListTransactionFollowUpVM.FollowUpCompletionDate;
            oFollowUpCertificateVM.FollowUpCompletionDateHj = oListTransactionFollowUpVM.FollowUpCompletionDate.HasValue ? DateTimeUtility.ConvertToUmAlQuraCalendar(oListTransactionFollowUpVM.FollowUpCompletionDate.Value) : null;
            oFollowUpCertificateVM.FollowUpReceiveDate = oListTransactionFollowUpVM.FollowUpReceiveDate;
            oFollowUpCertificateVM.FollowUpReceiveDateHj = oListTransactionFollowUpVM.FollowUpReceiveDate.HasValue ? DateTimeUtility.ConvertToUmAlQuraCalendar(oListTransactionFollowUpVM.FollowUpReceiveDate.Value) : null;
            oFollowUpCertificateVM.IsCopy = oListTransactionFollowUpVM.IsCopy;
            oFollowUpCertificateVM.IsReminder = oListTransactionFollowUpVM.IsReminder;
            oFollowUpCertificateVM.IsEscalated = oListTransactionFollowUpVM.IsEscalated;
            oFollowUpCertificateVM.IsImportant = oListTransactionFollowUpVM.IsImportant;
            oFollowUpCertificateVM.HasChild = oListTransactionFollowUpVM.HasChild;
            oFollowUpCertificateVM.ParentId = oListTransactionFollowUpVM.ParentId;
            return oFollowUpCertificateVM;
        }


        public static TransactionFollowUpDTO MapFollowUpCertificateToDTO(FollowUpCertificateVM oListTransactionFollowUpVM)
        {
            if (oListTransactionFollowUpVM == null)
            {
                return new TransactionFollowUpDTO();
            }
            TransactionFollowUpDTO oFollowUpCertificateVM = new TransactionFollowUpDTO();

            oFollowUpCertificateVM.Id = oListTransactionFollowUpVM.FollowUpId;
            oFollowUpCertificateVM.TransactionId = oListTransactionFollowUpVM.TransactionId;
            oFollowUpCertificateVM.CreatingUserId = oListTransactionFollowUpVM.CreatingUserId;
            oFollowUpCertificateVM.CreatingEntityId = oListTransactionFollowUpVM.CreatingEntityId;
            oFollowUpCertificateVM.FollowUpUserId = oListTransactionFollowUpVM.FollowUpUserId;
            oFollowUpCertificateVM.FollowUpEntityId = oListTransactionFollowUpVM.FollowUpEntityId;
            oFollowUpCertificateVM.CreationDate = oListTransactionFollowUpVM.CreationDate;
            oFollowUpCertificateVM.FollowUpExpireDate = oListTransactionFollowUpVM.FollowUpExpireDate;
            oFollowUpCertificateVM.FollowUpExpireDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(oListTransactionFollowUpVM.FollowUpExpireDate);
            oFollowUpCertificateVM.FollowUpReason = oListTransactionFollowUpVM.FollowUpReason;
            oFollowUpCertificateVM.FollowUpTypeId = oListTransactionFollowUpVM.FollowUpTypeId;
            oFollowUpCertificateVM.FollowUpStatusId = oListTransactionFollowUpVM.FollowUpStatusId;
            oFollowUpCertificateVM.FollowUpMethodId = oListTransactionFollowUpVM.FollowUpMethodId;
            oFollowUpCertificateVM.FollowUpPriortyId = oListTransactionFollowUpVM.FollowUpPriortyId;
            oFollowUpCertificateVM.FollowUpProccessId = oListTransactionFollowUpVM.FollowUpProccessId;
            oFollowUpCertificateVM.FollowUpSourceId = oListTransactionFollowUpVM.FollowUpSourceId;
            oFollowUpCertificateVM.FollowUpProgressId = oListTransactionFollowUpVM.FollowUpProgressId;
            oFollowUpCertificateVM.FollowUpProccessNote = oListTransactionFollowUpVM.FollowUpProccessNote;
            oFollowUpCertificateVM.Notes = oListTransactionFollowUpVM.Notes;
            oFollowUpCertificateVM.Active = oListTransactionFollowUpVM.Active;
            oFollowUpCertificateVM.ProccessPeriod = oListTransactionFollowUpVM.ProccessPeriod;
            oFollowUpCertificateVM.ProccessPeriodDate = oListTransactionFollowUpVM.ProccessPeriodDate;
            oFollowUpCertificateVM.FollowUpCompletionDate = oListTransactionFollowUpVM.FollowUpCompletionDate;
            oFollowUpCertificateVM.FollowUpCompletionDateHj = oListTransactionFollowUpVM.FollowUpCompletionDate.HasValue ? DateTimeUtility.ConvertToUmAlQuraCalendar(oListTransactionFollowUpVM.FollowUpCompletionDate.Value) : null;
            oFollowUpCertificateVM.FollowUpReceiveDate = oListTransactionFollowUpVM.FollowUpReceiveDate;
            oFollowUpCertificateVM.IsCopy = oListTransactionFollowUpVM.IsCopy;
            oFollowUpCertificateVM.IsReminder = oListTransactionFollowUpVM.IsReminder;
            oFollowUpCertificateVM.IsEscalated = oListTransactionFollowUpVM.IsEscalated;
            oFollowUpCertificateVM.IsImportant = oListTransactionFollowUpVM.IsImportant;
            oFollowUpCertificateVM.HasChild = oListTransactionFollowUpVM.HasChild;
            oFollowUpCertificateVM.ParentId = oListTransactionFollowUpVM.ParentId;
            return oFollowUpCertificateVM;
        }


        public static PublicFollowupDto MapPublic(PublicFollowupVM publicFollowupVM)
        {
            if (publicFollowupVM == null || !publicFollowupVM.IsValid())
            {
                return null;
            }
            DateTime createdDate = DateTime.Now;
            PublicFollowupDto publicFollowupDto = new PublicFollowupDto
            {
                DateTo = publicFollowupVM.DateTo,
                DateToH = publicFollowupVM.DateToH,
                IsImportant = publicFollowupVM.IsImportant,
                PeriodId = publicFollowupVM.PeriodId.Value,
                ProccessId = publicFollowupVM.ProccessId.Value,
                Active = true,
                CreatingEntityId = SessionInfo.OrgUnitId,
                CreatingUserId = SessionInfo.CurrentUser.Id,
                CreationDate = createdDate,
                FollowUpStatusId = (int)FollowupStatus.New,
                FollowUpExpireDate = publicFollowupVM.ProccessId == -1 ? (DateTime)publicFollowupVM.DateTo : DateTime.Now.AddDays(Convert.ToInt32(publicFollowupVM.PeriodId)),
                FollowUpEntityId = SessionInfo.OrgUnitId,
                CreationDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(createdDate),
            };
            publicFollowupDto.FollowUpExpireDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(publicFollowupDto.FollowUpExpireDate);
            return publicFollowupDto;

        }
        public static PrivateFollowupDto MapPrivate(PrivateFollowupVM privateFollowupVM)
        {
            if (privateFollowupVM == null || !privateFollowupVM.IsValid()) { return null; }
            DateTime createdDate = DateTime.Now;
            PrivateFollowupDto privateFollowupDto = new PrivateFollowupDto
            {
                DateTo = privateFollowupVM.DateTo,
                UserId = SessionInfo.CurrentUser.Id,
                ProccessId = privateFollowupVM.ProccessId.Value,
                PeriodId = privateFollowupVM.PeriodId.Value,
                DateToH = privateFollowupVM.DateToH,
                EntityId = SessionInfo.OrgUnitId,
                IsImportant = privateFollowupVM.IsImportant,
                Active = true,
                CreatingEntityId = SessionInfo.OrgUnitId,
                CreatingUserId = SessionInfo.CurrentUser.Id,
                CreationDate = createdDate,
                FollowUpStatusId = (int)FollowupStatus.New,
                FollowUpExpireDate = privateFollowupVM.ProccessId == -1 ? (DateTime)privateFollowupVM.DateTo : DateTime.Now.AddDays(Convert.ToInt32(privateFollowupVM.PeriodId)),
                FollowUpEntityId = privateFollowupVM.EntityId.Value,
                FollowUpUserId = privateFollowupVM.UserId,
                CreationDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(createdDate),

            };
            privateFollowupDto.FollowUpExpireDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(privateFollowupDto.FollowUpExpireDate);

            return privateFollowupDto;
        }
    }
}