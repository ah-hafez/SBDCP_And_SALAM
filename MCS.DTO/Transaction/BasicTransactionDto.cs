using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class BasicTransactionDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateH { get; set; }
        public string Confidentiality { get; set; }
        public int? TransactionType { get; set; }
        public string Url { get; set; }
        public int TransactionCategoryId { get; set; }
        public long TransactionNumber { get; set; }


    }
}
