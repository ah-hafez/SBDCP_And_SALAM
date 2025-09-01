using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class LetterTypeMapping : EntityTypeConfiguration<LetterType>
    {
        public LetterTypeMapping()
        {            
            this.Ignore(l => l.Text);
        }
    }
}
