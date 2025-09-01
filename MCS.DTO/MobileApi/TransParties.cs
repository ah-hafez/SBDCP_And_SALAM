using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Domain
{
    public class TransPartiy
    {
        public int TransPartyId { get; set; }

        public int? PartyID { get; set; }

        public int? PersonID { get; set; }

        public string EntityName { get; set; }

        public string PersonName { get; set; }

        public int ProcessId { get; set; }

        public string ProcessDesc { get; set; }

        public string SendDateHJ { get; set; }

        public int RowStatus { get; set; }

        public string FromEntityName { get; set; }

        public string FromPersonName { get; set; }
    }
}