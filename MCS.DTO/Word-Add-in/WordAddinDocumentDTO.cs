using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
   public  class WordAddinDocumentDTO
    {

        public string userName { get; set; }

        public byte[] content { get; set; }

        public byte[] contentAsPDF { get; set; }

        public string TransactionId { get; set; }


        public string FileName { get; set; }

        public bool IsApproved { get; set; }

        public string GUIDFileName { get; set; }


    }

}
