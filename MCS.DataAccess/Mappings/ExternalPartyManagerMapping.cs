using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ExternalPartyManagerMapping : EntityTypeConfiguration<ExternalPartyManager>
    {
        public ExternalPartyManagerMapping()
        {
            this.Ignore(p => p.LocalName);
        }
    }
}
