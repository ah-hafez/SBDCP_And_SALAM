using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class Document
    {
        public string DocumentId { get; set; }
        public byte[] DocumentData { get; set; }
    }
}