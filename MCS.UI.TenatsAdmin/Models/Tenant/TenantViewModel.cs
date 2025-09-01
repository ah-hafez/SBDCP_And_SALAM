using MCS.UI.TenantsAdmin.Models.Tenant;

namespace MCS.UI.TenantsAdmin.Models
{
    public class TenantViewModel
    {
        public TenantVM Tenant { get; set; }
        public AddTenantVM AddTenant { get; set; }
        public EditTenantVM EditTenant { get; set; }

        public TenantViewModel()
        {
            Tenant = new TenantVM();
            AddTenant = new AddTenantVM();
            EditTenant = new EditTenantVM();
        }
    }
}