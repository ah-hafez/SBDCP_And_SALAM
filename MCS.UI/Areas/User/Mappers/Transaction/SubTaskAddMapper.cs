using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class SubTaskAddMapper
    {
        public static List<SubTaskAddVM> Map(IList<SubTaskAddDTO> subTaskAddDTOs)
        {
            if (subTaskAddDTOs == null || !subTaskAddDTOs.Any())
            {
                return new List<SubTaskAddVM>();
            }
            List<SubTaskAddVM> subTaskAddVMs = subTaskAddDTOs
                .Select(subTaskAddDTO => new SubTaskAddVM()
                { 
                    Attachment = DocumentMapper.Map(subTaskAddDTO.Attachment),
                    DeliveryDate = subTaskAddDTO.DeliveryDate,
                    DeliveryDateH = subTaskAddDTO.DeliveryDateH,
                    FromOrgUnitId = subTaskAddDTO.FromOrgUnitId,
                    TaskDescription = subTaskAddDTO.TaskDescription,
                    ToOrgUnitId = subTaskAddDTO.ToOrgUnitId,
                    ToOrgUnitName = subTaskAddDTO.ToOrgUnitName,
                    ToUserId = subTaskAddDTO.ToUserId,
                    ToUserName = subTaskAddDTO.ToUserName,
                    TransactionId = subTaskAddDTO.TransactionId
                }).ToList();

            return subTaskAddVMs;
        }
        public static List<SubTaskAddDTO> Map(IList<SubTaskAddVM> subTaskAddVMs)
        {
            if (subTaskAddVMs == null || !subTaskAddVMs.Any())
            {
                return new List<SubTaskAddDTO>();
            }
            List<SubTaskAddDTO> subTaskAddDTOs = subTaskAddVMs
                .Select(subTaskAddVM => new SubTaskAddDTO()
                {
                    Attachment = DocumentMapper.Map(subTaskAddVM.Attachment),
                    DeliveryDate = subTaskAddVM.DeliveryDate,
                    DeliveryDateH = subTaskAddVM.DeliveryDateH,
                    FromOrgUnitId = subTaskAddVM.FromOrgUnitId,
                    TaskDescription = subTaskAddVM.TaskDescription,
                    ToOrgUnitId = subTaskAddVM.ToOrgUnitId,
                    ToOrgUnitName = subTaskAddVM.ToOrgUnitName,
                    ToUserId = subTaskAddVM.ToUserId,
                    ToUserName = subTaskAddVM.ToUserName,
                    TransactionId = subTaskAddVM.TransactionId
                }).ToList();

            return subTaskAddDTOs;
        }


    }
}