using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;

namespace MCS.DTO
{
    public class TransactionLightDTO
    {
        public int Id { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public string  Number { get; set; }
        public string Barcode { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
    }
}
