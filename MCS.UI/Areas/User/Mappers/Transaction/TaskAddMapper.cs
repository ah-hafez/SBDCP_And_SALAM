using System;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TaskAddMapper
    {
        public static List<TaskAddVM> Map(IList<TaskAddDTO> taskAddDTOs)
        {
            if (taskAddDTOs == null || !taskAddDTOs.Any())
            {
                return new List<TaskAddVM>();
            }
            List<TaskAddVM> taskAddVMs = taskAddDTOs
                .Select(taskAddDTO => new TaskAddVM()
                {
                    Id = taskAddDTO.Id,
                    Attachment = DocumentMapper.Map(taskAddDTO.Attachment),
                    DeliveryDate = taskAddDTO.DeliveryDate.ToShortDateString(),
                    DeliveryDateH = taskAddDTO.DeliveryDateH,
                    ReceivedFromOrgUnitId = taskAddDTO.FromOrgUnitId,
                    IsExclusive = taskAddDTO.IsExclusive,
                    OrgSettings = taskAddDTO.OrgSettings,
                    OrgStructure = OrgStructureInfoMapper.Map(taskAddDTO.OrgStructure),
                    TaskDescription = taskAddDTO.TaskDescription,
                    TaskWorkflows = TaskWorkflowMapper.Map(taskAddDTO.TaskWorkflows),
                    SentToOrgUnitId = taskAddDTO.ToOrgUnitId,
                    SentToOrgUnitName = taskAddDTO.ToOrgUnitName,
                    SentToUserId = taskAddDTO.ToUserId,
                    SentToUserName = taskAddDTO.ToUserName,
                    ActionId = taskAddDTO.ActionId,
                    ActionName = taskAddDTO.ActionName,
                    ActionTypeId = taskAddDTO.ActionTypeId,
                    Status = taskAddDTO.Status,
                    StatusId = taskAddDTO.StatusId,
                    Notes = taskAddDTO.Notes,
                    DelayedDaysCount = DateTime.Now.Date > taskAddDTO.DeliveryDate.Date ? (Int32.Parse((DateTime.Now.Date - taskAddDTO.DeliveryDate.Date).Days.ToString()).ToString()):"0",
        }).ToList();

            return taskAddVMs;
        }
        public static List<TaskAddDTO> Map(IList<TaskAddVM> taskAddVMs)
        {
            if (taskAddVMs == null || !taskAddVMs.Any())
            {
                return new List<TaskAddDTO>();
            }
            List<TaskAddDTO> taskAddDTOs = taskAddVMs
                .Select(taskAddVM => new TaskAddDTO()
                {
                    Id = taskAddVM.Id,
                    Attachment = DocumentMapper.Map(taskAddVM.Attachment),
                    DeliveryDate = DateTime.Parse(taskAddVM.DeliveryDate),
                    DeliveryDateH = taskAddVM.DeliveryDateH,
                    FromOrgUnitId = taskAddVM.ReceivedFromOrgUnitId,
                    IsExclusive = taskAddVM.IsExclusive,
                    OrgSettings = taskAddVM.OrgSettings,
                    OrgStructure = OrgStructureInfoMapper.Map(taskAddVM.OrgStructure),
                    TaskDescription = taskAddVM.TaskDescription,
                    TaskWorkflows = TaskWorkflowMapper.Map(taskAddVM.TaskWorkflows),
                    ToOrgUnitId = taskAddVM.SentToOrgUnitId,
                    ToOrgUnitName = taskAddVM.SentToOrgUnitName,
                    ToUserId = taskAddVM.SentToUserId ?? 0,
                    ToUserName = taskAddVM.SentToUserName,
                    ActionId = taskAddVM.ActionId,
                    ActionName = taskAddVM.ActionName,
                    ActionTypeId = taskAddVM.ActionTypeId,
                    Status = taskAddVM.Status,
                    StatusId = taskAddVM.StatusId,
                    Notes = taskAddVM.Notes,
                }).ToList();

            return taskAddDTOs;
        }

    }
}