using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class Transaction
    {
        public int TransID { get; set; }
        public string TransNo { get; set; }
        public bool Has_Supporting_Attachments { get; set; }
        public string TransTitle { get; set; }
        public string TransDate { get; set; }
        public string TransFrom { get; set; }
        public int TransCategory { get; set; }
        public string FileSize { get; set; }
        public bool ReadOnly { get; set; }
        public string TransSourceRow { get; set; }
        public string TransNumberRow { get; set; }
    }
}