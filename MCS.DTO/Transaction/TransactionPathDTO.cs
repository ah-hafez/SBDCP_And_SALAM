using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionPathDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public int OrgUnitId { get; set; }
        public int TransactionTypeId { get; set; }
        public int Sort { get; set; }
        public bool IsReadOnly { get; set; }

        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
        public string TransactionTypeName { get; set; }

        public IList<TransactionPathDetailsDTO> TransactionPathDetails { get; set; }
    }
}
