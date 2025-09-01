using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class UserMobileMapping : EntityTypeConfiguration<UserMobile>
    {
        public UserMobileMapping()
        {
            Ignore(um => um.EntityId); 
            Ignore(um => um.LoginName);
        }
    }
}