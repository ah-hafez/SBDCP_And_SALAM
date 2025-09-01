using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionPathDetailsDTO
    {
        public int Id { get; set; }
        public int TransactionPathId { get; set; }
        public int? UserId { get; set; }
        public int OrgUnitId { get; set; }
        public int ActionId { get; set; }
        public int Sort { get; set; }
        public bool IsReadOnly { get; set; }

        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
        public string ActionName { get; set; }

    }
}
