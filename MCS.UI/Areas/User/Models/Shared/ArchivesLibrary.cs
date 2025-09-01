using MCS.Common.CustomAttributes;
using System;
namespace MCS.UI.Areas.User.Models.Shared
{
    public class ArchivesLibrary
    {
        [CustomDisplayName("User.Shared.DocumentNumber")]
        [CustomRequired("User.Shared.DocumentNumberRequired")]
        public string DocumentNum { get; set; }
        [CustomDisplayName("User.Shared.DocumentDate")]
        [CustomRequired("User.Shared.DocumentDateRequired")]
        public DateTime Date { get; set; }
        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public string ConfidentialityLevel { get; set; }
        [CustomDisplayName("User.Shared.DocumentType")]
        [CustomRequired("User.Shared.DocumentTypeRequired")]
        public string DocumentType { get; set; }
        [CustomDisplayName("User.Shared.Keywords")]
        [CustomRequired("User.Shared.KeywordsRequired")]
        public string Keywords { get; set; }
        public int PagesNum { get; set; }
        public string Operative { get; set; }
        public string Orgnization { get; set; }

    }
}