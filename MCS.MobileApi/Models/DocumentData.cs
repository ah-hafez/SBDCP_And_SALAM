using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class DocumentData
    {
        public DataResult Result { get; set; }
        public byte[] Binary { get; set; }
        public string BinaryData { get; set; }
    }
}