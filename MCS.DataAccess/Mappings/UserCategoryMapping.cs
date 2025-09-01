using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserCategoryMapping : EntityTypeConfiguration<UserCategory>
    {
        public UserCategoryMapping()
        {
            this.HasMany(uc => uc.CategoryTrays).WithOptional().HasForeignKey(c => c.UserCategoryId).WillCascadeOnDelete();
            this.Ignore(u => u.LocalName);
        }
    }
}
