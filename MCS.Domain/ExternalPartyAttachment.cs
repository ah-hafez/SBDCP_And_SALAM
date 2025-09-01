using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class ExternalPartyAttachment : EntityBase
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ExternalParty ExternalParty { get; set; }
        public int DocumentInfoId { get; set; }
        public virtual DocumentInfo DocumentInfo { get; set; }
        public int TransactionExternalCopyId { get; set; }
        public virtual TransactionExternalCopy TransactionExternalCopy { get; set; }
    }
}
