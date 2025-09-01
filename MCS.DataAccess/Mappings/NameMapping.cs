using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class NameMapping : EntityTypeConfiguration<Name>
    {
        public NameMapping()
        {
            this.Property(n=>n.Address).HasMaxLength(100);
            this.Property(n => n.CivilID).HasMaxLength(10);
            this.Property(n => n.Email).HasMaxLength(150);
            this.Property(n => n.FirstName).HasMaxLength(120);
            this.Property(n => n.MobileNumber).HasMaxLength(20);
            this.Property(n => n.Phone).HasMaxLength(15);
            this.Property(n => n.OtherInformation).HasMaxLength(200);
            this.Property(n => n.Gender);
            this.Property(n => n.TitleId);
            this.Property(n => n.RelativeRelation);
            this.Property(n => n.City);
        }
    }
}
