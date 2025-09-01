using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TrayMapping : EntityTypeConfiguration<Tray>
    {
        public TrayMapping()
        {
            this.Ignore(a => a.LocalName);
        }
    }
}
