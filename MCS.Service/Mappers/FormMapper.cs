using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class FormMapper
    {
        public static Form Map(FormAddDTO formAddDTO)
        {
            if (formAddDTO == null)
                return null;
            TransactionCategories transactionCategories =
                TransactionCategoryMapper.Map(formAddDTO.TransactionCategories);

            Form form = new Form()
            {
                TransactionCategories = transactionCategories,
                LocalizationIdentifier = formAddDTO.Description != null ? LocalizationIdentifierMapper.Map(formAddDTO.Description) : null,
                FormContent = DocumentMapper.Map(formAddDTO.FormContentDTO),
                Departments = (formAddDTO.DepartmentIds != null) ? MapDepartments(formAddDTO.DepartmentIds.ToList()) : null
            };

            return form;
        }

        public static Form Map(FormEditDTO formEditDTO)
        {
            if (formEditDTO == null)
                return null;
            TransactionCategories transactionCategories =
                TransactionCategoryMapper.Map(formEditDTO.TransactionCategories);

            Form form = new Form()
            {
                Id = formEditDTO.Id,
                TransactionCategories = transactionCategories,
                LocalizationIdentifier = formEditDTO.Description != null ? LocalizationIdentifierMapper.Map(formEditDTO.Description) : null,
                FormContent = DocumentMapper.Map(formEditDTO.FormContentDTO),
                Departments = (formEditDTO.DepartmentIds != null) ? MapDepartments(formEditDTO.DepartmentIds.ToList()) : new List<FormDepartment>()
            };
            if (form.Departments != null)
            {
                form.Departments.ToList().ForEach(d =>
                 {

                     d.FormId = formEditDTO.Id;
                 });
            }

            return form;
        }

        public static FormEditDTO Map(Form form, string cultureName)
        {
            if (form == null)
                return null;

            FormEditDTO formEditDTO = new FormEditDTO()
            {
                Id = form.Id,
                Description = form.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(form.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(form.TransactionCategories, cultureName),
                FormContentDTO = DocumentMapper.MapWithContent(form.FormContent),
                DepartmentIds = form.Departments.FirstOrDefault(a=>a.DepartmentId>0) != null ? form.Departments.Select(d => d.DepartmentId.Value).ToList():new List<int>(),
                IsActive = form.IsActive,
                IsLocked = form.IsLocked,
                LockedBy = form.LockedBy,
                //Status = form.Status
            };

            return formEditDTO;
        }

        public static List<FormDTO> Map(IList<Form> forms, string cultureName)
        {
            List<FormDTO> formDTOs = forms
                .Select(formDTO => new FormDTO()
                {
                    Id = formDTO.Id,
                    LocalName = formDTO.Text,
                    FormContentDTO = DocumentMapper.MapWithContent(formDTO.FormContent),
                    TransactionCategories = TransactionCategoryMapper.Map(formDTO.TransactionCategories, cultureName),
                    Description = formDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(formDTO.LocalizationIdentifier.Localizations) : null,
                    IsActive = formDTO.IsActive,
                    IsLocked = formDTO.IsLocked,
                    LockedBy = formDTO.LockedBy
                }).ToList();



            return formDTOs;
        }
        public static List<Form> Map(IList<FormDTO> formDTOs, string cultureName)
        {
            List<Form> forms = formDTOs
                .Select(form => new Form()
                {
                    Id = form.Id,
                    Text = form.LocalName,
                    FormContent = DocumentMapper.Map(form.FormContentDTO),
                    TransactionCategories = TransactionCategoryMapper.Map(form.TransactionCategories),
                    LocalizationIdentifier = form.Description != null ? LocalizationIdentifierMapper.Map(form.Description) : null,

                }).ToList();


            return forms;
        }


        private static List<FormDepartment> MapDepartments(IList<int> departmentsIds)
        {
            if (departmentsIds == null || !departmentsIds.Any())
            {
                return null;
            }
            List<FormDepartment> departments = new List<FormDepartment>();
            IOrgUnitBL OrgUnitBL = IoC.Resolve<IOrgUnitBL>();

            foreach (var id in departmentsIds)
            {
                departments.Add(
                    new FormDepartment
                    {
                        Department = OrgUnitBL.GetOrgUnitById(id),
                    });
            }

            return departments;
        }

        public static FormContentDTO MapFormContent(FormContent formContent)
        {
            if (formContent != null)
            {
                FormContentDTO formDocumentDTO = new FormContentDTO()
                {
                    Id = formContent.Id,
                    Content = formContent.Content
                };

                return formDocumentDTO;
            }

            return null;
        }

        private static FormContent MapFormContent(FormContentDTO formContentDTO)
        {
            if (formContentDTO != null)
            {
                FormContent formContent = new FormContent()
                {
                    Id = formContentDTO.Id,
                    Content  = formContentDTO.Content,
                };

                return formContent;
            }

            return null;
        }

        public static List<FormContentDTO> Map(IList<FormContent> formContents)
        {
            if (formContents == null || !formContents.Any())
            {
                return null;
            }
            List<FormContentDTO> formDocumentDTOs = formContents
                .Select(formDocumentDTO => new FormContentDTO()
                {
                    Id = formDocumentDTO.Id,
                    Content = formDocumentDTO.Content
                }).ToList();



            return formDocumentDTOs;
        }

        public static List<FormContent> Map(List<FormContentDTO> formContentDTOs)
        {
            if (formContentDTOs == null || !formContentDTOs.Any())
            {
                return null;
            }
            List<FormContent> formDocuments = formContentDTOs
                .Select(formDocumentDTO => new FormContent()
                {
                    Id = formDocumentDTO.Id,
                    Content = formDocumentDTO.Content
                }).ToList();



            return formDocuments;
        }
    }
}