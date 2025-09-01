using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class InquirySearchResultDTO
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string Subject { get; set; }
        public string DateH { get; set; }
        public DateTime Date { get; set; }
        public string StatusName { get; set; }
        public string ToEntity { get; set; }
        public string ToUser { get; set; }
        public int ToUserID { get; set; }
        public int ConfidentialityId { get; set; }
        public bool HasPermission { get; set; }
        public int? Weight { get; set; }
        public int TransactionTypeId { get; set; }
        public bool Encrypted { get; set; }
    }
}
