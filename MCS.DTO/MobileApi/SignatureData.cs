namespace MobileApi.Domain
{
    public class SignatureData
    {
        public byte[] Signature { get; set; }
        public bool HasPassword { get; set; }
        public string Password { get; set; }
        public string FreeText { get; set; }
    }
}
