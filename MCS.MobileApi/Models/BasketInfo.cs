namespace MobileApi.Models
{
    public class BasketInfo
    {
        public string BasketClass { get; set; }
        public int BasketLocation { get; set; }
        public string BasketName { get; set; }
        public bool IsBasketExist { get; set; }
        public string LeftDrawerName { get; set; }
        public string LeftLinkText { get; set; }
        public string LeftUrl { get; set; }
        public string LeftUrlParams { get; set; }
        public string RightDrawerName { get; set; }
        public string RightLinkText { get; set; }
        public string RightUrl { get; set; }
        public string RightUrlParams { get; set; }
        public int TrayID { get; set; }
        public int CountersType { get; set; }
        public string Color { get; set; }
        public BasketInfo(bool isBasketExist, string basketName, string leftLinkText, string leftUrl, string leftUrlParams,
           string rightLinkText, string rightUrl, string rightUrlParams, int basketLocation, string basketClass)
        {
            BasketClass = basketClass;
            BasketLocation = basketLocation;
            BasketName = basketName;
            IsBasketExist = isBasketExist;
            LeftDrawerName = string.Empty;
            LeftLinkText = leftLinkText;
            LeftUrl = leftUrl;
            LeftUrlParams = leftUrlParams;
            RightDrawerName = string.Empty;
            RightLinkText = rightLinkText;
            RightUrl = rightUrl;
            RightUrlParams = rightUrlParams;
        }
        public BasketInfo(bool isBasketExist, string basketName, string leftLinkText, string leftUrl, string leftUrlParams,
           string rightLinkText, string rightUrl, string rightUrlParams, int basketLocation, string basketClass, int trayID)
        {
            BasketClass = basketClass;
            BasketLocation = basketLocation;
            BasketName = basketName;
            IsBasketExist = isBasketExist;
            LeftDrawerName = string.Empty;
            LeftLinkText = leftLinkText;
            LeftUrl = leftUrl;
            LeftUrlParams = leftUrlParams;
            RightDrawerName = string.Empty;
            RightLinkText = rightLinkText;
            RightUrl = rightUrl;
            RightUrlParams = rightUrlParams;
            TrayID = trayID;
        }
        public BasketInfo(bool isBasketExist, string basketName, string leftLinkText, string leftUrl, string leftUrlParams,
           string rightLinkText, string rightUrl, string rightUrlParams, int basketLocation, string basketClass, int trayID, int countersType)
        {
            BasketClass = basketClass;
            BasketLocation = basketLocation;
            BasketName = basketName;
            IsBasketExist = isBasketExist;
            LeftDrawerName = string.Empty;
            LeftLinkText = leftLinkText;
            LeftUrl = leftUrl;
            LeftUrlParams = leftUrlParams;
            RightDrawerName = string.Empty;
            RightLinkText = rightLinkText;
            RightUrl = rightUrl;
            RightUrlParams = rightUrlParams;
            TrayID = trayID;
            CountersType = countersType;
        }
        public BasketInfo(bool isBasketExist, string basketName, string leftLinkText, string leftUrl, string leftUrlParams,
           string rightLinkText, string rightUrl, string rightUrlParams, int basketLocation, string basketClass, int trayID, int countersType, string color)
        {
            BasketClass = basketClass;
            BasketLocation = basketLocation;
            BasketName = basketName;
            IsBasketExist = isBasketExist;
            LeftDrawerName = string.Empty;
            LeftLinkText = leftLinkText;
            LeftUrl = leftUrl;
            LeftUrlParams = leftUrlParams;
            RightDrawerName = string.Empty;
            RightLinkText = rightLinkText;
            RightUrl = rightUrl;
            RightUrlParams = rightUrlParams;
            TrayID = trayID;
            CountersType = countersType;
            Color = color;
        }
    }
}