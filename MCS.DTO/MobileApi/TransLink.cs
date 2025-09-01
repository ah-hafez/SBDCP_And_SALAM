using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.MobileApi
{
    public class TransLink
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int ToTransactionId { get; set; }
    }
}
