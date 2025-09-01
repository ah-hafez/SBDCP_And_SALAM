using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionIndexLogMapping : EntityTypeConfiguration<TransactionIndexLog>
    {
        public TransactionIndexLogMapping()
        {
            this.Property(t => t.Assignments).HasMaxLength(50);
            this.Property(t => t.Barcode).HasMaxLength(50);
            this.Property(t => t.Color).HasMaxLength(50);
            this.Property(t => t.ConfidentialityNameAr).HasMaxLength(50);
            this.Property(t => t.ConfidentialityNameEn).HasMaxLength(50);
            this.Property(t => t.DateH).HasMaxLength(50);
            this.Property(t => t.OrgUnitNameAr).HasMaxLength(50);
            this.Property(t => t.OrgUnitNameEn).HasMaxLength(50);
            this.Property(t => t.PartyNameAr).HasMaxLength(50);
            this.Property(t => t.PartyNameEn).HasMaxLength(50);
            this.Property(t => t.PriorityNameAr).HasMaxLength(50);
            this.Property(t => t.PriorityNameEn).HasMaxLength(50);
            this.Property(t => t.SignedByNameAr).HasMaxLength(50);
            this.Property(t => t.SignedByNameEn).HasMaxLength(50);
            this.Property(t => t.StatusNameAr).HasMaxLength(50);
            this.Property(t => t.StatusNameEn).HasMaxLength(50);
            this.Property(t => t.SubjectClassifications).HasMaxLength(500);
            this.Property(t => t.TransactionTypeNameAr).HasMaxLength(50);
            this.Property(t => t.TransactionTypeNameEn).HasMaxLength(50);
            this.Property(t => t.TypeNameAr).HasMaxLength(50);
            this.Property(t => t.TypeNameEn).HasMaxLength(50);
        }
    }
}
