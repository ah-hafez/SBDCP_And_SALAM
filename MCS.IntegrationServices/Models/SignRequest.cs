
namespace MCS.IntegrationServices.Models
{
    public class SignRequest
    {
        public string uid { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public string csr { get; set; }
        public string SessionToken { get; set; }
    }
}