using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionDeliveryReportMapping : EntityTypeConfiguration<TransactionDeliveryReport>
    {
        public TransactionDeliveryReportMapping()
        {
            this.Property(t=>t.Number).HasMaxLength(50);
            this.Property(t => t.DateH).HasMaxLength(50);
        }
    }
}
