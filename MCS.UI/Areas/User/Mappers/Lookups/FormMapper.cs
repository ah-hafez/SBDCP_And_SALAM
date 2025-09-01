using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class FormMapper
    {
        public static List<FormVM> Map(IList<FormDTO> formDTOs)
        {
            if (formDTOs == null || !formDTOs.Any())
            {
                return new List<FormVM>();
            }
            List<FormVM> formVMs = formDTOs
                .Select(formDTO => new FormVM()
                { 
                    Id = formDTO.Id,
                    Description = LocalizationMapper.Map(formDTO.Description),
                    LocalName = formDTO.LocalName,
                    FormContentVM = DocumentMapper.Map(formDTO.FormContentDTO),
                    TransactionCategories = TransactionCategoryMapper.Map(formDTO.TransactionCategories)
                }).ToList();

            return formVMs;
        }
        public static List<FormDTO> Map(IList<FormVM> formVMs)
        {
            if (formVMs == null || !formVMs.Any())
            {
                return new List<FormDTO>();
            }
            List<FormDTO> formDTOs = formVMs
                .Select(formVM => new FormDTO()
                {
                    Id = formVM.Id,
                    Description = LocalizationMapper.Map(formVM.Description),
                    LocalName = formVM.LocalName,
                    FormContentDTO = DocumentMapper.Map(formVM.FormContentVM),
                    TransactionCategories = TransactionCategoryMapper.Map(formVM.TransactionCategories)
                }).ToList();

            return formDTOs;
        }
        public static List<FormAddVM> Map(IList<FormAddDTO> formAddDTOs)
        {
            if (formAddDTOs == null || !formAddDTOs.Any())
            {
                return new List<FormAddVM>();
            }
            List<FormAddVM> formAddVMs = formAddDTOs
                .Select(formAddDTO => new FormAddVM()
                {
                    DepartmentIds = formAddDTO.DepartmentIds,
                    Description = LocalizationMapper.Map(formAddDTO.Description),
                    FormContentVM = DocumentMapper.Map(formAddDTO.FormContentDTO),
                    TransactionCategories = TransactionCategoryMapper.Map(formAddDTO.TransactionCategories)
                }).ToList();

            return formAddVMs;
        }
        public static List<FormAddDTO> Map(IList<FormAddVM> formAddVMs)
        {
            if (formAddVMs == null || !formAddVMs.Any())
            {
                return new List<FormAddDTO>();
            }
            List<FormAddDTO> formAddDTOs = formAddVMs
                .Select(formAddVM => new FormAddDTO()
                {
                    DepartmentIds = formAddVM.DepartmentIds,
                    Description = LocalizationMapper.Map(formAddVM.Description),
                    FormContentDTO = DocumentMapper.Map(formAddVM.FormContentVM),
                    TransactionCategories = TransactionCategoryMapper.Map(formAddVM.TransactionCategories)
                }).ToList();

            return formAddDTOs;
        }
        public static List<FormContentDTO> Map(IList<FormContentVM> formContentVMs)
        {
            if (formContentVMs == null || !formContentVMs.Any())
            {
                return new List<FormContentDTO>();
            }
            List<FormContentDTO> formContentDTOs = formContentVMs
                .Select(formContentVM => new FormContentDTO()
                { 
                    Id = formContentVM.Id,
                    Content = formContentVM.Content
                }).ToList();

            return formContentDTOs;
        }
        public static List<FormContentVM> Map(IList<FormContentDTO> formContentDTOs)
        {
            if (formContentDTOs == null || !formContentDTOs.Any())
            {
                return new List<FormContentVM>();
            }
            List<FormContentVM> formContentVMs = formContentDTOs
                .Select(formContentDTO => new FormContentVM()
                { 
                    Id = formContentDTO.Id,
                    Content = formContentDTO.Content
                }).ToList();

            return formContentVMs;
        }
        public static FormContentVM Map(FormContentDTO formContentDTO)
        {
            if (formContentDTO != null)
            {
                FormContentVM formContentVM = new FormContentVM()
                { 
                    Id = formContentDTO.Id,
                    Content = formContentDTO.Content
                };

                return formContentVM;
            }
            return new FormContentVM();
        }
        public static FormContentDTO Map(FormContentVM formContentVM)
        {
            if (formContentVM != null)
            {
                FormContentDTO formContentDTO = new FormContentDTO()
                {
                    Id = formContentVM.Id,
                    Content = formContentVM.Content
                };

                return formContentDTO;
            }
            return new FormContentDTO();
        }
        public static List<FormEditVM> Map(IList<FormEditDTO> formEditDTOs)
        {
            if (formEditDTOs == null || !formEditDTOs.Any())
            {
                return new List<FormEditVM>();
            }
            List<FormEditVM> formEditVMs = formEditDTOs
                .Select(formEditDTO => new FormEditVM()
                {
                    Id = formEditDTO.Id,
                    DepartmentIds = formEditDTO.DepartmentIds,
                    Description = LocalizationMapper.Map(formEditDTO.Description),
                    FormContentVM = DocumentMapper.Map(formEditDTO.FormContentDTO),
                    TransactionCategories = TransactionCategoryMapper.Map(formEditDTO.TransactionCategories)
                }).ToList();

            return formEditVMs;
        }
        public static List<FormEditDTO> Map(IList<FormEditVM> formEditVMs)
        {
            if (formEditVMs == null || !formEditVMs.Any())
            {
                return new List<FormEditDTO>();
            }
            List<FormEditDTO> formEditDTOs = formEditVMs
                .Select(formEditVM => new FormEditDTO()
                {
                    Id = formEditVM.Id,
                    DepartmentIds = formEditVM.DepartmentIds,
                    Description = LocalizationMapper.Map(formEditVM.Description),
                    FormContentDTO = DocumentMapper.Map(formEditVM.FormContentVM),
                    TransactionCategories = TransactionCategoryMapper.Map(formEditVM.TransactionCategories)
                }).ToList();

            return formEditDTOs;
        }
    }
}