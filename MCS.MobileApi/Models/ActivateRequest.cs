namespace MobileApi.Models
{
    public class ActivateRequest
    {
        public string OrgName { get; set; }
        public string OrgId { get; set; }
        public string OrgCMC { get; set; }
        public string OrgOID { get; set; }
        public string SessionToken { get; set; }
    }
}