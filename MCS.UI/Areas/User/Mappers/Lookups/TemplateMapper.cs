using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class TemplateMapper
    {
        public static FormDTO Map(TemplateVM templateVM)
        {
            if (templateVM != null)
            {
                return new FormDTO
                {
                    Description = LocalizationMapper.Map(templateVM.Description),
                    FormContentDTO = DocumentMapper.Map(templateVM.FormContentVM),
                    Id = templateVM.Id,
                    LocalName = templateVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(templateVM.TransactionCategories)

                };
            }
            return null;
        }
        public static TemplateVM Map(FormDTO formDTO)
        {
            if (formDTO != null)
            {
                return new TemplateVM
                {
                    Description = LocalizationMapper.Map(formDTO.Description),
                    FormContentVM = DocumentMapper.Map(formDTO.FormContentDTO),
                    Id = formDTO.Id,
                    LocalName = formDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(formDTO.TransactionCategories)

                };
            }
            return null;
        }
        public static List<TemplateVM> Map(IList<FormDTO> formDTOs)
        {
            if (formDTOs == null || !formDTOs.Any())
            {
                return new List<TemplateVM>();
            }

            List<TemplateVM> formVMs = formDTOs.Select(formDTO => new TemplateVM()
            {
                Id = formDTO.Id,
                Description = LocalizationMapper.Map(formDTO.Description),
                FormContentVM = DocumentMapper.Map(formDTO.FormContentDTO),
                LocalName = formDTO.LocalName,
                TransactionCategories = TransactionCategoryMapper.Map(formDTO.TransactionCategories),
                IsActive = formDTO.IsActive,
                IsLocked = formDTO.IsLocked,
                LockedBy = formDTO.LockedBy
            }).ToList();

            return formVMs;
        }
        public static List<FormDTO> Map(IList<TemplateVM> formVMs)
        {
            if (formVMs == null || !formVMs.Any())
            { return null; }
            List<FormDTO> formDTOs = formVMs
                .Select(formVM => new FormDTO()
                {
                    Id = formVM.Id,
                    Description = LocalizationMapper.Map(formVM.Description),
                    FormContentDTO = DocumentMapper.Map(formVM.FormContentVM),
                    LocalName = formVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(formVM.TransactionCategories)
                }).ToList();

            return formDTOs;
        }
        public static FormAddDTO Map(TemplateAddVM formAddVM)
        {
            if (formAddVM != null)
            {
                return new FormAddDTO
                {

                    Description = LocalizationMapper.Map(formAddVM.Description),
                    FormContentDTO = DocumentMapper.Map(new DocumentVM() { Content = formAddVM.FormContentVM.Content }),
                    DepartmentIds = formAddVM.OrgUnitIds,
                    TransactionCategories = TransactionCategoryMapper.Map(formAddVM.TransactionCategories)

                };
            }
            return null;
        }
        public static FormAddDTO MapToAdd(TemplateEditVM formAddVM)
        {
            if (formAddVM != null)
            {
                return new FormAddDTO
                {

                    Description = LocalizationMapper.Map(formAddVM.Description),
                    FormContentDTO = DocumentMapper.Map(new DocumentVM() { Content = formAddVM.FormContentVM.Content }),
                    DepartmentIds = formAddVM.OrgUnitIds,
                    TransactionCategories = TransactionCategoryMapper.Map(formAddVM.TransactionCategories)

                };
            }
            return null;
        }
        public static TemplateEditVM Map(FormEditDTO formEditDTO)
        {
            if (formEditDTO != null)
            {
                DocumentVM documentVM = DocumentMapper.Map(formEditDTO.FormContentDTO);

                return new TemplateEditVM
                {
                    DepartmentIds = formEditDTO.DepartmentIds,
                    OrgUnitIds = formEditDTO.DepartmentIds,
                    Description = LocalizationMapper.Map(formEditDTO.Description),
                    FormContentVM = new TemplateContentVM() { Content = documentVM?.Content, Id = documentVM.Id },
                    Id = formEditDTO.Id,
                    TransactionCategories = TransactionCategoryMapper.Map(formEditDTO.TransactionCategories),
                    IsActive = formEditDTO.IsActive,
                    IsLocked = formEditDTO.IsLocked,
                    LockedBy = formEditDTO.LockedBy,
                    Status = formEditDTO.Status,
                    FileContent = formEditDTO.FormContentDTO != null && formEditDTO.FormContentDTO.Content != null
                    && formEditDTO.FormContentDTO.Content.Length > 0 ? Convert.ToBase64String(formEditDTO.FormContentDTO.Content) : ""
                };
            }
            return null;
        }
        public static TemplateAddVM MapToCopy(FormEditDTO formEditDTO)
        {
            if (formEditDTO != null)
            {
                DocumentVM documentVM = DocumentMapper.Map(formEditDTO.FormContentDTO);

                return new TemplateAddVM
                {
                    DepartmentIds = formEditDTO.DepartmentIds?.ToList(),
                    OrgUnitIds = formEditDTO.DepartmentIds,
                    Description = LocalizationMapper.Map(formEditDTO.Description),
                    FormContentVM = new TemplateContentVM() { Content = documentVM?.Content, Id = documentVM.Id },
                    TransactionCategories = TransactionCategoryMapper.Map(formEditDTO.TransactionCategories),
                    FileContent = formEditDTO.FormContentDTO != null && formEditDTO.FormContentDTO.Content != null
                    && formEditDTO.FormContentDTO.Content.Length > 0 ? Convert.ToBase64String(formEditDTO.FormContentDTO.Content) : ""
                };
            }
            return null;
        }
        public static FormEditDTO Map(TemplateEditVM formEditVM)
        {
            if (formEditVM != null)
            {
                return new FormEditDTO
                {

                    DepartmentIds = formEditVM.OrgUnitIds,
                    OrgUnitIds = formEditVM.OrgUnitIds,
                    Description = LocalizationMapper.Map(formEditVM.Description),
                    FormContentDTO = DocumentMapper.Map(new DocumentVM() { Id = formEditVM.FormContentVM.Id, Content = formEditVM.FormContentVM.Content }),
                    Id = formEditVM.Id,
                    TransactionCategories = TransactionCategoryMapper.Map(formEditVM.TransactionCategories)

                };
            }
            return null;
        }

    }
}