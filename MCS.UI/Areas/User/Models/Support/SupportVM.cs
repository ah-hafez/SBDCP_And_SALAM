using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Support
{
    public class SupportVM
    {
        [CustomDisplayName("User.Support.Subject")]
        [CustomRequired("User.Shared.Support.Subject")]
        public string Subject { get; set; }

        [CustomDisplayName("User.Support.Description")]
        [CustomRequired("User.Shared.Support.Description")]
        public string Description { get; set; }

        [CustomDisplayName("User.SupportType")]
        [CustomRequired("User.Shared.Support.SupportType")]
        public string SupportType { get; set; }

        [CustomDisplayName("User.Support.Category")]
        [CustomRequired("User.Shared.Support.SelectCategory")]
        public string Category { get; set; }

        [DisplayName("User.Transaction.Copy.Attachment")]
        public List<HttpPostedFileBase> Files { get; set; } //= new List<HttpPostedFileBase>();
    }
}