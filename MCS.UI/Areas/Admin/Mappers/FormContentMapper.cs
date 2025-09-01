using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class FormContentMapper
    {
        public static FormContentDTO Map(FormContentVM formContentVM)
        {
            if (formContentVM != null)
            {
                return new FormContentDTO
                {   
                    Content = formContentVM.Content,
                    Id = formContentVM.Id
                };
            }
            return null;
        }
        public static FormContentVM Map(FormContentDTO formContentDTO)
        {
            if (formContentDTO != null)
            {
                return new FormContentVM
                {
                    Content = formContentDTO.Content,
                    Id = formContentDTO.Id
                };
            }
            return null;
        }
    }
}