using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class FormMapper
    {
        public static FormDTO Map(FormVM formVM)
        {
            if (formVM != null)
            {
                return new FormDTO
                {
                    Description = LocalizationMapper.Map(formVM.Description),
                    FormContentDTO = DocumentMapper.Map(formVM.FormContentVM),
                    Id = formVM.Id,
                    LocalName = formVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(formVM.TransactionCategories)

                };
            }
            return null;
        }
        public static FormVM Map(FormDTO formDTO)
        {
            if (formDTO != null)
            {
                return new FormVM
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
        public static List<FormVM> Map(IList<FormDTO> formDTOs)
        {
            if (formDTOs == null || !formDTOs.Any())
            {
                return new List<FormVM>();
            }

            List<FormVM> formVMs = formDTOs.Select(formDTO => new FormVM()
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
        public static List<FormDTO> Map(IList<FormVM> formVMs)
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
        public static FormAddDTO Map(FormAddVM formAddVM)
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
        public static FormEditVM Map(FormEditDTO formEditDTO)
        {
            if (formEditDTO != null)
            {
                DocumentVM documentVM = DocumentMapper.Map(formEditDTO.FormContentDTO);

                return new FormEditVM
                {
                    DepartmentIds = formEditDTO.DepartmentIds,
                    OrgUnitIds = formEditDTO.DepartmentIds,
                    Description = LocalizationMapper.Map(formEditDTO.Description),
                    FormContentVM = new FormContentVM() { Content = documentVM?.Content, Id = documentVM.Id },
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
        public static FormEditDTO Map(FormEditVM formEditVM)
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