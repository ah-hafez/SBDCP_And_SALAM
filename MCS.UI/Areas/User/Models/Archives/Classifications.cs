using System;

namespace MCS.UI.Areas.User.Models.Archives
{
    public class Classification
    {
        public int Year { get; set; }
        public string Type { get; set; }
        public int? TransactionNum { get; set; }
        public int DirectoryNum { get; set; }
        public string Remark { get; set; }
        public string classification { get; set; }

    }
}