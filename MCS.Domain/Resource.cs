using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Resource : EntityBase
    {
        public string ResourceId { get; set; }
        public string Value { get; set; }
        public string Culture { get; set; }
        public string ResourceSet { get; set; }
        public string Type { get; set; }
        public byte[] BinFile { get; set; }
        public string TextFile { get; set; }
        public string Filename { get; set; }
        public string Comment { get; set; }
    }
}
