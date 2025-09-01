using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class TransactionReservationMapping : EntityTypeConfiguration<TransactionReservation>
    {
        public TransactionReservationMapping()
        {
            HasRequired(a => a.Entity).WithMany().WillCascadeOnDelete(false);
            HasRequired(a => a.User).WithMany().WillCascadeOnDelete(false);
        }
    }
}
