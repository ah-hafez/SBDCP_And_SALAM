using System.Data.Entity.ModelConfiguration;
using MCS.Domain;


namespace MCS.DataAccess
{
    public class SettingMapping : EntityTypeConfiguration<Setting>
    {
        public SettingMapping()
        {
            Property(s => s.Key).HasMaxLength(50);
            Property(s => s.Value).IsUnicode(false);
            Property(s => s.Description).HasMaxLength(1000);
        }
    }
}
